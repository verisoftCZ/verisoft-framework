
import { ChangeDetectionStrategy, Component, Input, Optional, Self } from '@angular/core';
import { ControlValueAccessor, NgControl, ReactiveFormsModule } from '@angular/forms';
import { GovDesignSystemModule } from '@gov-design-system-ce/angular';
import { 
  BaseFormInputComponent,
  FieldType,
  PASSWORD_COMPONENT_TOKEN,
  PasswordCore,
  SlotPosition,
  SlotPositionType,
  IconPosition,
  IconPositionType,
  FieldSize,
  FieldSizeType,
  PasswordStrength,
} from '@verisoft/ui-core';
import zxcvbn from 'zxcvbn';
import { Icons } from '../../icons';
import { GovSizePipe } from '../../pipes';
import { ErrorComponent } from '../errors';

@Component({
    selector: 'v-password',
    imports: [
    ReactiveFormsModule,
    GovDesignSystemModule,
    GovSizePipe,
    ErrorComponent
],
    templateUrl: './password.component.html',
    styleUrl: './password.component.scss',
    changeDetection: ChangeDetectionStrategy.OnPush,
    providers: [
        {
            provide: PASSWORD_COMPONENT_TOKEN,
            useExisting: PasswordComponent
        }
    ]
})

export class PasswordComponent 
  extends BaseFormInputComponent
  implements ControlValueAccessor, PasswordCore 
{
  @Input() toggleMask = true;
  @Input() feedback = false;
  @Input() labelSlot: SlotPositionType = SlotPosition.top;
  @Input() requiredStrength: PasswordStrength = PasswordStrength.None;
  @Input() message!: string;
  @Input() name! : string;
  @Input() role! : string;
  @Input() icon!: string;
  @Input() iconPos: IconPositionType = IconPosition.right;
  @Input() size: FieldSizeType | undefined = FieldSize.medium;
  @Input() messageSlot: SlotPositionType = SlotPosition.bottom;

  icons = Icons;
  passwordStrength: PasswordStrength = PasswordStrength.None;
  type = FieldType.password;

  constructor(@Optional() @Self() ngControl: NgControl) {
    super(ngControl);
  }

  valueChange(value: string) {
    if (!value) {
      this.passwordStrength = 0;
      return;
    }

    const result = zxcvbn(value);
    this.passwordStrength = result.score;

    if (!this.formControl) {
      return;
    }

    if (result.score < this.requiredStrength) {
      this.formControl.setErrors({passwordLowStrength: true}, {emitEvent: false})
    }
  }
}
