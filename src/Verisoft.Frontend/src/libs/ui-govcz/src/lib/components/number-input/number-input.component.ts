
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
import { GovDesignSystemModule } from '@gov-design-system-ce/angular';
import {
  NumberInputCore,
  NUMBER_INPUT_COMPONENT_TOKEN,
  FieldType,
  BaseFormInputComponent,
  FieldSizeType,
  FieldSize,
  IconPositionType,
  IconPosition,
  SlotPositionType,
  SlotPosition,
  FieldTypeType,
} from '@verisoft/ui-core';
import { Icons } from '../../icons';
import { GovSizePipe } from '../../pipes';
import { ErrorComponent } from '../errors';
@Component({
    selector: 'v-number-input',
    imports: [
    ReactiveFormsModule,
    GovDesignSystemModule,
    GovSizePipe,
    ErrorComponent
],
    templateUrl: './number-input.component.html',
    styleUrl: './number-input.component.scss',
    changeDetection: ChangeDetectionStrategy.OnPush,
    providers: [
        {
            provide: NUMBER_INPUT_COMPONENT_TOKEN,
            useExisting: NumberInputComponent,
        },
    ]
})

export class NumberInputComponent
  extends BaseFormInputComponent
  implements ControlValueAccessor, NumberInputCore
{  
  @Input() decimals = 0;
  @Input() currency!: string;
  @Input() min!: number;
  @Input() max!: number;
  @Input() step: number | undefined;
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

  icons = Icons;

  constructor(@Optional() @Self() ngControl: NgControl) {
    super(ngControl);
  }

  onGovBlur(): void {
    const value = this.formControl?.value;
    if (value != null) {
      const rounded = parseFloat(parseFloat(String(value)).toFixed(this.decimals));
      if (rounded !== value) {
        this.formControl.setValue(rounded);
      }
    }
  }

  computeStep() {
    if (this.step !== undefined) {
      return this.step;
    }

    return this.decimals > 0 ? Math.pow(10, -this.decimals) : 1;
  }
}
