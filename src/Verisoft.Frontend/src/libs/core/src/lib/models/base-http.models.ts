/* eslint-disable @typescript-eslint/no-explicit-any */
import { HttpParams, HttpResponse } from '@angular/common/http';
import { InjectionToken } from '@angular/core';

export const BASE_URL_PATH = new InjectionToken<string>('BASE_URL_PATH');

export function requestParamsToHttpParams<T>(
  requestParams: Partial<RequestParams<T>>,
  httpParams: HttpParams = new HttpParams()
): HttpParams {
  if (requestParams.limit != undefined) {
    httpParams = httpParams.append('limit', requestParams.limit.toString());
  }

  if (requestParams.offset != undefined) {
    httpParams = httpParams.append('offset', requestParams.offset.toString());
  }

  if (requestParams.id != '' && requestParams.id != undefined) {
    httpParams = httpParams.append('id', requestParams.id as any);
  }

  httpParams = getFilter(requestParams, httpParams);
  httpParams = getSort(requestParams, httpParams);

  return httpParams;
}

function getFilter<T>(
  requestParams: Partial<RequestParams<any>>,
  httpParams: HttpParams
): HttpParams {
  if (!requestParams.filter) {
    return httpParams;
  }
  Object.keys(requestParams?.filter).forEach((key) => {
    const value = requestParams.filter?.[key as keyof Partial<T>];
    if (value != undefined) {
      httpParams = httpParams.append('Filter.' + key, value);
    }
  });
  return httpParams;
}

function getSort(
  requestParams: Partial<RequestParams<any>>,
  httpParams: HttpParams
): HttpParams {
  if (!requestParams.sort) {
    return httpParams;
  }

  requestParams.sort?.forEach((sort) => {
    httpParams = httpParams
      .append('Sort.Field', sort.field)
      .append('Sort.Direction', sort.direction);
  });
  return httpParams;
}

export function saveFile(response: HttpResponse<Blob>) {
  const fileName = response.headers
    .get('Content-Disposition')
    ?.split(';')[1]
    .split('=')[1];
  const blob: Blob = response.body as Blob;
  const a = document.createElement('a');
  a.download = fileName ?? 'undefined';
  a.href = window.URL.createObjectURL(blob);
  a.click();
}

export enum SortDirection {
  asc = 'asc',
  desc = 'desc',
}

export declare type SortDirectionType = keyof typeof SortDirection;

export declare interface Sort {
  field: string;
  direction: SortDirectionType;
}

export interface RequestParams<T> extends AllDataRequestParams<T> {
  offset: number;
  limit: number;
  id?: string;
}

export interface AllDataRequestParams<T> {
  filter?: Partial<T>;
  sort?: Sort[];
}

export interface Page<T> {
  data: T[];
  total: number;
  limit: number;
  offset: number;
}

export const DEFAULT_SEARCH_LIMIT = 50;

export const DEFAULT_SEARCH_PARAMS: RequestParams<any> = {
  offset: 0,
  limit: DEFAULT_SEARCH_LIMIT,
  id: '',
};

export function normalizeRequest<T>(
  request: Partial<RequestParams<T>>,
  minLimit: number | undefined = undefined
): RequestParams<T> {
  return {
    offset: request?.offset ?? 0,
    limit: !request?.limit
      ? DEFAULT_SEARCH_LIMIT
      : minLimit && request.limit < minLimit
      ? minLimit
      : request.limit,
    filter: request?.filter,
    sort: request?.sort,
  };
}
