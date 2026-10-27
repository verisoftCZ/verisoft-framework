import { Component } from '@angular/core';
import {
  DOCS_CODE_LANG,
  DocumentationCodeBuilder,
} from '@verisoft/demo-documentation-ui';
import { IconComponent } from '@verisoft/ui-primeng';

const template = `
<div class="row">
  <div class="col-12">
    <h5>Prime Icons</h5>
    <v-icon name="home"></v-icon>
    <v-icon name="user" class="ms-3"></v-icon>
    <v-icon name="cog" class="ms-3"></v-icon>
    <v-icon name="sign-out" class="ms-3"></v-icon>
    <v-icon name="search" class="ms-3"></v-icon>
    <v-icon name="calendar" class="ms-3"></v-icon>
    <v-icon name="exclamation-triangle" class="ms-3"></v-icon>
  </div>
  <div class="col-12 mt-4">
    <h5>With Colors</h5>
    <v-icon name="check-circle" severity="success" class="me-3"></v-icon>
    <v-icon name="exclamation-triangle" severity="warning" class="me-3"></v-icon>
    <v-icon name="times-circle" severity="danger" class="me-3"></v-icon>
    <v-icon name="info-circle" severity="info" class="me-3"></v-icon>
    <v-icon name="home" severity="primary" class="me-3"></v-icon>
  </div>
</div>
`;

@Component({
  selector: 'v-doc-primeng-icon-basic-example',
  imports: [IconComponent],
  template,
})
export class PrimengIconBasicExampleComponent {
  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .buildCode();
}

