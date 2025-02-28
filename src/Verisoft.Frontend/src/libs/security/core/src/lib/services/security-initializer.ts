import { Router } from '@angular/router';
import { firstValueFrom, from, of, switchMap } from 'rxjs';
import { convertJWTToUser, SecurityConfig } from '../models';
import { AuthContextService } from './auth-context.service';
import { TokenProvider } from './token-provider';

export function securityInitializerFactory(
  tokenProvider: TokenProvider,
  authService: AuthContextService,
  config: SecurityConfig,
  router: Router
): () => Promise<unknown> {
  const initializationFn = tokenProvider.getToken().pipe(
    switchMap((token) => {
      const user = convertJWTToUser(token);
      if (config.loginPage && !user) {
        return from(router.navigate([config.loginPage]));
      }

      authService.setUser(user);
      return of({});
    })
  );

  return () => firstValueFrom(initializationFn)
}
