import { Component } from '@angular/core';
import {
  DOCS_CODE_LANG,
  DocumentationCodeBuilder,
} from '@verisoft/demo-documentation-ui';
import { TableColumnDirective } from '@verisoft/ui-core';
import {
  ActionButtonComponent,
  ActionButtonGroupComponent,
  Icons,
  TableComponent,
} from '@verisoft/ui-primeng';
import { Car, carColumns, carData } from '../../../data';

const template = `
<div style="height: 300px">
  <v-table [data]="data" [columns]="columns">
    <v-table-column [sortable]="false">
      <ng-template let-row>
        <v-action-button-group>
          <v-action-button label="Edit" [icon]="icons.edit" severity="primary" (click)="showAlert('Edit', row)" />
          <v-action-button label="Delete" [icon]="icons.delete" severity="danger" (click)="showAlert('Delete', row)" />
          <v-action-button label="Save" [icon]="icons.save" severity="success" (click)="showAlert('Save', row)" />
          <v-action-button label="Print" [icon]="icons.print" severity="help" (click)="showAlert('Print', row)" />
        </v-action-button-group>
      </ng-template>
    </v-table-column>
  </v-table>
</div>
`;

const ts =
  'imports: [TableComponent, ActionButtonGroupComponent, ActionButtonComponent, TableColumnDirective]';

@Component({
  selector: 'v-doc-primeng-table-action-buttons-example',
  imports: [
    TableComponent,
    ActionButtonGroupComponent,
    ActionButtonComponent,
    TableColumnDirective,
  ],
  standalone: true,
  template: template,
})
export class PrimengTableActionButtonsExampleComponent {
  code = new DocumentationCodeBuilder()
    .addCode(ts, DOCS_CODE_LANG.TS)
    .addCode(template, DOCS_CODE_LANG.HTML)
    .buildCode();

  data = carData;

  icons = Icons;

  columns = carColumns;

  showAlert(text: string, car: Car) {
    alert(text + ': ' + car?.model);
  }
}
