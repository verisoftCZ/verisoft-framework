import { InjectionToken, InputSignal } from "@angular/core";
import { ControlSeverityType, FieldSizeType } from "../../common";

export const ICONS_COMPONENT_TOKEN = new InjectionToken<IconsCore>(
    'IconsComponentToken'
);

export type IconLibraryType = 'native' | 'bootstrap';

export interface IconsCore {
    library: InputSignal<IconLibraryType>;
    name: InputSignal<string>;
    size: InputSignal<FieldSizeType | undefined>;
    severity: InputSignal<ControlSeverityType | undefined>;
    class: InputSignal<string | undefined>;
}