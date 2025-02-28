import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import {
  DOCS_CODE_LANG,
  DocumentationCodeBuilder,
} from '@verisoft/demo-documentation-ui';
import { GovSizePipe, TextfieldComponent } from '@verisoft/ui-govcz';

const template = `
<div class="row">
  <div class="col col-md-6">
    <v-textfield label="Name" size="medium"></v-textfield>
  </div>
  <div class="col col-md-6">
    <v-textfield label="Required" [required]="true"></v-textfield>
  </div>
  <div class="col col-md-6">
    <v-textfield label="Disabled" [(ngModel)]="value" [disabled]="true"></v-textfield>
  </div>
</div>
`;

@Component({
  selector: 'v-doc-govcz-text-field-basic-example',
  imports: [TextfieldComponent, FormsModule],
  standalone: true,
  template: template,
})
export class GovCzTextFieldBasicExampleComponent {
  value: string | null = null;
  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .buildCode();
}
