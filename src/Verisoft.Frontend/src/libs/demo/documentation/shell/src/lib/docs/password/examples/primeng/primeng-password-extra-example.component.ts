import { Component } from '@angular/core';
import {
  DOCS_CODE_LANG,
  DocumentationCodeBuilder,
} from '@verisoft/demo-documentation-ui';
import { PasswordComponent } from '@verisoft/ui-primeng';

const template = `
<v-password 
    label="Password"
    placeholder="Placeholder"
    formControlName="password"
    [toggleMask]="false"
    [feedback]="true"
/>
`;

@Component({
  selector: 'v-primeng-doc-password-extra-example',
  imports: [PasswordComponent],
  standalone: true,
  template: template,
})
export class PrimengPasswordExtraExampleComponent {
  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .buildCode();
}
