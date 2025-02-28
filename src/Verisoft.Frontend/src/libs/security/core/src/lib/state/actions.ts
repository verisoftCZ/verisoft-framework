import { createAction, props } from '@ngrx/store';
import { AuthenticatedUser } from '../models';

// sets or unsets the user
export const setUser = createAction(
  '[Auth] Set User',
  props<{ user: AuthenticatedUser | undefined }>()
);