import { ChangeDetectionStrategy, Component, EventEmitter, Input, Output } from "@angular/core";
import { Params, RouterModule } from "@angular/router";
import { GovDesignSystemModule } from "@gov-design-system-ce/angular";
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
    GovButtonTypeType
} from "@verisoft/ui-core";
import { GovColorPipe, GovSizePipe } from "../../pipes";

@Component({
    selector: "v-button",
    standalone: true,
    styleUrls: [
        "./button.component.scss"
    ],
    templateUrl: './button.component.html',
    imports: [
        GovDesignSystemModule, RouterModule, GovSizePipe, GovColorPipe
    ],
    changeDetection: ChangeDetectionStrategy.OnPush,
    providers: [
        {
            provide: BUTTON_COMPONENT_TOKEN,
            useExisting: ButtonComponent,
        }
    ],
})
export class ButtonComponent
    implements ButtonCore 
{
    @Input() label: string | undefined;
    @Input() icon: string | undefined;
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

    // eslint-disable-next-line @angular-eslint/no-output-native
    @Output() click = new EventEmitter<any>();

    handleClick(event: any) {
        this.click.emit(event);
    }
}