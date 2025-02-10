import { InjectionToken } from "@angular/core";
import { BaseFormCore } from "../base-form";

export const SWITCH_COMPONENT_TOKEN = new InjectionToken<SwitchCore>(
    'SwitchComponentToken'
);

// eslint-disable-next-line @typescript-eslint/no-empty-interface
export interface SwitchCore extends BaseFormCore {}
