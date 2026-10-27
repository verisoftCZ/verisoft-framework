
import { ChangeDetectionStrategy, Component, Input, Optional, Self } from '@angular/core';
import { ControlValueAccessor, NgControl, ReactiveFormsModule } from '@angular/forms';
import { GovDesignSystemModule } from '@gov-design-system-ce/angular';
import { TranslateModule } from '@ngx-translate/core';
import {
  BaseFormInputComponent,
  CalendarCore,
  CALENDAR_COMPONENT_TOKEN,
  DateInputFilterDirective,
  FieldType,
  FieldSizeType,
  FieldSize,
  SlotPosition,
  SlotPositionType,
} from '@verisoft/ui-core';
import { Icons } from '../../icons';
import { GovSizePipe } from '../../pipes';
import { ErrorComponent } from '../errors';

@Component({
    selector: 'v-calendar',
    imports: [
    ReactiveFormsModule,
    GovDesignSystemModule,
    GovSizePipe,
    ErrorComponent,
    TranslateModule,
    DateInputFilterDirective
],
    templateUrl: './calendar.component.html',
    styleUrl: './calendar.component.scss',
    changeDetection: ChangeDetectionStrategy.OnPush,
    providers: [
        {
            provide: CALENDAR_COMPONENT_TOKEN,
            useExisting: CalendarComponent
        }
    ]
})

export class CalendarComponent
  extends BaseFormInputComponent
  implements ControlValueAccessor, CalendarCore
{
  @Input() icon!: string;

  @Input() floatLabel: string | undefined;

  @Input() maxDate!: Date;

  @Input() minDate!: Date;

  @Input() header!: string;

  @Input() footer!: string;

  @Input() selectionMode: 'single' | 'multiple' | 'range' | undefined;

  @Input() size: FieldSizeType = FieldSize.medium;

  @Input() errorSlot: SlotPositionType = SlotPosition.bottom;

  @Input() labelSlot: SlotPositionType = SlotPosition.top;

  @Input() messageSlot: SlotPositionType = SlotPosition.bottom;

  @Input() message!: string;

  @Input() name!: string;

  type = FieldType.date;

  icons = Icons;

  constructor(@Optional() @Self() ngControl: NgControl) {
    super(ngControl);
  }
}
