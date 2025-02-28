import { CommonModule } from "@angular/common";
import { 
    ChangeDetectionStrategy,
    Component,
    Optional,
    Self,
    ViewEncapsulation,
    Input
} from "@angular/core";
import { ControlValueAccessor, NgControl, ReactiveFormsModule } from "@angular/forms";
import { RouterModule } from "@angular/router";
import { GovDesignSystemModule } from "@gov-design-system-ce/angular";
import { 
    BaseFormInputComponent, 
    CHECKBOX_COMPONENT_TOKEN, 
    CheckboxCore, 
    ErrorPipe,
    FieldSize,
    FieldSizeType,
    SlotPosition,
    SlotPositionType,
} from "@verisoft/ui-core";
import { v4 as uuidv4 } from 'uuid';
import { Icons } from "../../icons";
import { GovSizePipe } from "../../pipes";

@Component({
    selector: "v-checkbox",
    standalone: true,
    styleUrls: [
        "./checkbox.component.scss"
    ],
    templateUrl: './checkbox.component.html',
    imports: [
    CommonModule, GovDesignSystemModule, RouterModule, ReactiveFormsModule, ErrorPipe,
    GovSizePipe
],
    changeDetection: ChangeDetectionStrategy.OnPush,
    encapsulation: ViewEncapsulation.None,
    providers: [{
        provide: CHECKBOX_COMPONENT_TOKEN,
        useExisting: CheckboxComponent,
    }],
})
export class CheckboxComponent
    extends BaseFormInputComponent
    implements ControlValueAccessor, CheckboxCore
{
    @Input() size: FieldSizeType = FieldSize.medium;
    @Input() value!: string;
    @Input() name!: string;
    @Input() indeterminate = false;
    @Input() noLabel = false;
    @Input() errorSlot: SlotPositionType = SlotPosition.bottom;;

    icons = Icons;
    constructor(@Optional() @Self() ngControl: NgControl) {
        super(ngControl);
    }

    id = uuidv4();
}