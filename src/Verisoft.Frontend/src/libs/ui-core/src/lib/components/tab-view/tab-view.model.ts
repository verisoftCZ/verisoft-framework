import { InjectionToken, TemplateRef } from "@angular/core";

export const TAB_VIEW_COMPONENT_TOKEN = new InjectionToken<TabViewCore>(
    'TabVIewComponentToken'
);

export interface TabViewItemCore {
    title: string;
    url?: string;
    icon?: string;
    disabled?: boolean;
    content?: string;
    contentTemplate?: TemplateRef<unknown>
}

export interface TabViewCore {
    items: TabViewItemCore[];
    useRouting: boolean;
    activeIndex: number;
}