import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import {
  DOCS_CODE_LANG,
  DocumentationCodeBuilder,
} from '@verisoft/demo-documentation-ui';
import { HeaderComponent, ButtonComponent } from '@verisoft/ui-primeng';

const template = `
<v-header 
  title="Header with Content Projection"
  userName="John Doe"
  userRole="Administrator"
  imageUrl="assets/images/_global/logos/v-logo.webp"
  [fixed]="true">
  
  <div v-header-module>
    <span class="text-muted">Module: Administration</span>
  </div>
  
  <div v-header-actions>
    <v-button label="Action 1" severity="secondary" />
    <v-button label="Action 2" severity="secondary" />
  </div>
  
  <div v-header-info>
    <div class="d-flex align-items-center gap-2">
      <i class="pi pi-bell"></i>
      <span class="badge bg-danger">3</span>
    </div>
  </div>
</v-header>
`;

@Component({
    selector: 'v-doc-primeng-header-content-projection-example',
    imports: [HeaderComponent, ButtonComponent, CommonModule],
    template: template
})
export class PrimengHeaderContentProjectionExampleComponent {
  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .buildCode();
}
