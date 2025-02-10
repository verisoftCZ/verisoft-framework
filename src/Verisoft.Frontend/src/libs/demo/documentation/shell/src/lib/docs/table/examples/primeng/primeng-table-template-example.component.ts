import { Component } from '@angular/core';
import {
  DOCS_CODE_LANG,
  DocumentationCodeBuilder,
} from '@verisoft/demo-documentation-ui';
import { TableColumnDirective } from '@verisoft/ui-core';
import { TableComponent } from '@verisoft/ui-primeng';
import { carData } from '../../../data';

const template = `
<div style="height: 300px">
  <v-table [data]="data">
     <v-table-column id="type" />
     <v-table-column id="model" />
     <v-table-column id="year" />
     <v-table-column id="quantity" headerName="QTY" />
     <v-table-column id="price" />
  </v-table>
</div>
`;

@Component({
  selector: 'v-doc-primeng-table-template-example',
  imports: [TableComponent, TableColumnDirective],
  standalone: true,
  template: template,
})
export class PrimengTableTemplateExampleComponent {
  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .buildCode();

  data = carData;
}
