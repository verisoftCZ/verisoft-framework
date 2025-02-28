import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import {
  DOCS_CODE_LANG,
  DocumentationCodeBuilder,
} from '@verisoft/demo-documentation-ui';
import { SwitchComponent } from '@verisoft/ui-govcz';

const template = `
<div class="row">
  <div class="col">
    <v-switch label="Name"></v-switch>
  </div>
  <div class="col">
    <v-switch label="Required" [required]="true"></v-switch>
  </div>
  <div class="col">
    <v-switch label="Disabled" [(ngModel)]="value" [disabled]="true"></v-switch>
  </div>
</div>
`;

@Component({
  selector: 'v-doc-govcz-switch-basic-example',
  imports: [SwitchComponent, FormsModule],
  standalone: true,
  template: template,
})
export class GovCzSwitchBasicExampleComponent {
  value: boolean | null = null;
  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .buildCode();
}
