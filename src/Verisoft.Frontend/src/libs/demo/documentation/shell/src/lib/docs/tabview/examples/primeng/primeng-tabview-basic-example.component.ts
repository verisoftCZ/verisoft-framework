import { Component } from '@angular/core';
import {
  DOCS_CODE_LANG,
  DocumentationCodeBuilder,
} from '@verisoft/demo-documentation-ui';
import { TabViewModule } from '@verisoft/ui-primeng';

const template = `
<div class="row">
  <div class="col-12">
    <v-tab-view>
      <v-tab-view-item title="First Tab">
        <p>Content for first tab</p>
      </v-tab-view-item>
      <v-tab-view-item title="Second Tab">
        <p>Content for second tab</p>
      </v-tab-view-item>
      <v-tab-view-item title="Third Tab" [disabled]="true">
        <p>Content for third tab</p>
      </v-tab-view-item>
    </v-tab-view>
  </div>
</div>
`;

@Component({
  selector: 'v-doc-primeng-tabview-basic-example',
  imports: [TabViewModule],
  standalone: true,
  template: template,
})
export class PrimengTabViewBasicExampleComponent {
  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .buildCode();
}
