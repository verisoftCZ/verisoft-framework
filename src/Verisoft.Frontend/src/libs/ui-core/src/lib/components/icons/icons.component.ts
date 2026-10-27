import { Component, input } from "@angular/core";
import { ControlSeverityType, FieldSizeType } from "../../common";
import { IconLibraryType, ICONS_COMPONENT_TOKEN, IconsCore } from "./icons.model";

@Component({
    standalone: true,
    template: '',
    providers: [
        {
            provide: ICONS_COMPONENT_TOKEN,
            useExisting: IconsComponent
        }
    ]
})

export abstract class IconsComponent implements IconsCore {
    library = input<IconLibraryType>('native');
    name = input.required<string>();
    size = input<FieldSizeType | undefined>();
    severity = input<ControlSeverityType | undefined>();
    class = input<string>();
}