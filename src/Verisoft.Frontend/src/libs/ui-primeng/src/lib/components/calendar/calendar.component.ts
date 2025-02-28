import { CommonModule } from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  forwardRef,
  Input,
  Optional,
  Self,
} from '@angular/core';
import {
  ControlValueAccessor,
  NgControl,
  ReactiveFormsModule,
} from '@angular/forms';
import {
  BaseFormInputComponent,
  BaseInputControls,
  CALENDAR_COMPONENT_TOKEN,
  CalendarCore,
} from '@verisoft/ui-core';
import { CalendarModule } from 'primeng/calendar';
import { FloatLabelModule } from 'primeng/floatlabel';
import { FormFieldComponent } from '../form-field';
import { Icons } from '../../icons';

@Component({
  selector: 'v-calendar',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    CalendarModule,
    FormFieldComponent,
    FloatLabelModule,
  ],
  templateUrl: './calendar.component.html',
  styleUrl: './calendar.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [
    {
      provide: BaseInputControls,
      useExisting: forwardRef(() => CalendarComponent),
    },
    {
      provide: CALENDAR_COMPONENT_TOKEN,
      useExisting: CalendarComponent,
    },
  ],
})
export class CalendarComponent
  extends BaseFormInputComponent
  implements ControlValueAccessor, CalendarCore
{
  constructor(@Optional() @Self() ngControl: NgControl) {
    super(ngControl);
  }

  @Input() maxDate!: Date;
  @Input() icon = Icons.calendar;
  @Input() minDate!: Date;
  @Input() header!: string;
  @Input() footer!: string;
  @Input() floatLabel: string | undefined;
  @Input() selectionMode: 'single' | 'multiple' | 'range' | undefined =
    'single';

    icons = Icons;

  override formDisplay: "flex" | "block" = "block";
}
