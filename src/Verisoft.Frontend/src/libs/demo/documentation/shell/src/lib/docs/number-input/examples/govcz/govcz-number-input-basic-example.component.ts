import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import {
  DOCS_CODE_LANG,
  DocumentationCodeBuilder,
} from '@verisoft/demo-documentation-ui';
import { NumberInputComponent } from '@verisoft/ui-govcz';

const template = `
<div class="row">
  <div class="col col-md-4">
    <v-number-input label="Age" [min]="0" [max]="120"></v-number-input>
  </div>
  <div class="col col-md-4">
    <v-number-input label="Decimal" [decimals]="2"></v-number-input>
  </div>
  <div class="col col-md-4">
    <v-number-input label="Disabled" [(ngModel)]="value" [disabled]="true"></v-number-input>
  </div>
</div>
`;

@Component({
    selector: 'v-doc-govcz-number-input-basic-example',
    imports: [NumberInputComponent, FormsModule],
    template: template
})
export class GovCzNumberInputBasicExampleComponent {
  value: number | null = null;
  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .buildCode();
}
