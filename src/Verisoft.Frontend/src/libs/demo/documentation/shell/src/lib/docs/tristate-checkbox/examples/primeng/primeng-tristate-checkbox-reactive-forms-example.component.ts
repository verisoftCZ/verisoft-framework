import { Component, OnInit } from '@angular/core';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import {
  DOCS_CODE_LANG,
  DocumentationCodeBuilder,
} from '@verisoft/demo-documentation-ui';
import { TristatecheckboxComponent } from '@verisoft/ui-primeng';

const template = `
<div class="row" [formGroup]="formGroup">
  <div class="col-3">
    <v-tristatecheckbox label="Name" formControlName="name"></v-tristatecheckbox>
  </div>
  <div class="col-3">
    <v-tristatecheckbox label="Required" formControlName="required" [required]="true"></v-tristatecheckbox>
  </div>
  <div class="col-3">
    <v-tristatecheckbox label="Disabled" formControlName="disabled"></v-tristatecheckbox>
  </div>
  <div class="col-3">
    <v-tristatecheckbox label="Readonly" formControlName="readonly" [readonly]="true"></v-tristatecheckbox>
  </div>
</div>
`;

@Component({
  selector: 'v-doc-primeng-tristate-checkbox-reactive-forms-example',
  imports: [TristatecheckboxComponent, ReactiveFormsModule],
  standalone: true,
  template: template,
})
export class PrimengTristateCheckboxReactFormsExampleComponent {
  formGroup = new FormGroup({
    name: new FormControl<boolean | null>(null),
    required: new FormControl<boolean | null>(null, Validators.required),
    disabled: new FormControl<boolean | null>({value: null, disabled: true}),
    readonly: new FormControl<boolean | null>(null),
  });

  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .buildCode();
}
