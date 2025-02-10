import { CommonModule } from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  forwardRef,
  Input,
  Optional,
  Self,
} from '@angular/core';
import {
  ControlValueAccessor,
  NgControl,
  ReactiveFormsModule,
} from '@angular/forms';
import { BaseFormInputComponent, BaseInputControls, NUMBER_INPUT_COMPONENT_TOKEN, NumberInputCore } from '@verisoft/ui-core';
import { InputNumberModule } from 'primeng/inputnumber';
import { FormFieldComponent } from '../form-field';

@Component({
  selector: 'v-number-input',
  standalone: true,
  imports: [
    CommonModule,
    InputNumberModule,
    ReactiveFormsModule,
    FormFieldComponent,
  ],
  templateUrl: './number-input.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [
    {
      provide: BaseInputControls,
      useExisting: forwardRef(() => NumberInputComponent),
    },
    {
      provide: NUMBER_INPUT_COMPONENT_TOKEN,
      useExisting: NumberInputComponent,
    },
  ],
})
export class NumberInputComponent
  extends BaseFormInputComponent
  implements ControlValueAccessor, NumberInputCore
{
  constructor(@Optional() @Self() ngControl: NgControl) {
    super(ngControl);
  }

  @Input() mode!: string;
  @Input() currency!: string;
  @Input() min!: number;
  @Input() max!: number;
  @Input() step = 1;
}
