import { Component } from '@angular/core';
import {
  DOCS_CODE_LANG,
  DocumentationCodeBuilder,
} from '@verisoft/demo-documentation-ui';
import { TextfieldComponent } from '@verisoft/ui-primeng';

const template = `
    <v-textfield label="Small" size="small"></v-textfield>
    <v-textfield label="Medium" size="medium"></v-textfield>
    <v-textfield label="Large" size="large"></v-textfield>
`;

@Component({
  selector: 'v-doc-primeng-text-field-size-example',
  imports: [TextfieldComponent],
  standalone: true,
  template: template,
})
export class PrimengTextFieldSizeExampleComponent {
  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .buildCode();
}
