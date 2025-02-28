import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { map, Observable } from 'rxjs';
import { AuthenticatedUser } from '../models';
import { hasRequiredPermission, hasRequiredRole } from '../models/functions';
import { setUser } from '../state/actions';
import { selectIsAuthenticated, selectUser } from '../state/selectors';

@Injectable({
  providedIn: 'root',
})
export class AuthContextService {
  user$: Observable<AuthenticatedUser | undefined>;
  isAuthenticated$: Observable<boolean>;

  constructor(private store: Store) {
    this.user$ = this.store.select(selectUser);
    this.isAuthenticated$ = this.store.select(selectIsAuthenticated);
  }

  setUser(user: AuthenticatedUser | undefined): void {
    this.store.dispatch(setUser({ user }));
  }

  hasRequiredPermission(
    requiredPermissions: string | string[]
  ): Observable<boolean> {
    return this.user$.pipe(
      map((user) => hasRequiredPermission(user, requiredPermissions))
    );
  }

  hasRequiredRole(requiredPermissions: string | string[]): Observable<boolean> {
    return this.user$.pipe(
      map((user) => hasRequiredRole(user, requiredPermissions))
    );
  }
}
