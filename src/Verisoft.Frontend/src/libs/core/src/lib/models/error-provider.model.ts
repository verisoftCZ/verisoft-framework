import { InjectionToken } from '@angular/core';
import { ValidationErrors } from '@angular/forms';
import { Observable } from 'rxjs';

export interface ErrorProvider {
  mapError(errors: ValidationErrors): Observable<string | undefined>;
  errors: Record<string, (value?: any) => string>;
}

export const ERROR_PROVIDER_TOKEN
  = new InjectionToken<ErrorProvider>('ERROR_PROVIDER_TOKEN');

export enum CustomValidationCodes {}

export const ErrorMap: Record<string, (value?: any) => string> = {
  required: () => `VALIDATIONS.REQUIRED`,
  email: () => `VALIDATIONS.EMAIL`,
  minLength: () => 'VALIDATIONS.MIN_LENGTH',
  maxLength: () => 'VALIDATIONS.MAX_LENGTH',
};
