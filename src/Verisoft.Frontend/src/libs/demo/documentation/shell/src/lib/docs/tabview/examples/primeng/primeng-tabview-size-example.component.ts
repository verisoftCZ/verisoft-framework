import { Component } from '@angular/core';
import {
  DOCS_CODE_LANG,
  DocumentationCodeBuilder,
} from '@verisoft/demo-documentation-ui';
import { TabViewModule } from '@verisoft/ui-primeng';

const template = `
    <v-tab-view>
      <v-tab-view-item title="Small Tab">
        <p>Content for small tab</p>
      </v-tab-view-item>
      <v-tab-view-item title="Medium Tab">
        <p>Content for medium tab</p>
      </v-tab-view-item>
      <v-tab-view-item title="Large Tab">
        <p>Content for large tab</p>
      </v-tab-view-item>
    </v-tab-view>
`;

@Component({
  selector: 'v-doc-primeng-tabview-size-example',
  imports: [TabViewModule],
  standalone: true,
  template: template,
})
export class PrimengTabViewSizeExampleComponent {
  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .buildCode();
}
