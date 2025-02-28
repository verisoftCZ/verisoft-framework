import { Component, Input } from "@angular/core";
import { ICONS_COMPONENT_TOKEN } from "./icons.model";

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

export class IconsComponent{
    @Input() name!: string;
}