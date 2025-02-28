import { InjectionToken } from '@angular/core';

export const PAGE_HEADER_COMPONENT_TOKEN = new InjectionToken<PageHeaderCore>(
    'PageHeaderComponentToken'
);

export interface PageHeaderCore {
    title: string;
    subtitle: string | undefined;
    showBackButton: boolean;
}

export interface PageHeader {
    title: string;
    subtitle?: string;
    showBackButton?: boolean;
}