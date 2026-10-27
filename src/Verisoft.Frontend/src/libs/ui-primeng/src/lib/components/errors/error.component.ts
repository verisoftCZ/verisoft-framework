import { CommonModule } from "@angular/common";
import { Component, inject, Input, } from "@angular/core";
import { NgControl, ValidationErrors } from "@angular/forms";
import { ERROR_PROVIDER_TOKEN } from "@verisoft/core";
import { ControlSeverityType, SlotPosition, SlotPositionType } from "@verisoft/ui-core";
import { MessageModule } from "primeng/message";
import { Observable } from "rxjs";
import { Icons } from "../../icons";
import { MessageSeverityPipe } from "../../pipes";

@Component({
  selector: "v-validation-message",
  templateUrl: "./error.component.html",
  styleUrl: "./error.component.scss",
  imports: [CommonModule, MessageModule, MessageSeverityPipe]
})
export class ErrorComponent {
  @Input({ required: true }) ngControl?: NgControl;

  @Input() errorSlot: SlotPositionType = SlotPosition.bottom

  @Input() messageSlot: SlotPositionType = SlotPosition.bottom

  @Input() variant = 'simple';

  @Input() severity: ControlSeverityType = 'danger';

  readonly errorService = inject(ERROR_PROVIDER_TOKEN);

  icons = Icons

  getErrorMessage(errors: ValidationErrors): Observable<string | undefined> {
    return this.errorService.mapError(errors);
  }
}