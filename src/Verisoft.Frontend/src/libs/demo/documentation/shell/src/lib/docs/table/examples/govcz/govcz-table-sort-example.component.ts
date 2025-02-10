import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import {
  DOCS_CODE_LANG,
  DocumentationCodeBuilder,
} from '@verisoft/demo-documentation-ui';
import { RadioButtonComponent, TableComponent } from '@verisoft/ui-govcz';
import { carColumns, carData } from '../../../data';

const template = `
<div class="mb-4">
  <v-radiobutton label="Sort" [(ngModel)]="sortingMode" [items]="sortingModes" />
</div>
<div style="height: 300px">
  <v-table [columns]="columns" [data]="data" [sortMultiple]="sortingMode == 'multiple'">
  </v-table>
</div>
`;

@Component({
  selector: 'v-doc-govcz-table-sort-example',
  imports: [TableComponent, RadioButtonComponent, FormsModule],
  standalone: true,
  template: template,
})
export class GovCzTableSortExampleComponent {
  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .buildCode();

  columns = carColumns;

  data = carData;

  sortingMode = 'single';

  sortingModes = [
    { id: 'single', value: 'single' },
    { id: 'multiple', value: 'multiple' },
  ];

}
