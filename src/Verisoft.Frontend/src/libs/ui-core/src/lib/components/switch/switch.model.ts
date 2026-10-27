import { InjectionToken } from "@angular/core";
import { BaseFormCore } from "../base-form";

export const SWITCH_COMPONENT_TOKEN = new InjectionToken<SwitchCore>(
    'SwitchComponentToken'
);

export type SwitchCore = BaseFormCore
