import { Component } from '@angular/core';
import {
  DOCS_CODE_LANG,
  DocumentationCodeBuilder,
} from '@verisoft/demo-documentation-ui';
import { PasswordComponent } from '@verisoft/ui-govcz';

const template = `
<v-password label="Enter your password" />
`;

@Component({
  selector: 'v-doc-govcz-password-basic-example',
  imports: [PasswordComponent],
  standalone: true,
  template: template,
})
export class GovCzPasswordBasicExampleComponent {
  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .buildCode();
}
