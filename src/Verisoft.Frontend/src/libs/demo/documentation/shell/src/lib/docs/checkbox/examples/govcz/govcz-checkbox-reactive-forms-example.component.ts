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
import { CheckboxComponent } from '@verisoft/ui-govcz';

const template = `
<div class="row" [formGroup]="formGroup">
  <div class="col-3">
    <v-checkbox label="Name" formControlName="name"></v-checkbox>
  </div>
  <div class="col-3">
    <v-checkbox label="Required" formControlName="required" [required]="true"></v-checkbox>
  </div>
  <div class="col-3">
    <v-checkbox label="Disabled" formControlName="disabled"></v-checkbox>
  </div>
  <div class="col-3">
    <v-checkbox label="Readonly" formControlName="readonly" [readonly]="true"></v-checkbox>
  </div>
</div>
`;

const typescript = `
  formGroup = new FormGroup({
    name: new FormControl<boolean | null>(null),
    required: new FormControl<boolean | null>(null, Validators.required),
    disabled: new FormControl<boolean | null>(null),
    readonly: new FormControl<boolean | null>(null),
  });
  
  ngOnInit(): void {
    this.formGroup.get('disabled')?.disable();
  }
`;

@Component({
  selector: 'v-doc-govcz-checkbox-reactive-forms-example',
  imports: [CheckboxComponent, ReactiveFormsModule],
  standalone: true,
  template: template,
})
export class GovCzCheckboxReactFormsExampleComponent implements OnInit {
  formGroup = new FormGroup({
    name: new FormControl<boolean | null>(null),
    required: new FormControl<boolean | null>(null, Validators.required),
    disabled: new FormControl<boolean | null>(null),
    readonly: new FormControl<boolean | null>(null),
  });

  ngOnInit(): void {
    this.formGroup.get('disabled')?.disable();
  }

  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .addCode(typescript, DOCS_CODE_LANG.TS)
    .buildCode();
}
