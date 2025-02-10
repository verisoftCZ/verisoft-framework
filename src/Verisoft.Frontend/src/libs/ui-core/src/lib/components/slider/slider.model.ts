import { InjectionToken } from '@angular/core';
import { BaseFormCore } from '../base-form';

export const SLIDER_COMPONENT_TOKEN = new InjectionToken<SliderCore>(
    'SliderComponentToken'
);

export interface SliderCore extends BaseFormCore {
    step: number;
    min: number;
    max: number;
    range: boolean;
}