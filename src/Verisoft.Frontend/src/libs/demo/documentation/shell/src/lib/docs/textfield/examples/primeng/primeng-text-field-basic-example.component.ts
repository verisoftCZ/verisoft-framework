import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import {
  DOCS_CODE_LANG,
  DocumentationCodeBuilder,
} from '@verisoft/demo-documentation-ui';
import { TextfieldComponent } from '@verisoft/ui-primeng';

const template = `
<div class="row">
  <div class="col col-md-6">
    <v-textfield label="Name"></v-textfield>
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
  selector: 'v-doc-primeng-text-field-basic-example',
  imports: [TextfieldComponent, FormsModule],
  standalone: true,
  template: template,
})
export class PrimengTextFieldBasicExampleComponent {
  value: string | null = null;
  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .buildCode();
}
