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
import { GovDesignSystemModule } from '@gov-design-system-ce/angular';
import {
  NumberInputCore,
  NUMBER_INPUT_COMPONENT_TOKEN,
  FieldType,
  BaseInputControls,
  BaseFormInputComponent,
  ErrorPipe,
  FieldSizeType,
  FieldSize,
  IconPositionType,
  IconPosition,
  SlotPositionType,
  SlotPosition,
  FieldTypeType,
} from '@verisoft/ui-core';
import { GovSizePipe } from '../../pipes';
@Component({
  selector: 'v-number-input',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    GovDesignSystemModule,
    ErrorPipe,
    GovSizePipe
  ],
  templateUrl: './number-input.component.html',
  styleUrl: './number-input.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [
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
  @Input() mode!: string;
  @Input() currency!: string;
  @Input() min!: number;
  @Input() max!: number;
  @Input() step = 1;
  @Input() floatLabel!: boolean;
  @Input() type: FieldTypeType = FieldType.number;
  @Input() minlength = 0;
  @Input() maxlength = 524288;
  @Input() prefix!: string;
  @Input() sufix!: string;
  @Input() message!: string;
  @Input() name! : string;
  @Input() role! : string;
  @Input() size: FieldSizeType | undefined = FieldSize.medium;
  @Input() icon!: string;
  @Input() iconPos: IconPositionType = IconPosition.right;
  @Input() labelSlot: SlotPositionType = SlotPosition.top;
  @Input() errorSlot: SlotPositionType = SlotPosition.bottom;
  @Input() messageSlot: SlotPositionType = SlotPosition.bottom;

  constructor(@Optional() @Self() ngControl: NgControl) {
    super(ngControl);
  }
}
