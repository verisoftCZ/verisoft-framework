import { Component } from '@angular/core';
import {
  DOCS_CODE_LANG,
  DocumentationCodeBuilder,
} from '@verisoft/demo-documentation-ui';
import { RadioButtonItem } from '@verisoft/ui-core';
import { RadioButtonComponent } from '@verisoft/ui-govcz';

const template = `
<v-radiobutton label="Example radiobutton" [items]="items" />
`;

const typescript = `items: RadioButtonItem<any>[] = [ { id: '123', value: 'Yes' }, { id: '321', value: 'No' } ];`;

@Component({
  selector: 'v-doc-govcz-radiobutton-basic-example',
  imports: [RadioButtonComponent],
  standalone: true,
  template: template,
})
export class GovCzRadiobuttonBasicExampleComponent {
  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .addCode(typescript, DOCS_CODE_LANG.TS)
    .buildCode();

  items: RadioButtonItem<any>[] = [
    { id: '123', value: 'Yes' },
    { id: '321', value: 'No' },
  ];
}
