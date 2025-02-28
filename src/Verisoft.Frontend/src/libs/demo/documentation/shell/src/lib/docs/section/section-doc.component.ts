import { Component, inject } from '@angular/core';
import { DOCS_CONFIG, DocumentationBuilderService, DocumentationCofig, DocumentationPageComponent } from '@verisoft/demo-documentation-ui';
import { getDocumentation } from './section-doc.model';

@Component({
  template: `<v-doc-page
    title="Section"
    description="Section is a custom angular element designed to wrap and hide elements inside of it."
    [items]="items"
    ></v-doc-page>`,
  imports: [DocumentationPageComponent],
  standalone: true,
})
export class SectionDocComponent {
  builder = inject(DocumentationBuilderService);
  config = inject<DocumentationCofig>(DOCS_CONFIG);
  items = getDocumentation(this.builder, this.config.isPrimeng);
}
