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
import { RadioButtonItem } from '@verisoft/ui-core';
import { RadioButtonComponent } from '@verisoft/ui-primeng';

const template = `
<div class="row" [formGroup]="formGroup">
  <div class="col-12">
    <v-radiobutton label="Stock radiobutton" formControlName="name" [items]="items" />
  </div>
  <div class="col-12">
    <v-radiobutton label="Required radiobutton" formControlName="required" [required]="true" [items]="requiredItems" />
  </div>
  <div class="col-12">
    <v-radiobutton label="Disabled radiobutton" formControlName="disabled" [items]="disabledItems" />
  </div>
</div>
`;

const typescript = `
formGroup = new FormGroup({
  name: new FormControl<string>(''),
  required: new FormControl<string>('', Validators.required),
  disabled: new FormControl<string>(''),
});

items: RadioButtonItem<any>[] = [{ id: '123', value: 'Yes' }, { id: '321', value: 'No' }];
requiredItems: RadioButtonItem<any>[] = [{ id: '333', value: 'Yes' }, { id: '444', value: 'No' }];
disabledItems: RadioButtonItem<any>[] = [{ id: '555', value: 'Yes' }, { id: '666', value: 'No' }];

ngOnInit(): void {
  this.formGroup.get('disabled')?.disable();
}
`;

@Component({
  selector: 'v-doc-primeng-radiobutton-reactive-forms-example',
  imports: [ReactiveFormsModule, RadioButtonComponent],
  standalone: true,
  template: template,
})
export class PrimengRadiobuttonReactiveFormsExampleComponent implements OnInit {
  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .addCode(typescript, DOCS_CODE_LANG.TS)
    .buildCode();

  formGroup = new FormGroup({
    name: new FormControl<string>(''),
    required: new FormControl<string>('', Validators.required),
    disabled: new FormControl<string>(''),
  });

  items: RadioButtonItem<any>[] = [
    { id: '123', value: 'Yes' },
    { id: '321', value: 'No' },
  ];
  requiredItems: RadioButtonItem<any>[] = [
    { id: '333', value: 'Yes' },
    { id: '444', value: 'No' },
  ];
  disabledItems: RadioButtonItem<any>[] = [
    { id: '555', value: 'Yes' },
    { id: '666', value: 'No' },
  ];

  ngOnInit(): void {
    this.formGroup.get('disabled')?.disable();
  }
}
