import { InjectionToken } from '@angular/core';
import { IconPositionType } from '../../common';
import { BaseFormCore } from '../base-form';

export const INPUT_GROUP_COMPONENT_TOKEN = new InjectionToken<InputGroupCore>(
    'HeaderComponentToken'
);

export interface InputGroupItem {
    icon?: string;
    text?: string;
    position: IconPositionType;
}

export interface InputGroupCore extends BaseFormCore {
    items: InputGroupItem[];
}