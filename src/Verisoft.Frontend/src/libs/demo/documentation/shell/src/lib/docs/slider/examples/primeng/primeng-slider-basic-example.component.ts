import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import {
  DOCS_CODE_LANG,
  DocumentationCodeBuilder,
} from '@verisoft/demo-documentation-ui';
import { SliderComponent } from '@verisoft/ui-primeng';

const template = `
<div class="row">
  <div class="col col-md-6">
    <v-slider label="Age"></v-slider>
  </div>
  <div class="col col-md-6">
    <v-slider label="Disabled" [(ngModel)]="value" [disabled]="true"></v-slider>
  </div>
</div>
`;

@Component({
  selector: 'v-doc-primeng-slider-basic-example',
  imports: [SliderComponent, FormsModule],
  standalone: true,
  template: template,
})
export class PrimengSliderBasicExampleComponent {
  value: number | null = null;
  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .buildCode();
}
