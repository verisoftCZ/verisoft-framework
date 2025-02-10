import { JsonPipe } from '@angular/common';
import { Component, inject } from '@angular/core';
import { DOCS_CONFIG, DocumentationBuilderService, DocumentationCofig, DocumentationPageComponent } from '@verisoft/demo-documentation-ui';
import { getDocumentation } from './snackbar-doc.model';

@Component({
  template: `<v-doc-page
    title="Snackbar"
    description="Snackbar is an extension to a prime-ng component <strong>p-toast</strong>."
    [items]="items"
  ></v-doc-page>`,
  imports: [DocumentationPageComponent, JsonPipe],
  standalone: true,
})
export class SnackbarDocComponent {
  builder = inject(DocumentationBuilderService);
  config = inject<DocumentationCofig>(DOCS_CONFIG);
  items = getDocumentation(this.builder, this.config.isPrimeng);
}
