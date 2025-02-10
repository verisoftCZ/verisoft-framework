import { Component } from '@angular/core';
import {
  DOCS_CODE_LANG,
  DocumentationCodeBuilder,
} from '@verisoft/demo-documentation-ui';
import { TableComponent } from '@verisoft/ui-govcz';
import { carColumns, carData } from '../../../data';

const template = `
<div style="height: 300px">
  <v-table [columns]="columns" [data]="data" [loading]="true">
  </v-table>
</div>
`;

@Component({
  selector: 'v-doc-govcz-table-loading-example',
  imports: [TableComponent],
  standalone: true,
  template: template,
})
export class GovCzTableLoadingExampleComponent {
  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .buildCode();

  columns = carColumns;

  data = carData;
}
