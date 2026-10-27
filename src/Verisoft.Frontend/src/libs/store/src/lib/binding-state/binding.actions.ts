import { createAction, props } from '@ngrx/store';

export function createAddBindingAction<TCreate>(bindingsRepository: string) {
  return createAction(
    `[${bindingsRepository}/API] Add bindings`,
    props<{ binding: TCreate }>()
  );
}

export function createAddBindingsAction<TCreate>(bindingsRepository: string) {
  return createAction(
    `[${bindingsRepository}/API] Add bindings`,
    props<{ bindings: TCreate[] }>()
  );
}

export function createAddBindingsFailureAction(bindingsRepository: string) {
  return createAction(
    `[${bindingsRepository}/API] Add bindings failure`,
    props<{ error: any }>()
  );
}

export function createAddBindingsSuccessAction(bindingsRepository: string) {
  return createAction(
    `[${bindingsRepository}/API] Add bindings success`
  );
}

export function createEditBindingAction<TEdit>(bindingsRepository: string) {
  return createAction(
    `[${bindingsRepository}/API] Edit binding`,
    props<{ binding: TEdit }>()
  );
}

export function createEditBindingFailureAction(bindingsRepository: string) {
  return createAction(
    `[${bindingsRepository}/API] Edit binding failure`,
    props<{ error: any }>()
  );
}

export function createEditBindingSuccessAction(bindingsRepository: string) {
  return createAction(
    `[${bindingsRepository}/API] Edit binding success`
  );
}

export function createDeleteBindingAction(bindingsRepository: string) {
  return createAction(
    `[${bindingsRepository}/API] Delete binding`,
    props<{ id: number | string }>()
  );
}

export function createDeleteBindingsAction(bindingsRepository: string) {
  return createAction(
    `[${bindingsRepository}/API] Delete bindings`,
    props<{ bindingIds: number[] }>()
  );
}

export function createDeleteBindingsFailureAction(bindingsRepository: string) {
  return createAction(
    `[${bindingsRepository}/API] Delete bindings failure`,
    props<{ error: any }>()
  );
}

export function createDeleteBindingsSuccessAction(bindingsRepository: string) {
  return createAction(
    `[${bindingsRepository}/API] Delete bindings success`
  );
}

