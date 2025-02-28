import { Component } from '@angular/core';
import {
  DOCS_CODE_LANG,
  DocumentationCodeBuilder,
} from '@verisoft/demo-documentation-ui';
import { SectionComponent, TextfieldComponent } from '@verisoft/ui-govcz';

const template = `
<v-section title="Example section">
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
  selector: 'v-govcz-doc-section-basic-example',
  imports: [TextfieldComponent, SectionComponent],
  standalone: true,
  template: template,
})
export class GovCzSectionBasicExampleComponent {
  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .buildCode();
}
