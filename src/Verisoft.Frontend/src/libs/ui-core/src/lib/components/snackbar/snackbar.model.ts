import { InjectionToken } from '@angular/core';

export const SNACKBAR_COMPONENT_TOKEN = new InjectionToken<SnackbarCore>(
    'SnackbarComponentToken'
);

// eslint-disable-next-line @typescript-eslint/no-empty-interface
export interface SnackbarCore {}