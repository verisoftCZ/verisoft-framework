import {
  ChangeDetectionStrategy,
  Component,
  forwardRef,
  inject,
  Input,
  LOCALE_ID,
  Optional,
  Self,
} from '@angular/core';
import {
  ControlValueAccessor,
  NgControl,
  ReactiveFormsModule,
} from '@angular/forms';
import {
  BaseFormInputComponent,
  BaseInputControls,
  NUMBER_INPUT_COMPONENT_TOKEN,
  NumberInputCore,
} from '@verisoft/ui-core';
import { InputNumberModule } from 'primeng/inputnumber';
import { ErrorComponent } from '../errors';
import { FormFieldComponent } from '../form-field';

@Component({
  selector: 'v-number-input',
  imports: [
    InputNumberModule,
    ReactiveFormsModule,
    FormFieldComponent,
    ErrorComponent,
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

  @Input() decimals = 0;
  @Input() currency!: string;
  @Input() min!: number;
  @Input() max!: number;
  @Input() step: number | undefined;
  @Input() errorMessage?: string = 'test';
  @Input() locale: string = inject(LOCALE_ID);

  computeStep() {
    if (this.step !== undefined) {
      return this.step;
    }

    return this.decimals > 0 ? Math.pow(10, -this.decimals) : 1;
  }
}
