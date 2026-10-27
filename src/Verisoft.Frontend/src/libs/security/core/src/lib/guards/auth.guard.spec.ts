import { TestBed } from '@angular/core/testing';
import { ActivatedRouteSnapshot, Router, UrlTree } from '@angular/router';
import { of } from 'rxjs';
import { AuthenticatedUser, SecurityConfig } from '../models';
import { SECURITY_CONFIG } from '../provider';
import { AuthContextService } from '../services';
import { AuthGuard } from './auth.guard';

describe('AuthGuard', () => {
  let guard: AuthGuard;
  let routerMock: { parseUrl: jest.Mock };
  let authContextMock: { user$: ReturnType<typeof of<AuthenticatedUser | undefined>> };
  let notAuthorizedTree: UrlTree;

  const securityConfig: SecurityConfig = {
    tokenStorageKey: 'APP_TOKEN',
    notAuthorizedPage: '/not-authorized',
  };

  const createUser = (roles: string[], permissions: string[]): AuthenticatedUser => ({
    userName: 'test-user',
    userId: '1',
    displayName: 'Test User',
    email: 'test@example.com',
    roles,
    permissions,
  });

  beforeEach(() => {
    notAuthorizedTree = {} as UrlTree;
    routerMock = {
      parseUrl: jest.fn().mockReturnValue(notAuthorizedTree),
    };
    authContextMock = {
      user$: of(undefined),
    };

    TestBed.configureTestingModule({
      providers: [
        AuthGuard,
        {
          provide: Router,
          useValue: routerMock,
        },
        {
          provide: AuthContextService,
          useValue: authContextMock,
        },
        {
          provide: SECURITY_CONFIG,
          useValue: securityConfig,
        },
      ],
    });

    guard = TestBed.inject(AuthGuard);
  });

  it('should deny access when required role is missing even if permission has the same name', (done) => {
    authContextMock.user$ = of(createUser(['USER'], ['ADMIN']));
    const route = { data: { roles: 'ADMIN' } } as ActivatedRouteSnapshot;

    guard.canActivate(route).subscribe((result) => {
      expect(result).toBe(notAuthorizedTree);
      expect(routerMock.parseUrl).toHaveBeenCalledWith('/not-authorized');
      done();
    });
  });

  it('should allow access when user has required role even if permission with the same name is missing', (done) => {
    authContextMock.user$ = of(createUser(['ADMIN'], ['READ']));
    const route = { data: { roles: 'ADMIN' } } as ActivatedRouteSnapshot;

    guard.canActivate(route).subscribe((result) => {
      expect(result).toBe(true);
      expect(routerMock.parseUrl).not.toHaveBeenCalled();
      done();
    });
  });

  it('should allow access when roles are provided as an array and user has any required role', (done) => {
    authContextMock.user$ = of(createUser(['USER'], ['READ']));
    const route = {
      data: { roles: ['MANAGER', 'USER'] },
    } as ActivatedRouteSnapshot;

    guard.canActivate(route).subscribe((result) => {
      expect(result).toBe(true);
      expect(routerMock.parseUrl).not.toHaveBeenCalled();
      done();
    });
  });

  it('should deny access when roles are provided as an array and user has none of them', (done) => {
    authContextMock.user$ = of(createUser(['GUEST'], ['READ']));
    const route = {
      data: { roles: ['MANAGER', 'USER'] },
    } as ActivatedRouteSnapshot;

    guard.canActivate(route).subscribe((result) => {
      expect(result).toBe(notAuthorizedTree);
      expect(routerMock.parseUrl).toHaveBeenCalledWith('/not-authorized');
      done();
    });
  });
});
