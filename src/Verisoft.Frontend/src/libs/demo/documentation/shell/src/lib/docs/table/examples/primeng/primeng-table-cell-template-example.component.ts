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
    <v-table-column id="type" headerName="Type" />
    <v-table-column id="model" headerName="Model" />
    <v-table-column id="year" headerName="Year">
      <ng-template let-row>
        <span style="background: #4CAF50; color: white; padding: 2px 8px; border-radius: 12px;">
          {{ row.year }}
        </span>
      </ng-template>
    </v-table-column>
    <v-table-column id="price" headerName="Price" />
  </v-table>
</div>
`;

@Component({
    selector: 'v-doc-primeng-table-cell-template-example',
    imports: [TableComponent, TableColumnDirective],
    template: template
})
export class PrimengTableCellTemplateExampleComponent {
  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .buildCode();

  data = carData;
}
