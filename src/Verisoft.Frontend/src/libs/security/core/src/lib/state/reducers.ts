import { createReducer, on } from '@ngrx/store';
import { setUser } from './actions';
import { initialState } from './state';

export const authReducer = createReducer(
  initialState,
  on(setUser, (state, action) => ({
    ...state,
    user: action.user
  }))
);