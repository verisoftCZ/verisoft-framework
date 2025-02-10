import { Component, inject } from '@angular/core';
import { DOCS_CONFIG, DocumentationBuilderService, DocumentationCofig, DocumentationPageComponent } from '@verisoft/demo-documentation-ui';
import { getDocumentation } from './number-input-doc.model';

@Component({
  template: `<v-doc-page
    title="Number input"
    description="NumberInput is an extension of the standard input element specifically designed for numeric values. It includes built-in validation messages to ensure that the input meets specified criteria (e.g., range, integer vs. decimal) and features a label for better accessibility and user guidance."
    [items]="items"
  ></v-doc-page>`,
  imports: [DocumentationPageComponent],
  standalone: true,
})
export class NumberInputDocComponent {
  builder = inject(DocumentationBuilderService);
  config = inject<DocumentationCofig>(DOCS_CONFIG);
  items = getDocumentation(this.builder, this.config.isPrimeng);
}
