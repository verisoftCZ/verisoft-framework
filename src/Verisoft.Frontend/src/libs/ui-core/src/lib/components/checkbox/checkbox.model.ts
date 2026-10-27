import { InjectionToken, InputSignal } from '@angular/core';
import { BaseFormCore } from '../base-form';

export const CHECKBOX_COMPONENT_TOKEN = new InjectionToken<CheckboxCore>(
    'CheckboxComponentToken'
);

export interface CheckboxCore extends BaseFormCore {
    indeterminate: InputSignal<boolean>;
}
