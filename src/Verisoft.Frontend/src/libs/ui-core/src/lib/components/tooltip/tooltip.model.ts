import { InjectionToken, Signal } from '@angular/core';
import { ControlSeverityType, FieldSizeType, PositionType } from '../../common';

export const TOOLTIP_COMPONENT_TOKEN = new InjectionToken<TooltipCore>(
    'TooltipComponentToken'
);

export interface TooltipCore {
    color: Signal<ControlSeverityType | undefined>;
    position: Signal<PositionType | undefined>;
    size: Signal<FieldSizeType | undefined>;
    tooltipText: Signal<string | undefined>;
}