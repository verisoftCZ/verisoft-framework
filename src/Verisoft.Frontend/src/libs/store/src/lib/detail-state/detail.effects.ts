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
  createInitDetailAction,
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
  successMessge?: string;
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
          if(!get$) return EMPTY;
        return get$?.pipe(
          map((item) => {
            return createLoadDetailSuccessAction(detailsRepository)({ item });
          }),
          catchError((error) => {
            console.error('Error', error);
            config?.snackbar?.showError(config?.errorMessage ?? error);
            return of(
              createLoadDetailFailureAction(detailsRepository)({ error })
            );
          })
        );
      })
    );
  });
}

  export function createSaveDetailEffect<
    T extends { id: number },
    SERVICE extends BaseHttpService<T>
  >(
    detailsRepository: string,
    actions$: Actions,
    service: SERVICE,
    detail$: Observable<T | undefined>,
  ) {
    return createEffect(() =>
      actions$.pipe(
        ofType(createSaveDetailAction(detailsRepository)),
        withLatestFrom(detail$),
        switchMap(([, savedItem]) => {
          const isNew = !savedItem?.id;
          const saveAction = isNew
            ? service.post(savedItem as T)
            : service.put(savedItem.id, savedItem);
          return saveAction.pipe(
            map((item) => {
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
  config?: CreateInitFormConfig<unknown>,
) {
  return createEffect(
    () =>
      actions$.pipe(
        ofType(createSaveDetailSuccessAction(detailsRepository)),
        tap(() => {
          config?.snackbar?.showSuccess(config?.successMessge ?? `Item successfully saved.`);
        })
      ),
    {
      dispatch: false,
    }
  );
}

  export function createSaveDetailFailureEffect(
    detailsRepository: string,
    actions$: Actions,
    config?: CreateInitFormConfig<unknown>,
  ) {
    return createEffect(
      () =>
        actions$.pipe(
          ofType(createSaveDetailFailureAction(detailsRepository)),
          tap(({error}) => {
            config?.snackbar?.showError(config.errorMessage ?? `Error saving item. ${error.message}`);
          })
        ),
      {
        dispatch: false,
      }
    );
}
