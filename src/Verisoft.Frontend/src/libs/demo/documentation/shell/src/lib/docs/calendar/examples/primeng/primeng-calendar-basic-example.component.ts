import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import {
  DOCS_CODE_LANG,
  DocumentationCodeBuilder,
} from '@verisoft/demo-documentation-ui';
import { CalendarComponent } from '@verisoft/ui-primeng';

const template = `
<div class="row">
  <div class="col col-md-6">
    <v-calendar label="Calendar"/>
  </div>
  <div class="col col-md-6">
    <v-calendar label="Required" [required]="true"/>
  </div>
  <div class="col col-md-6">
    <v-calendar label="Disabled" [(ngModel)]="value" [disabled]="true"/>
  </div>
</div>
`;

@Component({
  selector: 'v-doc-primeng-calendar-basic-example',
  imports: [CalendarComponent, FormsModule],
  standalone: true,
  template: template,
})
export class PrimengCalendarBasicExampleComponent {
  value = null;
  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .buildCode();
}
