import { Component, inject } from '@angular/core';
import {
  DOCS_CONFIG,
  DocumentationBuilderService,
  DocumentationCofig,
  DocumentationPageComponent,
} from '@verisoft/demo-documentation-ui';
import { getDocumentation } from './base-form-doc.model';

@Component({
  template: `<v-doc-page
    title="Base Form"
    description="The BaseForm directive provides a foundation for creating form components with built-in state management, validation, and data transformation. It offers a structured approach to handling forms with automatic dirty checking, validation state management, and consistent form submission handling."
    [items]="items"
  ></v-doc-page>`,
  imports: [DocumentationPageComponent],
  standalone: true,
})
export class BaseFormDocComponent {
  builder = inject(DocumentationBuilderService);
  config = inject<DocumentationCofig>(DOCS_CONFIG);
  items = getDocumentation(this.builder, this.config.isPrimeng);
}
