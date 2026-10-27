import { ValidatorFn } from '@angular/forms';
import { DatasourceType } from '@verisoft/core';
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
  readonly?: boolean;
  datasource?: DatasourceType<any>;
  filterField?: string;
  showFilter?: boolean;
  localSearch?: boolean;
}

export enum GenericFieldType {
  'dropdown' = 'dropdown',
  'checkbox' = 'checkbox',
  'simplecheckbox' = 'simplecheckbox',
  'calendar' = 'calendar',
  'multiselect' = 'multiselect',
  'text' = 'text',
}

export type GenericFieldTypeType = keyof typeof GenericFieldType;
