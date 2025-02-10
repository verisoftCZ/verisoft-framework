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
import {
  BaseFormInputComponent,
  BaseInputControls,
  FieldSize,
  FieldSizeType,
  FieldType,
  FieldTypeType,
  TEXTFIELD_COMPONENT_TOKEN,
  TextfieldCore,
} from '@verisoft/ui-core';
import { FloatLabelModule } from 'primeng/floatlabel';
import { InputTextModule } from 'primeng/inputtext';
import { FormFieldComponent } from '../form-field/form-field.component';

@Component({
  selector: 'v-textfield',
  standalone: true,
  imports: [
    CommonModule,
    InputTextModule,
    ReactiveFormsModule,
    FormFieldComponent,
    FloatLabelModule,
  ],
  templateUrl: './textfield.component.html',
  styleUrl: './textfield.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [
    {
      provide: BaseInputControls,
      useExisting: forwardRef(() => TextfieldComponent),
    },
    {
      provide: TEXTFIELD_COMPONENT_TOKEN,
      useExisting: TextfieldComponent,
    },
  ],
})
export class TextfieldComponent
  extends BaseFormInputComponent
  implements ControlValueAccessor, TextfieldCore
{
  constructor(@Optional() @Self() ngControl: NgControl) {
    super(ngControl);
  }

  @Input() size: FieldSizeType | undefined = FieldSize.medium;
  @Input() type: FieldTypeType = FieldType.text;
  @Input() floatLabel = false;
}
