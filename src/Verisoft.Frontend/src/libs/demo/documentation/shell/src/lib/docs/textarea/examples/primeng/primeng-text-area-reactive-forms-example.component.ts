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
import { TextareaComponent } from '@verisoft/ui-primeng';

const template = `
<div class="row" [formGroup]="formGroup">
  <div class="col col-md-6">
    <v-textarea label="Name" formControlName="name"></v-textarea>
  </div>
  <div class="col col-md-6">
    <v-textarea label="Required" formControlName="required"></v-textarea>
  </div>
  <div class="col col-md-6">
    <v-textarea label="Disabled" formControlName="disabled"></v-textarea>
  </div>
  <div class="col col-md-6">
    <v-textarea label="Non-adjustable" [autoResize]="true"></v-textarea>
  </div>
</div>
`;

const typescript = `
  formGroup = new FormGroup({
    name: new FormControl<string>(''),
    required: new FormControl<string>('', Validators.required),
    disabled: new FormControl<string>(''),
    nonAdjustable: new FormControl<string>(''),
  });
  
  ngOnInit(): void {
    this.formGroup.get('disabled')?.disable();
  }
`;

@Component({
  selector: 'v-doc-primeng-text-area-reactive-forms-example',
  imports: [TextareaComponent, ReactiveFormsModule],
  standalone: true,
  template: template,
})
export class PrimengTextAreaReactFormsExampleComponent implements OnInit {
  formGroup = new FormGroup({
    name: new FormControl<string>(''),
    required: new FormControl<string>('', Validators.required),
    disabled: new FormControl<string>(''),
    nonAdjustable: new FormControl<string>(''),
  });

  ngOnInit(): void {
    this.formGroup.get('disabled')?.disable();
  }

  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .addCode(typescript, DOCS_CODE_LANG.TS)
    .buildCode();
}
