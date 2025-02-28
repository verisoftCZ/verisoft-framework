import { Component, inject } from '@angular/core';
import { DOCS_CONFIG, DocumentationBuilderService, DocumentationCofig, DocumentationPageComponent } from '@verisoft/demo-documentation-ui';
import { getDocumentation } from './stepper-doc.model';

@Component({
  template: `<v-doc-page
    title="Stepper"
    description="Stepper is a customizable and reusable component that can be used to step through multiple items in an Angular application."
    [items]="items"
  ></v-doc-page>`,
  imports: [DocumentationPageComponent],
  standalone: true,
})
export class StepperDocComponent {
  builder = inject(DocumentationBuilderService);
  config = inject<DocumentationCofig>(DOCS_CONFIG);
  items = getDocumentation(this.builder, this.config.isPrimeng);
}
