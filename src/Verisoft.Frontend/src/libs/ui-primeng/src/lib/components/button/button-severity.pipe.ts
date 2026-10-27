import { Pipe, PipeTransform } from '@angular/core';
import { ControlSeverity, ControlSeverityType } from '@verisoft/ui-core';
import { ButtonSeverity } from 'primeng/button';

@Pipe({
    name: 'buttonSeverity'
})
export class ButtonSeverityPipe implements PipeTransform {
    transform(severity: ControlSeverityType | undefined | null): ButtonSeverity {
        if (severity === undefined || severity === null) {
            return null;
        }

        if (severity === ControlSeverity.warning) {
            return 'warn';
        }

        return severity;
    }
}