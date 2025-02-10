import { InjectionToken } from '@angular/core';
import { NgControl } from '@angular/forms';

export const FORM_FIELD_COMPONENT_TOKEN = new InjectionToken<FormFieldCore>(
    'FormFieldComponentToken'
);

export interface FormFieldCore {
    ngControl?: NgControl;
    label?: string;
    tooltip?: string;
    required: boolean;
    testId?: string;
    display: 'flex' | 'block';
}