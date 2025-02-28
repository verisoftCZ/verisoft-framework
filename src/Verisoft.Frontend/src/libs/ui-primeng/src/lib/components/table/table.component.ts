import { CommonModule } from '@angular/common';
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
  ViewChild,
  ViewEncapsulation,
} from '@angular/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import {
  DEFAULT_SEARCH_LIMIT,
  LazyLoadEvent,
  Sort,
  SortDirection,
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
  TableSelectionModeType,
} from '@verisoft/ui-core';
import { SortMeta } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import { RippleModule } from 'primeng/ripple';
import { Table, TableLazyLoadEvent, TableModule, TablePageEvent } from 'primeng/table';
import { TableFilterPipe } from './table-filter.pipe';
import { convertToFilter } from './table.models';
import { Icons } from '../../icons';

@Component({
  selector: 'v-table',
  standalone: true,
  imports: [
    CommonModule,
    TableModule,
    RouterModule,
    ButtonModule,
    RippleModule,
    TableFilterPipe,
  ],
  providers: [
    {
      provide: TABLE_COMPONENT_TOKEN,
      useExisting: TableComponent,
    },
  ],
  templateUrl: './table.component.html',
  styleUrl: './table.component.scss',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TableComponent<T>
  implements AfterViewInit, OnChanges, TableCore<T>
{
  @ContentChildren(TABLE_COLUMN_PROVIDER) viewColumns!: QueryList<
    ColumnProvider<T>
  >;

  @ViewChild('table') table: Table | undefined;

  @ContentChild('rowDetail', { static: false })
  rowDetailTemplate!: TemplateRef<any>;

  sorters!: Sort[];

  @Input()
  data!: T[];

  @Input()
  total = 0;

  @Input() filter: Partial<T> | undefined;

  @Input()
  loading!: boolean;

  @Input()
  scrollable = true;

  @Input()
  pageSize = DEFAULT_SEARCH_LIMIT;

  @Input()
  currentPage!: number;

  @Input()
  showPaginator = true;

  @Input()
  sortMultiple = false;

  @Input()
  lazy = false;

  @Input()
  selectionMode: TableSelectionModeType | undefined;

  @Input()
  showPageSizePicker = true;

  @Input()
  entityKey: string | undefined;

  @Input()
  set columns(value: ColumnDefinition<T>[]) {
    this._columns = value;
    this.tableColumns = (value ?? []).map((x) => new ColumnModel(x));
  }

  get columns(): ColumnDefinition<T>[] {
    return this._columns;
  }

  @Input() selection: T[] = [];

  @Output()
  pageSizeChange = new EventEmitter<number>();

  @Output()
  selectionChange = new EventEmitter<T[]>();

  @Output()
  pageChange = new EventEmitter<TablePageEvent>();

  @Output()
  sortChange = new EventEmitter<Sort[]>();

  @Output()
  lazyLoad = new EventEmitter<LazyLoadEvent>();

  sorted!: boolean;

  cdRef = inject(ChangeDetectorRef);

  tableColumns: ColumnModel<T>[] = [];

  tableRows: RowModel<T>[] = [];

  tableSelection: RowModel<T>[] | RowModel<T> | undefined;

  expandedRows = {};

  expansionTemplate!: { template: TemplateRef<any> };

  sortMeta: SortMeta[] | undefined;

  sortSignleColumn: SortMeta | undefined;

  globalFilterFields: string[] | undefined;

  router = inject(Router);

  route = inject(ActivatedRoute);

  icons = Icons;

  private _columns!: ColumnDefinition<T>[];

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['data']) {
      this.updateRowModels();
    }

    if (changes['selection'] || changes['data']) {
      this.tableSelection = this.selection
        ? this.tableRows.filter((x) => this.selection.includes(x.row))
        : undefined;
    }

    if (changes['filter'] && !changes['filter'].isFirstChange()) {
        this.globalFilterFields = this.columns.filter(x => x.id).map(x => 'row.' + x.id);
        setTimeout(() => {
          this.table?._filter();
        }, 0);
    }
  }

  ngAfterViewInit(): void {
    if (this.viewColumns?.length) {
      const templateColumnModel = this.viewColumns
        .toArray()
        .map((provider) => new ColumnModel(provider.getDefinition()));
      this.tableColumns = [...this.tableColumns, ...templateColumnModel];
    }

    this.expansionTemplate = {
      template: this.rowDetailTemplate,
    };
    this.cdRef.detectChanges();
  }

  updateRowModels(): void {
    this.tableRows = (this.data ?? []).map(
      (x) => new RowModel(x, false, false)
    );
  }

  pageChanged(page: TablePageEvent) {
    this.pageChange.emit(page);
  }

  pageSizeChanged(pageSize: number) {
    this.pageSizeChange.emit(pageSize);
    this.pageSize = pageSize;
  }

  sortChanged(event: SortMeta | { multisortmeta: SortMeta[] }) {
    let eventEmiterArg: Sort[];
    if ((<SortMeta>event).field) {
      const singleEvent = <SortMeta>event;
      eventEmiterArg = this.getSorts([singleEvent]) as Sort[];
      this.sortMeta = undefined;
      this.sortSignleColumn = singleEvent;
    } else {
      const multiEvent = <{ multisortmeta: SortMeta[] }>event;
      eventEmiterArg = this.getSorts(multiEvent.multisortmeta) as Sort[];
      this.sortMeta = multiEvent.multisortmeta;
      this.sortSignleColumn = undefined;
    }

    this.sortChange.emit(eventEmiterArg);
  }

  selectionChanged(event: RowModel<T>[] | RowModel<T>) {
    this.tableSelection = event;
    if (this.selectionMode) {
      const eventArgArray = Array.isArray(event) ? event : [event];
      const selection = event ? eventArgArray.map((x) => x.row) : [];
      this.selectionChange.emit(selection);
    }
  }

  fireLazyLoad(event: TableLazyLoadEvent) {
    if (this.lazy) {
      const sort = this.sortSignleColumn
        ? this.getSorts([this.sortSignleColumn])
        : this.sortMeta
        ? this.getSorts(this.sortMeta)
        : undefined;
      const lazyLoadEvent: LazyLoadEvent = {
        sort,
        offset: event.first ?? 0,
        limit: event.rows ?? this.pageSize,
        filter: convertToFilter(event.filters),
      };

      this.lazyLoad.emit(lazyLoadEvent);
    }
  }

  private removePrefix(input: string, prefix: string): string {
    return input.startsWith(prefix) ? input.slice(prefix.length) : input;
  }

  private convertToSortDirection(value: number): SortDirectionType {
    return value === 1 ? SortDirection.asc : SortDirection.desc;
  }

  private getSorts(value: SortMeta[] | undefined) {
    return value
      ? value.map((x: SortMeta) => ({
          field: this.removePrefix(x.field, 'row.'),
          direction: this.convertToSortDirection(x.order),
        }))
      : undefined;
  }
}
