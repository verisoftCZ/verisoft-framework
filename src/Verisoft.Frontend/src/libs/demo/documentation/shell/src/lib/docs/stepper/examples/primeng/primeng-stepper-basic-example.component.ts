import { Component, TemplateRef } from '@angular/core';
import {
  DOCS_CODE_LANG,
  DocumentationCodeBuilder,
} from '@verisoft/demo-documentation-ui';
import { StepperItem } from '@verisoft/ui-core';
import { StepperComponent } from '@verisoft/ui-primeng';

const template = `
<div class="row">
  <div class="col col-md-12">
    <v-stepper [items]="items"/>
  </div>
</div>
`;

const ts = `
items: StepperItem[] = [
  {
    header: 'Step 1',
    template: {} as TemplateRef<any>, // Replace with actual TemplateRef
  },
  {
    header: 'Step 2',
    template: {} as TemplateRef<any>, // Replace with actual TemplateRef
  },
  {
    header: 'Step 3',
    template: {} as TemplateRef<any>, // Replace with actual TemplateRef
  },
];
`

@Component({
  selector: 'v-doc-primeng-stepper-basic-example',
  imports: [StepperComponent],
  standalone: true,
  template: template,
})
export class PrimengStepperBasicExampleComponent {
  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .addCode(ts, DOCS_CODE_LANG.TS)
    .buildCode();

  items: StepperItem[] = [
    {
      header: 'Step 1',
      template: {} as TemplateRef<any>, // Replace with actual TemplateRef
    },
    {
      header: 'Step 2',
      template: {} as TemplateRef<any>, // Replace with actual TemplateRef
    },
    {
      header: 'Step 3',
      template: {} as TemplateRef<any>, // Replace with actual TemplateRef
    },
  ];
}
