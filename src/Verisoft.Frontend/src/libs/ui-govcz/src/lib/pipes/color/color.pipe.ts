import { Pipe, PipeTransform } from "@angular/core";
import { ControlSeverity, ControlSeverityType, GovControlSeverity } from "@verisoft/ui-core";

@Pipe({
    name: 'govColor',
    standalone: true,
})
export class GovColorPipe implements PipeTransform {
    transform(color: ControlSeverityType | undefined): string | undefined {
        switch (color) {
            case ControlSeverity.danger:
                return GovControlSeverity.error;
            
            case ControlSeverity.info:
                return GovControlSeverity.neutral;

            case ControlSeverity.help:
            case ControlSeverity.contrast:
                return GovControlSeverity.primary;
        }

        return color;
    }
}
