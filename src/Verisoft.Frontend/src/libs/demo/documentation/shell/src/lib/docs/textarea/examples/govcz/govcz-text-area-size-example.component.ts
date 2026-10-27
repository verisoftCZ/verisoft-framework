import { Component } from '@angular/core';
import {
  DOCS_CODE_LANG,
  DocumentationCodeBuilder,
} from '@verisoft/demo-documentation-ui';
import { TextareaComponent } from '@verisoft/ui-govcz';

const template = `
    <v-textarea [label]="'Rows = 1'" [rows]="1"></v-textarea>
    <v-textarea [label]="'Rows = 7'" [rows]="7"></v-textarea>
`;

@Component({
    selector: 'v-doc-govcz-text-area-size-example',
    imports: [TextareaComponent],
    template: template
})
export class GovczTextAreaSizeExampleComponent {
  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .buildCode();
}
