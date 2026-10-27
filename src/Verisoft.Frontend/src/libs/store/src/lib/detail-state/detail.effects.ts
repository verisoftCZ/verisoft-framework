import { Router } from '@angular/router';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { BaseHttpService } from '@verisoft/core';
import {
  catchError,
  EMPTY,
  map,
  Observable,
  of,
  switchMap,
  tap,
  withLatestFrom,
} from 'rxjs';
import {
  createDeleteDetailAction,
  createDeleteDetailFailureAction,
  createDeleteDetailSuccessAction,
  createInitDetailAction,
  createInitNewDetailAction,
  createLoadDetailFailureAction,
  createLoadDetailSuccessAction,
  createSaveDetailAction,
  createSaveDetailFailureAction,
  createSaveDetailSuccessAction,
} from './detail.actions';

export interface CreateInitFormConfig<T> {
  service?: BaseHttpService<T>;
  get?: (obj: any) => Observable<T>;
  snackbar?: any;
  errorMessage?: string;
  notFoundMessage?: string;
  successMessge?: string;
}

export interface DetailEffectConfig<T> {
  detail$: Observable<T | undefined>;
  snackbar?: any;
  router?: Router;
  redirectPath?: string;
  useItemId?: boolean;
  navigationMap?: Map<keyof T, string>;
}

export function createInitDetailEffect<T>(
  detailsRepository: string,
  actions$: Actions,
  config: CreateInitFormConfig<T>
) {
  return createEffect(() => {
    return actions$.pipe(
      ofType(createInitDetailAction(detailsRepository)),
      switchMap(({ obj }) => {
        const get$: Observable<any> = (config?.service?.get(obj as any) ??
          config?.get?.(obj)) as Observable<any>;
        if (!get$) return EMPTY;
        return get$?.pipe(
          map((item) => {
            return createLoadDetailSuccessAction(detailsRepository)({ item });
          }),
          catchError((error) => {
            if (error.status === 404) {
              config?.snackbar?.showError(config?.notFoundMessage ?? error);
            } else {
              config?.snackbar?.showError(config?.errorMessage ?? error);
            }

            console.error('Error', error);
            return of(
              createLoadDetailFailureAction(detailsRepository)({ error })
            );
          })
        );
      })
    );
  });
}

export function createInitNewDetailEffect(
  detailsRepository: string,
  actions$: Actions
) {
  return createEffect(() =>
    actions$.pipe(
      ofType(createInitNewDetailAction(detailsRepository)),
      map(() => {
        return createLoadDetailSuccessAction(detailsRepository)({ item: null });
      })
    )
  );
}

export function createSaveDetailEffect<
  T extends { id: number | string },
  SERVICE extends BaseHttpService<T>
>(
  detailsRepository: string,
  actions$: Actions,
  service: SERVICE,
  config: DetailEffectConfig<T>
) {
  return createEffect(() =>
    actions$.pipe(
      ofType(createSaveDetailAction(detailsRepository)),
      withLatestFrom(config.detail$),
      switchMap(([, savedItem]) => {
        const isNew = !savedItem?.id;
        const saveAction = isNew
          ? service.post(savedItem as T)
          : service.put(savedItem.id, savedItem);
        return saveAction.pipe(
          map((item) => {
            if (isNew && config?.router && config?.redirectPath) {
              const suffix = config?.useItemId ? '/' + item.id : '';
              config.router.navigateByUrl(config.redirectPath + suffix);
            }

            if (isNew && config?.router && config?.navigationMap) {
              const url = constructUrlFromNavigationMap(item, config.navigationMap)
              config.router.navigateByUrl(url);
            }

            return createSaveDetailSuccessAction(detailsRepository)({
              item: item,
            });
          }),
          catchError((error) => {
            console.error('Error', error);
            return of(
              createSaveDetailFailureAction(detailsRepository)({ error })
            );
          })
        );
      })
    )
  );
}

export function createSaveDetailSuccessEffect(
  detailsRepository: string,
  actions$: Actions,
  config?: CreateInitFormConfig<unknown>
) {
  return createEffect(
    () =>
      actions$.pipe(
        ofType(createSaveDetailSuccessAction(detailsRepository)),
        map((data: any) => {
          config?.snackbar?.showSuccess(
            config?.successMessge ?? `Item successfully saved.`
          );
          return createInitDetailAction(detailsRepository)({ obj: data.item.id });
        })
      )
  );
}

export function createSaveDetailFailureEffect(
  detailsRepository: string,
  actions$: Actions,
  config?: CreateInitFormConfig<unknown>
) {
  return createEffect(
    () =>
      actions$.pipe(
        ofType(createSaveDetailFailureAction(detailsRepository)),
        tap(({ error }) => {
          config?.snackbar?.showError(
            config.errorMessage ?? `Error saving item. ${error.message}`
          );
        })
      ),
    {
      dispatch: false,
    }
  );
}

export function createDeleteDetailEffect<
  T extends { id: number | string },
  SERVICE extends BaseHttpService<T>
>(
  detailsRepository: string,
  actions$: Actions,
  service: SERVICE,
  config: DetailEffectConfig<T>,
) {
  return createEffect(() =>
    actions$.pipe(
      ofType(createDeleteDetailAction(detailsRepository)),
      withLatestFrom(config.detail$),
      switchMap(([, savedItem]) => {
        const id = savedItem?.id;
        if (!id) {
          return of(
            createSaveDetailFailureAction(detailsRepository)({
              error: 'Item has no id',
            })
          );
        }

        return service.delete(id).pipe(
          map(() => {
            if (config?.router && config?.redirectPath) {
              config.router.navigateByUrl(`${config.redirectPath}`);
            }

            if (config?.router && config?.navigationMap) {
              const url = constructUrlFromNavigationMap(savedItem, config.navigationMap)
              config.router.navigateByUrl(url);
            }

            return createDeleteDetailSuccessAction(detailsRepository)();
          }),
          catchError((error) => {
            console.error('Error', error);
            config.snackbar?.showError('Failed to delete item.');
            return of(
              createDeleteDetailFailureAction(detailsRepository)({ error })
            );
          })
        );
      })
    )
  );
}

export function createDeleteDetailSuccessEffect(
  detailsRepository: string,
  actions$: Actions,
  config?: CreateInitFormConfig<unknown>
) {
  return createEffect(
    () =>
      actions$.pipe(
        ofType(createDeleteDetailSuccessAction(detailsRepository)),
        tap(() => {
          config?.snackbar?.showSuccess(
            config.successMessge ?? `Item successfully deleted.`
          );
        })
      ),
    {
      dispatch: false,
    }
  );
}

export function createDeleteDetailFailureEffect(
  detailsRepository: string,
  actions$: Actions,
  config?: CreateInitFormConfig<unknown>
) {
  return createEffect(
    () =>
      actions$.pipe(
        ofType(createDeleteDetailFailureAction(detailsRepository)),
        tap(({ error }) => {
          config?.snackbar?.showError(
            config.errorMessage ?? `Error deleting item. ${error.message}`
          );
        })
      ),
    {
      dispatch: false,
    }
  );
}

function constructUrlFromNavigationMap<T extends object>(
  item: T,
  navigationMap: Map<keyof T, string>)
  : string {
  let url = '';
  navigationMap.forEach((urlPartValue, suffixPropertyKey) => {
    const suffix = item[suffixPropertyKey];
    if (suffix) {
      url += `${urlPartValue}/${suffix}`;
    }
  });

  return url;
}
