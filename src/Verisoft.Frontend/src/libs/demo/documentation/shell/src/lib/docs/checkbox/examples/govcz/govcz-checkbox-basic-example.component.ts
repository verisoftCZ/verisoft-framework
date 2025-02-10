import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import {
  DOCS_CODE_LANG,
  DocumentationCodeBuilder,
} from '@verisoft/demo-documentation-ui';
import { CheckboxComponent } from '@verisoft/ui-govcz';

const template = `
<div class="row">
  <div class="col-3">
    <v-checkbox label="Name"></v-checkbox>
  </div>
  <div class="col-3">
    <v-checkbox label="Required" [required]="true"></v-checkbox>
  </div>
  <div class="col-3">
      <v-checkbox label="Disabled" [(ngModel)]="value" [disabled]="true"></v-checkbox>
  </div>
  <div class="col-3">
    <v-checkbox label="Readonly" [readonly]="true"></v-checkbox>
  </div>
</div>
`;

@Component({
  selector: 'v-doc-govcz-checkbox-basic-example',
  imports: [CheckboxComponent, FormsModule],
  standalone: true,
  template: template,
})
export class GovCzCheckboxBasicExampleComponent {
  value: boolean | null = null;
  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .buildCode();
}
