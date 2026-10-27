import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import {
  DOCS_CODE_LANG,
  DocumentationCodeBuilder,
} from '@verisoft/demo-documentation-ui';
import { CheckboxComponent } from '@verisoft/ui-primeng';

const template = `
<div class="row">
  <div class="col-12">
    <v-checkbox label="Indeterminate Checkbox" [(ngModel)]="value" [indeterminate]="true"></v-checkbox>
  </div>
</div>
`;

@Component({
    selector: 'v-doc-primeng-checkbox-indeterminate-example',
    imports: [CheckboxComponent, FormsModule],
    template: template
})
export class PrimengCheckboxIndeterminateExampleComponent {
  value: boolean | null = null;

  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .buildCode();
}
