import { CommonModule } from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  Input,
  Optional,
  Self,
} from '@angular/core';
import {
  ControlValueAccessor,
  NgControl,
  ReactiveFormsModule,
} from '@angular/forms';
import { BaseFormInputComponent, BaseInputControls, PASSWORD_COMPONENT_TOKEN, PasswordCore } from '@verisoft/ui-core';
import { FloatLabelModule } from 'primeng/floatlabel';
import { PasswordModule } from 'primeng/password';
import { FormFieldComponent } from '../form-field';

@Component({
  selector: 'v-password',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    PasswordModule,
    FloatLabelModule,
    FormFieldComponent
  ],
  templateUrl: './password.component.html',
  styleUrl: './password.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [
    {
      provide: BaseInputControls,
      useExisting: PasswordComponent,
    },
    {
      provide: PASSWORD_COMPONENT_TOKEN,
      useExisting: PasswordComponent,
    },
  ],
})
export class PasswordComponent 
  extends BaseFormInputComponent
  implements ControlValueAccessor, PasswordCore
{
  @Input() toggleMask = true;
  
  @Input() feedback = false;

  override formDisplay: "flex" | "block" = "block";

  constructor(@Optional() @Self() ngControl: NgControl) {
    super(ngControl);
  }
}
