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
    selector: 'v-doc-primeng-password-extra-example',
    imports: [PasswordComponent],
    template: template
})
export class PrimengPasswordExtraExampleComponent {
  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .buildCode();
}
