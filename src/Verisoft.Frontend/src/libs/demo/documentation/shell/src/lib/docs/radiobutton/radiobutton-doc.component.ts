import { Component, inject } from '@angular/core';
import { DOCS_CONFIG, DocumentationBuilderService, DocumentationCofig, DocumentationPageComponent } from '@verisoft/demo-documentation-ui';
import { getDocumentation } from './radiobutton-doc.model';

@Component({
  template: `<v-doc-page
    title="Radiobutton"
    description="Radiobutton is a custom angular element designed to integrate a group of ng-prime's <strong>p-radioButton</strong> elements."
    [items]="items"
    ></v-doc-page>`,
  imports: [DocumentationPageComponent],
  standalone: true,
})
export class RadiobuttonDocComponent {
  builder = inject(DocumentationBuilderService);
  config = inject<DocumentationCofig>(DOCS_CONFIG);
  items = getDocumentation(this.builder, this.config.isPrimeng);
}
