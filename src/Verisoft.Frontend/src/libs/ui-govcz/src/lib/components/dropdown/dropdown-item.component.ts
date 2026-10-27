import {
  ChangeDetectionStrategy,
  Component,
  Input,
  ViewEncapsulation,
} from '@angular/core';
import { DropDownItemData } from './dropdown.model';

@Component({
  selector: 'v-dropdown-item',
  standalone: true,
  templateUrl: './dropdown-item.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  encapsulation: ViewEncapsulation.None,
})
export class DropdownItemComponent<T> {
  @Input() item : DropDownItemData<T> | undefined;
}
