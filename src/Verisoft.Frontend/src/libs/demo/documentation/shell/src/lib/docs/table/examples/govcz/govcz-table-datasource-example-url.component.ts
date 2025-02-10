import { Component } from '@angular/core';
import {
  DOCS_CODE_LANG,
  DocumentationCodeBuilder,
} from '@verisoft/demo-documentation-ui';
import { TableDatasourceDirective } from '@verisoft/ui-core';
import { TableComponent } from '@verisoft/ui-govcz';

const template = `
<div style="height: 300px">
  <v-table useDatasource [columns]="columns" datasource="/v2/user/list" tableName="users" >
  </v-table>
</div>
`;

const typescript = `@Component({
  selector: 'v-doc-govcz-table-datasource-url-example',
  imports: [TableComponent, TableDatasourceDirective],
  standalone: true,
  template: template,
})
export class GovCzTableDataSourceUrlExampleComponent {`;

@Component({
  selector: 'v-doc-govcz-table-datasource-url-example',
  imports: [TableComponent, TableDatasourceDirective],
  standalone: true,
  template: template,
})
export class GovCzTableDataSourceUrlExampleComponent {
  code = new DocumentationCodeBuilder()
    .addCode(typescript, DOCS_CODE_LANG.TS)
    .addCode(template, DOCS_CODE_LANG.HTML)
    .buildCode();

  columns = [
    { id: 'firstName' },
    { id: 'surname' },
    { id: 'email' },
  ];
}
