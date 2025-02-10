import { InjectionToken } from '@angular/core';
import { BaseFormDirectiveCore } from '../base-form';

export const TABLE_FILTER_COMPONENT_TOKEN = new InjectionToken<TableFilterCore<any>>(
    'TableFilterComponentToken'
);

export interface FilterDefinition {
    optionLabel: string;
    label: string;
    optionValue?: string;
    url?: string;
    filterType?: 'dropdown' | 'checkbox' | 'calendar' | 'multiselect';
    initialValue?: any;
}

export interface TableFilterCore<T> extends BaseFormDirectiveCore<T> {
    filterDefinitions: FilterDefinition[];
    title: string;
    hideSearch?: boolean;
    debounceTime?: number;
}
