import { InjectionToken } from '@angular/core';
import { ActivatedRoute } from "@angular/router";
import { MenuItem } from '../side-menu';

export const BREADCRUMB_COMPONENT_TOKEN = new InjectionToken<BreadcrumbCore>(
    'BreadcrumbComponentToken'
);

export interface BreadcrumbCore {
    items: MenuItem[];
    homeRoute: string;
    useHomeRoute: boolean;
}

export interface Breadcrumb {
    label: string;
    routerLink?: string;
    url?: string;
    active?: boolean;
    activatedRoute?: ActivatedRoute;
}
