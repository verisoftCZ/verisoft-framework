import { Component, inject } from '@angular/core';
import { DOCS_CONFIG, DocumentationBuilderService, DocumentationCofig, DocumentationPageComponent } from '@verisoft/demo-documentation-ui';
import { getDocumentation } from './table-doc.model';

@Component({
  template: `<v-doc-page
    title="Table"
    description="The Table component provides a powerful and flexible system for displaying data"
    [items]="items"
  ></v-doc-page>`,
  imports: [DocumentationPageComponent],
  standalone: true,
})
export class TableDocComponent {
  builder = inject(DocumentationBuilderService);
  config = inject<DocumentationCofig>(DOCS_CONFIG);
  items = getDocumentation(this.builder, this.config.isPrimeng);
}