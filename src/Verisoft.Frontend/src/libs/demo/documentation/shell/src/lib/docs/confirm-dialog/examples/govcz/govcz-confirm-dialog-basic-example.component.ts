import { Component, inject } from '@angular/core';
import {
  DOCS_CODE_LANG,
  DocumentationCodeBuilder,
} from '@verisoft/demo-documentation-ui';
import { DialogService } from '@verisoft/ui-core';
import {
  ButtonComponent,
  SnackbarService,
  Icons
} from '@verisoft/ui-govcz';

const template = `
<div class="row">
  <div class="col-2">
    <v-button
      label="Show dialog"
      (click)="showDialog()"
    />
  </div>
  <div class="col-2">
    <v-button
      label="Show success dialog"
      severity="success"
      (click)="showSuccessDialog()"
    />
  </div>
</div>
`;

const typescript = `
showDialog() {
  this.dialogService.showDialog({
    severity: 'primary',
    innerHtml: '<p>Dialog body</p>',
    title: 'Dialog title',
    headerIcon: Icons.warning,
    showCancelButton: true,
    confirmButtonText: 'Confirm',
    cancelButtonText: 'Close',
  });
}

showSuccessDialog() {
  this.dialogService.showDialog({
    severity: 'success',
    title: 'Dialog title',
    headerIcon: Icons.check,
    confirmButtonText: 'Show a toast',
    confirmButtonFn: () => SnackbarService.showSuccess('Action successful!'),
  });
}
`;

@Component({
  selector: 'v-doc-govcz-confirm-dialog-basic-example',
  imports: [ButtonComponent],
  standalone: true,
  template: template,
})
export class GovCzConfirmDialogBasicExampleComponent {
  dialogService = inject(DialogService);
  snackbar = inject(SnackbarService);
  icons = Icons;

  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .addCode(typescript, DOCS_CODE_LANG.TS)
    .buildCode();

  showDialog() {
    this.dialogService.showDialog({
      severity: 'primary',
      innerHTML: `<p>Dialog body</p>`,
      title: 'Dialog title',
      headerIcon: Icons.warning,
      showCancelButton: true,
      confirmButtonText: 'Confirm',
      cancelButtonText: 'Close',
    });
  }

  showSuccessDialog() {
    this.dialogService.showDialog({
      severity: 'success',
      title: 'Dialog title',
      headerIcon: Icons.check,
      confirmButtonText: 'Show a toast',
      confirmButtonFn: () => this.snackbar.showSuccess('Action successful!'),
    });
  }
}
