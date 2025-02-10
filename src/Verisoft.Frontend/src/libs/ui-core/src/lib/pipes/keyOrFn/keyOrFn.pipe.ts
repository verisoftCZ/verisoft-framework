/* eslint-disable @typescript-eslint/no-explicit-any */
import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'keyOrFn',
  pure: false,
  standalone: true,
})
export class KeyOrFunctionPipe implements PipeTransform {
  transform(keyOrFn: any, row: any): any {
    if (keyOrFn instanceof Function) {
      return keyOrFn(row);
    } else if (typeof keyOrFn === 'string') {
      const value = row[keyOrFn];
      if (value) {
        return value;
      }
    } else if (typeof keyOrFn === 'boolean') {
      return keyOrFn;
    }
    return '';
  }
}
