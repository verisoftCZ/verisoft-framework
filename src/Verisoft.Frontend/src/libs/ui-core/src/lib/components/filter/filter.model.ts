import { InjectionToken } from '@angular/core';
import { ControlValueAccessor } from '@angular/forms';
import { GenericFieldDefinition } from '../generic-form';

export const FILTER_COMPONENT_TOKEN = new InjectionToken<FilterCore>(
    'FilterComponentToken'
);

export interface FilterCore extends ControlValueAccessor {
    fields: GenericFieldDefinition[];
    title?: string;
    fulltextFieldName: string;
    showFulltext: boolean;
    showFilters: boolean;
    autoBind: boolean;
    debounceTime?: number;
    filterDetailTitle?: string;
    filterDetailShowFulltext: boolean;
    applyButtonLabel?: string;
    clearButtonLabel?: string;
}

import { FormGroup } from '@angular/forms';

export function getFilledControlCount(formGroup: FormGroup): number {
  let count = 0;
  Object.keys(formGroup.controls).forEach((key) => {
    const control = formGroup.get(key);
    if (control?.value) {
      if (typeof control.value === 'boolean') {
        count++;
      }
      if (typeof control.value === 'string' && control.value.trim() !== '') {
        count++;
      }
    }
  });

  return count;
}