import { Component } from '@angular/core';
import { DOCS_CODE_LANG, DocumentationCodeBuilder } from '@verisoft/demo-documentation-ui';
import { FeatureListComponent } from '@verisoft/ui-govcz';
import { carColumns, carData } from '../../../data';

const template = `
<div class="row">
  <v-feature-list title="Demo user list" datasource="client" tableName="Demo" [showAdd]="true" [showDownload]="true" [showDelete]="true"></v-feature-list>
</div>
`;

@Component({
  selector: 'v-doc-govcz-feature-list-basic-example',
  imports: [FeatureListComponent],
  standalone: true,
  template: template,
})
export class GovCzFeatureListBasicExampleComponent {
  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .buildCode();

  columns = carColumns;
  data = carData;
}
