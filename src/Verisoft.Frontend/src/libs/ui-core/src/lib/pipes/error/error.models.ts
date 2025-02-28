import { AbstractControl, ValidationErrors } from '@angular/forms';

type ErrorValue = { key: string; value: string } | null;

export function getFirstErrorFromControl<T extends AbstractControl>(
  control: T
): ErrorValue {
  if (!control || !control.errors) {
    return null;
  }
  const errors = control.errors ?? false;
  if (errors) {
    const key = Object.keys(control.errors)[0];
    const value = control.errors[key];
    return { key, value };
  }
  return null;
}

export function getFirstError(errors: ValidationErrors): ErrorValue {
  if (errors) {
    const key = Object.keys(errors)[0];
    const value = errors[key];
    return { key, value };
  }
  return null;
}
