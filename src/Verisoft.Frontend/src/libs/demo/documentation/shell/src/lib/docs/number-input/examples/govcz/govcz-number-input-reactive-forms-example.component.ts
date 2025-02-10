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
import { NumberInputComponent } from '@verisoft/ui-govcz';

const template = `
<div class="row" [formGroup]="formGroup">
  <div class="col col-md-6">
    <v-number-input label="Age" formControlName="age"></v-number-input>
  </div>
  <div class="col col-md-6">
    <v-number-input label="Required" formControlName="required"></v-number-input>
  </div>
  <div class="col col-md-6">
    <v-number-input label="Disabled" formControlName="disabled"></v-number-input>
  </div>
</div>
`;

const typescript = `
  formGroup = new FormGroup({
    age: new FormControl<number>(0),
    required: new FormControl<number>(0, Validators.required),
    disabled: new FormControl<number>(0),
  });
  
  ngOnInit(): void {
    this.formGroup.get('disabled')?.disable();
  }
`;

@Component({
  selector: 'v-doc-govcz-number-input-reactive-forms-example',
  imports: [NumberInputComponent, ReactiveFormsModule],
  standalone: true,
  template: template,
})
export class GovCzNumberInputReactFormsExampleComponent implements OnInit {
  formGroup = new FormGroup({
    age: new FormControl<number>(0),
    required: new FormControl<number>(0, Validators.required),
    disabled: new FormControl<number>(0),
  });

  ngOnInit(): void {
    this.formGroup.get('disabled')?.disable();
  }

  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .addCode(typescript, DOCS_CODE_LANG.TS)
    .buildCode();
}
