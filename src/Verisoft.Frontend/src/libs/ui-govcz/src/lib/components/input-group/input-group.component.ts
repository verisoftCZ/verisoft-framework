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
  BaseFormInputComponent,
  INPUT_GROUP_COMPONENT_TOKEN,
  InputGroupCore,
  InputGroupItem,
  FieldSizeType,
  FieldSize,
  IconPositionType,
  IconPosition,
  SlotPositionType,
  SlotPosition,
  FieldTypeType,
  FieldType,
  ErrorPipe,
} from '@verisoft/ui-core';
import { GovSizePipe } from '../../pipes';

@Component({
  selector: 'v-input-group',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    GovDesignSystemModule,
    ErrorPipe,
    GovSizePipe,
  ],
  templateUrl: './input-group.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [
    {
      provide: INPUT_GROUP_COMPONENT_TOKEN,
      useExisting: InputGroupComponent,
    },
  ],
})
export class InputGroupComponent
  extends BaseFormInputComponent
  implements ControlValueAccessor, InputGroupCore
{
  @Input() floatLabel!: boolean;
  @Input() type: FieldTypeType = FieldType.text;
  @Input() minlength = 0;
  @Input() items!: InputGroupItem[];
  @Input() prefix!: string;
  @Input() sufix!: string;
  @Input() maxlength = 524288;
  @Input() name! : string;
  @Input() role! : string;
  @Input() message! : string;
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
