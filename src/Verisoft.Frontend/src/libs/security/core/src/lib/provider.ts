import { EnvironmentProviders, InjectionToken, ModuleWithProviders, NgModule, Provider, inject, provideAppInitializer } from '@angular/core';
import { Router } from '@angular/router';
import { StoreModule } from '@ngrx/store';
import { AuthGuard } from './guards';
import { SecurityConfig } from './models';
import {
  AuthContextService,
  LocalStorageTokenProvider,
  securityInitializerFactory,
  TokenProvider,
} from './services';
import { SecurityFeature } from './state/feature';

export function provideSecurity(
  config: Partial<SecurityConfig> | undefined = undefined
): (Provider | EnvironmentProviders)[] {
  const securityConfig: SecurityConfig = {
    tokenStorageKey: 'APP_TOKEN',
    notAuthorizedPage: '/not-authorized',
    ...(config ?? {}),
  };
  return [
    AuthGuard,
    SECURITY_INITIALIZER_PROVIDER,
    { provide: SECURITY_CONTEXT_TOKEN_PROVIDER, useClass: LocalStorageTokenProvider },
    { provide: SECURITY_CONFIG, useValue: securityConfig },
  ];
}

@NgModule({
  imports: [StoreModule.forFeature(SecurityFeature)],
})
export class SecurityModule {
  static forRoot(
    config?: Partial<SecurityConfig>
  ): ModuleWithProviders<SecurityModule> {
    return {
      ngModule: SecurityModule,
      providers: [...provideSecurity(config)],
    };
  }
}

export const SECURITY_CONTEXT_TOKEN_PROVIDER = new InjectionToken('SECURITY_CONTEXT_TOKEN_PROVIDER');

export const SECURITY_CONFIG = new InjectionToken('SECURITY_CONFIG');


export const SECURITY_INITIALIZER_PROVIDER: EnvironmentProviders = provideAppInitializer(() => {
  const initializerFn = (securityInitializerFactory)(inject<TokenProvider>(SECURITY_CONTEXT_TOKEN_PROVIDER), inject(AuthContextService), inject<SecurityConfig>(SECURITY_CONFIG), inject(Router));
  return initializerFn();
});