import {
  UntypedFormControl,
  UntypedFormGroup,
} from '@angular/forms';
import { GenericFieldDefinition } from '@verisoft/ui-core';

export function generateFormGroup(
  fields: GenericFieldDefinition[] | undefined,
  lastGroupValue: UntypedFormGroup | undefined,
  inputGroup: UntypedFormGroup | undefined,
  inputGroupChanged = false
): UntypedFormGroup {
  const formGroup =
    (inputGroupChanged
      ? inputGroup ?? lastGroupValue
      : lastGroupValue ?? inputGroup) ?? new UntypedFormGroup({});

  fields?.forEach((field) => {
    const control = formGroup.get(field.name);
    if (!control) {
      formGroup.addControl(
        field.name,
        new UntypedFormControl(field.value, field.validator)
      );
    } else {
      control.setValidators(field.validator ?? null);
    }
  });
  if (!inputGroupChanged) {
    for (const field in formGroup.controls) {
      const control = fields?.find((x) => x.name == field);
      if (!control) {
        formGroup.removeControl(field);
      }
    }
  }

  return formGroup;
}

export function getColumnClass(value?: number): string | undefined {
  if (!value) {
    return undefined;
  }

  switch (value) {
    case 1:
      return 'col-12';
    case 2:
      return 'col-12 col-md-6';
    case 3:
      return 'col-12 col-md-4';
    case 4:
      return 'col-12 col-md-3';
    case 6:
      return 'col-12 col-md-2';
  }
  
  return 'col-12 col-md-1';
}
