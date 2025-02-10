import { Component, inject } from '@angular/core';
import { DOCS_CONFIG, DocumentationBuilderService, DocumentationCofig, DocumentationPageComponent } from '@verisoft/demo-documentation-ui';
import { getDocumentation } from './input-group-doc.model';

@Component({
  template: `<v-doc-page
    title="Input Group"
    description="The InputGroup component allows users to group multiple input elements together. It provides a user-friendly interface for combining inputs, with options for customizing the display and behavior of the input group. The component supports various configurations, including custom labels, icons, and validation messages, ensuring a seamless and accessible user experience."
    [items]="items"
  ></v-doc-page>`,
  imports: [DocumentationPageComponent],
  standalone: true,
})
export class InputGroupDocComponent {
  builder = inject(DocumentationBuilderService);
  config = inject<DocumentationCofig>(DOCS_CONFIG);
  items = getDocumentation(this.builder, this.config.isPrimeng);
}