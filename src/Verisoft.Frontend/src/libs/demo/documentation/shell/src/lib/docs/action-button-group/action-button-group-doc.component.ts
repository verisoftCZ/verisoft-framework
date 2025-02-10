import { Component, inject } from '@angular/core';
import {
  DOCS_CONFIG,
  DocumentationBuilderService,
  DocumentationCofig,
  DocumentationPageComponent,
} from '@verisoft/demo-documentation-ui';
import { getDocumentation } from './action-button-group-doc.model';

@Component({
  template: `<v-doc-page
    title="Action Button Group"
    description="The ActionButtonGroup is a powerful component for organizing and displaying multiple action buttons in a responsive and user-friendly way. It automatically manages button visibility based on screen size, moving excess buttons into a dropdown menu. The component supports full customization of button appearances, including sizes, icons, colors, and tooltips. It's particularly useful for toolbars, action menus, and command interfaces where multiple actions need to be presented in a space-efficient manner."
    [items]="items"
  ></v-doc-page>`,
  imports: [DocumentationPageComponent],
  standalone: true,
})
export class ActionButtonGroupDocComponent {
  builder = inject(DocumentationBuilderService);
  config = inject<DocumentationCofig>(DOCS_CONFIG);
  items = getDocumentation(this.builder, this.config.isPrimeng);
}
