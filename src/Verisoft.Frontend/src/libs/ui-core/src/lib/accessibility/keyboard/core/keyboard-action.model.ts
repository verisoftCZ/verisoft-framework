export type LinearNavigationAction = 'next' | 'previous' | 'first' | 'last';

export type InteractionAction = 'activate' | 'close' | 'dismiss';

export function assertNever(value: never): never {
  throw new Error(`Unexpected value: ${String(value)}`);
}
