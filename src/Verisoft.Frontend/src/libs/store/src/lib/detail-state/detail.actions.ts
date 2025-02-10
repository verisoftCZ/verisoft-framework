import { createAction, props } from '@ngrx/store';
import { FormState } from './detail.models';

export function createInitDetailAction(detailsRepository: string) {
  return createAction(
    `[${detailsRepository} Page] Init`,
    props<{ obj: string | null | number | undefined | any }>()
  );
}

export function createLoadDetailSuccessAction<T>(detailsRepository: string) {
  return createAction(
    `[${detailsRepository}/API] Load ${detailsRepository} Success`,
    props<{ item: T }>()
  );
}

export function createLoadDetailFailureAction(detailsRepository: string) {
  return createAction(
    `[${detailsRepository}/API] Load ${detailsRepository} Failure`,
    props<{ error: any }>()
  );
}

export function createUpdateDetailAction<T>(detailsRepository: string) {
  return createAction(
    `[${detailsRepository}/API] Update ${detailsRepository}`,
    props<{ item: T }>()
  );
}

export function createUpdateFormStateAction(detailsRepository: string) {
  return createAction(
    `[${detailsRepository}/API] Update Form State`,
    props<{ formState: FormState }>()
  );
}

export function createSaveDetailAction(detailsRepository: string) {
  return createAction(`[${detailsRepository}/API] Save Detail `);
}

export function createSaveDetailSuccessAction<T>(detailsRepository: string) {
  return createAction(
    `[${detailsRepository}/API] Save Detail Success`,
    props<{ item: T }>()
  );
}

export function createSaveDetailFailureAction(detailsRepository: string) {
  return createAction(
    `[${detailsRepository}/API] Save Detail Failure`,
    props<{ error: any }>()
  );
}

export function createUpdateSaveDetailAction(detailsRepository: string) {
  return createAction(`[${detailsRepository}/API] Update Detail `);
}

export function createResetStateAction(detailsRepository: string) {
  return createAction(`[${detailsRepository}/API] Reset State`);
}
