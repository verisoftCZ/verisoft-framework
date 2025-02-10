import { HttpClient } from '@angular/common/http';
import {
  ChangeDetectorRef,
  Directive,
  inject,
  Input,
  OnChanges,
  OnInit,
  SimpleChanges,
} from '@angular/core';
import { Store } from '@ngrx/store';
import {
  BASE_URL_PATH,
  convertDatasource,
  DataSourceFunctionType,
  DatasourceType,
  LazyLoadEvent,
  normalizeRequest,
  Page,
  RequestParams,
} from '@verisoft/core';
import {
  BehaviorSubject,
  catchError,
  debounceTime,
  map,
  of,
  switchMap,
  takeUntil,
  tap,
} from 'rxjs';
import {
  DEFAULT_DEBOUNCE_TIME,
  ExtendedRequestType,
  isFilterEmpty,
  setComponentProperties,
} from '../common';
import {
  TABLE_COMPONENT_TOKEN,
  TableCore,
  UnsubscribeComponent,
} from '../components';

@Directive({
  // eslint-disable-next-line @angular-eslint/directive-selector
  selector: 'v-table[useDatasource]',
  exportAs: 'useDatasource',
  standalone: true,
})
export class TableDatasourceDirective<T>
  extends UnsubscribeComponent
  implements OnInit, OnChanges
{
  store = inject(Store);

  @Input() autoBind = true;

  @Input({ required: true }) tableName!: string;

  @Input() debounceTime = DEFAULT_DEBOUNCE_TIME;

  @Input() datasource!: DatasourceType<T>;

  @Input() transformFn?: (data: T) => unknown;

  private tableComponent = inject<TableCore<T>>(TABLE_COMPONENT_TOKEN, {
    self: true,
  });
  private changeDetectorRef = inject(ChangeDetectorRef);
  private httpClient = inject(HttpClient);
  private baseUrl: string = inject(BASE_URL_PATH);

  private dataSourceService?: DataSourceFunctionType<T>;
  private parameters$ = new BehaviorSubject<Partial<ExtendedRequestType<T>>>(
    {}
  );

  params$ = this.parameters$.asObservable();

  ngOnInit(): void {
    if (!this.tableName) {
      throw new Error('Property tableName must be defined.');
    }

    this.tableComponent.lazyLoad
      .pipe(takeUntil(this.destroyed$))
      .subscribe((value: LazyLoadEvent) => {
        this.parameters$.next({
          offset: value?.offset,
          limit: value?.limit,
          filter: value?.filter,
          sort: value?.sort,
        });
      });

    this.parameters$
      .pipe(
        takeUntil(this.destroyed$),
        map((request) => normalizeRequest(request)),
        debounceTime(this.debounceTime),
        tap(() => {
          this.changeComponent(this.tableComponent, {
            lazy: true,
            loading: true,
          });
        }),
        switchMap((request) =>
          this.dataSourceService
            ? this.dataSourceService(request).pipe(
                map((response) => ({ request, response }))
              )
            : of({
                request,
                response: {
                  data: [] as T[],
                  total: 0,
                  limit: request.limit,
                  offset: request.offset,
                } as Page<T>,
              })
        ),
        catchError((request) => {
          this.changeComponent(this.tableComponent, {
            loading: false,
          });
          return of({
            request: request as ExtendedRequestType<T>,
            response: {
              data: [] as T[],
              total: 0,
              limit: request.limit,
              offset: request.offset,
            } as Page<T>,
          });
        })
      )
      .subscribe(({ request, response }) =>
        this.setDataToControl(request, response)
      );
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['datasource']) {
      this.dataSourceService = convertDatasource(
        this.datasource,
        this.baseUrl,
        this.httpClient
      );

      if (this.autoBind) {
        this.parameters$.next({ offset: 0 });
      }
    }
  }

  reload() {
    this.parameters$.next({ ...this.parameters$.value });
  }

  private changeComponent(
    component: TableCore<T>,
    value: Partial<TableCore<T>>
  ) {
    setComponentProperties(component, value);
    this.changeDetectorRef.detectChanges();
  }

  private setDataToControl(request: RequestParams<T>, result: Page<T>): void {
    const data = result.data;
    const total = result.total;
    const transferedData = this.transformFn
      ? data.map((x) => this.transformFn?.(x))
      : data;

    this.changeComponent(this.tableComponent, {
      lazy: !isFilterEmpty(request.filter) || data.length < total,
      data: transferedData as T[],
      loading: false,
      total,
    });
  }
}
