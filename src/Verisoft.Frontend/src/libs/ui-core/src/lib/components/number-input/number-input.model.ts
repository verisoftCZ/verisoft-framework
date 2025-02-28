import { InjectionToken } from '@angular/core';
import { BaseFormCore } from '../base-form';

export const NUMBER_INPUT_COMPONENT_TOKEN = new InjectionToken<NumberInputCore>(
    'NumberInputComponentToken'
);

export interface NumberInputCore extends BaseFormCore {
    mode: string;
    currency: string;
    min: number;
    max: number;
    step: number;
}