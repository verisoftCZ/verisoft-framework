import { AuthenticatedUser } from './authenticated-user.model';

export function hasRequiredPermission(
  user: AuthenticatedUser | undefined,
  permission: string | string[]
): boolean {
  return hasItems(user?.permissions, permission);
}

export function hasRequiredRole(
  user: AuthenticatedUser | undefined,
  role: string | string[]
): boolean {
  return hasItems(user?.roles, role);
}

function hasItems(
  userItems: string[] | undefined,
  neededItems: string | string[]
): boolean {
  if (!neededItems) {
    return true;
  }

  if (!userItems) {
    return false;
  }

  const userItemsSet = new Set(userItems);
  if (Array.isArray(neededItems)) {
    return neededItems.some((item) => hasItems(userItems, item));
  }

  if (neededItems.includes(',')) {
    const splitItems = neededItems.split(',').map((i) => i.trim());
    return splitItems.every((i) => userItemsSet.has(i));
  }

  return userItemsSet.has(neededItems);
}

export function convertJWTToUser(
  base64Token?: string
): AuthenticatedUser | undefined {
  if (!base64Token) {
    return undefined;
  }

  try {
    const parts = base64Token.split('.');
    if (parts.length < 2) {
      return undefined;
    }

    const payloadBase64 = parts[1];
    const payloadJson = atob(payloadBase64);
    const payload = JSON.parse(payloadJson);

    const userName = payload['sub'] ?? payload['nameid'];
    if (!userName) {
      return undefined;
    }

    const user: AuthenticatedUser = {
      userName,
      email: payload['email'],
      displayName: payload['name'],
      roles: convertToArray(payload, 'role'),
      permissions: convertToArray(payload, 'permission'),
    };

    return user;
  } catch (error) {
    return undefined;
  }
}

function convertToArray(
  item: { [key: string]: string | string[] },
  propertyName: string
): string[] | undefined {
  const rawValue = item[propertyName];
  if (!rawValue) {
    return undefined;
  }

  if (Array.isArray(rawValue)) {
    return rawValue;
  }

  return [rawValue];
}
