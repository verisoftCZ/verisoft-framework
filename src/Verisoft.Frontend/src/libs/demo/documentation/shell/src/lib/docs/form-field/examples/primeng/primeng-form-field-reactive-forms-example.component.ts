import { Component } from '@angular/core';
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
import { PasswordComponent, TextfieldComponent, FormFieldComponent } from '@verisoft/ui-primeng';

const template = `
<div [formGroup]="formGroup">
  <v-form-field label="Username">
    <v-textfield class="col"  formControlName="name"/>
  </v-form-field>
    <v-password label="Password" formControlName="password" class="col"/>
</div>
`;

const typescript = `
formGroup = new FormGroup({
  name: new FormControl<string>('', Validators.required),
  password: new FormControl<string>('', Validators.required),
});
`;

@Component({
  selector: 'v-doc-primeng-form-field-reactive-forms-example',
  imports: [ReactiveFormsModule, TextfieldComponent, PasswordComponent, FormFieldComponent],
  standalone: true,
  template: template,
})
export class PrimengFormFieldReactiveFormsExampleComponent {
  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .addCode(typescript, DOCS_CODE_LANG.TS)
    .buildCode();

  formGroup = new FormGroup({
    name: new FormControl<string>('', Validators.required),
    password: new FormControl<string>('', Validators.required),
  });
}
