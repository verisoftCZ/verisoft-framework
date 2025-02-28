import { Directive } from '@angular/core';
import { FilterFieldDirective } from '../../filter';
@Directive({
  // eslint-disable-next-line @angular-eslint/directive-selector
  selector: 'v-feature-list-filter-field',
  standalone: true,
})
export class FeatureListFilterFieldDirective extends FilterFieldDirective {}