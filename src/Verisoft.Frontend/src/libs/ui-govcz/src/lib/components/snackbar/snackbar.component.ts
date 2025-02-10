import { CommonModule } from '@angular/common';
import { Component, Inject, Input, Optional } from '@angular/core';
import { GovDesignSystemModule } from '@gov-design-system-ce/angular';
import { SNACKBAR_COMPONENT_TOKEN, SnackbarCore } from '@verisoft/ui-core';
import { SnackbarConfig } from './snackbar.model';

@Component({
  selector: 'v-snackbar',
  standalone: true,
  imports: [CommonModule, GovDesignSystemModule],
  templateUrl: './snackbar.component.html',
  styleUrl: './snackbar.component.scss',
  providers: [
    {
      provide: SNACKBAR_COMPONENT_TOKEN,
      useExisting: SnackbarComponent,
    },
  ],
})
export class SnackbarComponent 
  implements SnackbarCore
{
  @Input({required: true}) message!: string;

  @Input() icon!: string;

  @Input() closeLabel!: string;

  @Input() color: "error" | "neutral" | "primary" | "success" | "warning" = "primary";

  @Input() gravity: "bottom" | "top" = "top";

  @Input() position: "center" | "left" | "right" = "right";

  @Input() time = 4000;

  @Input() type: "bold" | "subtle" = "subtle";

  constructor(
    @Optional() @Inject('TOAST_CONFIG') public config: SnackbarConfig,
  ) { }
}
