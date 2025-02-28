import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, Input, Optional, Self } from '@angular/core';
import { ControlValueAccessor, NgControl, ReactiveFormsModule } from '@angular/forms';
import { GovDesignSystemModule } from '@gov-design-system-ce/angular';
import { 
  BaseFormInputComponent,
  FieldType,
  PASSWORD_COMPONENT_TOKEN,
  PasswordCore,
  ErrorPipe,
  SlotPosition,
  SlotPositionType,
  IconPosition,
  IconPositionType,
  FieldSize,
  FieldSizeType
} from '@verisoft/ui-core';
import { GovSizePipe } from '../../pipes';
import zxcvbn from 'zxcvbn';
import { Icons } from '../../icons';

@Component({
  selector: 'v-password',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    GovDesignSystemModule,
    ErrorPipe,
    GovSizePipe,
  ],
  templateUrl: './password.component.html',
  styleUrl: './password.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [
    {
      provide: PASSWORD_COMPONENT_TOKEN,
      useExisting: PasswordComponent
    }
  ],
})

export class PasswordComponent 
  extends BaseFormInputComponent
  implements ControlValueAccessor, PasswordCore 
{
  @Input() toggleMask = true;
  @Input() feedback = false;
  @Input() labelSlot: SlotPositionType = SlotPosition.top;
  @Input() errorSlot: SlotPositionType = SlotPosition.bottom;
  @Input() message!: string;
  @Input() name! : string;
  @Input() role! : string;
  @Input() icon!: string;
  @Input() iconPos: IconPositionType = IconPosition.right;
  @Input() size: FieldSizeType | undefined = FieldSize.medium;

  icons = Icons;
  passwordStrength: 0 | 1 | 2 | 3 | 4 = 0;
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
  }
}
