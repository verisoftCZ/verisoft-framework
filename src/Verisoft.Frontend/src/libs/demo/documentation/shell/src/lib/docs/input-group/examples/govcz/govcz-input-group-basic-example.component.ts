import { Component } from '@angular/core';
import {
  DOCS_CODE_LANG,
  DocumentationCodeBuilder,
} from '@verisoft/demo-documentation-ui';
import { InputGroupComponent } from '@verisoft/ui-govcz';

const template = `
<div class="row">
  <div class="col col-md-6">
    <v-input-group 
      prefix="user"
      label="Username"/>
  </div>
  <div class="col col-md-6">
    <v-input-group 
      label="Price"
      sufix="$"/>
  </div>
</div>
`;

@Component({
  selector: 'v-doc-govcz-input-group-basic-example',
  imports: [InputGroupComponent],
  standalone: true,
  template: template,
})
export class GovCzInputGroupBasicExampleComponent {
  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .buildCode();
}
