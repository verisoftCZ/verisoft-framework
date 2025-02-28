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
import { BaseFormInputComponent, BaseInputControls, SLIDER_COMPONENT_TOKEN, SliderCore } from '@verisoft/ui-core';
import { SliderModule } from 'primeng/slider';
import { FormFieldComponent } from '../form-field';

@Component({
  selector: 'v-slider',
  standalone: true,
  imports: [
    CommonModule,
    SliderModule,
    ReactiveFormsModule,
    FormFieldComponent,
  ],
  templateUrl: './slider.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [
    {
      provide: BaseInputControls,
      useExisting: forwardRef(() => SliderComponent),
    },
    {
      provide: SLIDER_COMPONENT_TOKEN,
      useExisting: SliderComponent,
    },
  ],
})
export class SliderComponent
  extends BaseFormInputComponent
  implements ControlValueAccessor, SliderCore
{
  @Input() step = 1;

  @Input() min = 0;

  @Input() max = 100;

  @Input() range = false;

  constructor(@Optional() @Self() ngControl: NgControl) {
    super(ngControl);
  }
}
