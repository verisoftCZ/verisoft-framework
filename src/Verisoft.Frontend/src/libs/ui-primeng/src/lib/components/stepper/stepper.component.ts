import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, EventEmitter, Input, Output } from '@angular/core';
import {
  StepperCore,
  STEPPER_COMPONENT_TOKEN,
  StepperItem,
  LayoutType,
  LayoutTypeType,
  FieldSizeType,
  FieldSize
} from '@verisoft/ui-core';
import { StepperModule } from 'primeng/stepper';
import { ButtonComponent } from '../button';
import { Icons } from '../../icons';

@Component({
  selector: 'v-stepper',
  standalone: true,
  imports: [CommonModule, ButtonComponent, StepperModule],
  templateUrl: './stepper.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [
    { provide: STEPPER_COMPONENT_TOKEN, useExisting: StepperComponent },
  ],
})
export class StepperComponent 
  implements StepperCore
{
  @Input() items!: StepperItem[];

  icons = Icons;
  @Input() prefix!: string;
  @Input() label!: string;
  @Input() size: FieldSizeType | undefined = FieldSize.medium;
  @Input() layout: LayoutTypeType = LayoutType.horizontal;
  @Input() icon!: string;
  @Input() activeIndex = 0;
  @Input() annotation!: string;
  @Output() activeIndexChange = new EventEmitter<number>();
}
