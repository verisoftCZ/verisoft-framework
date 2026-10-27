
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
import { TranslateModule } from '@ngx-translate/core';
import {
  TextfieldCore,
  FieldType,
  BaseFormInputComponent,
  FieldSizeType,
  FieldSize,
  IconPositionType,
  IconPosition,
  SlotPositionType,
  SlotPosition,
  TEXTAREA_COMPONENT_TOKEN,
  TextareaCore,
} from '@verisoft/ui-core';
import { Icons } from '../../icons';
import { GovSizePipe } from '../../pipes';
import { ErrorComponent } from '../errors';
@Component({
  selector: 'v-textarea',
  imports: [
    ReactiveFormsModule,
    GovDesignSystemModule,
    GovSizePipe,
    ErrorComponent,
    TranslateModule
],
  templateUrl: './textarea.component.html',
  styleUrl: './textarea.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [
    {
      provide: TEXTAREA_COMPONENT_TOKEN,
      useExisting: TextareaComponent,
    },
  ]
})
export class TextareaComponent
  extends BaseFormInputComponent
  implements ControlValueAccessor, TextareaCore {
  @Input() floatLabel!: string | undefined;
  @Input() type = FieldType.text;
  @Input() minlength = 0;
  @Input() maxlength = 524288;
  @Input() prefix!: string;
  @Input() sufix!: string;
  @Input() message!: string;
  @Input() name!: string;
  @Input() role!: string;
  @Input() size: FieldSizeType = FieldSize.medium;
  @Input() icon!: string;
  @Input() iconPos: IconPositionType = IconPosition.right;
  @Input() labelSlot: SlotPositionType = SlotPosition.top;
  @Input() messageSlot: SlotPositionType = SlotPosition.bottom;
  @Input() rows = 0;
  @Input() cols = 0;
  @Input() autoResize = false;

  icons = Icons;

  constructor(@Optional() @Self() ngControl: NgControl) {
    super(ngControl);
  }
}
