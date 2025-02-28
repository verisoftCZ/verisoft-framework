import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import {
  DOCS_CODE_LANG,
  DocumentationCodeBuilder,
} from '@verisoft/demo-documentation-ui';
import { TristatecheckboxComponent } from '@verisoft/ui-primeng';

const template = `
<div class="row">
  <div class="col-3">
    <v-tristatecheckbox label="Name"></v-tristatecheckbox>
  </div>
  <div class="col-3">
    <v-tristatecheckbox label="Required" [required]="true"></v-tristatecheckbox>
  </div>
  <div class="col-3">
      <v-tristatecheckbox label="Disabled" [(ngModel)]="value" [disabled]="true"></v-tristatecheckbox>
  </div>
  <div class="col-3">
    <v-tristatecheckbox label="Readonly" [readonly]="true"></v-tristatecheckbox>
  </div>
</div>
`;

@Component({
  selector: 'v-doc-primeng-tristate-checkbox-basic-example',
  imports: [TristatecheckboxComponent, FormsModule],
  standalone: true,
  template: template,
})
export class PrimengTristateCheckboxBasicExampleComponent {
  value: boolean | null = null;
  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .buildCode();
}
