import { InjectionToken } from '@angular/core';

export const SNACKBAR_COMPONENT_TOKEN = new InjectionToken<SnackbarCore>(
    'SnackbarComponentToken'
);

export type SnackbarCore = object
