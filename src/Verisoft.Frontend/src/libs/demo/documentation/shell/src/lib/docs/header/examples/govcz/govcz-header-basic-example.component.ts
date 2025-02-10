import { Component } from '@angular/core';
import {
  DOCS_CODE_LANG,
  DocumentationCodeBuilder,
} from '@verisoft/demo-documentation-ui';
import { HeaderComponent } from '@verisoft/ui-govcz';

const template = `
<v-header 
  title="Header example"
  userName="This is a name"
  userRole="And this is a role"
  imageUrl="assets/images/_global/logos/v-logo.webp"
/>
`;

@Component({
  selector: 'v-doc-govcz-header-basic-example',
  imports: [HeaderComponent],
  standalone: true,
  template: template,
})
export class GovCzHeaderBasicExampleComponent {
  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .buildCode();
}
