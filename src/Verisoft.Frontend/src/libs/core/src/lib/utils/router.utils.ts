import { assertInInjectionContext, inject, Signal } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { ActivatedRoute, Params } from '@angular/router';
import { map } from 'rxjs';

export function injectParams(): Signal<Params>;
export function injectParams(key: string): Signal<string | null>;
export function injectParams<T>(transform: (params: Params) => T): Signal<T>;
export function injectParams<T>(
  keyOrTransform?: string | ((params: Params) => T),
): Signal<T | Params | string | null> {
  assertInInjectionContext(injectParams);

  const route = inject(ActivatedRoute);

  if (typeof keyOrTransform === 'function') {
    return toSignal(route.params.pipe(map(keyOrTransform)), { requireSync: true });
  }

  const getParam = (params: Params) =>
    keyOrTransform ? params?.[keyOrTransform] ?? null : params;

  return toSignal(route.params.pipe(map(getParam)), { requireSync: true });
}

export function injectParam(key: string): Signal<string | null> {
  assertInInjectionContext(injectParam);
  return injectParams(key);
}
