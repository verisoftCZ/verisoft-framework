import { Component } from '@angular/core';
import {
  DOCS_CODE_LANG,
  DocumentationCodeBuilder,
} from '@verisoft/demo-documentation-ui';
import { TabViewModule } from '@verisoft/ui-primeng';

const template = `
    <v-tab-view [useRouting]="true">
      <v-tab-view-item title="First tab" url="./first"></v-tab-view-item>
      <v-tab-view-item title="Second Tab" url="./second"></v-tab-view-item>
    </v-tab-view>
`;

@Component({
  selector: 'v-doc-primeng-tabview-size-example',
  imports: [TabViewModule],
  standalone: true,
  template: template,
})
export class PrimengTabViewRouterExampleComponent {
  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .buildCode();
}
