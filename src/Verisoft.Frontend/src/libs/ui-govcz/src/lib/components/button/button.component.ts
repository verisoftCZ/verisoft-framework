import {
    ChangeDetectionStrategy,
    Component,
    EventEmitter,
    Input,
    Output,
} from "@angular/core";
import { NgTemplateOutlet } from "@angular/common";
import { Params, RouterModule } from "@angular/router";
import { GovDesignSystemModule } from "@gov-design-system-ce/angular";
import { ButtonEvent, GovButtonCustomEvent } from "@gov-design-system-ce/components";
import { TranslateModule } from "@ngx-translate/core";
import {
    BUTTON_COMPONENT_TOKEN,
    ButtonCore,
    IconPositionType,
    IconPosition,
    FieldSize,
    FieldSizeType,
    ControlSeverity,
    ControlSeverityType,
    GovButtonType,
    GovButtonTypeType,
    IconLibraryType,
    PositionType,
} from "@verisoft/ui-core";
import { GovColorPipe, GovSizePipe } from "../../pipes";
import { IconComponent } from "../icon";

@Component({
    selector: "v-button",
    styleUrl: "./button.component.scss",
    templateUrl: './button.component.html',
    imports: [
        GovDesignSystemModule, RouterModule, GovSizePipe, GovColorPipe, TranslateModule, IconComponent, NgTemplateOutlet
    ],
    changeDetection: ChangeDetectionStrategy.OnPush,
    host: {
        '[class.v-button--expanded]': 'expanded'
    },
    providers: [
        {
            provide: BUTTON_COMPONENT_TOKEN,
            useExisting: ButtonComponent,
        }
    ]
})
export class ButtonComponent
    implements ButtonCore
{
    @Input() label: string | undefined;
    @Input() icon: string | undefined;
    @Input() library: IconLibraryType = 'native';
    @Input() badge!: string;
    @Input() iconPos: IconPositionType = IconPosition.left;
    @Input() disabled = false;
    @Input() rounded!: boolean;
    @Input() outlined!: boolean;
    @Input() raised!: boolean;
    @Input() routerLink!: any[];
    @Input() size: FieldSizeType | undefined = FieldSize.medium;
    @Input() queryParams!: Params;
    @Input() severity: ControlSeverityType | undefined = ControlSeverity.primary;
    @Input() type: GovButtonTypeType = GovButtonType.solid;
    @Input() expanded = false;
    @Input() name!: string;
    @Input() tooltip?: string;
    @Input() tooltipPosition?: PositionType;

    // eslint-disable-next-line @angular-eslint/no-output-native
    @Output() click = new EventEmitter<any>();

    handleClick(event: GovButtonCustomEvent<ButtonEvent> | MouseEvent) {
        if (this.disabled) {
            event.stopPropagation();
            return;
        }

        this.click.emit(event);
    }
}
