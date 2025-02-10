import { inject, Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { SecurityConfig } from '../models';
import { SECURITY_CONFIG } from '../provider';
import { TokenProvider } from './token-provider';

@Injectable()
export class SessionStorageTokenProvider implements TokenProvider {
  private config = inject<SecurityConfig>(SECURITY_CONFIG);

  getToken(): Observable<string | undefined> {
    const token = sessionStorage.getItem(this.config.tokenStorageKey);
    return of(token ?? undefined);
  }
}
