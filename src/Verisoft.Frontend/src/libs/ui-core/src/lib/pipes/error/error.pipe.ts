import { Pipe, PipeTransform } from '@angular/core';
import { ValidationErrors } from '@angular/forms';
import { ErrorCodesFns } from './error.codes';
import { getFirstError } from './error.models';

const EMPTY = '';

@Pipe({
  name: 'error',
  standalone: true,
})
export class ErrorPipe implements PipeTransform {
  transform(errors: ValidationErrors | undefined | null): string {
    if (!errors) {
      return EMPTY;
    }
    const error = getFirstError(errors);
    if (error) {
      const errorFn = ErrorCodesFns[error.key];
      if (!errorFn) {
        return EMPTY;
      }
      return ErrorCodesFns[error.key](error.value);
    }
    return EMPTY;
  }
}
