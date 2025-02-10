import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import {
  DOCS_CODE_LANG,
  DocumentationCodeBuilder,
} from '@verisoft/demo-documentation-ui';
import {
  ActionButtonComponent,
  ActionButtonGroupComponent,
  FilterComponent,
  FilterFieldDirective,
  Icons,
} from '@verisoft/ui-govcz';

const template = `
    <v-filter title="Demo user list" [autoBind]="true" [(ngModel)]="filter">
      <v-filter-field name="name" label="Name" type="text"></v-filter-field>
      <v-action-button-group [maxItems]="2">
        <v-action-button label="Add" [icon]="icons.add"></v-action-button>
        <v-action-button label="Delete" [icon]="icons.delete"></v-action-button>
        <v-action-button
          label="Send Email"
          icon="pi pi-envelope"
        ></v-action-button>
        <v-action-button label="Settings" [icon]="icons.settings"></v-action-button>
      </v-action-button-group>
    </v-filter>
`;

@Component({
  selector: 'v-doc-govcz-filter-basic-example',
  imports: [
    FilterComponent,
    ActionButtonGroupComponent,
    ActionButtonComponent,
    FilterFieldDirective,
    FormsModule,
  ],
  standalone: true,
  template: template,
})
export class GovCzFilterBasicExampleComponent {
  filter: unknown;

  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .buildCode();

  icons = Icons;
}
