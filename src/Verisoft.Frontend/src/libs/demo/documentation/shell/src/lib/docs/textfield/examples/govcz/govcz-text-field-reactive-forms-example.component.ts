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
import { TextfieldComponent } from '@verisoft/ui-govcz';

const template = `
<div class="row" [formGroup]="formGroup">
  <div class="col col-md-6">
    <v-textfield label="Name" formControlName="name"></v-textfield>
  </div>
  <div class="col col-md-6">
    <v-textfield label="Required" formControlName="required"></v-textfield>
  </div>
  <div class="col col-md-6">
    <v-textfield label="Disabled" formControlName="disabled"></v-textfield>
  </div>
</div>
`;

const typescript = `
  formGroup = new FormGroup({
    name: new FormControl<string>(''),
    required: new FormControl<string>('', Validators.required),
    disabled: new FormControl<string>(''),
  });
  
  ngOnInit(): void {
    this.formGroup.get('disabled')?.disable();
  }
`;

@Component({
  selector: 'v-doc-govcz-text-field-reactive-forms-example',
  imports: [TextfieldComponent, ReactiveFormsModule],
  standalone: true,
  template: template,
})
export class GovCzTextFieldReactFormsExampleComponent implements OnInit {
  formGroup = new FormGroup({
    name: new FormControl<string>(''),
    required: new FormControl<string>('', Validators.required),
    disabled: new FormControl<string>(''),
  });

  ngOnInit(): void {
    this.formGroup.get('disabled')?.disable();
  }

  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .addCode(typescript, DOCS_CODE_LANG.TS)
    .buildCode();
}
