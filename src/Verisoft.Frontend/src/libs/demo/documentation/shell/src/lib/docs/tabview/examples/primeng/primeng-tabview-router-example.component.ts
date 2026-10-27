import { Component } from '@angular/core';
import {
  DOCS_CODE_LANG,
  DocumentationCodeBuilder,
} from '@verisoft/demo-documentation-ui';
import { TabViewComponent, TabViewItemComponent } from '@verisoft/ui-primeng';

const template = `
    <v-tab-view [useRouting]="true">
      <v-tab-view-item title="First tab" url="./first"></v-tab-view-item>
      <v-tab-view-item title="Second Tab" url="./second"></v-tab-view-item>
    </v-tab-view>
`;

@Component({
  selector: 'v-doc-primeng-tabview-routing-example',
  imports: [TabViewComponent, TabViewItemComponent],
  template: template
})
export class PrimengTabViewRouterExampleComponent {
  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .buildCode();
}
