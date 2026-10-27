import { Component } from '@angular/core';
import {
  DOCS_CODE_LANG,
  DocumentationCodeBuilder,
} from '@verisoft/demo-documentation-ui';
import { FeatureListColumnDefinition } from '@verisoft/ui-core';
import { FeatureListComponent } from '@verisoft/ui-govcz';

interface ServerUser {
  id: number;
  firstName: string;
  surname: string;
  email: string;
}

const template = `
<div class="row" style="height: 350px">
  <v-feature-list title="Users from server" tableName="ServerUsers" [datasource]="datasource" [columns]="columns" [showFulltext]="true" [filterDialogShowFulltext]="false"></v-feature-list>
</div>
`;

@Component({
    selector: 'v-doc-govcz-feature-list-server-example',
    imports: [FeatureListComponent],
    template: template
})
export class GovCzFeatureListServerExampleComponent {
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
