import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { DatasourceType } from '@verisoft/core';
import {
  DOCS_CODE_LANG,
  DocumentationCodeBuilder,
} from '@verisoft/demo-documentation-ui';
import { DatasourceDirective } from '@verisoft/ui-core';
import { MultiselectComponent } from '@verisoft/ui-primeng';
import { delay, of } from 'rxjs';

const template = `
<v-multiselect label="Car" useDatasource [datasource]="datasource" optionLabel="name" optionValue="id" />`;

const RegistrationTs = `
  @Component({
    selector: 'v-doc-primeng-multiselect-datasource-example',
    imports: [MultiselectComponent, DatasourceDirective, FormsModule],
    standalone: true,
    template: template,
  })
  export class PrimengMultiselectDatasourceExampleComponent 
`;

const ts = `
  datasource: DatasourceType<Car> = of([
    { id: 1, name: 'Audi' },
    { id: 2, name: 'BMW' },
    { id: 3, name: 'Fiat' },
    { id: 4, name: 'Toyota' },
    { id: 5, name: 'Volvo' },
  ]);
`;

interface Car {
  id: number;
  name: string;
}

@Component({
  selector: 'v-doc-primeng-multiselect-datasource-example',
  imports: [MultiselectComponent, DatasourceDirective, FormsModule],
  standalone: true,
  template: template,
})
export class PrimengMultiSelectDatasourceExampleComponent {
  code = new DocumentationCodeBuilder()
    .addCode(RegistrationTs, DOCS_CODE_LANG.TS)
    .addCode(template, DOCS_CODE_LANG.HTML)
    .addCode(ts, DOCS_CODE_LANG.TS)
    .buildCode();

  datasource: DatasourceType<Car> = of([
    { id: 1, name: 'Audi' },
    { id: 2, name: 'BMW' },
    { id: 3, name: 'Fiat' },
    { id: 4, name: 'Toyota' },
    { id: 5, name: 'Volvo' },
  ]);
}
