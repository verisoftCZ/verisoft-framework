export interface BaseFormCore {
  label?: string;
  required: boolean;
  readonly: boolean;
  tooltip: string;
  formDisplay: 'flex' | 'block';
  clearable: boolean;
  placeholder?: string
  testId?: string;
}

export interface BaseFormDirectiveCore<T> {
  initialData: T | any;
}

export interface FormState {
  dirty: boolean;
  valid: boolean;
}

export function isFormStateEqual(current: FormState, other: FormState) {
  if (!current && !other) {
    return true;
  }

  if (current && other) {
    return current.dirty === other.dirty && current.valid === other.valid;
  }

  return false;
}
