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
import { GovDesignSystemModule } from '@gov-design-system-ce/angular';
import {
  TextfieldCore,
  TEXTFIELD_COMPONENT_TOKEN,
  FieldType,
  BaseFormInputComponent,
  ErrorPipe,
  FieldSizeType,
  FieldSize,
  IconPositionType,
  IconPosition,
  SlotPositionType,
  SlotPosition,
  FieldTypeType
} from '@verisoft/ui-core';
import { GovSizePipe } from '../../pipes';
@Component({
  selector: 'v-textfield',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    GovDesignSystemModule,
    ErrorPipe,
    GovSizePipe,
  ],
  templateUrl: './textfield.component.html',
  styleUrl: './textfield.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [
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
  @Input() floatLabel!: boolean;
  @Input() type: FieldTypeType = FieldType.text;
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
