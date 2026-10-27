import { Component } from '@angular/core';
import {
  DOCS_CODE_LANG,
  DocumentationCodeBuilder,
} from '@verisoft/demo-documentation-ui';
import { TagComponent } from '@verisoft/ui-govcz';

const template = `
<div class="row">
  <div class="col-12 d-flex gap-2">
    <v-tag label="Primary" [severity]="'primary'" />
    <v-tag label="Secondary" [severity]="'secondary'" />
    <v-tag label="Neutral" [severity]="'info'" />
    <v-tag label="Error" [severity]="'danger'" />
    <v-tag label="Success" [severity]="'success'" />
    <v-tag label="Warning" [severity]="'warning'" />
  </div>
  <div class="col-12 mt-3">
    <h5>Small Size</h5>
    <div class="d-flex gap-2">
      <v-tag label="Primary" [severity]="'primary'" size="small" />
      <v-tag label="Success" [severity]="'success'" size="small" />
      <v-tag label="Error" [severity]="'danger'" size="small" />
    </div>
  </div>
  <div class="col-12 mt-3">
    <h5>Medium Size (Default)</h5>
    <div class="d-flex gap-2">
      <v-tag label="Primary" [severity]="'primary'" size="medium" />
      <v-tag label="Success" [severity]="'success'" size="medium" />
      <v-tag label="Error" [severity]="'danger'" size="medium" />
    </div>
  </div>
  <div class="col-12 mt-3">
    <h5>Large Size</h5>
    <div class="d-flex gap-2">
      <v-tag label="Primary" [severity]="'primary'" size="large" />
      <v-tag label="Success" [severity]="'success'" size="large" />
      <v-tag label="Error" [severity]="'danger'" size="large" />
    </div>
  </div>
  <div class="col-12 mt-3">
    <h5>With Custom Content</h5>
    <div class="d-flex gap-2">
      <v-tag [severity]="'primary'">Custom Text</v-tag>
      <v-tag [severity]="'success'">Completed</v-tag>
      <v-tag [severity]="'warning'">Warning</v-tag>
    </div>
  </div>
</div>
`;

@Component({
    selector: 'v-doc-govcz-tag-basic-example',
    imports: [TagComponent],
    template: template
})
export class GovCzTagBasicExampleComponent {
  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .buildCode();
}

