import { InjectionToken } from '@angular/core';
import { FieldSizeType, FieldTypeType } from '../../common';
import { BaseFormCore } from '../base-form';

export const TEXTFIELD_COMPONENT_TOKEN = new InjectionToken<TextfieldCore>(
    'TextfieldComponentToken'
);

export interface TextfieldCore extends BaseFormCore {
    size: FieldSizeType | undefined;
    type: FieldTypeType;
    floatLabel: boolean;
}