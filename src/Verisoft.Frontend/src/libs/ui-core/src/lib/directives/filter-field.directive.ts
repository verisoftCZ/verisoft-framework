import { Directive, Input } from '@angular/core';
import { ValidatorFn } from '@angular/forms';
import { DatasourceType } from '@verisoft/core';
import { GenericFieldDefinition, GenericFieldType, GenericFieldTypeType } from '../components';

@Directive({
  // eslint-disable-next-line @angular-eslint/directive-selector
  selector: 'v-filter-field',
  standalone: true,
})
export class FilterFieldDirective implements GenericFieldDefinition {
  @Input({ required: true }) name!: string;

  @Input() type: GenericFieldTypeType | undefined = GenericFieldType.text;

  @Input() label: string | undefined;

  @Input() optionLabel!: string;

  @Input() optionValue: string | undefined;

  @Input() options: unknown[] | undefined;

  @Input() value?: unknown;

  @Input() validator?: ValidatorFn[];

  @Input() datasource: DatasourceType<any> | undefined;

  @Input() filterField?: string;

  @Input() showFilter?: boolean;

  @Input() localSearch?: boolean;
}
