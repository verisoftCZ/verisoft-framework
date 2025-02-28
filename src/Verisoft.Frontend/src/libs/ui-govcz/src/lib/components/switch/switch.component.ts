import { CommonModule } from "@angular/common";
import { ChangeDetectionStrategy, Component, Input, Optional, Self, ViewEncapsulation } from "@angular/core";
import { ReactiveFormsModule, NgControl } from "@angular/forms";
import { GovDesignSystemModule } from "@gov-design-system-ce/angular";
import { SWITCH_COMPONENT_TOKEN, SwitchCore, ErrorPipe, BaseFormInputComponent, FieldSizeType, FieldSize, SlotPositionType, SlotPosition } from "@verisoft/ui-core";
import { Icons } from "../../icons";
import { GovSizePipe } from "../../pipes";

@Component({
  selector: "v-switch",
  standalone: true,
  styleUrls: [
    "./switch.component.scss"
  ],
  templateUrl: './switch.component.html',
  imports: [
    CommonModule, GovDesignSystemModule, ErrorPipe, ReactiveFormsModule, GovSizePipe
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [{
    provide: SWITCH_COMPONENT_TOKEN,
    useExisting: SwitchComponent,
  }],
})
export class SwitchComponent
  extends BaseFormInputComponent
  implements SwitchCore
{
  @Input() size: FieldSizeType = FieldSize.medium;
  @Input() errorSlot: SlotPositionType = SlotPosition.bottom;;
  @Input() noLabel = false;
  @Input() name!: string;

  icons = Icons;

  constructor(@Optional() @Self() ngControl: NgControl) {
    super(ngControl);
  }
}