import { CommonModule } from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  Input,
  Optional,
  Self,
  ViewEncapsulation,
} from '@angular/core';
import {
  ControlValueAccessor,
  NgControl,
  ReactiveFormsModule,
} from '@angular/forms';
import {
  BaseFormInputComponent,
  BaseInputControls,
  RADIOBUTTON_COMPONENT_TOKEN,
  RadiobuttonCore,
  RadioButtonItem,
} from '@verisoft/ui-core';
import { RadioButtonModule } from 'primeng/radiobutton';
import { FormFieldComponent } from '../form-field/form-field.component';

@Component({
  selector: 'v-radiobutton',
  standalone: true,
  imports: [
    CommonModule,
    FormFieldComponent,
    RadioButtonModule,
    ReactiveFormsModule,
  ],
  templateUrl: './radiobutton.component.html',
  styleUrl: './radiobutton.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
  encapsulation: ViewEncapsulation.None,
  providers: [
    {
      provide: BaseInputControls,
      useExisting: RadioButtonComponent,
    },
    {
      provide: RADIOBUTTON_COMPONENT_TOKEN,
      useExisting: RadioButtonComponent,
    },
  ],
})
export class RadioButtonComponent<T>
  extends BaseFormInputComponent
  implements ControlValueAccessor, RadiobuttonCore<T>
{
  constructor(@Optional() @Self() ngControl: NgControl) {
    super(ngControl);
  }

  @Input() radioGroupName = Math.random().toString();

  @Input() items: RadioButtonItem<T>[] = [];
}
