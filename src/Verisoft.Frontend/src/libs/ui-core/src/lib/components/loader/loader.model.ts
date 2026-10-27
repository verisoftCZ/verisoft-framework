import { InjectionToken } from '@angular/core';

export const LOADER_COMPONENT_TOKEN = new InjectionToken<LoaderCore>(
    'LoaderComponentToken'
);

export type LoaderCore = object
