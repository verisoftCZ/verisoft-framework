import { Component, inject } from '@angular/core';
import {
  DOCS_CODE_LANG,
  DocumentationCodeBuilder,
} from '@verisoft/demo-documentation-ui';
import {
  ButtonComponent,
  SnackbarComponent,
  SnackbarService,
} from '@verisoft/ui-primeng';

const template = `
<div class="d-flex justify-content-between">
  <div>
    <v-button label="Success" severity="success" (click)="showSuccess()"></v-button>
  </div>
  <div>
    <v-button label="Info" severity="info" (click)="showInfo()"></v-button>
  </div>
  <div>
    <v-button label="Warning" severity="warning" (click)="showWarn()"></v-button>
  </div>
  <div>
    <v-button label="Error" severity="danger" (click)="showError()"></v-button>
  </div>
</div>
`;

const typescript = `
  showSuccess() {
    this.snackbar.showSuccess('');
  }

  showInfo() {
    this.snackbar.showInfo('');
  }

  showWarn() {
    this.snackbar.showWarn('');
  }

  showError() {
    this.snackbar.showError('');
  }
`;

@Component({
  selector: 'v-doc-primeng-snackbar-basic-example',
  imports: [SnackbarComponent, ButtonComponent],
  standalone: true,
  template: template,
})
export class PrimengSnackbarBasicExampleComponent {
  snackbar = inject(SnackbarService);

  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .addCode(typescript, DOCS_CODE_LANG.TS)
    .buildCode();

  showSuccess() {
    this.snackbar.showSuccess('');
  }

  showInfo() {
    this.snackbar.showInfo('');
  }

  showWarn() {
    this.snackbar.showWarn('');
  }

  showError() {
    this.snackbar.showError('');
  }
}
