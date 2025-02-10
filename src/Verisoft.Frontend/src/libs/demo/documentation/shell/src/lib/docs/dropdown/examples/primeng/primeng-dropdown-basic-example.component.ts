import { Component } from '@angular/core';
import {
  DOCS_CODE_LANG,
  DocumentationCodeBuilder,
} from '@verisoft/demo-documentation-ui';
import { DropdownComponent } from '@verisoft/ui-primeng';

const template = `
<v-dropdown label="Label" [options]="options" />
`;

const ts = `
options: string[] = ['First', 'Second', 'Third'];
`;

@Component({
  selector: 'v-doc-primeng-dropdown-basic-example',
  imports: [DropdownComponent],
  standalone: true,
  template: template,
})
export class PrimengDropdownBasicExampleComponent {
  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .addCode(ts, DOCS_CODE_LANG.TS)
    .buildCode();

  options: string[] = ['First', 'Second', 'Third'];
}
