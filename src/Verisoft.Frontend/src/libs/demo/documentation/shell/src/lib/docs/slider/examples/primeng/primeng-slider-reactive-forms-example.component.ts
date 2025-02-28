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
import { SliderComponent } from '@verisoft/ui-primeng';

const template = `
<div class="row" [formGroup]="formGroup">
  <div class="col col-md-6">
    <v-slider label="Value" formControlName="value"></v-slider>
  </div>
  <div class="col col-md-6">
    <v-slider label="Required" formControlName="required"></v-slider>
  </div>
  <div class="col col-md-6">
    <v-slider label="Disabled" formControlName="disabled"></v-slider>
  </div>
</div>
`;

const typescript = `
  formGroup = new FormGroup({
    value: new FormControl<number>(0),
    required: new FormControl<number>(0, Validators.required),
    disabled: new FormControl<number>(0),
  });

  ngOnInit(): void {
    this.formGroup.get('disabled')?.disable();
  }
`;

@Component({
  selector: 'v-doc-primeng-slider-reactive-forms-example',
  imports: [ReactiveFormsModule, SliderComponent],
  standalone: true,
  template: template,
})
export class PrimengSliderReactiveFormsExampleComponent implements OnInit {
  formGroup = new FormGroup({
    value: new FormControl<number>(0),
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
