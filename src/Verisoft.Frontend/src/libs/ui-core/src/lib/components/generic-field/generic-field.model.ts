import { InjectionToken } from '@angular/core';
import { DataSourceComponentModel } from '../../common';
import { BaseFormCore } from '../base-form';

export const GENERIC_FIELD_COMPONENT_TOKEN = new InjectionToken<GenericFieldCore<any>>(
    'GenericFieldComponentToken'
);

export interface GenericFieldCore<T> extends DataSourceComponentModel<T>, BaseFormCore {
}
