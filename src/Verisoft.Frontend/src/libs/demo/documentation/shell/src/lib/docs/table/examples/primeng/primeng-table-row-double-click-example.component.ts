import { Component } from '@angular/core';
import {
  DOCS_CODE_LANG,
  DocumentationCodeBuilder,
} from '@verisoft/demo-documentation-ui';
import { TableComponent } from '@verisoft/ui-primeng';
import { Car, carColumns, carData } from '../../../data';

const template = `
<div style="height: 300px">
  <v-table [columns]="columns" [data]="data" (rowDoubleClick)="onRowDoubleClick($event)">
  </v-table>
</div>

<div class="mt-4">
  <strong>Double clicked:</strong>
  @if (doubleClicked) {
    <div>{{doubleClicked.type}} - {{doubleClicked.model}}</div>
  }
</div>
`;

@Component({
    selector: 'v-doc-primeng-table-row-double-click-example',
    imports: [TableComponent],
    template: template
})
export class PrimengTableRowDoubleClickExampleComponent {
  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .buildCode();

  columns = carColumns;

  data = carData;

  doubleClicked: Car | undefined;

  onRowDoubleClick(car: Car) {
    this.doubleClicked = car;
  }
}
