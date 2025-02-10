import { EventEmitter, InjectionToken, TemplateRef } from "@angular/core";
import { FieldSizeType, LayoutTypeType } from "../../common";

export const STEPPER_COMPONENT_TOKEN = new InjectionToken<StepperCore>(
    'StepperComponentToken'
);

export interface StepperItem {
    header: string;
    template?: TemplateRef<any>;
    annotation?: string;
    prefix?: string;
}

export interface StepperCore {
    items: StepperItem[]
    layout: LayoutTypeType
    activeIndexChange: EventEmitter<number>
    size: FieldSizeType | undefined;
    icon: string;
    prefix: string;
    annotation: string;
    label: string;
}