import { Directive, Input } from '@angular/core';
import { FEATURE_LIST_COLUMN_PROVIDER, FeatureListColumnDefinition, FeatureListColumnFilterOptions, FeatureListColumnProvider, GenericFieldType, GenericFieldTypeType, TableColumnDirective } from '../components';

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
export class FeatureListColumnDirective<T>
  extends TableColumnDirective<T>
  implements FeatureListColumnProvider<T>
{
  @Input() filter = false;

  @Input() type: GenericFieldTypeType = GenericFieldType.text;

  @Input() filterOptions?: FeatureListColumnFilterOptions;

  override getDefinition(): FeatureListColumnDefinition<T> {
    const definition = super.getDefinition() as FeatureListColumnDefinition<T>;
    definition.filter = this.filter;
    definition.type = this.type;
    definition.format = this.format;
    definition.filterOptions = this.filterOptions;
    return definition;
  }
}
