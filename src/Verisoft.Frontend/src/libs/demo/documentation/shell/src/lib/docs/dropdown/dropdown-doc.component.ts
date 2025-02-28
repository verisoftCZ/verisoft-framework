import { Component, inject } from '@angular/core';
import {
  DOCS_CONFIG,
  DocumentationBuilderService,
  DocumentationCofig,
  DocumentationPageComponent,
} from '@verisoft/demo-documentation-ui';
import { getDocumentation } from './dropdown-doc.model';

@Component({
  template: `<v-doc-page
    title="Dropdown"
    description="Dropdown is an extension to the prime-ng component <strong>p-dropdown</strong>."
    [items]="items"
  ></v-doc-page>`,
  imports: [DocumentationPageComponent],
  standalone: true,
})
export class DropdownDocComponent {
  builder = inject(DocumentationBuilderService);
  config = inject<DocumentationCofig>(DOCS_CONFIG);
  items = getDocumentation(this.builder, this.config.isPrimeng);
}
