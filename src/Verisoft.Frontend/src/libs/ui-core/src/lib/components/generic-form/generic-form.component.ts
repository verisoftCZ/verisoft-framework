import { ValidatorFn } from '@angular/forms';
import { FieldSizeType } from '../../common';

export interface GenericFieldDefinition {
  validator?: ValidatorFn[];
  type?: GenericFieldTypeType;
  label?: string;
  floatLabel?: boolean;
  name: string;
  optionLabel?: string;
  optionValue?: string;
  options?: unknown[];
  value?: unknown;
  testId?: string;
  size?: FieldSizeType;
}

export enum GenericFieldType {
  'dropdown' = 'dropdown',
  'checkbox' = 'checkbox',
  'calendar' = 'calendar',
  'multiselect' = 'multiselect',
  'text' = 'text',
}

export type GenericFieldTypeType = keyof typeof GenericFieldType;
