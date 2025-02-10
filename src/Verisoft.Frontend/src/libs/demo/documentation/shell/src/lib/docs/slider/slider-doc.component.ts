import { Component, inject } from '@angular/core';
import { DOCS_CONFIG, DocumentationBuilderService, DocumentationCofig, DocumentationPageComponent } from '@verisoft/demo-documentation-ui';
import { getDocumentation } from './slider-doc.model';

@Component({
  template: `<v-doc-page
    title="Slider"
    description="The Slider component allows users to select a value or range of values by dragging a handle along a track. It provides a user-friendly interface for selecting numeric values, with options for customizing the display and behavior of the slider. The component supports various configurations, including custom labels, icons, and validation messages, ensuring a seamless and accessible user experience."
    [items]="items"
  ></v-doc-page>`,
  imports: [DocumentationPageComponent],
  standalone: true,
})
export class SliderDocComponent {
  builder = inject(DocumentationBuilderService);
  config = inject<DocumentationCofig>(DOCS_CONFIG);
  items = getDocumentation(this.builder, this.config.isPrimeng);
}