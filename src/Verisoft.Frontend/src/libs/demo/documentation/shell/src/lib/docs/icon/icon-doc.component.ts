import { Component, inject } from '@angular/core';
import {
  DOCS_CONFIG,
  DocumentationBuilderService,
  DocumentationCofig,
  DocumentationPageComponent,
} from '@verisoft/demo-documentation-ui';
import { getDocumentation } from './icon-doc.model';

@Component({
    template: `<v-doc-page
    title="Icon"
    description="Icon is a customizable and reusable component that can be used to display icons from multiple icon libraries in an Angular application."
    [items]="items" />`,
    imports: [DocumentationPageComponent]
})
export class IconDocComponent {
 private readonly builder = inject(DocumentationBuilderService);
 private readonly config = inject<DocumentationCofig>(DOCS_CONFIG);
 
 protected readonly items = getDocumentation(this.builder, this.config.isPrimeng);
}

