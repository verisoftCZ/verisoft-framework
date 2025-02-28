import { Component, OnInit } from '@angular/core';
import {
  FormControl,
  FormGroup,
  Validators,
  ReactiveFormsModule,
} from '@angular/forms';
import {
  DOCS_CODE_LANG,
  DocumentationCodeBuilder,
} from '@verisoft/demo-documentation-ui';
import { DropdownComponent } from '@verisoft/ui-govcz';

const template = `
<div class="row" [formGroup]="formGroup">
  <div class="col col-md-6">
    <v-dropdown 
      label="Searchable"
      formControlName="searchable"
      [options]="first"
      [optionLabel]="'name'"
      [optionValue]="'value'" />
  </div>
  <div class="col col-md-6">
    <v-dropdown 
      label="Required"
      formControlName="required"
      [editable]="false"
      [options]="second" />
  </div>
  <div class="col col-md-6">
    <v-dropdown
      label="Disabled"
      formControlName="disabled"
      [options]="third" />
  </div>
</div>
`;

const typescript = `
formGroup = new FormGroup({
  searchable: new FormControl<string>(''),
  required: new FormControl<string>('', Validators.required),
  disabled: new FormControl<string>(''),
});

ngOnInit(): void {
  this.formGroup.get('disabled')?.disable();
}

first: any[] = [
  { value: 'Lorem ipsum', name: 'Lorem'},
  { value: 'Donec finibus', name: 'Donec' },
  { value: 'Orci varius', name: 'Orci' }
];
second: string[] = ['Nulla facilisi', 'Nullam pharetra', 'Fusce pulvinar'];
third: string[] = ['Pellentesque metus', 'Donec finibus', 'Cras fringilla'];
`;

@Component({
  selector: 'v-doc-govcz-dropdown-reactive-forms-example',
  imports: [ReactiveFormsModule, DropdownComponent],
  standalone: true,
  template: template,
})
export class GovCzDropdownReactiveFormsExampleComponent implements OnInit {
  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .addCode(typescript, DOCS_CODE_LANG.TS)
    .buildCode();

  formGroup = new FormGroup({
    searchable: new FormControl<string | null>(null),
    required: new FormControl<string | null>(null, Validators.required),
    disabled: new FormControl<string | null>(null),
  });

  first: any[] = [
    { value: 'Lorem ipsum', name: 'Lorem' },
    { value: 'Donec finibus', name: 'Donec' },
    { value: 'Orci varius', name: 'Orci' },
  ];
  second: string[] = ['Nulla facilisi', 'Nullam pharetra', 'Fusce pulvinar'];
  third: string[] = ['Pellentesque metus', 'Donec finibus', 'Cras fringilla'];

  ngOnInit(): void {
    this.formGroup.get('disabled')?.disable();
  }
}
