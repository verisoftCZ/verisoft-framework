import { Component, inject } from '@angular/core';
import { DOCS_CONFIG, DocumentationBuilderService, DocumentationCofig, DocumentationPageComponent } from '@verisoft/demo-documentation-ui';
import { getDocumentation } from './page-header-doc.model';

@Component({
  template: `<v-doc-page
    title="Page Header"
    description="The PageHeader component provides a consistent header for pages, including a title, subtitle, and optional action buttons. It helps in maintaining a uniform look and feel across different sections of the application."
    [items]="items"
  ></v-doc-page>`,
  imports: [DocumentationPageComponent],
  standalone: true,
})
export class PageHeaderDocComponent {
  builder = inject(DocumentationBuilderService);
  config = inject<DocumentationCofig>(DOCS_CONFIG);
  items = getDocumentation(this.builder, this.config.isPrimeng);
}