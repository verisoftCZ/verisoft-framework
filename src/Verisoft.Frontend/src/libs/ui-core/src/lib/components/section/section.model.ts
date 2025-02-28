import { InjectionToken } from '@angular/core';

export const SECTION_COMPONENT_TOKEN = new InjectionToken<SectionCore>(
    'SectionComponentToken'
);

export interface SectionCore {
    title: string;
    showContent: boolean;
    backgroundColor: string;
}