import { Component } from '@angular/core';
import {
  DOCS_CODE_LANG,
  DocumentationCodeBuilder,
} from '@verisoft/demo-documentation-ui';
import { TextareaComponent } from '@verisoft/ui-primeng';

const template = `
    <v-textarea [label]="'Rows = 1'" [rows]="1"></v-textarea>
    <v-textarea [label]="'Rows = 7'" [rows]="7"></v-textarea>
`;

@Component({
  selector: 'v-doc-primeng-text-area-size-example',
  imports: [TextareaComponent],
  standalone: true,
  template: template,
})
export class PrimengTextAreaSizeExampleComponent {
  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .buildCode();
}
