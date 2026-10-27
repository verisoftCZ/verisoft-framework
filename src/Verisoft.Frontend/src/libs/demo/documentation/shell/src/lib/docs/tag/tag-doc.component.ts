import { Component, inject } from '@angular/core';
import {
  DOCS_CONFIG,
  DocumentationBuilderService,
  DocumentationCofig,
  DocumentationPageComponent,
} from '@verisoft/demo-documentation-ui';
import { getDocumentation } from './tag-doc.model';

@Component({
    template: `<v-doc-page
    title="Tag"
    description="Tag is a customizable and reusable component that can be used to display labels, statuses, and categories in an Angular application."
    [items]="items"
  ></v-doc-page>`,
    imports: [DocumentationPageComponent]
})
export class TagDocComponent {
  private readonly builder = inject(DocumentationBuilderService);
  private readonly config = inject<DocumentationCofig>(DOCS_CONFIG);
  
  protected readonly items = getDocumentation(this.builder, this.config.isPrimeng);
}

