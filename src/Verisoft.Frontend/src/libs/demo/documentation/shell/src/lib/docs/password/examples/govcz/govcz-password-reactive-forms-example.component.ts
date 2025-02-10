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
import { PasswordComponent, TextfieldComponent } from '@verisoft/ui-govcz';

const template = `
<div [formGroup]="formGroup">
  <div class="col">
    <v-textfield label="Username" formControlName="name" />
  </div>
  <div class="col">
    <v-password label="Password" formControlName="password" />
  </div>
</div>
`;

const typescript = `
formGroup = new FormGroup({
  name: new FormControl<string>('', Validators.required),
  password: new FormControl<string>('', Validators.required),
});

ngOnInit(): void {
  this.formGroup.get('disabled')?.disable();
}
`;

@Component({
  selector: 'v-govcz-doc-password-reactive-forms-example',
  imports: [ReactiveFormsModule, TextfieldComponent, PasswordComponent],
  standalone: true,
  template: template,
})
export class GovCzPasswordReactiveFormsExampleComponent implements OnInit {
  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .addCode(typescript, DOCS_CODE_LANG.TS)
    .buildCode();

  formGroup = new FormGroup({
    name: new FormControl<string | null>(null, Validators.required),
    password: new FormControl<string | null>(null, Validators.required),
  });

  ngOnInit(): void {
    this.formGroup.get('disabled')?.disable();
  }
}
