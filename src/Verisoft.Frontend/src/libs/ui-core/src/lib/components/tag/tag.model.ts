import { InjectionToken, InputSignal } from "@angular/core";
import { ControlSeverityType, FieldSizeType } from "../../common";

export const TAG_COMPONENT_TOKEN = new InjectionToken<TagCore>('TagComponentToken');

export type TagVariant = 'bold' | 'subtle';

export interface TagCore {
    label: InputSignal<string | undefined>;
    severity: InputSignal<ControlSeverityType | undefined>;
    variant: InputSignal<TagVariant>;
    size: InputSignal<FieldSizeType>;
  }