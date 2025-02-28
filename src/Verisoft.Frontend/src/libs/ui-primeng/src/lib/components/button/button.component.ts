import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { Params, RouterModule } from '@angular/router';
import {
  BUTTON_COMPONENT_TOKEN,
  ButtonCore,
  ControlSeverityType,
  FieldSizeType,
  IconPosition,
  IconPositionType,
} from '@verisoft/ui-core';
import { ButtonModule } from 'primeng/button';
import { RippleModule } from 'primeng/ripple';

@Component({
  selector: 'v-button',
  standalone: true,
  imports: [CommonModule, ButtonModule, RippleModule, RouterModule],
  templateUrl: './button.component.html',
  styleUrl: './button.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [
    {
      provide: BUTTON_COMPONENT_TOKEN,
      useExisting: ButtonComponent,
    },
  ],
})
export class ButtonComponent implements ButtonCore {
  @Input() label?: string;
  @Input() icon?: string;
  @Input() badge?: string;
  @Input() iconPos: IconPositionType = IconPosition.right;
  @Input() disabled!: boolean;
  @Input() rounded!: boolean;
  @Input() outlined!: boolean;
  @Input() raised!: boolean;
  @Input() severity?: ControlSeverityType;
  @Input() routerLink!: any[];
  @Input() size: FieldSizeType | undefined;
  @Input() queryParams?: Params;

  fireClick(event: Event): void {
    if (this.disabled) {
      event.stopPropagation();
      event.stopImmediatePropagation();
    }
  }
}
