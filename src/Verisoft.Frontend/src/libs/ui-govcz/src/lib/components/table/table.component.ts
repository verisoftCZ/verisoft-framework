import { NgTemplateOutlet } from '@angular/common';
import {
  AfterViewInit,
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  ContentChild,
  ContentChildren,
  EventEmitter,
  inject,
  Input,
  OnChanges,
  Output,
  QueryList,
  SimpleChanges,
  TemplateRef,
} from '@angular/core';
import { RouterModule } from '@angular/router';
import { GovDesignSystemModule } from '@gov-design-system-ce/angular';
import {
  FormCheckboxEvent,
  FormSelectEvent,
} from '@gov-design-system-ce/components';
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
  RowModel,
  TABLE_COLUMN_PROVIDER,
  TABLE_COMPONENT_TOKEN,
  TableCore,
  TableSelectionMode,
} from '@verisoft/ui-core';
import { getNextSortDirection } from './table.models';

@Component({
  selector: 'v-table',
  standalone: true,
  imports: [RouterModule, GovDesignSystemModule, NgTemplateOutlet],
  templateUrl: './table.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  styleUrl: './table.component.scss',
  providers: [
    {
      provide: TABLE_COMPONENT_TOKEN,
      useExisting: TableComponent,
    },
  ],
})
export class TableComponent<T>
  implements OnChanges, AfterViewInit, TableCore<T>
{
  @ContentChildren(TABLE_COLUMN_PROVIDER) viewColumns!: QueryList<
    ColumnProvider<T>
  >;

  @ContentChild('rowDetail', { static: false })
  rowDetailTemplate!: TemplateRef<{ $implicit: T }>;

  @Input() sorters!: Sort[];
  @Input() data!: T[];
  @Input() total = 0;
  @Input() filter: Partial<T> | undefined;
  @Input() loading = false;
  @Input() scrollable = true;
  @Input() pageSize = DEFAULT_SEARCH_LIMIT;
  @Input() currentPage = 1;
  @Input() showPaginator = true;
  @Input() sortMultiple = false;
  @Input() lazy = false;
  @Input() selectionMode: 'single' | 'multiple' | undefined;
  @Input() selection: T[] = [];
  @Input() showPageSizePicker = true;
  @Input() entityKey: string | undefined;
  @Output() selectionChange = new EventEmitter<T[]>();
  @Output() lazyLoad = new EventEmitter<LazyLoadEvent>();

  @Input()
  set columns(value: ColumnDefinition<T>[]) {
    this._columns = value;
    this.updateColumnModels();
  }

  get columns(): ColumnDefinition<T>[] {
    return this._columns;
  }

  private cdRef = inject(ChangeDetectorRef);

  private _columns: ColumnDefinition<T, keyof T>[] = [];

  tableColumns: ColumnModel<T>[] = [];

  tableRows: RowModel<T>[] = [];

  pageSizeOptions = [10, 25, 50, 100].map((x) => ({ value: x.toString() }));

  allSelected = false;

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

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['data']) {
      this.updateRowModels();
    }

    if (changes['selection']) {
      this.selectSelected();
    }
  }

  ngAfterViewInit(): void {
    if (this.viewColumns?.length) {
      this.updateColumnModels();
      this.cdRef.detectChanges();
    }
  }

  sortColumn(column: ColumnModel<T>) {
    if (column.sortable) {
      column.sortDirection = getNextSortDirection(column.sortDirection, this.sortMultiple);

      if (!this.sortMultiple) {
        this.tableColumns
          .filter((x) => x !== column)
          .forEach((x) => {
            x.sortDirection = undefined;
          });
      }

      const sorts: Sort[] = this.getSorts().map((x) => ({
        field: 'row.' + x.field,
        direction: x.direction,
      }));

      if (!this.lazy) {
        this.tableRows = multiSort(this.tableRows, sorts);
      } else {
        this.fireLazyLoad();
      }
    }
  }

  changePage(event: { detail: { pagination: { currentPage: number } } }) {
    this.currentPage = event.detail.pagination.currentPage;
    this.fireLazyLoad();
  }

  changePageSize(event: CustomEvent<FormSelectEvent>) {
    this.pageSize = parseInt(event.detail.value);
    this.currentPage = 1;
    this.fireLazyLoad();
  }

  selectRow(row: RowModel<T>) {
    if (this.selectionMode === 'single') {
      row.selected = !row.selected;

      const selectedItems = this.tableRows.filter(
        (x) => x.selected && x !== row
      );

      selectedItems.forEach((x) => (x.selected = false));
      this.fireSelectionChange();
    }
  }

  toggleAll(event: CustomEvent<FormCheckboxEvent>) {
    this.allSelected = event.detail.checked;
    this.tableRows.forEach((x) => (x.selected = this.allSelected));
    this.fireSelectionChange();
  }

  toggleDetail(row: RowModel<T>) {
    row.expanded = !row.expanded;
  }

  selectRowCheckbox(row: RowModel<T>, event: CustomEvent<FormCheckboxEvent>) {
    row.selected = event.detail.checked;
    this.allSelected = this.tableRows.every((x) => x.selected);
    this.fireSelectionChange();
  }

  private updateColumnModels() {
    const vewColumns = this.viewColumns
      ?.toArray()
      .map((x) => x.getDefinition());
    this.tableColumns = [...(this._columns ?? []), ...(vewColumns ?? [])].map(
      (x) => new ColumnModel(x)
    );
  }

  private updateRowModels(): void {
    this.tableRows = (this.data ?? []).map(
      (x) => new RowModel(x, false, false)
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

  private getSorts(): Sort[] {
    return this.tableColumns
      .filter((x) => x.sortable && x.sortDirection)
      .map((x) => ({
        field: x.id,
        direction: <SortDirectionType>x.sortDirection,
      }));
  }
}
