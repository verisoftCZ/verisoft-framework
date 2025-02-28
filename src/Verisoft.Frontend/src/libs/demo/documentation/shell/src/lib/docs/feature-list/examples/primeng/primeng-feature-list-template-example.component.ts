import { Component } from '@angular/core';
import {
  DOCS_CODE_LANG,
  DocumentationCodeBuilder,
} from '@verisoft/demo-documentation-ui';
import { FeatureListColumnDirective, FeatureListComponent } from '@verisoft/ui-primeng';
import { carData } from '../../../data';

const template = `
<div class="row" style="height: 350px">
  <v-feature-list title="Demo user list" tableName="Demo" datasource="/v2/user/list" [showAdd]="true" [showDownload]="true" [showDelete]="true">
    <v-feature-list-column id="id" headerName="Id"/>
    <v-feature-list-column id="firstName" headerName="First name" [filter]="true" />
    <v-feature-list-column id="surname" headerName="Surname" [filter]="true" />
    <v-feature-list-column id="email" headerName="Email" />
  </v-feature-list>
</div>
`;
@Component({
  selector: 'v-doc-primeng-feature-list-template-example',
  imports: [FeatureListComponent, FeatureListColumnDirective],
  standalone: true,
  template: template,
})
export class PrimengFeatureListTemplateExampleComponent {
  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .buildCode();

  data = carData;
}
