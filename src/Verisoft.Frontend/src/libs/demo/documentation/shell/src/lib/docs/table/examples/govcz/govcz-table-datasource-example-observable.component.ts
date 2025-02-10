import { Component } from '@angular/core';
import {
  DOCS_CODE_LANG,
  DocumentationCodeBuilder,
} from '@verisoft/demo-documentation-ui';
import { TableDatasourceDirective } from '@verisoft/ui-core';
import { TableComponent } from '@verisoft/ui-govcz';
import { of } from 'rxjs';

const template = `
<div style="height: 300px">
  <v-table useDatasource [columns]="columns" [datasource]="data$" tableName="users" >
  </v-table>
</div>
`;

const typescript = `@Component({
  selector: 'v-doc-govcz-table-datasource-url-example',
  imports: [TableComponent, TableDatasourceDirective],
  standalone: true,
  template: template,
})
export class GovCzTableDataSourceObservableExampleComponent {
 
  data$ = of([
    { firstName : 'John', surname : 'Doe', email : 'john.doe@outlook.com' },
    { firstName  : 'Martin', surname  : 'Smith', email  : 'martin.smith@myemail.com' }
  ]);

  ...
}`;

@Component({
  selector: 'v-doc-govcz-table-datasource-observable-example',
  imports: [TableComponent, TableDatasourceDirective],
  standalone: true,
  template: template,
})
export class GovCzTableDataSourceObservableExampleComponent {
  code = new DocumentationCodeBuilder()
    .addCode(typescript, DOCS_CODE_LANG.TS)
    .addCode(template, DOCS_CODE_LANG.HTML)
    .buildCode();

  data$ = of([
    { firstName: 'John', surname: 'Doe', email: 'john.doe@outlook.com' },
    {
      firstName: 'Martin',
      surname: 'Smith',
      email: 'martin.smith@myemail.com',
    },
  ]);

  columns = [{ id: 'firstName' }, { id: 'surname' }, { id: 'email' }];
}
