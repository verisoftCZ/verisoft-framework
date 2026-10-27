import { Component } from '@angular/core';
import {
  DOCS_CODE_LANG,
  DocumentationCodeBuilder,
} from '@verisoft/demo-documentation-ui';
import { TagComponent } from '@verisoft/ui-govcz';

const template = `
<div class="row">
  <div class="col-12">
    <h5>Bold Type (Default)</h5>
    <v-tag label="Primary" [severity]="'primary'" variant="bold"></v-tag>
    <v-tag label="Success" [severity]="'success'" variant="bold" class="ms-2"></v-tag>
    <v-tag label="Error" [severity]="'danger'" variant="bold" class="ms-2"></v-tag>
    <v-tag label="Warning" [severity]="'warning'" variant="bold" class="ms-2"></v-tag>
  </div>
  <div class="col-12 mt-3">
    <h5>Subtle Type</h5>
    <v-tag label="Primary" [severity]="'primary'" variant="subtle"></v-tag>
    <v-tag label="Success" [severity]="'success'" variant="subtle" class="ms-2"></v-tag>
    <v-tag label="Error" [severity]="'danger'" variant="subtle" class="ms-2"></v-tag>
    <v-tag label="Warning" [severity]="'warning'" variant="subtle" class="ms-2"></v-tag>
  </div>
</div>
`;

@Component({
    selector: 'v-doc-govcz-tag-type-example',
    imports: [TagComponent],
    template: template
})
export class GovCzTagTypeExampleComponent {
  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .buildCode();
}

