import { Component } from '@angular/core';
import {
  DOCS_CODE_LANG,
  DocumentationCodeBuilder,
} from '@verisoft/demo-documentation-ui';
import { TableComponent } from '@verisoft/ui-govcz';
import { carColumns, carData } from '../../../data';

const template = `
<div style="height: 300px">
  <v-table [data]="data" [columns]="columns">
    <ng-template #rowDetail let-row>
      <div class="row mt-4">
          <div class="col-md-4">Type: {{row.type}}</div>
          <div class="col-md-4">Model: {{row.model}}</div>
          <div class="col-md-4">Year: {{row.year}}</div>
      </div>
    </ng-template>
  </v-table>
</div>
`;

@Component({
  selector: 'v-doc-govcz-doc-table-row-detail-example',
  imports: [TableComponent],
  standalone: true,
  template: template,
})
export class GovCzTableRowDetailExampleComponent {
  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .buildCode();

  data = carData;

  columns = carColumns;
}
