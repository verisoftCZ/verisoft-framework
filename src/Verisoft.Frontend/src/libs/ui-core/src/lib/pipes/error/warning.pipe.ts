import { Pipe, PipeTransform } from '@angular/core';
import { ValidationErrors } from '@angular/forms';
import { getFirstError } from './error.models';
import { WarningCodesFns } from './warning.codes';

const EMPTY = '';

@Pipe({
  name: 'warning',
  standalone: true,
})
export class WarningPipe implements PipeTransform {
  transform(warnings: ValidationErrors | undefined | null): string {
    if (!warnings) {
      return EMPTY;
    }
    const error = getFirstError(warnings);
    if (error) {
      const errorFn = WarningCodesFns[error.key];
      if (!errorFn) {
        return EMPTY;
      }
      return WarningCodesFns[error.key](error.value);
    }
    return EMPTY;
  }
}
