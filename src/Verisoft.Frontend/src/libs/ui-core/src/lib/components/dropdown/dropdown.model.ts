import { InjectionToken } from '@angular/core';
import { DataSourceComponentModel } from '../../common';
import { BaseFormCore } from '../base-form';

export const DROPDOWN_COMPONENT_TOKEN = new InjectionToken<DropdownCore<any>>(
    'DropdownComponentToken'
);

export interface DropdownCore<T> extends DataSourceComponentModel<T>, BaseFormCore {
    dropdownIcon?: string;
    floatLabel?: string;
    editable: boolean;
}
