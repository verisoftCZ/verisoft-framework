import { Component } from '@angular/core';
import {
  DOCS_CODE_LANG,
  DocumentationCodeBuilder,
} from '@verisoft/demo-documentation-ui';
import { MenuItem } from '@verisoft/ui-core';
import { BreadcrumbComponent } from '@verisoft/ui-primeng';

const template = `
<div class="row">
  <v-breadcrumb [items]="items" [useHomeRoute]="true" />
</div>
`;

const ts = `
items: MenuItem[] = [
  { 
    label: 'Dokumentace',
    icon: 'pi pi-cog',
    url: '/docs/',
  },
  {
    label: 'Breadcrumb',
    icon: 'pi pi-sitemap',
    url: 'breadcrumb',
  }
];
`;

@Component({
  selector: 'v-doc-primeng-breadcrumb-basic-example',
  imports: [BreadcrumbComponent],
  standalone: true,
  template: template,
})
export class PrimengBreadcrumbBasicExampleComponent {
  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .addCode(ts, DOCS_CODE_LANG.TS)
    .buildCode();

  items: MenuItem[] = [
    {
      label: 'Dokumentace',
      icon: 'pi pi-cog',
      url: '/docs/',
    },
    {
      label: 'Breadcrumb',
      icon: 'pi pi-sitemap',
      url: 'breadcrumb',
    },
  ];
}
