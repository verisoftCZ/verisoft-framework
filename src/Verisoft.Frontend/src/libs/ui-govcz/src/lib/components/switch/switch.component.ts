
import { ChangeDetectionStrategy, Component, Input, Optional, Self } from "@angular/core";
import { ReactiveFormsModule, NgControl } from "@angular/forms";
import { GovDesignSystemModule } from "@gov-design-system-ce/angular";
import { SWITCH_COMPONENT_TOKEN, SwitchCore, BaseFormInputComponent, FieldSizeType, FieldSize } from "@verisoft/ui-core";
import { Icons } from "../../icons";
import { GovSizePipe } from "../../pipes";
import { ErrorComponent } from "../errors";

@Component({
    selector: "v-switch",
    styleUrl: "./switch.component.scss",
    templateUrl: './switch.component.html',
    imports: [
    GovDesignSystemModule,
    ReactiveFormsModule,
    GovSizePipe,
    ErrorComponent
],
    changeDetection: ChangeDetectionStrategy.OnPush,
    providers: [{
            provide: SWITCH_COMPONENT_TOKEN,
            useExisting: SwitchComponent,
        }]
})
export class SwitchComponent
  extends BaseFormInputComponent
  implements SwitchCore
{
  @Input() size: FieldSizeType = FieldSize.medium;
  @Input() noLabel = false;
  @Input() name!: string;

  icons = Icons;

  constructor(@Optional() @Self() ngControl: NgControl) {
    super(ngControl);
  }
}