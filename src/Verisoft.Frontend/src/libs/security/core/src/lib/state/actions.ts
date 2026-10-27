import { createAction, props } from '@ngrx/store';
import { AuthenticatedUser } from '../models';

export const setUser = createAction(
  '[Auth] Set User',
  props<{ user: AuthenticatedUser | undefined }>()
);
