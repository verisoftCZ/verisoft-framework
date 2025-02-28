import { Component } from '@angular/core';
import {
  DOCS_CODE_LANG,
  DocumentationCodeBuilder,
} from '@verisoft/demo-documentation-ui';
import { TextfieldComponent } from '@verisoft/ui-govcz';

const template = `
    <v-textfield label="Small" size="small"></v-textfield>
    <v-textfield label="Medium" size="medium"></v-textfield>
    <v-textfield label="Large" size="large"></v-textfield>
`;

@Component({
  selector: 'v-doc-govcz-text-field-size-example',
  imports: [TextfieldComponent],
  standalone: true,
  template: template,
})
export class GovCzTextFieldSizeExampleComponent {
  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .buildCode();
}
