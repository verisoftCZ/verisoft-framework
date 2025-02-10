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
import { SwitchComponent } from '@verisoft/ui-primeng';

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
    name: new FormControl<boolean>(false),
    required: new FormControl<boolean>(false, Validators.required),
    disabled: new FormControl<boolean>(false),
  });
  
  ngOnInit(): void {
    this.formGroup.get('disabled')?.disable();
  }
`;

@Component({
  selector: 'v-doc-primeng-switch-reactive-forms-example',
  imports: [SwitchComponent, ReactiveFormsModule],
  standalone: true,
  template: template,
})
export class PrimengSwitchReactFormsExampleComponent implements OnInit {
  formGroup = new FormGroup({
    name: new FormControl<boolean>(false),
    required: new FormControl<boolean>(false, Validators.required),
    disabled: new FormControl<boolean>(false),
  });

  ngOnInit(): void {
    this.formGroup.get('disabled')?.disable();
  }

  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .addCode(typescript, DOCS_CODE_LANG.TS)
    .buildCode();
}
