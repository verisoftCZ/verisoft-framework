import {
  Component,
  ViewEncapsulation,
  input,
  output,
  viewChild,
} from '@angular/core';
import { GovDesignSystemModule } from '@gov-design-system-ce/angular';
import {
  ControlSeverity,
  ControlSeverityType,
  DROPDOWN_BUTTON_COMPONENT_TOKEN,
  DropdownButtonCore,
  FieldSizeType,
  GovButtonType,
  GovButtonTypeType,
  MenuItem,
  MenuKeyboardDirective,
  MenuKeyboardItemDirective,
  MenuKeyboardListDirective,
  MenuKeyboardTriggerDirective,
  MenuKeyboardNavigationInput,
} from '@verisoft/ui-core';
import { GovDropdownKeyboardDirective } from '../../directives';
import { ButtonComponent } from '../button';
import { GOV_DROPDOWN_BUTTON_ACTIVE_ITEM_CLASS } from './dropdown-button.constants';

@Component({
  selector: 'v-dropdown-button',
  templateUrl: './dropdown-button.component.html',
  styleUrl: './dropdown-button.component.scss',
  encapsulation: ViewEncapsulation.None,
  imports: [
    GovDesignSystemModule,
    ButtonComponent,
    GovDropdownKeyboardDirective,
    MenuKeyboardDirective,
    MenuKeyboardTriggerDirective,
    MenuKeyboardListDirective,
    MenuKeyboardItemDirective,
  ],
  providers: [
    {
      provide: DROPDOWN_BUTTON_COMPONENT_TOKEN,
      useExisting: DropdownButtonComponent,
    },
  ],
})
export class DropdownButtonComponent implements DropdownButtonCore {
  label = input<string | undefined>();
  icon = input<string | undefined>();
  rounded = input<boolean | undefined>(false);
  raised = input<boolean | undefined>(false);
  outlined = input<boolean | undefined>(false);
  severity = input<ControlSeverityType | undefined>(ControlSeverity.primary);
  size = input<FieldSizeType | undefined>();
  type = input<GovButtonTypeType>(GovButtonType.solid);
  items = input<MenuItem[] | undefined>(undefined);
  keyboardNavigation = input<MenuKeyboardNavigationInput>(true);

  protected readonly menuKeyboardDefaults = {
    activeItemClass: GOV_DROPDOWN_BUTTON_ACTIVE_ITEM_CLASS,
  };

  fireClick = output<MenuItem>();

  private readonly menuKeyboard = viewChild(MenuKeyboardDirective);

  fireClickItem(item: MenuItem): void {
    if (item.disabled) {
      return;
    }

    if (item.command) {
      item.command();
    } else {
      this.fireClick.emit(item);
    }
  }

  protected selectItem(item: MenuItem): void {
    if (item.disabled) {
      return;
    }

    this.fireClickItem(item);
    void this.menuKeyboard()?.close(true);
  }
}
