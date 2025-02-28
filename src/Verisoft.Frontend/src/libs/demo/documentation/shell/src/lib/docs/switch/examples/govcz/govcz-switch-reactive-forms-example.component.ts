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
import { SwitchComponent } from '@verisoft/ui-govcz';

const template = `
<div class="row" [formGroup]="formGroup">
  <div class="col">
    <v-switch [label]="'Name'" formControlName="name"></v-switch>
  </div>
  <div class="col">
    <v-switch label="Required" formControlName="required"></v-switch>
  </div>
  <div class="col">
    <v-switch label="Disabled" formControlName="disabled"></v-switch>
  </div>
</div>
`;

const typescript = `
  formGroup = new FormGroup({
    name: new FormControl<boolean | null>(null),
    required: new FormControl<boolean | null>(null, Validators.required),
    disabled: new FormControl<boolean | null>(null),
  });
  
  ngOnInit(): void {
    this.formGroup.get('disabled')?.disable();
  }
`;

@Component({
  selector: 'v-doc-govcz-switch-reactive-forms-example',
  imports: [SwitchComponent, ReactiveFormsModule],
  standalone: true,
  template: template,
})
export class GovCzSwitchReactFormsExampleComponent implements OnInit {
  formGroup = new FormGroup({
    name: new FormControl<boolean | null>(null),
    required: new FormControl<boolean | null>(null, Validators.required),
    disabled: new FormControl<boolean | null>(null),
  });


  ngOnInit(): void {
    this.formGroup.get('disabled')?.disable();
  }

  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .addCode(typescript, DOCS_CODE_LANG.TS)
    .buildCode();
}
