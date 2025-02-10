import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import {
  DOCS_CODE_LANG,
  DocumentationCodeBuilder,
} from '@verisoft/demo-documentation-ui';
import { DatasourceDirective } from '@verisoft/ui-core';
import { DropdownComponent } from '@verisoft/ui-govcz';
import { User } from '../data';

const template = `
<v-dropdown label="User" useDatasource datasource="/v2/user/list" [transformFn]="transform" loadingText="⋘ 𝑙𝑜𝑎𝑑𝑖𝑛𝑔 𝑑𝑎𝑡𝑎... ⋙" filterField="searchField"  optionLabel="fullname" optionValue="id" [(ngModel)]="selected" />`;

@Component({
  selector: 'v-doc-govcz-dropdown-datasource-url-example',
  imports: [DropdownComponent, DatasourceDirective, FormsModule],
  standalone: true,
  template: template,
})
export class GovCzDropdownDatasourceUrlExampleComponent {
  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .addCode(
      `
  selected?: number;

  transform = (user: User) => ({
    ...user,
    fullname: user.surname + ' ' + user.firsName,
  });
}`
    )
    .buildCode();

  selected?: number;

  transform = (user: User) => ({
    ...user,
    fullname: user.surname + ' ' + user.firstName,
  });
}

