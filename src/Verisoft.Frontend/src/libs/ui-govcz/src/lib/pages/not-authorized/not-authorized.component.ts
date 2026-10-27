import { Component } from "@angular/core";
import { GovDesignSystemModule } from "@gov-design-system-ce/angular";
import { ButtonComponent } from "../../components";

@Component({
    selector: 'v-not-authorized',
    templateUrl: "./not-authorized.component.html",
    imports: [
        GovDesignSystemModule,
        ButtonComponent,
    ]
})
export class NotAuthorizedComponent {
    goBack(): void {
        history.back();
    }
}
