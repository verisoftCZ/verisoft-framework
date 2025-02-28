import { CommonModule } from "@angular/common";
import { 
    ChangeDetectionStrategy,
    Component,
    Optional,
    Self,
    Input
} from "@angular/core";
import { 
    ControlValueAccessor,
    NgControl,
    ReactiveFormsModule
} from "@angular/forms";
import { GovDesignSystemModule } from "@gov-design-system-ce/angular";
import { 
    BaseFormInputComponent, 
    TRISTATE_CHECKBOX_COMPONENT_TOKEN, 
    TristateCheckboxCore, 
    ErrorPipe,
    SlotPositionType,
    SlotPosition,
    FieldSizeType,
    FieldSize,
} from "@verisoft/ui-core";
import { GovSizePipe } from "../../pipes";
import { Icons } from "../../icons";

@Component({
    selector: "v-tristatecheckbox",
    standalone: true,
    styleUrl: "./tristatecheckbox.component.scss",
    templateUrl: './tristatecheckbox.component.html',
    imports: [
        CommonModule, 
        GovDesignSystemModule, 
        ReactiveFormsModule, 
        ErrorPipe,
        GovSizePipe,
    ],
    changeDetection: ChangeDetectionStrategy.OnPush,
    providers: [{
        provide: TRISTATE_CHECKBOX_COMPONENT_TOKEN,
        useExisting: TristatecheckboxComponent,
    }],
})
export class TristatecheckboxComponent
    extends BaseFormInputComponent
    implements ControlValueAccessor, TristateCheckboxCore
{   
    @Input() size: FieldSizeType = FieldSize.medium;
    @Input() value!: string;
    @Input() name!: string;
    @Input() noLabel = false;
    @Input() errorSlot: SlotPositionType = SlotPosition.bottom;
    @Input() checked!: boolean;
    
    isChecked = -1;
    icons = Icons;

    constructor(@Optional() @Self() ngControl: NgControl) {
        super(ngControl);
    }

    toggleCheckbox(): void {
        if (this.isChecked === 0) {
            this.isChecked = 1;
            this.formControl.setValue(true, {emitEvent: true});
        } else if (this.isChecked === 1) {
            this.isChecked = -1;
            this.formControl.setValue(null, {emitEvent: true});
        } else {
            this.isChecked = 0;
            this.formControl.setValue(false, {emitEvent: true});
        }
    }
}
