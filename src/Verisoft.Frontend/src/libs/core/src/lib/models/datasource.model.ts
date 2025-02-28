import { HttpClient } from '@angular/common/http';
import { isObservable, map, Observable, of } from 'rxjs';
import { BaseHttpService } from '../services/base-http.service';
import { AllItemDatasource } from './all-item.datasource';
import { Page, RequestParams } from './base-http.models';
import { DEFAULT_PAGE_SIZE } from './constants';

export type DatasourceType<T> =
  | string
  | Observable<T[]>
  | Observable<Page<T>>
  | BaseHttpService<T>
  | T[]
  | Page<T>
  | AllItemDatasource<T>;

export type DataSourceFunctionType<T> = (
  requestParams: RequestParams<T>
) => Observable<Page<T>>;

export function convertDatasource<T>(
  datasource: DatasourceType<T>,
  basePath: string,
  httpClient: HttpClient
): DataSourceFunctionType<T> {
  if (!datasource) {
    throw new Error('Datasource is not defined');
  }

  if (typeof datasource === 'string') {
    const service = new BaseHttpService<T>(httpClient, basePath, datasource);
    return (requestParams: RequestParams<T>) =>
      service.fetchList(requestParams);
  }

  if (datasource instanceof BaseHttpService) {
    return (requestParams: RequestParams<T>) =>
      datasource.fetchList(requestParams);
  }

  if (isObservable(datasource)) {
    return () =>
      (datasource as Observable<Page<T> | T[]>).pipe(map(convertArrayToPage));
  }

  const allItemDatasource = <AllItemDatasource<T>>datasource;
  if (allItemDatasource.getData$) {
    return () =>
      allItemDatasource
        .getData$()
        .pipe(map((data) => convertArrayToPage(data)));
  }

  const page = <Page<T>>datasource;
  if (page.data) {
    return () => of(page);
  }

  if (Array.isArray(datasource)) {
    return () => of(convertArrayToPage(datasource));
  }

  throw new Error('Datasource is not supported');
}

function convertArrayToPage<T>(data: Page<T> | T[]): Page<T> {
  if (Array.isArray(data)) {
    return {
      data: data ?? [],
      total: data?.length ?? 0,
      limit: data?.length ?? DEFAULT_PAGE_SIZE,
      offset: 0,
    };
  }

  return data as Page<T>;
}
