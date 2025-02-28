import { Component, TemplateRef } from '@angular/core';
import {
  DOCS_CODE_LANG,
  DocumentationCodeBuilder,
} from '@verisoft/demo-documentation-ui';
import { StepperItem } from '@verisoft/ui-core';
import { StepperComponent } from '@verisoft/ui-govcz';

const template = `
<div class="row">
  <div class="col col-md-12">
    <v-stepper [items]="items" layout="horizontal"/>
  </div>
</div>
`;

const ts = `
items: StepperItem[] = [
  {
    prefix: '1',
    header: 'Step 1',
    annotation: 'Annotation text 1',
  },
  {
    prefix: '2',
    header: 'Step 2',
    annotation: 'Annotation text 2',
  },
  {
    prefix: '3',
    header: 'Step 3',
    annotation: 'Annotation text 3',
  },
];
`

@Component({
  selector: 'v-doc-govcz-stepper-basic-example',
  imports: [StepperComponent],
  standalone: true,
  template: template,
})
export class GovCzStepperBasicExampleComponent {
  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .addCode(ts, DOCS_CODE_LANG.TS)
    .buildCode();

  items: StepperItem[] = [
    {
      header: 'Step 1',
      prefix: '1',
      annotation: 'Annotation text 1',
    },
    {
      header: 'Step 2',
      prefix: '2',
      annotation: 'Annotation text 2',
    },
    {
      header: 'Step 3',
      prefix: '3',
      annotation: 'Annotation text 3',
    },
  ];
}
