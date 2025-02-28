import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import {
  DOCS_CODE_LANG,
  DocumentationCodeBuilder,
} from '@verisoft/demo-documentation-ui';
import { TextareaComponent } from '@verisoft/ui-primeng';

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
  selector: 'v-doc-primeng-text-area-basic-example',
  imports: [TextareaComponent, FormsModule],
  standalone: true,
  template: template,
})
export class PrimengTextAreaBasicExampleComponent {
  value: string | null = null;
  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .buildCode();
}
