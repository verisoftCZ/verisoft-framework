import { CommonModule } from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  Optional,
  Self,
} from '@angular/core';
import {
  ControlValueAccessor,
  NgControl,
  ReactiveFormsModule,
} from '@angular/forms';
import { BaseFormInputComponent, BaseInputControls, CHECKBOX_COMPONENT_TOKEN, CheckboxCore } from '@verisoft/ui-core';
import { CheckboxModule } from 'primeng/checkbox';
import { FormFieldComponent } from '../form-field';
@Component({
  selector: 'v-checkbox',
  standalone: true,
  imports: [CommonModule, CheckboxModule, ReactiveFormsModule, FormFieldComponent],
  templateUrl: './checkbox.component.html',
  styleUrl: './checkbox.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [
    {
      provide: BaseInputControls,
      useExisting: CheckboxComponent,
    },
    {
      provide: CHECKBOX_COMPONENT_TOKEN,
      useExisting: CheckboxComponent,
    },
  ],
})
export class CheckboxComponent
  extends BaseFormInputComponent
  implements ControlValueAccessor, CheckboxCore
{
  constructor(@Optional() @Self() ngControl: NgControl) {
    super(ngControl);
  }
}
