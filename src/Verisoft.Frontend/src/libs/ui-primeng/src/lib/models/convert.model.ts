import { ControlSeverity, ControlSeverityType, FieldSize, FieldSizeType } from "@verisoft/ui-core";

export type PrimeMessageSeverity = 'success' | 'secondary' | 'info' | 'warn' | 'danger' | 'contrast' | undefined | null;

export type PrimeSeverity = 'success' | 'info' | 'warn' | 'danger' | 'help' | 'primary' | 'secondary' | 'contrast' | null | undefined;

export const convertToPrimeSeverity = (severity: ControlSeverityType | undefined | null): PrimeSeverity => {
    switch (severity) {
        case ControlSeverity.success:
            return 'success';
        case ControlSeverity.primary:
            return 'primary';
        case ControlSeverity.info:
            return 'info';
        case ControlSeverity.help:
            return 'help';
        case ControlSeverity.secondary:
            return 'secondary';
        case ControlSeverity.contrast:
            return 'contrast';
        case ControlSeverity.warning:
            return 'warn';
        case ControlSeverity.danger:
            return 'danger';
        default:
            return null;
    }
}

export const convertToMessagePrimeSeverity = (severity: ControlSeverityType | undefined | null): PrimeMessageSeverity => {
    switch (severity) {
        case ControlSeverity.success:
            return 'success';
        case ControlSeverity.info:
            return 'info';
        case ControlSeverity.warning:
            return 'warn';
        case ControlSeverity.danger:
            return 'danger';
        default:
            return null;
    }   
}

export type PrimeSize = 'small' | 'large' | undefined | null;

export const convertToPrimeSize = (size: FieldSizeType | undefined | null): PrimeSize => {
    switch (size) {
        case FieldSize.small:
            return 'small';
        case FieldSize.large:
        case FieldSize.xl:
            return 'large';
        default:
            return null;
    }
}