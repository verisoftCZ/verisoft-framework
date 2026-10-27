
import {
  ChangeDetectionStrategy,
  Component,
  Input,
  OnInit,
  Optional,
  Self,
} from '@angular/core';
import {
  ControlValueAccessor,
  FormControl,
  NgControl,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { GovDesignSystemModule } from '@gov-design-system-ce/angular';
import { TranslateModule } from '@ngx-translate/core';
import {
  TextfieldCore,
  TEXTFIELD_COMPONENT_TOKEN,
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
import { TooltipComponent } from '../tooltip';
@Component({
    selector: 'v-textfield',
    imports: [
    ReactiveFormsModule,
    GovDesignSystemModule,
    TooltipComponent,
    GovSizePipe,
    TranslateModule,
    ErrorComponent
],
    templateUrl: './textfield.component.html',
    styleUrl: './textfield.component.scss',
    changeDetection: ChangeDetectionStrategy.OnPush,
    providers: [
        {
            provide: TEXTFIELD_COMPONENT_TOKEN,
            useExisting: TextfieldComponent,
        },
    ]
})
export class TextfieldComponent
  extends BaseFormInputComponent
  implements ControlValueAccessor, TextfieldCore, OnInit
{
  @Input() floatLabel!: boolean;
  @Input() type: FieldTypeType = FieldType.text;
  @Input() minlength = 0;
  @Input() maxlength = 524288;
  @Input() min = -2147483648;
  @Input() max = 2147483647;
  @Input() prefix!: string;
  @Input() sufix!: string;
  @Input() message!: string;
  @Input() name!: string;
  @Input() role!: string;
  @Input() size: FieldSizeType | undefined = FieldSize.medium;
  @Input() icon!: string;
  @Input() iconPos: IconPositionType = IconPosition.right;
  @Input() labelSlot: SlotPositionType = SlotPosition.top;
  @Input() errorSlot: SlotPositionType = SlotPosition.bottom;
  @Input() messageSlot: SlotPositionType = SlotPosition.bottom;

  icons = Icons;

  constructor(@Optional() @Self() ngControl: NgControl) {
    super(ngControl);
    if (ngControl) {
      ngControl.valueAccessor = this;
    }
  }

  override ngOnInit(): void {
    super.ngOnInit();

    if (!this.formControl && this.ngControl) {
      this.formControl = this.ngControl.control as FormControl;
    }

    if (this.formControl && this.type === FieldType.number) {
      this.setMinMaxNumberInputValidators();
    }
  }

  private setMinMaxNumberInputValidators(): void {
    if (!this.formControl) {
      return;
    }

    const validators = [];
    if (this.min !== undefined && this.min !== null) {
      validators.push(Validators.min(this.min));
    }
    if (this.max !== undefined && this.max !== null) {
      validators.push(Validators.max(this.max));
    }

    this.formControl.addValidators(validators);
    this.formControl.updateValueAndValidity({ onlySelf: true, emitEvent: true });
  }
}
