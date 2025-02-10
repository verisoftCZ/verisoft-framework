import { Component, Input } from "@angular/core";
import { GovDesignSystemModule } from "@gov-design-system-ce/angular";
import { GovSizePipe } from "../../pipes";

@Component({
    standalone: true,
    selector: "v-tooltip",
    templateUrl: './tooltip.component.html',
    styleUrl: "./tooltip.component.scss",
    imports: [
        GovDesignSystemModule,
        GovSizePipe
    ],
})
export class TooltipComponent {
    @Input() color: 'primary' | 'secondary' = 'primary';

    @Input() size: 'small' | 'medium' | 'large' = 'medium';

    @Input() position: 'top' | 'right' | 'bottom' | 'left' = 'top';

    @Input({required: true}) message!: string;

    @Input() label = '?';
}