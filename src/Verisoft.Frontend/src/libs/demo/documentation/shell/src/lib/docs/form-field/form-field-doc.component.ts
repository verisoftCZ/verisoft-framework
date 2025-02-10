import { Component, inject } from '@angular/core';
import { DOCS_CONFIG, DocumentationBuilderService, DocumentationCofig, DocumentationPageComponent } from '@verisoft/demo-documentation-ui';
import { getDocumentation } from './form-field-doc.model';

@Component({
  template: `<v-doc-page
    title="FormField"
    description="Formfield is custom wrapper around input elements designed to be always the same style and appearance."
    [items]="items"
    ></v-doc-page>`,
  imports: [DocumentationPageComponent],
  standalone: true,
})
export class FormFieldDocComponent {
  builder = inject(DocumentationBuilderService);
  config = inject<DocumentationCofig>(DOCS_CONFIG);
  items = getDocumentation(this.builder, this.config.isPrimeng);
}
