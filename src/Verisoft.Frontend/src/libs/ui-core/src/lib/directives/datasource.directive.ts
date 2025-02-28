/* eslint-disable @angular-eslint/directive-selector */
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
import {
  BASE_URL_PATH,
  convertDatasource,
  DataSourceFunctionType,
  DatasourceType,
  DEFAULT_SEARCH_LIMIT,
  FilterEvent,
  LazyLoadEvent,
  normalizeRequest,
  Page,
  RequestParams,
} from '@verisoft/core';
import {
  BehaviorSubject,
  catchError,
  debounceTime,
  filter,
  map,
  of,
  switchMap,
  takeUntil,
  tap,
} from 'rxjs';
import {
  DataSourceComponentModel,
  DEFAULT_DEBOUNCE_TIME,
  ExtendedRequestType,
  setComponentProperties,
  setDataToArray,
} from '../common';
import {
  DROPDOWN_COMPONENT_TOKEN,
  DropdownCore,
  GENERIC_FIELD_COMPONENT_TOKEN,
  GenericFieldCore,
  MULTISELECT_COMPONENT_TOKEN,
  MultiselectCore,
  UnsubscribeComponent,
} from '../components';

@Directive({
  selector:
    'v-dropdown[useDatasource], v-multiselect[useDatasource], v-generic-field[useDatasource]',
  standalone: true,
})
export class DatasourceDirective<T>
  extends UnsubscribeComponent
  implements OnChanges, OnInit
{
  @Input() datasource!: DatasourceType<T>;

  @Input() autoBind = true;

  @Input() loadingText = '... loading ...';

  @Input() filterField: string | undefined = 'fulltext';

  @Input() transformFn?: (data: T) => unknown;

  get activeComponent(): DataSourceComponentModel<T> {
    return (this.dropdownComponent ??
      this.multiSelectComponent ??
      this.genericField) as DataSourceComponentModel<T>;
  }

  private httpClient = inject(HttpClient);

  private baseUrl: string = inject(BASE_URL_PATH);

  private changeDetectorRef = inject(ChangeDetectorRef);

  private isAllDataLoaded = false;

  private parameters$ = new BehaviorSubject<Partial<ExtendedRequestType<T>>>({});

  private lastParameter = {};

  private dropdownComponent = inject<DropdownCore<T>>(
    DROPDOWN_COMPONENT_TOKEN,
    { optional: true }
  );

  private multiSelectComponent = inject<MultiselectCore<T>>(
    MULTISELECT_COMPONENT_TOKEN,
    {
      optional: true,
    }
  );

  private genericField = inject<GenericFieldCore<T>>(
    GENERIC_FIELD_COMPONENT_TOKEN,
    { optional: true }
  );

  private dataSourceService?: DataSourceFunctionType<T>;

  private loadingPlaceholderItem: { [key: string]: unknown } = {};

  ngOnInit(): void {
    this.activeComponent.showed
      .pipe(takeUntil(this.destroyed$))
      .subscribe(() => {
        if (
          !this.autoBind &&
          !this.activeComponent.options &&
          !this.activeComponent.loading
        ) {
          this.parameters$.next({});
        }
      });

    this.activeComponent.lazyLoad
      .pipe(takeUntil(this.destroyed$))
      .subscribe((value: LazyLoadEvent) => {
        this.parameters$.next({ offset: value?.offset, limit: value?.limit });
      });

    this.activeComponent.filtered
      .pipe(takeUntil(this.destroyed$))
      .subscribe((value: FilterEvent) => {
        const property = this.filterField ?? this.activeComponent.optionLabel;
        if (property) {
          this.parameters$.next({
            offset: 0,
            filter: {
              [property]: value.filter ? value.filter : undefined,
            } as Partial<T>,
            useNewData: true,
          });
        }
      });

    this.parameters$
      .pipe(
        takeUntil(this.destroyed$),
        filter(request => !this.isDataForRequestLoaded(request)),
        map((request) => {
          const extendedParams = normalizeRequest({ ...this.lastParameter, ...request }, DEFAULT_SEARCH_LIMIT) as ExtendedRequestType<T>;
          extendedParams.useNewData = request.useNewData ?? false;
          return extendedParams;
        }),
        tap((request) => {
          this.lastParameter = request;
        }),
        debounceTime(DEFAULT_DEBOUNCE_TIME),
        tap(() => {
          this.changeComponent(this.activeComponent, {
            loading: true,
          });
        }),
        switchMap((request) =>
          this.dataSourceService
            ? this.dataSourceService(request as RequestParams<T>).pipe(
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
          this.changeComponent(this.activeComponent, {
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

  private isDataForRequestLoaded(request: Partial<ExtendedRequestType<T>>) {
    if (request.useNewData){
      return false;
    }

    const offset = request.offset ?? 0;
    const limit = request.limit ?? DEFAULT_SEARCH_LIMIT;
    const options = this.activeComponent.options;
    if (!options){
      return false;
    }

    if (options.length < offset + limit) {
      return false;
    }
    
    const allItemsFilled = options.slice(offset, offset + limit).every(item => item !== undefined && item != this.loadingPlaceholderItem);    
    return allItemsFilled;
  }

  private setDataToControl(request: ExtendedRequestType<T>, result: Page<T>): void {
    this.isAllDataLoaded = result.total <= result.data.length;
    const data = result.data;
    const total = result.total;
    const offset = request.offset;
    const transferedData = this.transformFn
      ? data.map((x) => this.transformFn?.(x))
      : data;

    if (this.activeComponent.optionLabel) {
      this.loadingPlaceholderItem[this.activeComponent.optionLabel] =
        this.loadingText;
    }

    if (this.activeComponent.optionValue) {
      this.loadingPlaceholderItem[this.activeComponent.optionValue] = -1;
    }

    const options = request.useNewData ? undefined : this.activeComponent.options;
    const newOptions = setDataToArray(
      options,
      transferedData,
      offset,
      total,
      this.loadingPlaceholderItem
    );

    this.changeComponent(this.activeComponent, {
      options: newOptions as T[],
      loading: false,
      lazy: !this.isAllDataLoaded,
    });
  }

  private changeComponent(
    component: DataSourceComponentModel<T>,
    value: Partial<DataSourceComponentModel<T>>
  ) {
    setComponentProperties(component, value);
    this.changeDetectorRef.detectChanges();
  }
}
