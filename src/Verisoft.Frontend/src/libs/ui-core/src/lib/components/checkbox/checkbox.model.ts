import { InjectionToken } from '@angular/core';
import { BaseFormCore } from '../base-form';

export const CHECKBOX_COMPONENT_TOKEN = new InjectionToken<CheckboxCore>(
    'CheckboxComponentToken'
);

// eslint-disable-next-line @typescript-eslint/no-empty-interface
export interface CheckboxCore extends BaseFormCore {}
