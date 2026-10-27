import { Component } from '@angular/core';
import {
  DOCS_CODE_LANG,
  DocumentationCodeBuilder,
} from '@verisoft/demo-documentation-ui';
import { TagComponent } from '@verisoft/ui-primeng';

const template = `
<div class="row g-4">
  <div class="col-12">
    <h5>Bold Variant</h5>
    <div class="d-flex gap-2 align-items-center">
      <v-tag label="Small" variant="bold" size="small" [severity]="'primary'"></v-tag>
      <v-tag label="Medium" variant="bold" size="medium" [severity]="'success'"></v-tag>
      <v-tag label="Large" variant="bold" size="large" [severity]="'warning'"></v-tag>
      <v-tag label="Large Accent" variant="bold" size="large" [severity]="'danger'"></v-tag>
    </div>
  </div>
  <div class="col-12">
    <h5>Subtle Variant</h5>
    <div class="d-flex gap-2 align-items-center">
      <v-tag label="Small" variant="subtle" size="small" [severity]="'primary'"></v-tag>
      <v-tag label="Medium" variant="subtle" size="medium" [severity]="'success'"></v-tag>
      <v-tag label="Large" variant="subtle" size="large" [severity]="'warning'"></v-tag>
      <v-tag label="Large Accent" variant="subtle" size="large" [severity]="'danger'"></v-tag>
    </div>
  </div>
</div>
`;

@Component({
  selector: 'v-doc-primeng-tag-variant-example',
  imports: [TagComponent],
  template,
})
export class PrimengTagVariantExampleComponent {
  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .buildCode();
}

