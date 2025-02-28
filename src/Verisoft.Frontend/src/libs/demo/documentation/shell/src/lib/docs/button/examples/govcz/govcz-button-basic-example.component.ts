import { Component } from '@angular/core';
import {
  DOCS_CODE_LANG,
  DocumentationCodeBuilder,
} from '@verisoft/demo-documentation-ui';
import { ButtonComponent, Icons } from '@verisoft/ui-govcz';

const template = `
<div class="row">
  <div class="col-12">
    <v-button label="Primary" (click)="fireClick()"></v-button>
    <v-button label="Secondary" class="ms-2" severity="secondary" (click)="fireClick()"></v-button>
    <v-button label="Success" class="ms-2" severity="success" (click)="fireClick()"></v-button>
    <v-button label="Info" class="ms-2" severity="info" (click)="fireClick()"></v-button>
    <v-button label="Warning" class="ms-2" severity="warning" (click)="fireClick()"></v-button>
    <v-button label="Danger" class="ms-2" severity="danger" (click)="fireClick()"></v-button>
    <v-button label="Help" class="ms-2" severity="help" (click)="fireClick()"></v-button>
    <v-button label="Contrast" class="ms-2" severity="contrast" (click)="fireClick()"></v-button>
  </div>
  <div class="col-12 mt-2">
    <v-button label="Primary" [disabled]="true" (click)="fireClick()"></v-button>
    <v-button label="Secondary" [disabled]="true" class="ms-2" severity="secondary" (click)="fireClick()"></v-button>
    <v-button label="Success" [disabled]="true" class="ms-2" severity="success" (click)="fireClick()"></v-button>
    <v-button label="Info" [disabled]="true" class="ms-2" severity="info" (click)="fireClick()"></v-button>
    <v-button label="Warning" [disabled]="true" class="ms-2" severity="warning" (click)="fireClick()"></v-button>
    <v-button label="Danger" [disabled]="true" class="ms-2" severity="danger" (click)="fireClick()"></v-button>
    <v-button label="Help" [disabled]="true" class="ms-2" severity="help" (click)="fireClick()"></v-button>
    <v-button label="Contrast" [disabled]="true" class="ms-2" severity="contrast" (click)="fireClick()"></v-button>
  </div>
  <div class="col-12 mt-2">
    <v-button label="Primary" [outlined]="true" (click)="fireClick()"></v-button>
    <v-button label="Secondary" [outlined]="true" class="ms-2" severity="secondary" (click)="fireClick()"></v-button>
    <v-button label="Success" [outlined]="true" class="ms-2" severity="success" (click)="fireClick()"></v-button>
    <v-button label="Info" [outlined]="true" class="ms-2" severity="info" (click)="fireClick()"></v-button>
    <v-button label="Warning" [outlined]="true" class="ms-2" severity="warning" (click)="fireClick()"></v-button>
    <v-button label="Danger" [outlined]="true" class="ms-2" severity="danger" (click)="fireClick()"></v-button>
    <v-button label="Help" [outlined]="true" class="ms-2" severity="help" (click)="fireClick()"></v-button>
    <v-button label="Contrast" [outlined]="true" class="ms-2" severity="contrast" (click)="fireClick()"></v-button>
  </div>
  <div class="col-12 mt-2">
    <v-button label="Primary" [rounded]="true" (click)="fireClick()"></v-button>
    <v-button label="Secondary" [rounded]="true" class="ms-2" severity="secondary" (click)="fireClick()"></v-button>
    <v-button label="Success" [rounded]="true" class="ms-2" severity="success" (click)="fireClick()"></v-button>
    <v-button label="Info" [rounded]="true" class="ms-2" severity="info" (click)="fireClick()"></v-button>
    <v-button label="Warning" [rounded]="true" class="ms-2" severity="warning" (click)="fireClick()"></v-button>
    <v-button label="Danger" [rounded]="true" class="ms-2" severity="danger" (click)="fireClick()"></v-button>
    <v-button label="Help" [rounded]="true" class="ms-2" severity="help" (click)="fireClick()"></v-button>
    <v-button label="Contrast" [rounded]="true" class="ms-2" severity="contrast" (click)="fireClick()"></v-button>
  </div>
  <div class="col-12 mt-2">
    <v-button  [icon]="icons.save" severity="primary" (click)="fireClick()"></v-button>
    <v-button  [icon]="icons.save" class="ms-2" severity="secondary" (click)="fireClick()"></v-button>
    <v-button  [icon]="icons.save" class="ms-2" severity="success" (click)="fireClick()"></v-button>
    <v-button  [icon]="icons.save" class="ms-2" severity="info" (click)="fireClick()"></v-button>
    <v-button  [icon]="icons.save" class="ms-2" severity="warning" (click)="fireClick()"></v-button>
    <v-button  [icon]="icons.save" class="ms-2" severity="danger" (click)="fireClick()"></v-button>
    <v-button  [icon]="icons.save" class="ms-2" severity="help" (click)="fireClick()"></v-button>
    <v-button  [icon]="icons.save" class="ms-2" severity="contrast" (click)="fireClick()"></v-button>
  </div>
  <div class="col-12 mt-2">
    <v-button  [icon]="icons.save" [outlined]="true" severity="primary" (click)="fireClick()"></v-button>
    <v-button  [icon]="icons.save" [outlined]="true" class="ms-2" severity="secondary" (click)="fireClick()"></v-button>
    <v-button  [icon]="icons.save" [outlined]="true" class="ms-2" severity="success" (click)="fireClick()"></v-button>
    <v-button  [icon]="icons.save" [outlined]="true" class="ms-2" severity="info" (click)="fireClick()"></v-button>
    <v-button  [icon]="icons.save" [outlined]="true" class="ms-2" severity="warning" (click)="fireClick()"></v-button>
    <v-button  [icon]="icons.save" [outlined]="true" class="ms-2" severity="danger" (click)="fireClick()"></v-button>
    <v-button  [icon]="icons.save" [outlined]="true" class="ms-2" severity="help" (click)="fireClick()"></v-button>
    <v-button  [icon]="icons.save" [outlined]="true" class="ms-2" severity="contrast" (click)="fireClick()"></v-button>
  </div>
  <div class="col-12 mt-2">
    <v-button  [icon]="icons.save" [rounded]="true" severity="primary" (click)="fireClick()"></v-button>
    <v-button  [icon]="icons.save" [rounded]="true" class="ms-2" severity="secondary" (click)="fireClick()"></v-button>
    <v-button  [icon]="icons.save" [rounded]="true" class="ms-2" severity="success" (click)="fireClick()"></v-button>
    <v-button  [icon]="icons.save" [rounded]="true" class="ms-2" severity="info" (click)="fireClick()"></v-button>
    <v-button  [icon]="icons.save" [rounded]="true" class="ms-2" severity="warning" (click)="fireClick()"></v-button>
    <v-button  [icon]="icons.save" [rounded]="true" class="ms-2" severity="danger" (click)="fireClick()"></v-button>
    <v-button  [icon]="icons.save" [rounded]="true" class="ms-2" severity="help" (click)="fireClick()"></v-button>
    <v-button  [icon]="icons.save" [rounded]="true" class="ms-2" severity="contrast" (click)="fireClick()"></v-button>
  </div>
</div>
`;

@Component({
  selector: 'v-doc-govcz-button-basic-example',
  imports: [ButtonComponent],
  standalone: true,
  template: template,
})
export class GovCzButtonBasicExampleComponent {
  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .buildCode();

  icons = Icons;

  fireClick() {
    alert('Button Clicked');
  }
}
