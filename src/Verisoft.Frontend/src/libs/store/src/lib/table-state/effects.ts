import { Actions, createEffect, ofType } from '@ngrx/effects';
import {  createFeatureSelector, createSelector, Store } from '@ngrx/store';
import { BaseHttpService, DEFAULT_SEARCH_LIMIT, Page, RequestParams } from '@verisoft/core';
import { catchError, map, Observable, of, switchMap, withLatestFrom } from 'rxjs';
import {
  createGetPageTableAction,
  createDataLoadSuccessTableAction,
  createDataLoadErrorTableAction,
  createRemoveRangeTableAction,
} from './actions';
import { TableState } from './models';

export interface CreateGetPageActionConfig<T> {
  service?: BaseHttpService<T>;
  fetchList?: (requestParams: RequestParams<any>) => Observable<Page<T>>;
  snackbar?: any;
  ngrxFeatureKey?: string;
  requireFilters?: boolean;
}

export function createGetPageTableEffect<T>(
  tableRepository: string,
  actions$: Actions,
  config: CreateGetPageActionConfig<T>
) {
  return createEffect(() => {
    return actions$.pipe(
      ofType(createGetPageTableAction(tableRepository)),
      switchMap(({ page, filter, sort, size }) => {
        const requestParams: RequestParams<any> = {
          offset: page * size,
          limit: size,
          sort,
          filter,
        };

        const params = config.service?.createParams(requestParams) ?? requestParams;

        const fetchList$: Observable<Page<T>> = (config.service?.fetchList(
          params
        ) ?? config.fetchList?.(params)) as Observable<Page<T>>;
        if (!fetchList$) {
          throw new Error('Service or fetchList$ must by defined.');
        }

        return fetchList$.pipe(
          map((gPage) => {
            const p = { ...gPage, number: page };
            return createDataLoadSuccessTableAction(tableRepository)({
              gPage: p,
            });
          }),
          catchError((error) => {
            config.snackbar?.showError(error.message);
            return of(
              createDataLoadErrorTableAction(tableRepository)({ error })
            );
          })
        );
      })
    );
  });
}

export function createRemoveRangeTableEffect<T>(
  tableRepository: string,
  ngrxFeatureKey: string,
  actions$: Actions,
  store$: Store<any>,
  config: CreateGetPageActionConfig<T>
) {
  return createEffect(() => {
    if(!config?.service) {
      throw new Error('Service must be defined!');
    }

    const selectedItems = createSelector(
      createFeatureSelector<any>(ngrxFeatureKey),
      (state: any) => {
        return (state?.[tableRepository] as TableState<any>)?.selectedItems as any;
      }
    );

    return actions$.pipe(
      ofType(createRemoveRangeTableAction(tableRepository)),
      withLatestFrom(selectedItems),
      switchMap(([, {selectedItems}]) => {
        return config.service!.removeRange(selectedItems).pipe(
          map(() => { 
            return createGetPageTableAction(tableRepository)
            ({ page: 0, size: DEFAULT_SEARCH_LIMIT })
          }),
          catchError(error => { 
            return of(createDataLoadErrorTableAction(tableRepository)({ error }))
          })
        );
      })
    );
  });
}
