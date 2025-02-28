import { AuthenticatedUser } from './authenticated-user.model';
import {
  convertJWTToUser,
  hasRequiredPermission,
  hasRequiredRole,
} from './functions';

describe('Permissions Utils', () => {
  let user: AuthenticatedUser;

  beforeEach(() => {
    user = {
      userName: 'testUser',
      email: 'test@email.cz',
      displayName: 'Test User',
      permissions: ['READ', 'WRITE', 'DELETE_USER'],
      roles: ['ADMIN', 'USER'],
    };
  });

  describe('hasRequiredPermission', () => {
    it('should return false if user is undefined', () => {
      expect(hasRequiredPermission(undefined, 'READ')).toBe(false);
    });

    it('should return false if user.permissions is undefined', () => {
      const noPermissionsUser: AuthenticatedUser = {
        ...user,
        permissions: undefined,
      };
      expect(hasRequiredPermission(noPermissionsUser, 'READ')).toBe(false);
    });

    it('should return true for a single permission the user has', () => {
      expect(hasRequiredPermission(user, 'READ')).toBe(true);
    });

    it('should return false for a single permission the user does not have', () => {
      expect(hasRequiredPermission(user, 'UNKNOWN')).toBe(false);
    });

    it('should return true for comma-delimited permissions if user has ALL', () => {
      expect(hasRequiredPermission(user, 'READ,WRITE')).toBe(true);
    });

    it('should return false for comma-delimited permissions if missing one', () => {
      expect(hasRequiredPermission(user, 'READ,UPDATE_USER')).toBe(false);
    });

    it('should return true if user has ANY of the permissions in the array', () => {
      expect(hasRequiredPermission(user, ['READ', 'UPDATE_USER'])).toBe(true);
    });

    it('should return false if user does not have ANY of the permissions in the array', () => {
      expect(hasRequiredPermission(user, ['UPDATE_USER', 'CREATE_POST'])).toBe(
        false
      );
    });
  });

  describe('hasRequiredRole', () => {
    it('should return false if user is undefined', () => {
      expect(hasRequiredRole(undefined, 'ADMIN')).toBe(false);
    });

    it('should return false if user.roles is undefined', () => {
      const noRolesUser: AuthenticatedUser = { ...user, roles: undefined };
      expect(hasRequiredRole(noRolesUser, 'ADMIN')).toBe(false);
    });

    it('should return true for a single role the user has', () => {
      expect(hasRequiredRole(user, 'ADMIN')).toBe(true);
    });

    it('should return false for a single role the user does not have', () => {
      expect(hasRequiredRole(user, 'MANAGER')).toBe(false);
    });

    it('should return true for comma-delimited roles if user has ALL', () => {
      // user has 'ADMIN' and 'USER'
      expect(hasRequiredRole(user, 'ADMIN,USER')).toBe(true);
    });

    it('should return false for comma-delimited roles if missing one', () => {
      // user has 'ADMIN' and 'USER' but not 'MANAGER'
      expect(hasRequiredRole(user, 'ADMIN,MANAGER')).toBe(false);
    });

    it('should return true if user has ANY of the roles in the array', () => {
      expect(hasRequiredRole(user, ['USER', 'MANAGER'])).toBe(true);
    });

    it('should return false if user does not have ANY of the roles in the array', () => {
      expect(hasRequiredRole(user, ['GUEST', 'MANAGER'])).toBe(false);
    });
  });

  describe('convertJWTToUser', () => {
    it('should return undefined if base64Token is undefined', () => {
      expect(convertJWTToUser(undefined)).toBeUndefined();
    });

    it('should return undefined if base64Token is invalid', () => {
      expect(convertJWTToUser('invalid.token')).toBeUndefined();
    });

    it('should return undefined if payload does not contain userName', () => {
      const token =
        btoa('header') + '.' + btoa(JSON.stringify({})) + '.signature';
      expect(convertJWTToUser(token)).toBeUndefined();
    });

    it('should return a user object if payload contains userName', () => {
      const payload = {
        sub: 'testUser',
        email: 'test@example.com',
        name: 'Test User',
        role: 'ADMIN',
        permission: 'READ',
      };
      const token =
        btoa('header') + '.' + btoa(JSON.stringify(payload)) + '.signature';
      const user = convertJWTToUser(token);
      expect(user).toEqual({
        userName: 'testUser',
        email: 'test@example.com',
        displayName: 'Test User',
        roles: ['ADMIN'],
        permissions: ['READ'],
      });
    });

    it('should return a user object if payload contains nameid', () => {
      const payload = {
        nameid: 'testUser',
        email: 'test@example.com',
        name: 'Test User',
        role: 'ADMIN',
        permission: 'READ',
      };
      const token =
        btoa('header') + '.' + btoa(JSON.stringify(payload)) + '.signature';
      const user = convertJWTToUser(token);
      expect(user).toEqual({
        userName: 'testUser',
        email: 'test@example.com',
        displayName: 'Test User',
        roles: ['ADMIN'],
        permissions: ['READ'],
      });
    });

    it('should return undefined if an error occurs during parsing', () => {
      const token =
        btoa('header') + '.' + btoa('invalidPayload') + '.signature';
      expect(convertJWTToUser(token)).toBeUndefined();
    });
  });
});
