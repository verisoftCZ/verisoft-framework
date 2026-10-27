import { CommonModule } from "@angular/common";
import { Component, inject, Input, } from "@angular/core";
import { NgControl, ValidationErrors } from "@angular/forms";
import { GovDesignSystemModule } from '@gov-design-system-ce/angular';
import { ERROR_PROVIDER_TOKEN } from "@verisoft/core";
import { SlotPosition, SlotPositionType } from "@verisoft/ui-core";
import { Observable } from "rxjs";
import { Icons } from "../../icons";

@Component({
  selector: "v-validation-message",
  templateUrl: "./error.component.html",
  styleUrl: "./error.component.scss",
  imports: [CommonModule, GovDesignSystemModule]
})
export class ErrorComponent {
  @Input({ required: true }) ngControl?: NgControl;

  @Input() errorSlot: SlotPositionType = SlotPosition.bottom

  @Input() messageSlot: SlotPositionType = SlotPosition.bottom

  readonly errorService = inject(ERROR_PROVIDER_TOKEN);
  icons = Icons

  getErrorMessage(errors: ValidationErrors): Observable<string | undefined> {
    return this.errorService.mapError(errors);
  }
}