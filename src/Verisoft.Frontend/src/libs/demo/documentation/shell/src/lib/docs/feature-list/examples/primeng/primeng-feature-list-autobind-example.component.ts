import { Component } from '@angular/core';
import {
  DOCS_CODE_LANG,
  DocumentationCodeBuilder,
} from '@verisoft/demo-documentation-ui';
import { FeatureListColumnDefinition } from '@verisoft/ui-core';
import { ButtonComponent, FeatureListComponent } from '@verisoft/ui-primeng';

interface ServerUser {
  id: number;
  firstName: string;
  surname: string;
  email: string;
}

const template = `
<div class="row" style="height: 350px">
  <v-button label="Load data" class="mb-2" (click)="featureList.reload()"></v-button>
  <v-feature-list #featureList title="Users from server" tableName="ServerUsersManual" [autoBind]="false" [datasource]="datasource" [columns]="columns"></v-feature-list>
</div>
`;

@Component({
    selector: 'v-doc-primeng-feature-list-autobind-example',
    imports: [FeatureListComponent, ButtonComponent],
    template: template
})
export class PrimengFeatureListAutobindExampleComponent {
  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .buildCode();

  datasource = '/v2/user/list';

  columns: FeatureListColumnDefinition<ServerUser>[] = [
    { id: 'firstName', headerName: 'First Name', filter: true },
    { id: 'surname', headerName: 'Surname', filter: true, sortDirection: 'desc' },
    { id: 'email', headerName: 'Email', filter: true },
  ];
}
