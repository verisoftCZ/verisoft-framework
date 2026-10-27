import { Component } from "@angular/core";
import { GovDesignSystemModule } from "@gov-design-system-ce/angular";
import { ButtonComponent } from "../../components";

@Component({
    selector: 'v-not-found',
    templateUrl: "./not-found.component.html",
    imports: [
        GovDesignSystemModule,
        ButtonComponent
    ]
})
export class NotFoundComponent {
    goBack(): void {
        history.back();
    }
}
