import { Component } from '@angular/core';
import {
  DOCS_CODE_LANG,
  DocumentationCodeBuilder,
} from '@verisoft/demo-documentation-ui';
import { FormFieldComponent, TextfieldComponent } from '@verisoft/ui-primeng';

const template = `
<v-form-field label="Basic label">
  <v-textfield class="w-100" />
</v-form-field>
`;

@Component({
  selector: 'v-doc-primeng-form-field-basic-example',
  imports: [FormFieldComponent, TextfieldComponent],
  standalone: true,
  template: template,
})
export class PrimengFormFieldBasicExampleComponent {
  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .buildCode();
}
