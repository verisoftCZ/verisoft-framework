import { InjectionToken } from '@angular/core';
import { Params } from '@angular/router';
import { ControlSeverityType, FieldSizeType, IconPositionType } from '../../common';

export const BUTTON_COMPONENT_TOKEN = new InjectionToken<ButtonCore>(
    'ButtonComponentToken'
);

export interface ButtonCore {
    label?: string;
    icon?: string;
    badge?: string;
    iconPos: IconPositionType;
    rounded: boolean;
    outlined: boolean;
    raised: boolean;
    severity?: ControlSeverityType;
    routerLink: any[];
    size: FieldSizeType | undefined;
    queryParams?: Params;
}
