import { InjectionToken, InputSignal } from "@angular/core";
import { MenuKeyboardNavigationInput } from "../../accessibility";
import { ControlSeverityType, FieldSizeType } from "../../common";
import { MenuItem } from "../side-menu";

export const DROPDOWN_BUTTON_COMPONENT_TOKEN = new InjectionToken<DropdownButtonCore>(
    'DropdownButtonComponentToken'
);

export interface DropdownButtonCore {
    label: InputSignal<string | undefined>;
    icon: InputSignal<string | undefined>;
    rounded: InputSignal<boolean | undefined>;
    outlined: InputSignal<boolean | undefined>;
    raised: InputSignal<boolean | undefined>;
    severity: InputSignal<ControlSeverityType | undefined>;
    size: InputSignal<FieldSizeType | undefined>;
    items: InputSignal<MenuItem[] | undefined>;
    keyboardNavigation?: InputSignal<MenuKeyboardNavigationInput>;
}