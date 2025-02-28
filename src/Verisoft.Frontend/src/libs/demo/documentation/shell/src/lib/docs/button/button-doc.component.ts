import { Component, inject } from '@angular/core';
import {
  DOCS_CONFIG,
  DocumentationBuilderService,
  DocumentationCofig,
  DocumentationPageComponent,
} from '@verisoft/demo-documentation-ui';
import { getDocumentation } from './button-doc.model';

@Component({
  template: `<v-doc-page
    title="Button"
    description="Button is a customizable and reusable component that can be used to trigger actions in an Angular application."
    [items]="items"
  ></v-doc-page>`,
  imports: [DocumentationPageComponent],
  standalone: true,
})
export class ButtonDocComponent {
  builder = inject(DocumentationBuilderService);
  config = inject<DocumentationCofig>(DOCS_CONFIG);
  items = getDocumentation(this.builder, this.config.isPrimeng);
}
