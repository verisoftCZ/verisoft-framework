import { Component } from '@angular/core';
import {
  DOCS_CODE_LANG,
  DocumentationCodeBuilder,
} from '@verisoft/demo-documentation-ui';
import {
  PageHeaderComponent,
  ActionButtonGroupComponent,
  ActionButtonComponent
} from '@verisoft/ui-primeng';

const template = `
<v-page-header
  title="Page Title"
  subtitle="Page Subtitle"
  [showBackButton]="true"
>
  <v-action-button-group>
    <v-action-button label="Action 1"></v-action-button>
    <v-action-button label="Action 2"></v-action-button>
  </v-action-button-group>
</v-page-header>
`;

@Component({
  selector: 'v-doc-primeng-page-header-basic-example',
  imports: [PageHeaderComponent, ActionButtonGroupComponent, ActionButtonComponent],
  standalone: true,
  template: template,
})
export class PrimengPageHeaderBasicExampleComponent {
  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .buildCode();
}
