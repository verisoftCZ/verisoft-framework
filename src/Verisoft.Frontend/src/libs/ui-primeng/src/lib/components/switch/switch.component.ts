import { CommonModule } from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  forwardRef,
  Optional,
  Self,
} from '@angular/core';
import {
  ControlValueAccessor,
  NgControl,
  ReactiveFormsModule,
} from '@angular/forms';
import { BaseFormInputComponent, BaseInputControls, SWITCH_COMPONENT_TOKEN, SwitchCore } from '@verisoft/ui-core';
import { InputSwitchModule } from 'primeng/inputswitch';
import { v4 as uuidv4 } from 'uuid';
import { FormFieldComponent } from '../form-field';

@Component({
  selector: 'v-switch',
  standalone: true,
  imports: [
    CommonModule,
    InputSwitchModule,
    ReactiveFormsModule,
    FormFieldComponent,
  ],
  templateUrl: './switch.component.html',
  styleUrl: './switch.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [
    {
      provide: BaseInputControls,
      useExisting: forwardRef(() => SwitchComponent),
      multi: true,
    },
    {
      provide: SWITCH_COMPONENT_TOKEN,
      useExisting: SwitchComponent,
    },
  ],
})
export class SwitchComponent
  extends BaseFormInputComponent
  implements ControlValueAccessor, SwitchCore
{
  constructor(@Optional() @Self() ngControl: NgControl) {
    super(ngControl);
  }

  id = uuidv4();
}
