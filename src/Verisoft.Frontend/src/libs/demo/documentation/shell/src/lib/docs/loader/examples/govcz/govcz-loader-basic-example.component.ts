import { Component } from '@angular/core';
import {
  DOCS_CODE_LANG,
  DocumentationCodeBuilder,
} from '@verisoft/demo-documentation-ui';
import { LoaderComponent } from '@verisoft/ui-govcz';

const template = `
<div class= "d-flex justify-content-center">
  <v-loader></v-loader>
</div>

`;

@Component({
  selector: 'v-doc-govcz-loader-basic-example',
  imports: [LoaderComponent],
  standalone: true,
  template: template,
})
export class GovCzLoaderBasicExampleComponent {
  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .buildCode();
}
