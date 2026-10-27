import { toCzechDateTimeString } from "@verisoft/core";

/* eslint-disable @typescript-eslint/ban-types */
export class Format {
    static concatArray(value: any[], itemFormatter?: Function, delimeter = ';') {
        if (!value) {
            return '';
        }

        return value
            .map(item => itemFormatter ? itemFormatter(item) : item)
            .filter(item => item !== undefined && item !== null)
            .join(`${delimeter} `);
    }

    static czechDate(value: Date | string) {
        return toCzechDateTimeString(value);
    }

    static date(value: Date | string) {
        if (!value) {
            return '';
        }

        if (typeof value === 'string') {
            value = new Date(value);
        }

        return value.getDate() + '. ' + (value.getMonth() + 1) + '. ' + value.getFullYear();
    }

    static dateAndTime(value: Date | string) {
        if (!value) {
            return '';
        }

        if (typeof value === 'string') {
            value = new Date(value);
        }

        return ('0' + value.getDate()).slice(-2)
            + '.' + ('0' + (value.getMonth() + 1)).slice(-2)
            + '.' + value.getFullYear() + ' ' + ('0' + value.getHours()).slice(-2)
            + ':' + ('0' + value.getMinutes()).slice(-2) + ':' + ('0' + value.getSeconds()).slice(-2);
    }

    static convertToUserLocaleDate(value?: Date | string) {
        if (!value) {
            return '';
        }

        if (typeof value === 'string') {
            value = new Date(value);
        }

        return value.toLocaleDateString(navigator.language, { timeZone: 'UTC' });
    }

    static bool(value: boolean) {
        if (value) {
            return 'Yes';
        } else {
            return 'No';
        }
    }

    static boolWithIcon(value: boolean) {
        if (value) {
            return `<span>✓</span>`;
        } else {
            return `<span>⨯</span>`;
        }
    }
}
