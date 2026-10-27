import { Component, inject } from '@angular/core';
import { DOCS_CONFIG, DocumentationBuilderService, DocumentationCofig, DocumentationPageComponent } from '@verisoft/demo-documentation-ui';
import { getDocumentation } from './dropdown-button-doc.model';

@Component({
    template: `<v-doc-page
    title="Dropdown Button"
    description="The DropdownButton component provides a powerful button with dropdown menu functionality for displaying multiple action options"
    [items]="items"
  ></v-doc-page>`,
    imports: [DocumentationPageComponent]
})
export class DropdownButtonDocComponent {
  builder = inject(DocumentationBuilderService);
  config = inject<DocumentationCofig>(DOCS_CONFIG);
  items = getDocumentation(this.builder, this.config.isPrimeng);
}
