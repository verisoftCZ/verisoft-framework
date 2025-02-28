import { Component } from '@angular/core';
import {
  DOCS_CODE_LANG,
  DocumentationCodeBuilder,
} from '@verisoft/demo-documentation-ui';
import { CalendarComponent } from '@verisoft/ui-govcz';

const template = `
<div class="row">
  <div class="col col-md-6">
    <v-calendar label="Single" icon="pi-stop" selectionMode="single"/>
  </div>
  <div class="col col-md-6">
    <v-calendar label="Multiple" icon="pi-stop" selectionMode="multiple"/>
  </div>
  <div class="col col-md-6">
    <v-calendar label="Range" icon="pi-stop" selectionMode="range"/>
  </div>
`;

@Component({
  selector: 'v-doc-govcz-calendar-extra-example',
  imports: [CalendarComponent],
  standalone: true,
  template: template,
})
export class GovCzFormFieldExtraExampleComponent {
  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .buildCode();
}
