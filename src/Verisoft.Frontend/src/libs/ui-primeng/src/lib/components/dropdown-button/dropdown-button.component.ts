import { Component, computed, input } from "@angular/core";
import { DropdownButtonCore, DROPDOWN_BUTTON_COMPONENT_TOKEN, ControlSeverityType, ControlSeverity, FieldSizeType, MenuItem } from "@verisoft/ui-core";
import { SplitButtonModule } from 'primeng/splitbutton';
import { convertToPrimeSeverity, convertToPrimeSize } from "../../models";

@Component({
  selector: 'v-dropdown-button',
  templateUrl: './dropdown-button.component.html',
  styles: [`::ng-deep.p-splitbutton-button { display: none; }`],
  imports: [SplitButtonModule],
  providers: [
    { provide: DROPDOWN_BUTTON_COMPONENT_TOKEN, useExisting: DropdownButtonComponent }
  ]
})
export class DropdownButtonComponent implements DropdownButtonCore {
  label = input<string | undefined>();
  icon = input<string | undefined>();
  rounded = input<boolean | undefined>(false);
  raised = input<boolean | undefined>(false);
  outlined = input<boolean | undefined>(false);
  severity = input<ControlSeverityType | undefined>(ControlSeverity.primary);
  size = input<FieldSizeType | undefined>();
  items = input<MenuItem[] | undefined>(undefined);

  primengSeverity = computed(() => convertToPrimeSeverity(this.severity()));
  primengSize = computed(() => convertToPrimeSize(this.size()));
  primengItems = computed(() => this.items()?.map(x => ({
    label: x.label,
    icon: x.icon,
    command: () => x.command?.(),
    url: x.url,
    routerLink: x.routerLink,
    class: x.class,

  })) || []);
}