import { Component, inject } from '@angular/core';
import { DOCS_CONFIG, DocumentationBuilderService, DocumentationCofig, DocumentationPageComponent } from '@verisoft/demo-documentation-ui';
import { getDocumentation } from './tabview-doc.model';

@Component({
  template: `<v-doc-page
    title="TabView"
    description="TabView is a navigation component that organizes content into multiple tabs. It provides an intuitive interface for switching between different content sections, with support for dynamic content loading, form integration, and customizable styling."
    [items]="items"
  ></v-doc-page>`,
  imports: [DocumentationPageComponent],
  standalone: true,
})
export class TabViewDocComponent {
  builder = inject(DocumentationBuilderService);
  config = inject<DocumentationCofig>(DOCS_CONFIG);
  items = getDocumentation(this.builder, this.config.isPrimeng);
}