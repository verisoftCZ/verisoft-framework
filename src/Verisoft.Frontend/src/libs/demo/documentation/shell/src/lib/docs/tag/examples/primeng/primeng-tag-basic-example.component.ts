import { Component } from '@angular/core';
import {
  DOCS_CODE_LANG,
  DocumentationCodeBuilder,
} from '@verisoft/demo-documentation-ui';
import { TagComponent } from '@verisoft/ui-primeng';

const template = `
<div class="row">
  <div class="col-12 d-flex gap-2">
    <v-tag label="Primary" [severity]="'primary'"></v-tag>
    <v-tag label="Success" [severity]="'success'"></v-tag>
    <v-tag label="Info" [severity]="'info'"></v-tag>
    <v-tag label="Warning" [severity]="'warning'"></v-tag>
    <v-tag label="Danger" [severity]="'danger'"></v-tag>
  </div>
  <div class="col-12 mt-3">
    <h5>Custom Content</h5>
    <div class="d-flex gap-2">
      <v-tag [severity]="'primary'">
        <i class="pi pi-info-circle me-2"></i>
        Info Tag
      </v-tag>
      <v-tag [severity]="'success'">
        <strong>Completed</strong>
      </v-tag>
      <v-tag [severity]="'warning'">
        <span>Pending</span>
      </v-tag>
    </div>
  </div>
</div>
`;

@Component({
  selector: 'v-doc-primeng-tag-basic-example',
  imports: [TagComponent],
  template,
})
export class PrimengTagBasicExampleComponent {
  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .buildCode();
}

