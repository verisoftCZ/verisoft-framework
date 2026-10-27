import { Component } from '@angular/core';
import { SortDirection } from '@verisoft/core';
import {
  DOCS_CODE_LANG,
  DocumentationCodeBuilder,
} from '@verisoft/demo-documentation-ui';
import { TableDatasourceDirective } from '@verisoft/ui-core';
import { TableComponent } from '@verisoft/ui-primeng';

const template = `
<div style="height: 300px">
  <v-table useDatasource [columns]="columns" datasource="/v2/user/list" tableName="users" [sortMultiple]="true" >
  </v-table>
</div>
`;

const typescript = `@Component({
  selector: 'v-doc-primeng-table-datasource-url-example',
  imports: [TableComponent, TableDatasourceDirective],
  standalone: true,
  template: template,
})
export class PrimengTableDataSourceUrlExampleComponent {`;

@Component({
    selector: 'v-doc-primeng-table-datasource-url-example',
    imports: [TableComponent, TableDatasourceDirective],
    template: template
})
export class PrimengTableDataSourceUrlExampleComponent {
  code = new DocumentationCodeBuilder()
    .addCode(typescript, DOCS_CODE_LANG.TS)
    .addCode(template, DOCS_CODE_LANG.HTML)
    .buildCode();

  columns = [
    { id: 'firstName', sortDirection: SortDirection.asc, sortPriority: 2 },
    { id: 'surname', sortDirection: SortDirection.desc, sortPriority: 1 },
    { id: 'email' },
  ];
}
