import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import {
  DOCS_CODE_LANG,
  DocumentationCodeBuilder,
} from '@verisoft/demo-documentation-ui';
import { MultiselectComponent } from '@verisoft/ui-govcz';

const template = `
<div class="row">
  <div class="col col-md-6">
    <v-multiselect label="Age"></v-multiselect>
  </div>
  <div class="col col-md-6">
    <v-multiselect label="Disabled" [(ngModel)]="value" [disabled]="true"></v-multiselect>
  </div>
</div>
`;

@Component({
  selector: 'v-doc-govcz-multiselect-basic-example',
  imports: [MultiselectComponent, FormsModule],
  standalone: true,
  template: template,
})
export class GovCzMultiselectBasicExampleComponent {
  value = null;
  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .buildCode();
}
