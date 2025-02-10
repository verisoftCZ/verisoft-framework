import { Component, inject } from '@angular/core';
import { DOCS_CONFIG, DocumentationBuilderService, DocumentationCofig, DocumentationPageComponent } from '@verisoft/demo-documentation-ui';
import { getDocumentation } from './loader-doc.model';
@Component({
  template: `<v-doc-page
    title="Loader"
    description="Loader is a component that is used in instances of either loading data or waiting for an action to finish."
    [items]="items"
    ></v-doc-page>`,
  imports: [DocumentationPageComponent],
  standalone: true,
})
export class LoaderDocComponent {
  builder = inject(DocumentationBuilderService);
  config = inject<DocumentationCofig>(DOCS_CONFIG);
  items = getDocumentation(this.builder, this.config.isPrimeng);
}
