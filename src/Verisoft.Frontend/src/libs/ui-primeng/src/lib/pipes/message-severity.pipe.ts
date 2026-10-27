import { Pipe, PipeTransform } from '@angular/core';
import { ControlSeverity, ControlSeverityType } from '@verisoft/ui-core';

@Pipe({
    name: 'messageSeverity',
    standalone: true
})
export class MessageSeverityPipe implements PipeTransform {
    transform(severity: ControlSeverityType): 'success' | 'info' | 'warn' | 'error' | 'secondary' | 'contrast' | undefined | null {
        switch (severity) {
            case ControlSeverity.success:
                return 'success';
            case ControlSeverity.info:
            case ControlSeverity.help:
                return 'info';
            case ControlSeverity.warning:
                return 'warn';
            case ControlSeverity.danger:
                return 'error';
            case ControlSeverity.secondary:
                return 'secondary';
            case ControlSeverity.contrast:
                return 'contrast';
        }

        return 'info';
    }
}