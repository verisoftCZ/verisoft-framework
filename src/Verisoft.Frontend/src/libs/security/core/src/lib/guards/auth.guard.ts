import { inject, Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivate,
  CanActivateChild,
  Router,
  UrlTree,
} from '@angular/router';
import { map, Observable, of, switchMap } from 'rxjs';
import { SecurityConfig } from '../models';
import { hasRequiredPermission } from '../models/functions';
import { SECURITY_CONFIG } from '../provider';
import { AuthContextService } from '../services';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate, CanActivateChild {
  private config = inject<SecurityConfig>(SECURITY_CONFIG);
  private router = inject(Router);
  private authContext = inject(AuthContextService);
  
  canActivate(route: ActivatedRouteSnapshot): Observable<boolean | UrlTree> {
    return this.checkPermissionsAndRolesAndNavigate(route, this.config.notAuthorizedPage);
  }

  canActivateChild(
    childRoute: ActivatedRouteSnapshot
  ): Observable<boolean | UrlTree> {
    return this.checkPermissionsAndRolesAndNavigate(childRoute, this.config.notAuthorizedPage);
  }

  private checkPermissionsAndRolesAndNavigate(
    route: ActivatedRouteSnapshot,
    notAuthorizedUrl: string | undefined
  ): Observable<boolean | UrlTree> {
    const requiredPermissions = route.data['permissions'] as
      | string
      | string[]
      | undefined;
    const requiredRoles = route.data['roles'] as string | string[] | undefined;

    return this.authContext.user$.pipe(
      map(
        (user) =>
          user &&
          (!requiredPermissions ||
            hasRequiredPermission(user, requiredPermissions)) &&
          (!requiredRoles || hasRequiredPermission(user, requiredRoles))
      ),
      switchMap((hasPermission) => {
        if (!hasPermission && notAuthorizedUrl) {
          return of(this.router.parseUrl(notAuthorizedUrl));
        }
        return of(!!hasPermission);
      })
    );
  }
}
