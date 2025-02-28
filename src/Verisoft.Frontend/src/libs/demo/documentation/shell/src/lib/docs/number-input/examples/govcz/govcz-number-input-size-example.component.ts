import { Component } from '@angular/core';
import {
  DOCS_CODE_LANG,
  DocumentationCodeBuilder,
} from '@verisoft/demo-documentation-ui';
import { NumberInputComponent } from '@verisoft/ui-govcz';

const template = `
    <v-number-input label="Small" size="small"></v-number-input>
    <v-number-input label="Medium" size="medium"></v-number-input>
    <v-number-input label="Large" size="large"></v-number-input>
`;

@Component({
  selector: 'v-doc-govcz-number-input-size-example',
  imports: [NumberInputComponent],
  standalone: true,
  template: template,
})
export class GovCzNumberInputSizeExampleComponent {
  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .buildCode();
}
