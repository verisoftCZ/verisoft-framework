import { Component } from '@angular/core';
import {
  DOCS_CODE_LANG,
  DocumentationCodeBuilder,
} from '@verisoft/demo-documentation-ui';
import { ButtonComponent } from '@verisoft/ui-primeng';

const template = `
    <v-button label="Small" size="small"></v-button>
    <v-button label="Large" size="large" class="ms-2"></v-button>
`;

@Component({
  selector: 'v-doc-primeng-button-size-example',
  imports: [ButtonComponent],
  standalone: true,
  template: template,
})
export class PrimengButtonSizeExampleComponent {
  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .buildCode();
}
