import { Component } from '@angular/core';
import {
  DOCS_CODE_LANG,
  DocumentationCodeBuilder,
} from '@verisoft/demo-documentation-ui';
import { IconComponent } from '@verisoft/ui-primeng';

const template = `
<div class="row">
  <div class="col-12">
    <h5>Prime Icons - Different Sizes</h5>
    <div class="d-flex align-items-center gap-3">
      <v-icon name="home" size="small"></v-icon>
      <v-icon name="home" size="medium"></v-icon>
      <v-icon name="home" size="large"></v-icon>
      <v-icon name="home" size="xl"></v-icon>
    </div>
  </div>
  <div class="col-12 mt-4">
    <h5>Sizes with Labels</h5>
    <div class="row">
      <div class="col-auto">
        <div class="text-center">
          <v-icon name="calendar" size="small"></v-icon>
          <div class="small mt-1">Small</div>
        </div>
      </div>
      <div class="col-auto">
        <div class="text-center">
          <v-icon name="calendar" size="medium"></v-icon>
          <div class="small mt-1">Medium</div>
        </div>
      </div>
      <div class="col-auto">
        <div class="text-center">
          <v-icon name="calendar" size="large"></v-icon>
          <div class="small mt-1">Large</div>
        </div>
      </div>
      <div class="col-auto">
        <div class="text-center">
          <v-icon name="calendar" size="xl"></v-icon>
          <div class="small mt-1">XL</div>
        </div>
      </div>
    </div>
  </div>
</div>
`;

@Component({
  selector: 'v-doc-primeng-icon-size-example',
  imports: [IconComponent],
  template,
})
export class PrimengIconSizeExampleComponent {
  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .buildCode();
}

