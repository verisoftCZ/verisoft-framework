import { InjectionToken } from '@angular/core';
import { BaseFormCore } from '../base-form';

export const PASSWORD_COMPONENT_TOKEN = new InjectionToken<PasswordCore>(
    'PasswordComponentToken'
);

export enum PasswordStrength {
    None = 0,
    Low = 1,
    Medium = 2,
    High = 3,
    Superb = 4
}

export interface PasswordCore extends BaseFormCore {
    toggleMask: boolean;
    feedback: boolean;
}

export interface PasswordValidationItem {
    severity: 'success' | 'error';
    validationLabel: string;
    validationFn: (x: string) => boolean;
}