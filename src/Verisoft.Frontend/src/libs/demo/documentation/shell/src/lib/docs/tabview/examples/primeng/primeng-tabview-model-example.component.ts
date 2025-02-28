import { Component } from '@angular/core';
import {
  DOCS_CODE_LANG,
  DocumentationCodeBuilder,
} from '@verisoft/demo-documentation-ui';
import { TabViewItemCore } from '@verisoft/ui-core';
import { TabViewModule } from '@verisoft/ui-primeng';

const template = `
<div class="row">
  <div class="col-12">
    <v-tab-view [items]="tabViewItems" [activeIndex]="1">
    </v-tab-view>
  </div>
</div>
`;

const ts = `tabViewItems: TabViewItemCore[] = [
    { title: 'Tab 1', content: 'Tab 1 content' },
    { title: 'Tab 2', content: 'Tab 2 content' },
  ];`;

@Component({
  selector: 'v-doc-primeng-tabview-model-example',
  imports: [TabViewModule],
  standalone: true,
  template: template,
})
export class PrimengTabViewModelExampleComponent {
  code = new DocumentationCodeBuilder()
    .addCode(ts, DOCS_CODE_LANG.TS)
    .addCode(template, DOCS_CODE_LANG.HTML)
    .buildCode();

  tabViewItems: TabViewItemCore[] = [
    { title: 'Tab 1', content: 'Tab 1 content' },
    { title: 'Tab 2', content: 'Tab 2 content' },
  ];
}
