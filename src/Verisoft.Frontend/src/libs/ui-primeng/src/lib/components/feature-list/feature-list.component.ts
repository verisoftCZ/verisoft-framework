import { HttpClient } from '@angular/common/http';
import {
  AfterViewInit,
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  ContentChildren,
  EventEmitter,
  inject,
  Input,
  Output,
  QueryList,
  ViewChild,
} from '@angular/core';
import {
  AllDataRequestParams,
  BASE_URL_PATH,
  BaseHttpService,
  convertDatasource,
  DatasourceType,
  DEFAULT_PAGE_SIZE,
  DEFAULT_SEARCH_LIMIT,
} from '@verisoft/core';
import { DEFAULT_DEBOUNCE_TIME } from '@verisoft/ui-core';
import {
  ColumnModel,
  DialogService,
  downloadFile,
  FEATURE_LIST_COLUMN_PROVIDER,
  FEATURE_LIST_COMPONENT_TOKEN,
  FeatureListColumnDefinition,
  FeatureListColumnProvider,
  FeatureListCore,
  FeatureListFilterFieldDirective,
  FeatureListFilterPipe,
  GenericFieldDefinition,
  MAX_COLUMN_CHAR_COUNT,
  TableDatasourceDirective,
  TableFilterDirective,
  TableSelectionMode,
  TableSelectionModeType,
} from '@verisoft/ui-core';
import { take, switchMap, forkJoin, Observable, of } from 'rxjs';
import { Icons } from '../../icons';
import { FilterComponent } from '../filter';
import {
  ActionButtonComponent,
  ActionButtonGroupComponent,
} from '../shared-components/action-button-group';
import { TableComponent } from '../table';

@Component({
    selector: 'v-feature-list',
    imports: [
        TableComponent,
        TableDatasourceDirective,
        FilterComponent,
        TableFilterDirective,
        ActionButtonGroupComponent,
        ActionButtonComponent,
        FeatureListFilterPipe,
    ],
    providers: [
        {
            provide: FEATURE_LIST_COMPONENT_TOKEN,
            useExisting: FeatureListComponent,
        },
    ],
    templateUrl: './feature-list.component.html',
    styleUrls: ['./feature-list.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class FeatureListComponent<T> implements AfterViewInit, FeatureListCore<T> {
  @ContentChildren(FEATURE_LIST_COLUMN_PROVIDER) viewColumns!: QueryList<
    FeatureListColumnProvider<T>
  >;

  @ContentChildren(FeatureListFilterFieldDirective)
  fieldDeclarations!: QueryList<FeatureListFilterFieldDirective>;

  @ViewChild(TableDatasourceDirective)
  tableDatasourceDirective!: TableDatasourceDirective<T>;

  @Input() autoBind = true;

  @Input() title!: string;

  @Input() columns!: FeatureListColumnDefinition<T>[];

  @Input() filters!: GenericFieldDefinition[];

  @Input() maxVisibleActions = 2;

  @Input({ required: true }) tableName!: string;

  @Input() ngrxFeatureKey?: string;

  @Input() maxVisibleMobileActions = 0;

  @Input() showExtendedFilter = false;

  @Input() showDownload = false;

  @Input() showDelete = false;

  @Input() showAdd = false;

  @Input() canDownload = true;

  @Input() canDelete = true;

  @Input() canAdd = true;

  @Input() useRouterFilter = true;

  @Input() fulltextFieldName = 'searchField';

  @Input() showFulltext = true;

  @Input() filterDialogShowFulltext = true;

  @Input() deleteConfirmMessage: string | undefined;

  @Input() autoDeleteEnabled = true;

  @Input() autoDownloadEnabled = true;

  @Input() downloadFileName = 'export.csv';

  @Input() datasource!: DatasourceType<T>;

  @Input() extraFilter: Partial<T> | undefined;

  @Input() disableCustomClicks = false;

  @Input() maximumColumnLength = MAX_COLUMN_CHAR_COUNT;

  @Input() filterDetailTitle = 'Set filters';

  @Input() applyButtonLabel = 'Apply';

  @Input() clearButtonLabel = 'Clear all';

  @Input() pageSize = DEFAULT_SEARCH_LIMIT;
  
  @Input() showPaginator = true;

  @Input() datasourceDebounceTime = DEFAULT_DEBOUNCE_TIME;

  @Output() addClick = new EventEmitter();

  @Output() downloadClick = new EventEmitter();

  @Output() deleteClick = new EventEmitter<T[]>();

  @Output() selectionChange = new EventEmitter();

  @Input() set selectionMode(mode: TableSelectionModeType | undefined) {
    this._selectionMode = mode;
  }

  get selectionMode(): TableSelectionModeType | undefined {
    return this.canDelete
      ? TableSelectionMode.single
      : this._selectionMode;
  }

  icons = Icons;

  private httpClient = inject(HttpClient);

  private baseUrl: string = inject(BASE_URL_PATH);

  private _selectionMode: TableSelectionModeType | undefined;

  private cdRef = inject(ChangeDetectorRef);

  private dialogService = inject(DialogService);

  selection: T[] = [];

  ngAfterViewInit(): void {
    if (this.viewColumns?.length) {
      const vewColumns = this.viewColumns
        ?.toArray()
        .map((x) => x.getDefinition());
      this.columns = [...(this.columns ?? []), ...(vewColumns ?? [])];
      this.cdRef.detectChanges();
    }

    if (this.fieldDeclarations?.length) {
      this.filters = this.fieldDeclarations?.toArray();
    }

    this.cdRef.detectChanges();
  }

  selectItems(items: T[]) {
    this.selection = items ?? [];

    if (this.selectionMode !== undefined) {
      this.selectionChange.emit(
        this.selectionMode === 'multiple'
          ? this.selection
          : this.selection[0]
      );
    }
  }

  reload() {
    this.tableDatasourceDirective.reload();
  }

  startDeleteItems() {
    if (!this.selection.length) {
      return;
    }

    if (this.deleteConfirmMessage) {
      const message = this.deleteConfirmMessage;
      this.dialogService.showDialog({
        innerHTML: message,
        confirmButtonFn: () => this.deleteItems(this.selection),
      });
    } else {
      this.deleteItems(this.selection);
    }
  }

  startDownload() {
    this.downloadClick.emit();
    if (!this.autoDownloadEnabled) {
      return;
    }

    const dowloadFn = this.createDownloadFn(this.datasource);
    if (dowloadFn) {
      this.tableDatasourceDirective.params$
        .pipe(
          take(1),
          switchMap((params) => dowloadFn(params))
        )
        .subscribe((blob) => {
          downloadFile(this.downloadFileName, blob);
        });
    }
  }

  private deleteItems(items: T[]) {
    if (!this.autoDeleteEnabled) {
      this.deleteClick.emit(items);
    } else {
      this.forceDelete(items);
    }
  }

  private forceDelete(items: T[]) {
    const deleteFn = this.createDeleteFn(this.datasource);
    if (deleteFn) {
      const deleteMethods = items.map((x) => deleteFn(x));
      forkJoin(deleteMethods)
        .pipe(take(1))
        .subscribe(() => this.finishDeletion(items));
    } else {
      this.finishDeletion(items);
    }
  }

  private createDeleteFn(
    datasource: DatasourceType<T>
  ): ((item: T) => Observable<unknown>) | undefined {
    if (!datasource) {
      return undefined;
    }

    if (typeof datasource === 'string') {
      const service = new BaseHttpService<T>(
        this.httpClient,
        this.baseUrl,
        datasource
      );

      return (item: T) => service.delete((item as { id: string }).id);
    }

    if (datasource instanceof BaseHttpService) {
      return (item: T) => datasource.delete((item as { id: string }).id);
    }

    return undefined;
  }

  private createDownloadFn(
    datasource: DatasourceType<T>
  ): ((request: AllDataRequestParams<T>) => Observable<Blob>) | undefined {
    if (!datasource) {
      return undefined;
    }

    if (typeof datasource === 'string') {
      const service = new BaseHttpService<T>(
        this.httpClient,
        this.baseUrl,
        datasource
      );

      return (request: AllDataRequestParams<T>) => service.export(request);
    }

    if (datasource instanceof BaseHttpService) {
      return (request: AllDataRequestParams<T>) => datasource.export(request);
    }

    const fetchFunction = convertDatasource(
      datasource,
      this.baseUrl,
      this.httpClient
    );

    return (request: AllDataRequestParams<T>) => {
      const allData: T[] = [];
      let offset = 0;
      const fetchAllData = (): Observable<Blob> => {
        return fetchFunction({
          ...request,
          offset,
          limit: DEFAULT_PAGE_SIZE,
        }).pipe(
          take(1),
          switchMap((response) => {
            allData.push(...response.data);
            if (allData.length < response.total) {
              offset = offset + DEFAULT_PAGE_SIZE;
              return fetchAllData();
            } else {
              return of(this.convertToBlob(allData));
            }
          })
        );
      };

      return fetchAllData();
    };
  }

  private finishDeletion(items: T[]) {
    if (this.datasource && Array.isArray(this.datasource)) {
      this.datasource = this.datasource.filter((x) => !items.includes(x));
    } else {
      this.tableDatasourceDirective.reload();
    }

    this.deleteClick.emit(items);
  }

  private convertToBlob(data: T[]): Blob {
    const columnModel = this.columns.map((x) => new ColumnModel(x));
    const headers = columnModel.map((x, index) => x.headerGetter(x.id, index));
    const dataValues = data.map((row, rowIndex) =>
      columnModel.map((column) => column.valueGetter(row, rowIndex))
    );
    const csvData = [headers, ...dataValues]
      .map((row) => row.join(','))
      .join('\n');
    return new Blob([csvData], { type: 'text/csv' });
  }
}
