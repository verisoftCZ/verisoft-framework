import { Component } from '@angular/core';
import {
  DOCS_CODE_LANG,
  DocumentationCodeBuilder,
} from '@verisoft/demo-documentation-ui';
import { FeatureListColumnDirective } from '@verisoft/ui-core';
import { FeatureListComponent } from '@verisoft/ui-govcz';
import { carData } from '../../../data';

const template = `
<div class="row" style="height: 350px">
  <v-feature-list title="Demo car list" filterDetailTitle="Nadpis" applyButtonLabel="OK" clearButtonLabel="Zrušit" tableName="Demo" [datasource]="data" [showAdd]="true" [showDownload]="true" [showDelete]="true" [filterDialogShowFulltext]="false">
    <v-feature-list-column id="type" headerName="Type" type="dropdown" [filter]="true" [filterOptions]="typeFilterOptions" />
    <v-feature-list-column id="model" headerName="Model" [filter]="true" />
    <v-feature-list-column id="year" headerName="Year" width="10%" />
    <v-feature-list-column id="quantity" headerName="QTY" />
    <v-feature-list-column id="price" headerName="Price" />
  </v-feature-list>
</div>
`;
@Component({
    selector: 'v-doc-govcz-feature-list-template-example',
    imports: [FeatureListComponent, FeatureListColumnDirective],
    template: template
})
export class GovCzFeatureListTemplateExampleComponent {
  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .buildCode();

  data = carData;
  typeFilterOptions = { options: [...new Set(carData.map((c) => c.type))].sort((a, b) => a.localeCompare(b, undefined, { sensitivity: 'base' })) };
}
