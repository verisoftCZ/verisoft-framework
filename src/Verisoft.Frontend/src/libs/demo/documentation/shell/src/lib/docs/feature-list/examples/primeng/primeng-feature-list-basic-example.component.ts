import { Component } from '@angular/core';
import {
  DOCS_CODE_LANG,
  DocumentationCodeBuilder,
} from '@verisoft/demo-documentation-ui';
import { FeatureListColumnDefinition } from '@verisoft/ui-core';
import { FeatureListComponent } from '@verisoft/ui-primeng';
import { Car, carColumns, carData } from '../../../data';

const template = `
<div class="row" style="height: 350px">
  <v-feature-list title="Demo user list" tableName="Demo" [datasource]="data" [columns]="columns" [showAdd]="true" [showDownload]="true" [showDelete]="true"></v-feature-list>
</div>
`;

@Component({
    selector: 'v-doc-primeng-feature-list-basic-example',
    imports: [FeatureListComponent],
    template: template
})
export class PrimengFeatureListBasicExampleComponent {
  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .buildCode();

  columns = carColumns as FeatureListColumnDefinition<Car>[];
  data = carData;
}
