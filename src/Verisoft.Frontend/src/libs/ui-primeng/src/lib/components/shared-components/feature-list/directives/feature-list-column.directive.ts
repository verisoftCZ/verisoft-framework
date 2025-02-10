import { Directive, Input } from '@angular/core';
import { TableColumnDirective } from '@verisoft/ui-core';
import { FEATURE_LIST_COLUMN_PROVIDER, FeatureListColumnDefinition, FeatureListColumnProvider } from '../feature-list-page.model';

@Directive({
  // eslint-disable-next-line @angular-eslint/directive-selector
  selector: 'v-feature-list-column',
  standalone: true,
  providers: [
    {
      provide: FEATURE_LIST_COLUMN_PROVIDER,
      useExisting: FeatureListColumnDirective,
      multi: true,
    },
  ],
})
export class FeatureListColumnDirective<T> extends TableColumnDirective<T> implements FeatureListColumnProvider<T> {
  @Input()
  filter = false;

  override getDefinition(): FeatureListColumnDefinition<T> {
    const definition = super.getDefinition() as FeatureListColumnDefinition<T>;
    definition.filter = this.filter;
    return definition;
  }
}
