import { Directive, Input } from '@angular/core';
import { GenericFieldDefinition, GenericFieldType, GenericFieldTypeType } from '@verisoft/ui-core';

@Directive({
  // eslint-disable-next-line @angular-eslint/directive-selector
  selector: 'v-filter-field',
  standalone: true,
})
export class FilterFieldDirective implements GenericFieldDefinition {
  
    @Input({ required: true }) name!: string;

    @Input() type?: GenericFieldTypeType | undefined = GenericFieldType.text;

    @Input() label?: string | undefined;

    @Input() optionLabel!: string;

    @Input() optionValue?: string | undefined;

    @Input() options?: unknown[] | undefined;

    @Input() value?: unknown;
}
