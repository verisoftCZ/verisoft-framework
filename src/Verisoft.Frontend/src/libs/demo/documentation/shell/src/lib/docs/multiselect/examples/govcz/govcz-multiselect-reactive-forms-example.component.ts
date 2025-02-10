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
import { MultiselectComponent } from '@verisoft/ui-govcz';

const template = `
<div class="row" [formGroup]="formGroup">
  <div class="col col-md-6">
    <v-multiselect label="Options" formControlName="options" [options]="options" optionLabel="name" optionValue="id"></v-multiselect>
  </div>
  <div class="col col-md-6">
    <v-multiselect label="Required" formControlName="required" [options]="options" optionLabel="name" optionValue="id"></v-multiselect>
  </div>
  <div class="col col-md-6">
    <v-multiselect label="Disabled" formControlName="disabled" [options]="options" optionLabel="name" optionValue="id"></v-multiselect>
  </div>
</div>
`;

const typescript = `
  formGroup = new FormGroup({
    options: new FormControl<number[]>([], Validators.required),
    required: new FormControl<number[]>([], Validators.required),
    disabled: new FormControl<number[]>([]),
  });

  options = [
    { id: 1, name: 'Option 1' },
    { id: 2, name: 'Option 2' },
    { id: 3, name: 'Option 3' },
  ];

  ngOnInit(): void {
    this.formGroup.get('disabled')?.disable();
  }
`;

@Component({
  selector: 'v-doc-govcz-multiselect-reactive-forms-example',
  imports: [ReactiveFormsModule, MultiselectComponent],
  standalone: true,
  template: template,
})
export class GovCzMultiselectReactiveFormsExampleComponent implements OnInit {
  formGroup = new FormGroup({
    options: new FormControl<number[]>([], Validators.required),
    required: new FormControl<number[]>([], Validators.required),
    disabled: new FormControl<number[]>([]),
  });

  options = [
    { id: 1, name: 'Option 1' },
    { id: 2, name: 'Option 2' },
    { id: 3, name: 'Option 3' },
  ];

  ngOnInit(): void {
    this.formGroup.get('disabled')?.disable();
  }

  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .addCode(typescript, 'index.ts')
    .buildCode();
}
