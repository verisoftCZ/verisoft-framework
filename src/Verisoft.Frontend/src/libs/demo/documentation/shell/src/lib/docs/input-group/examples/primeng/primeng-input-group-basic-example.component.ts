import { Component } from '@angular/core';
import {
  DOCS_CODE_LANG,
  DocumentationCodeBuilder,
} from '@verisoft/demo-documentation-ui';
import { InputGroupComponent } from '@verisoft/ui-primeng';

const template = `
<div class="row">
  <div class="col col-md-6">
    <v-input-group 
      label="Username" 
      [items]="[
        { icon: 'pi pi-user', position: 'left' }
      ]">
      <input pInputText placeholder="Username" />
    </v-input-group>
  </div>
  <div class="col col-md-6">
    <v-input-group 
      label="Price" 
      [items]="[
        { text: '$', position: 'left' },
        { text: '.00', position: 'right' }
      ]">
      <input pInputText placeholder="Amount" />
    </v-input-group>
  </div>
</div>
`;

@Component({
  selector: 'v-doc-primeng-input-group-basic-example',
  imports: [InputGroupComponent],
  standalone: true,
  template: template,
})
export class PrimengInputGroupBasicExampleComponent {
  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .buildCode();
}
