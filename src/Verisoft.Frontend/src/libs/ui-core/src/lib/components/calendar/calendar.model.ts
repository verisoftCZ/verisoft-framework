import { InjectionToken } from '@angular/core';
import { BaseFormCore } from '../base-form';

export const CALENDAR_COMPONENT_TOKEN = new InjectionToken<CalendarCore>(
    'CalendarComponentToken'
);

export interface CalendarCore extends BaseFormCore {
    maxDate: Date;
    icon: string;
    minDate: Date;
    header: string;
    footer: string;
    floatLabel: string | undefined;
    selectionMode: "single" | "multiple" | "range" | undefined;
}