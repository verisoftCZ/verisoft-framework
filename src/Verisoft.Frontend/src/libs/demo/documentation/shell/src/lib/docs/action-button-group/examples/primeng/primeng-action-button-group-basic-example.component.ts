import { Component } from '@angular/core';
import {
  DOCS_CODE_LANG,
  DocumentationCodeBuilder,
} from '@verisoft/demo-documentation-ui';
import {
  ActionButtonComponent,
  ActionButtonGroupComponent,
  Icons,
} from '@verisoft/ui-primeng';

const template = `
<v-action-button-group label="More">
  <v-action-button label="Edit" [icon]="icons.edit" severity="primary" (click)="notify('edit')" />
  <v-action-button label="Delete" [icon]="icons.delete" severity="danger" (click)="notify('delete')" [disabled]="true" />
  <v-action-button label="Save" [icon]="icons.save" severity="success" (click)="notify('save')" />
  <v-action-button label="Print" [icon]="icons.print" severity="help" (click)="notify('print')" />
</v-action-button-group>
`;

@Component({
  selector: 'v-doc-primeng-action-button-group-basic-example',
  imports: [ActionButtonGroupComponent, ActionButtonComponent],
  standalone: true,
  template: template,
})
export class PrimengActionButtonGroupBasicExampleComponent {
  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .buildCode();

  icons = Icons;

  notify(type: string) {
    alert('Button Clicked - action ' + type);
  }
}
