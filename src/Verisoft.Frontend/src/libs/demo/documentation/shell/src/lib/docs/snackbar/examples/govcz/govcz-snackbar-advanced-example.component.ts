import { Component, OnInit, inject } from '@angular/core';
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
import {
  ButtonComponent,
  Icons,
  SnackbarComponent,
  SnackbarService,
  TextfieldComponent,
} from '@verisoft/ui-govcz';

const template = `
<div [formGroup]="formGroup">
  <div class="row">
    <v-textfield 
      class="col-10"
      label="Success button input"
      formControlName="success"/>
    <v-button
      class="col-2"
      label="Success"
      severity="success"
      (click)="showSuccess()"/>
  </div>
  <div class="row">
    <v-textfield
      class="col-10"
      label="Info button input"
      formControlName="info"/>
    <v-button
      class="col-2"
      label="Info"
      severity="info"
      (click)="showInfo()"/>
  </div>
  <div class="row">
    <v-textfield 
      class="col-10"
      label="Warning button input"
      formControlName="warning"/>
    <v-button
      class="col-2"
      label="Warning"
      severity="warning"
      (click)="showWarn()"/>
  </div>
  <div class="row">
    <v-textfield 
      class="col-10"
      label="Error button input"
      formControlName="error"/>
    <v-button
      class="col-2"
      label="Error"
      severity="danger"
      (click)="showError()"/>
  </div>
</div>
`;

const typescript = `
  formGroup = new FormGroup({
    success: new FormControl<string>('', Validators.required),
    info: new FormControl<string>('', Validators.required),
    warning: new FormControl<string>('', Validators.required),
    error: new FormControl<string>('', Validators.required),
  });
  
  ngOnInit(): void {
    this.formGroup.get('disabled')?.disable();
  }

  showSuccess() {
    this.snackbar.showSuccess(
      this.formGroup.get('success')?.value ?? '',
      Icons.checkCircle
    );
  }

  showInfo() {
    this.snackbar.showInfo(
      this.formGroup.get('info')?.value ?? '',
      Icons.infoCircle
    );
  }

  showWarn() {
    this.snackbar.showWarn(
      this.formGroup.get('warning')?.value ?? '',
      Icon.warning
    );
  }

  showError() {
    this.snackbar.showError(
      this.formGroup.get('error')?.value ?? '',
      Icons.crossCircle
    );
  }
`;

@Component({
  selector: 'v-doc-govcz-snackbar-advanced-example',
  imports: [
    SnackbarComponent,
    ButtonComponent,
    TextfieldComponent,
    ReactiveFormsModule,
  ],
  standalone: true,
  template: template,
})
export class GovCzSnackbarAdvancedExampleComponent implements OnInit {
  snackbar = inject(SnackbarService);
  icons = Icons

  formGroup = new FormGroup({
    success: new FormControl<string>('Action succesful!', Validators.required),
    info: new FormControl<string>('New action available', Validators.required),
    warning: new FormControl<string>(
      'Solution is outdated',
      Validators.required
    ),
    error: new FormControl<string>('Something went wrong', Validators.required),
  });

  ngOnInit(): void {
    this.formGroup.get('disabled')?.disable();
  }

  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .addCode(typescript, DOCS_CODE_LANG.TS)
    .buildCode();

  showSuccess() {
    this.snackbar.showSuccess(
      this.formGroup.get('success')?.value ?? '',
      Icons.checkCircle
    );
  }

  showInfo() {
    this.snackbar.showInfo(
      this.formGroup.get('info')?.value ?? '',
      Icons.infoCircle
    );
  }

  showWarn() {
    this.snackbar.showWarn(
      this.formGroup.get('warning')?.value ?? '',
      Icons.warning
    );
  }

  showError() {
    this.snackbar.showError(
      this.formGroup.get('error')?.value ?? '',
      Icons.crossCircle
    );
  }
}
