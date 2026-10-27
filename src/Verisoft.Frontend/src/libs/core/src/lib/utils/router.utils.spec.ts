import { TestBed } from '@angular/core/testing';
import { ActivatedRoute, Params } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { injectParam, injectParams } from './router.utils';

describe('router.utils', () => {
  let params$: BehaviorSubject<Params>;

  beforeEach(() => {
    params$ = new BehaviorSubject<Params>({ id: '1', slug: 'first' });

    TestBed.configureTestingModule({
      providers: [
        {
          provide: ActivatedRoute,
          useValue: {
            params: params$.asObservable(),
          },
        },
      ],
    });
  });

  test('Should return all route params when no key or transform is passed', () => {
    const params = TestBed.runInInjectionContext(() => injectParams());

    expect(params()).toEqual({ id: '1', slug: 'first' });

    params$.next({ id: '2', slug: 'second' });
    expect(params()).toEqual({ id: '2', slug: 'second' });
  });

  test('Should return selected route param value when key is passed', () => {
    const id = TestBed.runInInjectionContext(() => injectParams('id'));

    expect(id()).toBe('1');

    params$.next({ id: '2', slug: 'second' });
    expect(id()).toBe('2');
  });

  test('Should return null when selected route param key is missing', () => {
    const missing = TestBed.runInInjectionContext(() => injectParams('missing'));

    expect(missing()).toBeNull();
  });

  test('Should return transformed value when transform function is passed', () => {
    const keys = TestBed.runInInjectionContext(() => injectParams((params) => Object.keys(params)));

    expect(keys()).toEqual(['id', 'slug']);
  });

  test('Should return selected route param value with injectParam helper', () => {
    const id = TestBed.runInInjectionContext(() => injectParam('id'));

    expect(id()).toBe('1');

    params$.next({ id: '42' });
    expect(id()).toBe('42');
  });
});
