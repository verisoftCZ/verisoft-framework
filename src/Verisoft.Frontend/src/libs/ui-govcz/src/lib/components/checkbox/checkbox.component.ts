
import {
    ChangeDetectionStrategy,
    Component,
    Optional,
    Self,
    Input,
    input,
} from "@angular/core";
import { ControlValueAccessor, NgControl, ReactiveFormsModule } from "@angular/forms";
import { RouterModule } from "@angular/router";
import { GovDesignSystemModule } from "@gov-design-system-ce/angular";
import { TranslateModule } from "@ngx-translate/core";
import {
    BaseFormInputComponent,
    CHECKBOX_COMPONENT_TOKEN,
    CheckboxCore,
    FieldSize,
    FieldSizeType,
} from "@verisoft/ui-core";
import { v4 as uuidv4 } from 'uuid';
import { Icons } from "../../icons";
import { GovSizePipe } from "../../pipes";
import { ErrorComponent } from "../errors";

@Component({
    selector: "v-checkbox",
    styleUrl: "./checkbox.component.scss",
    templateUrl: './checkbox.component.html',
    imports: [
    GovDesignSystemModule,
    RouterModule,
    ReactiveFormsModule,
    GovSizePipe,
    ErrorComponent,
    TranslateModule
],
    changeDetection: ChangeDetectionStrategy.OnPush,
    providers: [{
            provide: CHECKBOX_COMPONENT_TOKEN,
            useExisting: CheckboxComponent,
        }]
})
export class CheckboxComponent
  extends BaseFormInputComponent
  implements ControlValueAccessor, CheckboxCore
{
    @Input() size: FieldSizeType = FieldSize.medium;
    @Input() value!: string;
    @Input() name!: string;
    readonly indeterminate = input(false);
    @Input() noLabel = false;

    protected readonly icons = Icons;

    constructor(@Optional() @Self() ngControl: NgControl) {
        super(ngControl);
    }

    id = uuidv4();

    protected override coerceValue(value: any): any {
        return this.indeterminate() ? value : !!value;
    }
}
