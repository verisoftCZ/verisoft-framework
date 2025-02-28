import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Input,
  Output,
} from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import {
  FilterEvent,
  LazyLoadEvent,
} from '@verisoft/core';
import {
  BaseFormInputComponent,
  FieldSize,
  FieldSizeType,
  GENERIC_FIELD_COMPONENT_TOKEN,
  GenericFieldCore,
  GenericFieldType,
  GenericFieldTypeType,
} from '@verisoft/ui-core';
import { CalendarComponent } from '../../calendar';
import { DropdownComponent } from '../../dropdown';
import { MultiselectComponent } from '../../multiselect';
import { TextfieldComponent } from '../../textfield';
import { TristatecheckboxComponent } from '../../tristatecheckbox';
import { Icons } from '../../../icons';

@Component({
  selector: 'v-generic-field',
  standalone: true,
  imports: [
    DropdownComponent,
    CalendarComponent,
    TristatecheckboxComponent,
    MultiselectComponent,
    TextfieldComponent,
    ReactiveFormsModule,
  ],
  providers: [
    {
      provide: GENERIC_FIELD_COMPONENT_TOKEN,
      useExisting: GenericFieldComponent,
    },
  ],
  templateUrl: './generic-field.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class GenericFieldComponent<T>
  extends BaseFormInputComponent
  implements GenericFieldCore<T>
{
  @Input() type?: GenericFieldTypeType = GenericFieldType.text;

  @Input() floatLabel?: boolean;

  @Input() optionLabel: string | undefined;

  @Input() optionValue: string | undefined;

  @Input() options: T[] | undefined;

  @Input() size?: FieldSizeType = FieldSize.medium;

  @Input() loading = false;

  @Input() lazy = false;

  @Input() filter = true;

  @Output() changed = new EventEmitter<any>();
  @Output() showed = new EventEmitter<any>();
  @Output() cleared = new EventEmitter<any>();
  @Output() lazyLoad = new EventEmitter<LazyLoadEvent>();
  @Output() filtered = new EventEmitter<FilterEvent>();

  fieldTypes = GenericFieldType;
  icons = Icons;
}
