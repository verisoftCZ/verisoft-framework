import { Component, inject } from '@angular/core';
import { DOCS_CONFIG, DocumentationBuilderService, DocumentationCofig, DocumentationPageComponent } from '@verisoft/demo-documentation-ui';
import { getDocumentation } from './text-field-doc.model';

@Component({
  template: `<v-doc-page
    title="TextField"
    description="TextField is an extension to standard input element with validation messages and label."
    [items]="items"
  ></v-doc-page>`,
  imports: [DocumentationPageComponent],
  standalone: true,
})
export class TextFieldDocComponent {
  builder = inject(DocumentationBuilderService);
  config = inject<DocumentationCofig>(DOCS_CONFIG);
  items = getDocumentation(this.builder, this.config.isPrimeng);
}
