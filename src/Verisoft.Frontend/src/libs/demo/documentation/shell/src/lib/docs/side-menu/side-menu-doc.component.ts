import { Component, inject } from '@angular/core';
import { DOCS_CONFIG, DocumentationBuilderService, DocumentationCofig, DocumentationPageComponent } from '@verisoft/demo-documentation-ui';
import { getDocumentation } from './side-menu-doc.model';

@Component({
  template: `<v-doc-page
    title="Side menu"
    description="Side menu is custom angular element utilizing multiple services."
    [items]="items"
    ></v-doc-page>`,
  imports: [DocumentationPageComponent],
  standalone: true,
})
export class SideMenuDocComponent {
  builder = inject(DocumentationBuilderService);
  config = inject<DocumentationCofig>(DOCS_CONFIG);
  items = getDocumentation(this.builder, this.config.isPrimeng);
}
