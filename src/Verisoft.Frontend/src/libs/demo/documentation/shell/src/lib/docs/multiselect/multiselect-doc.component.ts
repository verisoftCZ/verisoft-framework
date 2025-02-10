import { Component, inject } from '@angular/core';
import { DOCS_CONFIG, DocumentationBuilderService, DocumentationCofig, DocumentationPageComponent } from '@verisoft/demo-documentation-ui';
import { getDocumentation } from './multiselect-doc.model';

@Component({
  template: `<v-doc-page
    title="MultiSelect"
    description="Multiselect is a versatile component that allows users to select multiple options from a dropdown list. It provides a user-friendly interface for selecting multiple items, with options for customizing the display and behavior of the dropdown. The component supports various configurations, including custom labels, icons, and validation messages, ensuring a seamless and accessible user experience."    [items]="items"
  ></v-doc-page>`,
  imports: [DocumentationPageComponent],
  standalone: true,
})
export class MultiselectDocComponent {
  builder = inject(DocumentationBuilderService);
  config = inject<DocumentationCofig>(DOCS_CONFIG);
  items = getDocumentation(this.builder, this.config.isPrimeng);
}
