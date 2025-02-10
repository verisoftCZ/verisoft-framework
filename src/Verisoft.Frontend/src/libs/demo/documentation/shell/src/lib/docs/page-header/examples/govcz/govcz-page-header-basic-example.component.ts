import { Component } from '@angular/core';
import {
  DOCS_CODE_LANG,
  DocumentationCodeBuilder,
} from '@verisoft/demo-documentation-ui';
import {
  PageHeaderComponent,
  ActionButtonGroupComponent,
} from '@verisoft/ui-govcz';

const template = `
<v-page-header
  title="Page Title"
  subtitle="Page Subtitle"
  [showBackButton]="true"
>
  <v-action-button-group>
    <button pButton type="button" label="Action 1"></button>
    <button pButton type="button" label="Action 2"></button>
  </v-action-button-group>
</v-page-header>
`;

@Component({
  selector: 'v-doc-govcz-page-header-basic-example',
  imports: [PageHeaderComponent, ActionButtonGroupComponent],
  standalone: true,
  template: template,
})
export class GovCzPageHeaderBasicExampleComponent {
  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .buildCode();
}
