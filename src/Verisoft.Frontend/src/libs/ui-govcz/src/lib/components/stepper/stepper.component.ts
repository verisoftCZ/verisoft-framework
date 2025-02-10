import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, EventEmitter, Input, Output } from '@angular/core';
import {
  StepperCore,
  STEPPER_COMPONENT_TOKEN,
  StepperItem,
  FieldSizeType,
  FieldSize,
  LayoutType,
  LayoutTypeType,
} from '@verisoft/ui-core';
import { GovDesignSystemModule } from '@gov-design-system-ce/angular';
import { ButtonComponent } from '../button';
import { GovSizePipe } from '../../pipes';
@Component({
  selector: 'v-stepper',
  standalone: true,
  imports: [
    CommonModule,
    ButtonComponent,
    GovDesignSystemModule,
    GovSizePipe,
],
  templateUrl: './stepper.component.html',
  styleUrl: './stepper.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [
    { provide: STEPPER_COMPONENT_TOKEN, useExisting: StepperComponent },
  ],
})
export class StepperComponent 
  implements StepperCore
{
  @Input() items: StepperItem[] = [];
  @Input() size: FieldSizeType | undefined = FieldSize.medium;
  @Input() icon!: string;
  @Input() prefix!: string;
  @Input() annotation!: string;
  @Input() label!: string;
  @Input() layout: LayoutTypeType = LayoutType.horizontal;
  @Output() activeIndexChange = new EventEmitter<number>();
  
  steps = 0;
  currentStep = 0;

  ngOnInit(): void {
    this.steps = this.items.length;
  }

  previousStep(): void {
    if (this.currentStep > 0) {
      this.currentStep--;
    }
  }

  nextStep(): void {
    if (this.currentStep < this.items.length - 1) {
      this.currentStep++;
    }
  }
}
