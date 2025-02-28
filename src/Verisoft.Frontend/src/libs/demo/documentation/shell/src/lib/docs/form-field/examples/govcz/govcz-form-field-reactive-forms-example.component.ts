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
import { PasswordComponent, TextfieldComponent, FormFieldComponent } from '@verisoft/ui-govcz';

const template = `
<div [formGroup]="formGroup">
  <v-form-field label="Username">
    <v-textfield class="w-100"  formControlName="name"/>
  </v-form-field>
  <v-form-field label="Password">
    <v-password class="w-100" formControlName="password"/>
  </v-form-field>
</div>
`;

const typescript = `
formGroup = new FormGroup({
  name: new FormControl<string>('', Validators.required),
  password: new FormControl<string>('', Validators.required),
});
`;

@Component({
  selector: 'v-doc-govcz-form-field-reactive-forms-example',
  imports: [ReactiveFormsModule, TextfieldComponent, PasswordComponent, FormFieldComponent],
  standalone: true,
  template: template,
})
export class GovCzFormFieldReactiveFormsExampleComponent {
  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .addCode(typescript, DOCS_CODE_LANG.TS)
    .buildCode();

  formGroup = new FormGroup({
    name: new FormControl<string>('', Validators.required),
    password: new FormControl<string>('', Validators.required),
  });
}
