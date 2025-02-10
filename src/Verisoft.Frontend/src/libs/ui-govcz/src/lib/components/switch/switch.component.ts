import { CommonModule } from "@angular/common";
import { ChangeDetectionStrategy, Component, Input, Optional, Self } from "@angular/core";
import { ReactiveFormsModule, NgControl } from "@angular/forms";
import { GovDesignSystemModule } from "@gov-design-system-ce/angular";
import { SWITCH_COMPONENT_TOKEN, SwitchCore, ErrorPipe, BaseFormInputComponent } from "@verisoft/ui-core";

@Component({
  selector: "v-switch",
  standalone: true,
  styleUrls: [
    "./switch.component.scss"
  ],
  templateUrl: './switch.component.html',
  imports: [
    CommonModule, GovDesignSystemModule, ErrorPipe, ReactiveFormsModule
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
  @Input() size: 'xs' | 's' | 'm' | 'l' | 'xl' = 'm';
  @Input() errorSlot: "top" | "bottom" = "top";
  @Input() noLabel = false;
  @Input() name!: string;

  constructor(@Optional() @Self() ngControl: NgControl) {
    super(ngControl);
  }
}