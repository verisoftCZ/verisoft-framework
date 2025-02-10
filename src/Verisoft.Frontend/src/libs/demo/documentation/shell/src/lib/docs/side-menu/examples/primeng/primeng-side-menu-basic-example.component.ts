import { Component } from '@angular/core';
import {
  DOCS_CODE_LANG,
  DocumentationCodeBuilder,
} from '@verisoft/demo-documentation-ui';
import { MenuItem } from '@verisoft/ui-core';
import { SideMenuComponent } from '@verisoft/ui-primeng';

const template = `
<v-side-menu 
  [items]="items"
  [logoUrl]="'assets/images/_global/logos/v-logo.webp'"
  userName="Small username"
  userRole="Smaller user role"
  style="top: 4rem; position: relative;"
/>
`;

const typescript = `
items: MenuItem[] = [
  { 
    label: 'First item',
    icon: 'pi pi-cog',
    children: [
      { label: 'Child one', url: '/some-url' },
      { label: 'Child two', url: '/some-other-url' },
    ]
  },
  { 
    label: 'Second item',
    icon: 'pi pi-cog',
    children: [
      { label: 'Child three', url: '/some-special-url' },
      { label: 'Child four', url: '/some-otherwordly-url' },
    ]
  }
];
`;

@Component({
  selector: 'v-primeng-doc-side-menu-basic-example',
  imports: [SideMenuComponent],
  standalone: true,
  template: template,
})
export class PrimengSideMenuBasicExampleComponent {
  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .addCode(typescript, DOCS_CODE_LANG.TS)
    .buildCode();

  items: MenuItem[] = [
    {
      label: 'First item',
      icon: 'pi pi-cog',
      children: [
        { label: 'Child one', url: '/some-url' },
        { label: 'Child two', url: '/some-other-url' },
      ],
    },
    {
      label: 'Second item',
      icon: 'pi pi-cog',
      children: [
        { label: 'Child three', url: '/some-special-url' },
        { label: 'Child four', url: '/some-otherwordly-url' },
      ],
    },
  ];
}
