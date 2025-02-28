import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import {
  DOCS_CODE_LANG,
  DocumentationCodeBuilder,
} from '@verisoft/demo-documentation-ui';
import { TableSelectionMode } from '@verisoft/ui-core';
import { RadioButtonComponent, TableComponent } from '@verisoft/ui-govcz';
import { Car, carColumns, carData } from '../../../data';

const template = `
<div class="mb-4">
  <v-radiobutton label="Selection Mode" [(ngModel)]="selectionMode" [items]="selectionModes" />
</div>
<div style="height: 300px">
  <v-table [columns]="columns" [data]="data" [selectionMode]="selectionMode" [(selection)]="selected">
  </v-table>
</div>

<div class="mt-4">
  <strong>Selected:</strong>
  @if (selected) {
    @for(car of selected; track car) {
      <div>{{car.type}} - {{car.model}}</div>
    }
  }
</div>
`;

@Component({
  selector: 'v-doc-govcz-table-loading-example',
  imports: [TableComponent, RadioButtonComponent, FormsModule],
  standalone: true,
  template: template,
})
export class GovCzTableSelectableExampleComponent {
  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .buildCode();

  columns = carColumns;

  data = carData;

  selectionMode = TableSelectionMode.single;

  selectionModes = [
    { id: 'single', value: 'single' },
    { id: 'multiple', value: 'multiple' },
  ];

  selected: Car[] = [carData[1]];
}
