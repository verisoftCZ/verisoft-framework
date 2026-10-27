import { Component } from "@angular/core";
import { GovDesignSystemModule } from "@gov-design-system-ce/angular";
import { ButtonComponent } from "./../../components";

@Component({
    selector: 'v-not-authenticated',
    templateUrl: "./not-authenticated.component.html",
    imports: [
        GovDesignSystemModule,
        ButtonComponent,
    ]
})
export class NotAuthenticatedComponent { }
