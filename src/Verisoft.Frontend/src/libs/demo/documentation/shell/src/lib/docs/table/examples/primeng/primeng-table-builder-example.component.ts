import { Component } from '@angular/core';
import {
  DOCS_CODE_LANG,
  DocumentationCodeBuilder,
} from '@verisoft/demo-documentation-ui';
import { TableBuilder } from '@verisoft/ui-core';
import { TableComponent } from '@verisoft/ui-primeng';
import { Car, carData } from '../../../data';

const template = `
<div style="height: 300px">
  <v-table [columns]="columns" [data]="data">
  </v-table>
</div>
`;

const typescript = `columns = new TableBuilder<Car>()
    .addTextColumn('type')
    .addTextColumn('model')
    .addNumberColumn('year')
    .addNumberColumn('quantity', (x) => x.headerName('QTY'))
    .addNumberColumn('price')
    .build();`

@Component({
  selector: 'v-doc-primeng-table-builder-example',
  imports: [TableComponent],
  standalone: true,
  template: template,
})
export class PrimengTableBuilderExampleComponent {

  code = new DocumentationCodeBuilder()
  .addCode(typescript)
    .addCode(template, DOCS_CODE_LANG.HTML)
    .buildCode();

  columns = new TableBuilder<Car>()
    .addTextColumn('type')
    .addTextColumn('model')
    .addNumberColumn('year')
    .addNumberColumn('quantity', (x) => x.headerName('QTY'))
    .addNumberColumn('price')
    .build();

  data = carData;
}
