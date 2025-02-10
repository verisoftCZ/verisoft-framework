import { InjectionToken } from '@angular/core';
import { BaseFormCore } from '../base-form';

export const TEXTAREA_COMPONENT_TOKEN = new InjectionToken<TextareaCore>(
    'TextareaComponentToken'
);

export interface TextareaCore extends BaseFormCore {
    rows: number;
    cols: number;
    autoResize: boolean;
    floatLabel: string | undefined;
}
