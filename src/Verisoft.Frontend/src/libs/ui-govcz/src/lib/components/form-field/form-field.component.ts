
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
  FieldSize,
  FieldSizeType,
  FieldType,
  IconPositionType,
  IconPosition,
  FormFieldCore,
  FORM_FIELD_COMPONENT_TOKEN,
  FieldTypeType,
  SlotPosition,
  SlotPositionType,
} from '@verisoft/ui-core';
import { Icons } from '../../icons';
import { GovSizePipe } from '../../pipes';
import { ErrorComponent } from '../errors';

@Component({
    selector: 'v-form-field',
    imports: [
    ReactiveFormsModule,
    GovDesignSystemModule,
    GovSizePipe,
    ErrorComponent
],
    templateUrl: './form-field.component.html',
    styleUrl: './form-field.component.scss',
    changeDetection: ChangeDetectionStrategy.OnPush,
    providers: [
        {
            provide: FORM_FIELD_COMPONENT_TOKEN,
            useExisting: FormFieldComponent,
        },
    ]
})
export class FormFieldComponent
  extends BaseFormInputComponent
  implements ControlValueAccessor, FormFieldCore
{
  constructor(@Optional() @Self() ngControl: NgControl) {
    super(ngControl);
  }

  @Input() floatLabel!: boolean;
  @Input() type: FieldTypeType = FieldType.text;
  @Input() minlength = 0;
  @Input() maxlength = 524288;
  @Input() prefix!: string;
  @Input() sufix!: string;
  @Input() message!: string;
  @Input() name! : string;
  @Input() role! : string;
  @Input() autocorrect: 'on' | 'off' = 'on';
  @Input() size: FieldSizeType | undefined = FieldSize.medium;
  @Input() icon!: string;
  @Input() iconPos: IconPositionType = IconPosition.right;
  @Input() display!: 'flex' | 'block';
  @Input() labelSlot: SlotPositionType = SlotPosition.top;
  @Input() messageSlot: SlotPositionType = SlotPosition.bottom;

  icons = Icons;
}
