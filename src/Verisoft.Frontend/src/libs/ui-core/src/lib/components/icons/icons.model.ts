import { InjectionToken } from "@angular/core";
import { CommonIcons } from "../../common/icons"

export const ICONS_COMPONENT_TOKEN = new InjectionToken<IconsCore>(
    'IconsComponentToken'
);

export interface IconsCore extends CommonIcons {
    name: string;
}