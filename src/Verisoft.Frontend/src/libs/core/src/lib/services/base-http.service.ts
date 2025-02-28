/* eslint-disable @typescript-eslint/no-explicit-any */
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import {
  AllDataRequestParams,
  DEFAULT_SEARCH_PARAMS,
  Page,
  RequestParams,
  requestParamsToHttpParams,
} from '../models/base-http.models';
import { LazyLoadEvent } from '../models/event.models';
import { ClearUtils } from '../utils/clear.utils';

export class BaseHttpService<T, TKey = number | string> {
  constructor(
    readonly http: HttpClient,
    readonly basePath: string,
    readonly entityName: string
  ) {
    this.basePath = basePath;
  }

  private get apiPath() {
    return this.basePath ? this.basePath + this.entityName : this.entityName;
  }

  fetchList(requestParams: RequestParams<T>): Observable<Page<T>> {
    const params = requestParamsToHttpParams<T>(requestParams);
    return this.http.get<Page<T>>(this.apiPath, { params }).pipe(
      map((response: Page<T>) => {
        response.limit = requestParams.limit;
        return response;
      })
    );
  }

  getData$(
    entityName: keyof T,
    searchTerm?: string | undefined,
    lazyLoad?: LazyLoadEvent
  ): Observable<T[]> {
    const filter: Partial<T> = { [entityName]: searchTerm } as Partial<T>;
    const params = lazyLoad
      ? requestParamsToHttpParams<T>({
          ...DEFAULT_SEARCH_PARAMS,
          offset: lazyLoad.offset,
          limit: lazyLoad.limit,
          filter,
        })
      : requestParamsToHttpParams<T>({
          ...DEFAULT_SEARCH_PARAMS,
          filter,
        });

    return this.http.get<T[]>(this.apiPath, { params });
  }

  get(id: TKey): Observable<T> {
    const url = `${this.apiPath}/${id}`;
    return this.http.get<T>(url);
  }

  post(entity: Partial<T>): Observable<T> {
    return this.http.post<T>(this.apiPath, entity);
  }

  put(id: TKey, entity: Partial<T>): Observable<T> {
    const url = `${this.apiPath}/${id}`;
    return this.http.put<T>(url, entity);
  }

  delete(id: TKey): Observable<T> {
    const url = `${this.apiPath}/${id}`;
    return this.http.delete<T>(url);
  }

  export(requestParams: AllDataRequestParams<T>): Observable<Blob> {
    const httpParams = requestParamsToHttpParams<T>(requestParams);
    const url = `${this.apiPath}/export`;
    return this.http.get(url, { responseType: 'blob', params: httpParams });
  }

  removeRange(entity: string[]) {
    return this.http.delete(this.apiPath, {
      body: entity,
    });
  }

  createParams<T>(requestParams: RequestParams<T>): any {
    const sorter = this.createSorter(requestParams);
    const filter = JSON.parse(JSON.stringify(requestParams.filter));
    const transformedFilter = filter;
    ClearUtils.recursiveObjectAttributesDeletation(transformedFilter);
    return {
      paging: {
        offset: requestParams.offset,
        limit: requestParams.limit,
      },
      ...(sorter && { sorting: sorter }),
      ...(transformedFilter && { filter: transformedFilter }),
    };
  }

  private createSorter<T>(requestParams: RequestParams<T>) {
    if (requestParams?.sort?.length === 1) {
      const sorter = requestParams.sort[0];
      return {
        sortField: sorter.field,
        sortOrder: sorter.direction,
      };
    }
    return null;
  }
}
