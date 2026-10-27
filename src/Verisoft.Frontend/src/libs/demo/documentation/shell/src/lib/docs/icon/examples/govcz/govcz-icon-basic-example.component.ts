import { Component } from '@angular/core';
import {
  DOCS_CODE_LANG,
  DocumentationCodeBuilder,
} from '@verisoft/demo-documentation-ui';
import { IconComponent } from '@verisoft/ui-govcz';

const template = `
<div class="row">
  <div class="col-12">
    <h5>Gov Icons</h5>
    <v-icon name="house" library="native"></v-icon>
    <v-icon name="user" library="native" class="ms-3"></v-icon>
    <v-icon name="settings" library="native" class="ms-3"></v-icon>
    <v-icon name="logout" library="native" class="ms-3"></v-icon>
    <v-icon name="search" library="native" class="ms-3"></v-icon>
    <v-icon name="calendar" library="native" class="ms-3"></v-icon>
    <v-icon name="exclamation-triangle-fill" library="native" class="ms-3"></v-icon>
  </div>
  <div class="col-12 mt-4">
    <h5>Bootstrap Icons</h5>
    <v-icon name="house-door-fill" library="bootstrap"></v-icon>
    <v-icon name="person-fill" library="bootstrap" class="ms-3"></v-icon>
    <v-icon name="gear-fill" library="bootstrap" class="ms-3"></v-icon>
    <v-icon name="box-arrow-right" library="bootstrap" class="ms-3"></v-icon>
    <v-icon name="search" library="bootstrap" class="ms-3"></v-icon>
    <v-icon name="calendar-event" library="bootstrap" class="ms-3"></v-icon>
    <v-icon name="exclamation-triangle-fill" library="bootstrap" class="ms-3"></v-icon>
  </div>
  <div class="col-12 mt-4">
    <h5>With Colors (Gov)</h5>
    <v-icon name="check-circle" library="native" severity="success" class="me-3"></v-icon>
    <v-icon name="exclamation-triangle-fill" library="native" severity="warning" class="me-3"></v-icon>
    <v-icon name="x-circle" library="native" severity="danger" class="me-3"></v-icon>
    <v-icon name="info-circle" library="native" severity="info" class="me-3"></v-icon>
    <v-icon name="house" library="native" severity="primary" class="me-3"></v-icon>
  </div>
  <div class="col-12 mt-4">
    <h5>With Colors (Bootstrap)</h5>
    <v-icon name="check-circle-fill" library="bootstrap" severity="success" class="me-3"></v-icon>
    <v-icon name="exclamation-triangle-fill" library="bootstrap" severity="warning" class="me-3"></v-icon>
    <v-icon name="x-circle-fill" library="bootstrap" severity="danger" class="me-3"></v-icon>
    <v-icon name="info-circle-fill" library="bootstrap" severity="info" class="me-3"></v-icon>
    <v-icon name="house-fill" library="bootstrap" severity="primary" class="me-3"></v-icon>
  </div>
</div>
`;

@Component({
    selector: 'v-doc-govcz-icon-basic-example',
    imports: [IconComponent],
    template: template
})
export class GovCzIconBasicExampleComponent {
  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .buildCode();
}

