import { InjectionToken } from '@angular/core';
import { BaseFormCore } from '../base-form';

export const RADIOBUTTON_COMPONENT_TOKEN = new InjectionToken<RadiobuttonCore<any>>(
    'RadiobuttonComponentToken'
);

export interface RadioButtonItem<T> {
    id: string;
    value: T; 
}

export interface RadiobuttonCore<T> extends BaseFormCore {
    radioGroupName: string;
    items: RadioButtonItem<T>[];
}