import {
  APP_INITIALIZER,
  InjectionToken,
  ModuleWithProviders,
  NgModule,
  Provider,
} from '@angular/core';
import { Router } from '@angular/router';
import { StoreModule } from '@ngrx/store';
import { HasPermissionDirective, HasRoleDirective } from './directives';
import { AuthGuard } from './guards';
import { SecurityConfig } from './models';
import {
  AuthContextService,
  LocalStorageTokenProvider,
  securityInitializerFactory,
} from './services';
import { SecurityFeature } from './state/feature';

export function provideSecurity(
  config: Partial<SecurityConfig> | undefined = undefined
): Provider[] {
  const securityConfig: SecurityConfig = {
    tokenStorageKey: 'APP_TOKEN',
    notAuthorizedPage: '/not-authorized',
    ...(config ?? {}),
  };
  return [
    AuthGuard,
    SECURITY_INITIALIZER_PROVIDER,
    { provide: TOKEN_PROVIDER, useClass: LocalStorageTokenProvider },
    { provide: SECURITY_CONFIG, useValue: securityConfig },
  ];
}

@NgModule({
  declarations: [HasPermissionDirective, HasRoleDirective],
  imports: [StoreModule.forFeature(SecurityFeature)],
  exports: [HasPermissionDirective, HasRoleDirective],
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

export const TOKEN_PROVIDER = new InjectionToken('SECURITY_TOKEN_PROVIDER');

export const SECURITY_CONFIG = new InjectionToken('SECURITY_CONFIG');

export const SECURITY_INITIALIZER_PROVIDER: Provider = {
  provide: APP_INITIALIZER,
  useFactory: securityInitializerFactory,
  deps: [TOKEN_PROVIDER, AuthContextService, SECURITY_CONFIG, Router],
  multi: true,
};
