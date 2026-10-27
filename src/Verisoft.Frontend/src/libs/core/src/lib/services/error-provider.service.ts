import { inject, Injectable } from '@angular/core';
import { ValidationErrors } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';
import { Observable, of } from 'rxjs';
import { ErrorProvider, ErrorMap } from '../models/error-provider.model';

@Injectable({
  providedIn: 'root'
})
export class LocalizableErrorProviderService implements ErrorProvider {
  private readonly translateService = inject(TranslateService);
  readonly errors: Record<string, (value?: any) => string> = ErrorMap;

  mapError(errors: ValidationErrors): Observable<string | undefined> {
    const error = this.getFirstValue(errors);
    if (!error || !error.key) {
      return of(undefined);
    }

    try {
      const value = this.errors[error.key](error.value);
      return this.translateService.get(value);
    } catch (_) {
      throw new Error(`Key "${error.key}" is not specified in the current error map!`);
    }
  }

  private getFirstValue(errors: ValidationErrors): { key: string; value: string } {
    const key = Object.keys(errors)[0];
    const value = errors[key];
    return { key, value };
  }
}
