import { Component } from '@angular/core';
import {
  DOCS_CODE_LANG,
  DocumentationCodeBuilder,
} from '@verisoft/demo-documentation-ui';
import { SectionComponent, TextfieldComponent } from '@verisoft/ui-primeng';

const template = `
<v-section title="Example of a hidden section" [showContent]="false" backgroundColor="#efefef">
  <div class="row">
    <div class="col col-md-6">
      <v-textfield label="Name"/>
    </div>
    <div class="col col-md-6">
      <v-textfield label="Required" [required]="true"/>
    </div>
  </div>
</v-section>
`;

@Component({
  selector: 'v-doc-primeng-section-advanced-example',
  imports: [TextfieldComponent, SectionComponent],
  standalone: true,
  template: template,
})
export class PrimengSectionAdvancedExampleComponent {
  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .buildCode();
}
