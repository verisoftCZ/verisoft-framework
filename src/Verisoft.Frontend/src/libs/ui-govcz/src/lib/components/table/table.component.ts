import { CommonModule, NgTemplateOutlet } from '@angular/common';
import {
  AfterViewInit,
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  ContentChild,
  ContentChildren,
  effect,
  EventEmitter,
  inject,
  Input,
  OnChanges,
  Output,
  QueryList,
  SimpleChanges,
  TemplateRef,
} from '@angular/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { GovDesignSystemModule } from '@gov-design-system-ce/angular';
import {
  FormCheckboxEvent,
  FormSelectEvent,
} from '@gov-design-system-ce/components';
import { TranslateModule } from '@ngx-translate/core';
import {
  DEFAULT_SEARCH_LIMIT,
  LazyLoadEvent,
  multiSort,
  Sort,
  SortDirectionType,
} from '@verisoft/core';
import {
  ColumnDefinition,
  ColumnModel,
  ColumnProvider,
  createRowDetailUrl,
  hasRowDetailRoute,
  navigateToRowDetail,
  openRowDetailInNewWindow,
  DEFAULT_PAGINATION,
  FieldSize,
  FieldSizeType,
  MAX_COLUMN_CHAR_COUNT,
  RowModel,
  TABLE_COLUMN_PROVIDER,
  TABLE_COMPONENT_TOKEN,
  TableCore,
  TableSelectionMode,
  TableService,
} from '@verisoft/ui-core';
import { Icons } from '../../icons';
import { GovSizePipe } from '../../pipes';
import { LoaderComponent } from '../loader';
import { TablePaginationInfoComponent } from './table-pagination-info.component';
import { getNextSortDirection } from './table.model';

@Component({
    selector: 'v-table',
    imports: [
        CommonModule,
        RouterModule,
        GovDesignSystemModule,
        NgTemplateOutlet,
        TranslateModule,
        GovSizePipe,
        LoaderComponent,
        TablePaginationInfoComponent,
    ],
    templateUrl: './table.component.html',
    changeDetection: ChangeDetectionStrategy.OnPush,
    styleUrl: './table.component.scss',
    providers: [
        {
            provide: TABLE_COMPONENT_TOKEN,
            useExisting: TableComponent,
        },
    ]
})
export class TableComponent<T>
  implements OnChanges, AfterViewInit, TableCore<T>
{
  @ContentChildren(TABLE_COLUMN_PROVIDER)
  viewColumns!: QueryList<ColumnProvider<T>>;

  @ContentChild('rowDetail', { static: false })
  rowDetailTemplate!: TemplateRef<{ $implicit: T }>;

  @ContentChild('rowDetailButton', { static: false })
  rowDetailButtonTemplate!: TemplateRef<{ $implicit: T }>;

  @Input() sorters!: Sort[];
  @Input() data!: T[];
  @Input() total = 0;
  @Input() filter: Partial<T> | undefined;
  @Input() extraFilter: Partial<T> | undefined;
  @Input() loading = false;
  @Input() scrollable = true;
  @Input() pageSize = DEFAULT_SEARCH_LIMIT;
  @Input() currentPage = 1;
  @Input() showPaginator = true;
  @Input() userTableWrapper = true;
  @Input() sortMultiple = false;
  @Input() lazy = false;
  @Input() selectionMode: 'single' | 'multiple' | undefined;
  @Input() selection: T[] = [];
  @Input() showPageSizePicker = true;
  @Input() entityKey: string | undefined;
  @Input() customRoute: string | undefined;
  @Input() disableCustomClicks = false;
  @Input() tableName?: string;
  @Input() size: FieldSizeType = FieldSize.medium;
  @Input() showActionButtons = false;
  @Input() maximumColumnLength = MAX_COLUMN_CHAR_COUNT;
  @Input() rowClass: (data: RowModel<T>) => string = () => '';

  @Input()
  set columns(value: ColumnDefinition<T>[]) {
    this._columns = value;
    this.updateColumnModels();
  }

  get columns(): ColumnDefinition<T>[] {
    return this._columns;
  }

  @Output() selectionChange = new EventEmitter<T[]>();
  @Output() lazyLoad = new EventEmitter<LazyLoadEvent>();
  @Output() sortChange = new EventEmitter<Sort[]>();
  @Output() download = new EventEmitter<RowModel<T>[]>();
  @Output() delete = new EventEmitter<RowModel<T>[]>();
  @Output() save = new EventEmitter<RowModel<T>[]>();
  @Output() rowDoubleClick = new EventEmitter<T>();

  private readonly cdRef = inject(ChangeDetectorRef);
  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);
  private readonly service = inject(TableService);
  private _columns: ColumnDefinition<T, keyof T>[] = [];

  tableColumns: ColumnModel<T>[] = [];
  tableRows: RowModel<T>[] = [];
  pageSizeOptions = DEFAULT_PAGINATION.map((x: number) => ({ value: x.toString() }));
  allSelected = false;
  icons = Icons;

  get govPageSize() {
    return this.pageSize.toString();
  }

  get tableRowView(): RowModel<T>[] {
    if (this.lazy) {
      return this.tableRows;
    } else {
      const start = (this.currentPage - 1) * this.pageSize;
      const end = start + this.pageSize;
      return this.tableRows.slice(start, end);
    }
  }

  get selected(): RowModel<T>[] {
    return this.tableRows.filter(r => r.selected);
  }

  get selectedCount(): number {
    return this.selected.length;
  }

  constructor() {
    effect(() => {
      this.reactToSignals();
    })
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['data']) {
      this.updateRowModels();
      if (!this.lazy && this.data) {
        this.total = this.data.length;
      }
    }

    if (changes['total']) {
      this.cdRef.markForCheck();
    }

    if (changes['selection']) {
      this.selectSelected();
    }

    if (changes['filter']) {
      this.updateFilter();
    }

    if (changes['columns']) {
      this.applyColumnSorts();
    }
  }

  ngAfterViewInit(): void {
    if (this.viewColumns?.length) {
      this.updateColumnModels();
      this.applyColumnSorts();
      this.cdRef.detectChanges();
    }
  }

  protected sortColumn(column: ColumnModel<T>) {
    if (column.sortable) {
      column.sortDirection = getNextSortDirection(
        column.sortDirection,
        this.sortMultiple
      );

      if (!this.sortMultiple) {
        this.tableColumns
          .filter((x) => x !== column)
          .forEach((x) => {
            x.sortDirection = undefined;
          });
      }

      this.currentPage = 1;

      if (!this.lazy) {
        this.applyClientSort(this.getSorts());
      } else {
        this.fireLazyLoad();
      }
    }
  }

  private applyColumnSorts(): void {
    const sorts = this.getSorts();
    if (!sorts.length) {
      return;
    }

    if (!this.lazy) {
      this.applyClientSort(sorts);
    } else {
      this.fireLazyLoad();
    }
  }

  private applyClientSort(sorts: Sort[]): void {
    this.tableRows = multiSort(
      this.tableRows,
      sorts.map((x) => ({ field: 'row.' + x.field, direction: x.direction }))
    );
    this.sortChange.emit(sorts);
  }

  protected changePage(event: { detail: { pagination: { currentPage: number } } }) {
    this.currentPage = event.detail.pagination.currentPage;
    this.fireLazyLoad();
  }

  protected changePageSize(event: CustomEvent<FormSelectEvent>) {
    this.pageSize = parseInt(event.detail.value);
    this.currentPage = 1;
    this.fireLazyLoad();
  }

  protected selectRow(row: RowModel<T>, event: MouseEvent) {
    if (event.ctrlKey || event.metaKey) {
      this.navigate(row, event);
      return;
    }

    if (this.selectionMode === 'single') {
      row.selected = !row.selected;
      this.tableRows.forEach((x) => (x !== row ? (x.selected = false) : null));
      this.fireSelectionChange();
    } else if (this.selectionMode === 'multiple') {
      row.selected = !row.selected;
      this.fireSelectionChange();
    }
  }

  protected onRowDoubleClick(row: RowModel<T>, event: MouseEvent) {
    if (this.disableCustomClicks) {
      return;
    }

    if (this.rowDoubleClick.observed) {
      this.rowDoubleClick.emit(row.row);
      return;
    }

    if (!hasRowDetailRoute(this.route, createRowDetailUrl(row, this.entityKey))) {
      return;
    }

    this.navigate(row, event);
  }

  protected navigate(row: RowModel<T>, event: MouseEvent) {
    if (this.disableCustomClicks) {
      return;
    }

    event.preventDefault();
    navigateToRowDetail(this.router, this.route, row, this.entityKey, event);
  }

  protected navigateNewWindow(row: RowModel<T>, event: MouseEvent) {
    if (this.disableCustomClicks || event.button !== 1) {
      return;
    }

    event.preventDefault();
    openRowDetailInNewWindow(this.router, this.route, row, this.entityKey);
  }

  protected toggleAll(event: CustomEvent<FormCheckboxEvent>) {
    this.allSelected = event.detail.checked;
    this.tableRows.forEach((x) => (x.selected = this.allSelected));
    this.fireSelectionChange();
  }

  protected toggleDetail(row: RowModel<T>) {
    row.expanded = !row.expanded;
    this.cdRef.detectChanges();
  }

  protected selectRowCheckbox(row: RowModel<T>, event: CustomEvent<FormCheckboxEvent>) {
    row.selected = event.detail.checked;
    this.allSelected = this.tableRows.every((x) => x.selected);
    this.fireSelectionChange();
  }

  protected truncate(text: string): string {
    return text.length > this.maximumColumnLength
      ? text.slice(0, this.maximumColumnLength) + '...'
      : text;
  }

  protected deselectAll() {
    this.allSelected = false;
    this.tableRows.forEach(x => (x.selected = this.allSelected));
  }

  private reactToSignals(): void {
    const reload = this.service.reload();

    if (reload === null) {
      return;
    }

    if (typeof reload === 'symbol') {
      this.fireLazyLoad();
    } else if (reload.name === this.tableName) {
      this.fireLazyLoad()
    }

    this.deselectAll();
  }

  private updateColumnModels() {
    const viewColumns = this.viewColumns
      ?.toArray()
      .map((x) => x.getDefinition());
    this.tableColumns = [...(this._columns ?? []), ...(viewColumns ?? [])].map(
      (x) => new ColumnModel(x)
    );
  }

  private updateRowModels(): void {
    this.tableRows = (this.data ?? []).map(
      (x) => new RowModel(x, false, false, undefined, undefined, undefined, this.customRoute, this.entityKey)
    );
  }

  private fireSelectionChange() {
    const selectedRows = this.tableRows
      .filter((x) => x.selected)
      .map((x) => x.row);
    this.selectionChange.emit(selectedRows);
  }

  private selectSelected() {
    if (this.selection?.length && this.selectionMode) {
      const selection =
        this.selectionMode === TableSelectionMode.single
          ? [this.selection[0]]
          : this.selection;
      const selectionSet = new Set(selection);
      this.tableRows.forEach((item) => {
        item.selected = selectionSet.has(item.row);
      });

      this.allSelected = this.tableRows.every((x) => x.selected);
    }
  }

  private fireLazyLoad() {
    const lazyLoadEvent: LazyLoadEvent = {
      sort: this.getSorts(),
      offset: (this.currentPage - 1) * this.pageSize,
      limit: this.pageSize,
      filter: this.filter,
    };

    this.lazyLoad.emit(lazyLoadEvent);
  }

  private updateFilter() {
    this.fireLazyLoad();
  }

  private getSorts(): Sort[] {
    return this.tableColumns
      .filter((x) => x.sortable && x.sortDirection)
      .sort((a, b) => (a.sortPriority ?? Number.MAX_SAFE_INTEGER) - (b.sortPriority ?? Number.MAX_SAFE_INTEGER))
      .map((x) => ({
        field: x.id,
        direction: <SortDirectionType>x.sortDirection,
      }));
  }
}
