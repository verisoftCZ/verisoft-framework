import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import {
  DOCS_CODE_LANG,
  DocumentationCodeBuilder,
} from '@verisoft/demo-documentation-ui';
import { TextareaComponent } from '@verisoft/ui-govcz';

const template = `
<div class="row">
  <div class="col col-md-6">
    <v-textarea label="Name"></v-textarea>
  </div>
  <div class="col col-md-6">
    <v-textarea label="Required" [required]="true"></v-textarea>
  </div>
  <div class="col col-md-6">
      <v-textarea label="Disabled" [(ngModel)]="value" [disabled]="true"></v-textarea>
  </div>
  <div class="col col-md-6">
    <v-textarea label="Non-adjustable" [autoResize]="true"></v-textarea>
  </div>
</div>
`;

@Component({
    selector: 'v-doc-govcz-text-area-basic-example',
    imports: [TextareaComponent, FormsModule],
    template: template
})
export class GovCzTextAreaBasicExampleComponent {
  value: string | null = null;
  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .buildCode();
}
