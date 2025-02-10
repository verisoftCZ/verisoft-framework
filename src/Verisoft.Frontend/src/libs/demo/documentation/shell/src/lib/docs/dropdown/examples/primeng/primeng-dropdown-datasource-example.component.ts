import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { DatasourceType } from '@verisoft/core';
import {
  DOCS_CODE_LANG,
  DocumentationCodeBuilder,
} from '@verisoft/demo-documentation-ui';
import { DatasourceDirective } from '@verisoft/ui-core';
import { DropdownComponent } from '@verisoft/ui-primeng';
import { delay, of } from 'rxjs';

const template = `
<v-dropdown label="Car" useDatasource [datasource]="datasource" optionLabel="name" optionValue="id" [(ngModel)]="selected" />`;

const RegistrationTs = `
  @Component({
    selector: 'v-doc-dropdown-datasource-example',
    imports: [DropdownComponent, DatasourceDirective, FormsModule],
    standalone: true,
    template: template,
  })
  export class DropdownDatasourceExampleComponent 
`;

const ts = `
  selected = 4;

  datasource: DatasourceType<Car> = of([
    { id: 1, name: 'Audi' },
    { id: 2, name: 'BMW' },
    { id: 3, name: 'Fiat' },
    { id: 4, name: 'Toyota' },
    { id: 5, name: 'Volvo' },
  ]).pipe(delay(5000));
`;

interface Car {
  id: number;
  name: string;
}

@Component({
  selector: 'v-doc-primeng-dropdown-datasource-example',
  imports: [DropdownComponent, DatasourceDirective, FormsModule],
  standalone: true,
  template: template,
})
export class PrimengDropdownDatasourceExampleComponent {
  code = new DocumentationCodeBuilder()
    .addCode(RegistrationTs, DOCS_CODE_LANG.TS)
    .addCode(template, DOCS_CODE_LANG.HTML)
    .addCode(ts, DOCS_CODE_LANG.TS)
    .buildCode();

  selected = 4;

  datasource: DatasourceType<Car> = of([
    { id: 1, name: 'Audi' },
    { id: 2, name: 'BMW' },
    { id: 3, name: 'Fiat' },
    { id: 4, name: 'Toyota' },
    { id: 5, name: 'Volvo' },
  ]).pipe(delay(5000));
}
