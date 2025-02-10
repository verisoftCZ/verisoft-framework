import { InjectionToken } from '@angular/core';
import { BaseFormCore } from '../base-form';

export const PASSWORD_COMPONENT_TOKEN = new InjectionToken<PasswordCore>(
    'PasswordComponentToken'
);

export interface PasswordCore extends BaseFormCore {
    toggleMask: boolean;
    feedback: boolean;
}