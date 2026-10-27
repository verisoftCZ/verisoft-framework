import { Actions, createEffect, ofType } from '@ngrx/effects';
import {
  catchError,
  map,
  Observable,
  of,
  switchMap,
  tap,
  withLatestFrom,
} from 'rxjs';
import {
  createAddBindingAction,
  createAddBindingsAction,
  createAddBindingsFailureAction,
  createAddBindingsSuccessAction,
  createDeleteBindingAction,
  createDeleteBindingsAction,
  createDeleteBindingsFailureAction,
  createDeleteBindingsSuccessAction,
  createEditBindingAction,
  createEditBindingFailureAction,
  createEditBindingSuccessAction,
} from './binding.actions';

export function createSingleBindingEffectsWithEdit<
  T extends { id: number | string },
  TCreate,
  TEdit extends { id?: number | string }
>(
  bindingsRepository: string,
  actions$: Actions,
  detail$: Observable<T | undefined>,
  bindingSingularName: string,
  bindingPluralName: string,
  addFunction: (
    bindings: TCreate,
  ) => Observable<any>,
  editFunction: (binding: TEdit, detailId: number | string) => Observable<any>,
  deleteFunction: (
    bindingIds: string | number,
    detailId: number | string
  ) => Observable<any>,
  snackbar: any,
  translateService: any,
  tableService: any,
  overrideDetailWithBinding = false,
) {
  const addEffect = createAddBindingEffect(
    bindingsRepository,
    actions$,
    detail$,
    bindingPluralName,
    addFunction,
    snackbar,
    translateService,
  );

  const editEffect = createEditBindingEffect(
    bindingsRepository,
    actions$,
    detail$,
    bindingSingularName,
    editFunction,
    snackbar,
    translateService,
    overrideDetailWithBinding
  );

  const deleteEffect = createDeleteBindingEffect(
    bindingsRepository,
    actions$,
    detail$,
    bindingPluralName,
    deleteFunction,
    snackbar,
    translateService
  );

  const modifiedSuccessEffect = createBindingModifiedSuccessEffect(
    bindingsRepository,
    actions$,
    tableService
  );

  return { addEffect, editEffect, deleteEffect, modifiedSuccessEffect };
}

export function createBindingsEffectsWithEdit<
  T extends { id: number | string },
  TCreate,
  TEdit extends { id?: number | string }
>(
  bindingsRepository: string,
  actions$: Actions,
  detail$: Observable<T | undefined>,
  bindingSingularName: string,
  bindingPluralName: string,
  addFunction: (
    bindings: TCreate[],
    detailId: number | string
  ) => Observable<any>,
  editFunction: (binding: TEdit, detailId: number | string) => Observable<any>,
  deleteFunction: (
    bindingIds: number[],
    detailId: number | string
  ) => Observable<any>,
  snackbar: any,
  translateService: any,
  tableService: any,
  overrideDetailWithBinding = false,
) {
  const addEffect = createAddBindingsEffect(
    bindingsRepository,
    actions$,
    detail$,
    bindingPluralName,
    addFunction,
    snackbar,
    translateService
  );

  const editEffect = createEditBindingEffect(
    bindingsRepository,
    actions$,
    detail$,
    bindingSingularName,
    editFunction,
    snackbar,
    translateService,
    overrideDetailWithBinding
  );

  const deleteEffect = createDeleteBindingsEffect(
    bindingsRepository,
    actions$,
    detail$,
    bindingPluralName,
    deleteFunction,
    snackbar,
    translateService
  );

  const modifiedSuccessEffect = createBindingModifiedSuccessEffect(
    bindingsRepository,
    actions$,
    tableService
  );

  return { addEffect, editEffect, deleteEffect, modifiedSuccessEffect };
}

export function createBindingsEffects<
  T extends { id: number | string },
  TCreate
>(
  bindingsRepository: string,
  actions$: Actions,
  detail$: Observable<T | undefined>,
  bindingPluralName: string,
  addFunction: (
    bindings: TCreate[],
    detailId: number | string
  ) => Observable<any>,
  deleteFunction: (
    bindingIds: number[],
    detailId: number | string
  ) => Observable<any>,
  snackbar: any,
  translateService: any,
  tableService: any
) {
  const addEffect = createAddBindingsEffect(
    bindingsRepository,
    actions$,
    detail$,
    bindingPluralName,
    addFunction,
    snackbar,
    translateService
  );

  const deleteEffect = createDeleteBindingsEffect(
    bindingsRepository,
    actions$,
    detail$,
    bindingPluralName,
    deleteFunction,
    snackbar,
    translateService
  );

  const modifiedSuccessEffect = createBindingModifiedSuccessEffect(
    bindingsRepository,
    actions$,
    tableService
  );

  return { addEffect, deleteEffect, modifiedSuccessEffect };
}

function createAddBindingEffect<T extends { id: number | string }, TCreate>(
  bindingsRepository: string,
  actions$: Actions,
  detail$: Observable<T | undefined>,
  bindingPluralName: string,
  addFunction: (
    bindings: TCreate,
  ) => Observable<any>,
  snackbar: any,
  translateService: any,
) {
  return createEffect(() =>
    actions$.pipe(
      ofType(createAddBindingAction<TCreate>(bindingsRepository)),
      withLatestFrom(detail$),
      switchMap(([{ binding }, detail]) => {
        const translatedPluralName =
          translateService.instant(bindingPluralName);
        const successMessage = `${translateService.instant(
          'BINDINGS_EFFECTS.SUCCESS_ADD'
        )} ${translatedPluralName}!`;
        const failureMessage = `${translateService.instant(
          'BINDINGS_EFFECTS.FAILURE_ADD'
        )} ${translatedPluralName}!`;

        return addFunction(binding).pipe(
          map(() => {
            snackbar.showSuccess(successMessage);
            return createAddBindingsSuccessAction(bindingsRepository)();
          }),
          catchError((error) => {
            snackbar.showError(failureMessage);
            return of(
              createAddBindingsFailureAction(bindingsRepository)({ error })
            );
          })
        );
      })
    )
  );
}

function createAddBindingsEffect<T extends { id: number | string }, TCreate>(
  bindingsRepository: string,
  actions$: Actions,
  detail$: Observable<T | undefined>,
  bindingPluralName: string,
  addFunction: (
    bindings: TCreate[],
    detailId: number | string
  ) => Observable<any>,
  snackbar: any,
  translateService: any
) {
  return createEffect(() =>
    actions$.pipe(
      ofType(createAddBindingsAction<TCreate>(bindingsRepository)),
      withLatestFrom(detail$),
      switchMap(([{ bindings }, detail]) => {
        const translatedPluralName =
          translateService.instant(bindingPluralName);
        const successMessage = `${translateService.instant(
          'BINDINGS_EFFECTS.SUCCESS_ADD'
        )} ${translatedPluralName}!`;
        const failureMessage = `${translateService.instant(
          'BINDINGS_EFFECTS.FAILURE_ADD'
        )} ${translatedPluralName}!`;

        if (!detail?.id) {
          snackbar.showError(failureMessage);
          return of(
            createAddBindingsFailureAction(bindingsRepository)({
              error: failureMessage,
            })
          );
        }

        return addFunction(bindings, detail.id).pipe(
          map(() => {
            snackbar.showSuccess(successMessage);
            return createAddBindingsSuccessAction(bindingsRepository)();
          }),
          catchError((error) => {
            snackbar.showError(failureMessage);
            return of(
              createAddBindingsFailureAction(bindingsRepository)({ error })
            );
          })
        );
      })
    )
  );
}

function createEditBindingEffect<
  T extends { id: number | string },
  TEdit extends { id?: number | string }
>(
  bindingsRepository: string,
  actions$: Actions,
  detail$: Observable<T | undefined>,
  bindingSingularName: string,
  editFunction: (binding: TEdit, detailId: number | string) => Observable<any>,
  snackbar: any,
  translateService: any,
  overrideDetailWithBinding = false,
) {
  return createEffect(() =>
    actions$.pipe(
      ofType(createEditBindingAction<TEdit>(bindingsRepository)),
      withLatestFrom(detail$),
      switchMap(([{ binding }, detail]) => {
        const translatedPluralName =
          translateService.instant(bindingSingularName);
        const successMessage = `${translateService.instant(
          'BINDINGS_EFFECTS.SUCCESS_EDIT'
        )} ${translatedPluralName}!`;
        const failureMessage = `${translateService.instant(
          'BINDINGS_EFFECTS.FAILURE_EDIT'
        )} ${translatedPluralName}!`;

        const id = overrideDetailWithBinding ? binding?.id : detail?.id;
        if (!id) {
          snackbar.showError(failureMessage);
          return of(
            createEditBindingFailureAction(bindingsRepository)({
              error: failureMessage,
            })
          );
        }

        return editFunction(binding, id).pipe(
          map(() => {
            snackbar.showSuccess(successMessage);
            return createEditBindingSuccessAction(bindingsRepository)();
          }),
          catchError((error) => {
            snackbar.showError(failureMessage);
            return of(
              createEditBindingFailureAction(bindingsRepository)({ error })
            );
          })
        );
      })
    )
  );
}

function createDeleteBindingEffect<T extends { id: number | string }>(
  bindingsRepository: string,
  actions$: Actions,
  detail$: Observable<T | undefined>,
  bindingPluralName: string,
  deleteFunction: (
    bindingIds: string | number,
    detailId: string | number
  ) => Observable<any>,
  snackbar: any,
  translateService: any
) {
  return createEffect(() =>
    actions$.pipe(
      ofType(createDeleteBindingAction(bindingsRepository)),
      withLatestFrom(detail$),
      switchMap(([{ id }, detail]) => {
        const translatedPluralName =
          translateService.instant(bindingPluralName);
        const successMessage = `${translateService.instant(
          'BINDINGS_EFFECTS.SUCCESS_DELETE'
        )} ${translatedPluralName}!`;
        const failureMessage = `${translateService.instant(
          'BINDINGS_EFFECTS.FAILURE_DELETE'
        )} ${translatedPluralName}!`;

        if (!id && !detail?.id) {
          snackbar.showError(failureMessage);
          return of(
            createDeleteBindingsFailureAction(bindingsRepository)({
              error: failureMessage,
            })
          );
        }

        return deleteFunction(id, id ?? detail?.id).pipe(
          map(() => {
            snackbar.showSuccess(successMessage);
            return createDeleteBindingsSuccessAction(bindingsRepository)();
          }),
          catchError((error) => {
            snackbar.showError(failureMessage);
            return of(
              createDeleteBindingsFailureAction(bindingsRepository)({ error })
            );
          })
        );
      })
    )
  );
}

function createDeleteBindingsEffect<T extends { id: number | string }>(
  bindingsRepository: string,
  actions$: Actions,
  detail$: Observable<T | undefined>,
  bindingPluralName: string,
  deleteFunction: (
    bindingIds: number[],
    detailId: string | number
  ) => Observable<any>,
  snackbar: any,
  translateService: any
) {
  return createEffect(() =>
    actions$.pipe(
      ofType(createDeleteBindingsAction(bindingsRepository)),
      withLatestFrom(detail$),
      switchMap(([{ bindingIds }, detail]) => {
        const translatedPluralName =
          translateService.instant(bindingPluralName);
        const successMessage = `${translateService.instant(
          'BINDINGS_EFFECTS.SUCCESS_DELETE'
        )} ${translatedPluralName}!`;
        const failureMessage = `${translateService.instant(
          'BINDINGS_EFFECTS.FAILURE_DELETE'
        )} ${translatedPluralName}!`;

        if (!detail?.id) {
          snackbar.showError(failureMessage);
          return of(
            createDeleteBindingsFailureAction(bindingsRepository)({
              error: failureMessage,
            })
          );
        }

        return deleteFunction(bindingIds, detail.id).pipe(
          map(() => {
            snackbar.showSuccess(successMessage);
            return createDeleteBindingsSuccessAction(bindingsRepository)();
          }),
          catchError((error) => {
            snackbar.showError(failureMessage);
            return of(
              createDeleteBindingsFailureAction(bindingsRepository)({ error })
            );
          })
        );
      })
    )
  );
}

function createBindingModifiedSuccessEffect(
  bindingsRepository: string,
  actions$: Actions,
  tableService: any
) {
  return createEffect(
    () =>
      actions$.pipe(
        ofType(
          createAddBindingsSuccessAction(bindingsRepository),
          createEditBindingSuccessAction(bindingsRepository),
          createDeleteBindingsSuccessAction(bindingsRepository)
        ),
        tap(() => {
          tableService.forceReload();
        })
      ),
    { dispatch: false }
  );
}
