import { createSelector } from '@ngrx/store';
import { SecurityFeature } from './feature';

export const selectIsAuthenticated = createSelector(
  SecurityFeature.selectUser,
  (user) => !!user
);

export const selectUser = SecurityFeature.selectUser;