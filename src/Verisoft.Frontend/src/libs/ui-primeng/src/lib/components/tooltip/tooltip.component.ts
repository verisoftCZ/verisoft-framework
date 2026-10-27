import { Component, input } from "@angular/core";
import { ControlSeverityType, FieldSizeType, PositionType, TooltipCore } from "@verisoft/ui-core";
import { TooltipModule } from "primeng/tooltip";

@Component({
    selector: "v-tooltip",
    templateUrl: './tooltip.component.html',
    imports: [
        TooltipModule,
    ]
})
export class TooltipComponent implements TooltipCore {
    color = input<ControlSeverityType | undefined>(undefined);
    position = input<PositionType | undefined>(undefined);
    size = input<FieldSizeType | undefined>(undefined);
    tooltipText = input<string | undefined>(undefined);
}
