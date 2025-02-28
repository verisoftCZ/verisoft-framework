import { Component } from '@angular/core';
import {
  DOCS_CODE_LANG,
  DocumentationCodeBuilder,
} from '@verisoft/demo-documentation-ui';
import { TableComponent } from '@verisoft/ui-govcz';
import { carColumns, carData } from '../../../data';

const template = `
<div style="height: 300px">
  <v-table [columns]="columns" [data]="data">
  </v-table>
</div>
`;

const typescript = `columns: ColumnDefinition<Car>[] = [
    { id: 'type' },
    { id: 'model' },
    { id: 'year' },
    { id: 'quantity', headerName: 'QTY' },
    { id: 'price' },
  ];

  data: Car[] = [
    {
      type: 'sedan',
      model: 'A4',
      year: 2018,
      quantity: 5,
      price: 1600000,
    },
    {
      type: 'sports car',
      model: 'Ferrari',
      year: 2017,
      quantity: 1,
      price: 4500000,
    },
    ...
  ];`;

@Component({
  selector: 'v-doc-govcz-table-basic-example',
  imports: [TableComponent],
  standalone: true,
  template: template,
})
export class GovCzTableBasicExampleComponent {
  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .addCode(typescript, DOCS_CODE_LANG.TS)
    .buildCode();

  columns = carColumns;

  data = carData;
}
