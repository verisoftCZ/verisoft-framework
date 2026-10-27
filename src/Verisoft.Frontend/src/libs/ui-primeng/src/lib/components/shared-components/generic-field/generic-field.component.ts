
import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Input,
  Output,
} from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import {
  DatasourceType,
  FilterEvent,
  LazyLoadEvent,
} from '@verisoft/core';
import {
  BaseFormInputComponent,
  DatasourceDirective,
  FieldSize,
  FieldSizeType,
  GENERIC_FIELD_COMPONENT_TOKEN,
  GenericFieldCore,
  GenericFieldType,
  GenericFieldTypeType,
} from '@verisoft/ui-core';
import { Icons } from '../../../icons';
import { CalendarComponent } from '../../calendar';
import { CheckboxComponent } from '../../checkbox';
import { DropdownComponent } from '../../dropdown';
import { MultiselectComponent } from '../../multiselect';
import { TextfieldComponent } from '../../textfield';

@Component({
    selector: 'v-generic-field',
    imports: [
    DropdownComponent,
    CalendarComponent,
    MultiselectComponent,
    TextfieldComponent,
    ReactiveFormsModule,
    CheckboxComponent,
    DatasourceDirective
],
    providers: [
        {
            provide: GENERIC_FIELD_COMPONENT_TOKEN,
            useExisting: GenericFieldComponent,
        },
    ],
    templateUrl: './generic-field.component.html',
    changeDetection: ChangeDetectionStrategy.OnPush
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

  @Input() datasource?: DatasourceType<T>

  @Input() filterField?: string;

  @Input() showFilter?: boolean;

  @Input() localSearch?: boolean;

  @Output() changed = new EventEmitter<any>();
  @Output() showed = new EventEmitter<any>();
  @Output() cleared = new EventEmitter<any>();
  @Output() lazyLoad = new EventEmitter<LazyLoadEvent>();
  @Output() filtered = new EventEmitter<FilterEvent>();

  fieldTypes = GenericFieldType;
  icons = Icons;
}
