import { Component, inject } from '@angular/core';
import { DOCS_CONFIG, DocumentationBuilderService, DocumentationCofig, DocumentationPageComponent } from '@verisoft/demo-documentation-ui';
import { getDocumentation } from './switch-doc.model';

@Component({
  template: `<v-doc-page
    title="Switch"
    description="Switch is an extension to a prime-ng component <strong>p-inputSwitch.</strong>."
    [items]="items"
  ></v-doc-page>`,
  imports: [DocumentationPageComponent],
  standalone: true,
})
export class SwitchDocComponent {
  builder = inject(DocumentationBuilderService);
  config = inject<DocumentationCofig>(DOCS_CONFIG);
  items = getDocumentation(this.builder, this.config.isPrimeng);
}
