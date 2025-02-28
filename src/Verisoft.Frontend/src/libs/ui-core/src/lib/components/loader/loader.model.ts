import { InjectionToken } from '@angular/core';

export const LOADER_COMPONENT_TOKEN = new InjectionToken<LoaderCore>(
    'LoaderComponentToken'
);

// eslint-disable-next-line @typescript-eslint/no-empty-interface
export interface LoaderCore {}
