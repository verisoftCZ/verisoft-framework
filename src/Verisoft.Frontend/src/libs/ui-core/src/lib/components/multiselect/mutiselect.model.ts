import { InjectionToken } from '@angular/core';
import { DataSourceComponentModel } from '../../common';
import { BaseFormCore } from '../base-form';

export const MULTISELECT_COMPONENT_TOKEN = new InjectionToken<MultiselectCore<any>>(
    'MultiselectComponentToken'
);

export interface MultiselectCore<T> extends DataSourceComponentModel<T>, BaseFormCore {
    dropdownIcon?: string;
    floatLabel?: string;
    editable: boolean;
}