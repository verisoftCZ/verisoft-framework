import { Component, inject } from '@angular/core';
import {
  DOCS_CONFIG,
  DocumentationBuilderService,
  DocumentationCofig,
  DocumentationPageComponent,
} from '@verisoft/demo-documentation-ui';
import { getDocumentation } from './feature-list-doc.model';

@Component({
  template: `<v-doc-page
    title="Feature List"
    description="Feature list is a custom component combining a table with bound filter component to ease usage and implementation."
    [items]="items"
  ></v-doc-page>`,
  imports: [DocumentationPageComponent],
  standalone: true,
})
export class FeatureListDocComponent {
  builder = inject(DocumentationBuilderService);
  config = inject<DocumentationCofig>(DOCS_CONFIG);
  items = getDocumentation(this.builder, this.config.isPrimeng);
}
