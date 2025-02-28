import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, Input, Optional, Self } from '@angular/core';
import { ControlValueAccessor, NgControl, ReactiveFormsModule } from '@angular/forms';
import { GovDesignSystemModule } from '@gov-design-system-ce/angular';
import {
  BaseFormInputComponent,
  ErrorPipe,
  CalendarCore,
  CALENDAR_COMPONENT_TOKEN,
  FieldType,
  FieldSizeType,
  FieldSize,
  SlotPosition,
  SlotPositionType,
} from '@verisoft/ui-core';
import { GovSizePipe } from '../../pipes';
import { Icons } from '../../icons';

@Component({
  selector: 'v-calendar',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    GovDesignSystemModule,
    ErrorPipe,
    GovSizePipe
  ],
  templateUrl: './calendar.component.html',
  styleUrl: './calendar.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [
    {
      provide: CALENDAR_COMPONENT_TOKEN,
      useExisting: CalendarComponent
    }
  ],
})

export class CalendarComponent
  extends BaseFormInputComponent
  implements ControlValueAccessor, CalendarCore
{
  @Input() icon!: string;

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
  
  @Input() floatLabel: string | undefined;

  type = FieldType.date;

  icons = Icons;

  constructor(@Optional() @Self() ngControl: NgControl) {
    super(ngControl);
  }
}
