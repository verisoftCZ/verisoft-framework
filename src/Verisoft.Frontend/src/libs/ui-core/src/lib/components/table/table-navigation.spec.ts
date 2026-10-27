import { ActivatedRoute, Router } from '@angular/router';
import {
  createRowDetailUrl,
  hasRowDetailRoute,
  navigateToRowDetail,
  openRowDetailInNewWindow,
} from './table-navigation';
import { RowModel } from './table.models';

describe('table-navigation', () => {
  const row = new RowModel({ id: 5, code: 'abc' }, false, false);
  const router = {
    navigate: jest.fn(),
    createUrlTree: jest.fn().mockReturnValue('tree'),
    serializeUrl: jest.fn().mockReturnValue('/list/5'),
  } as unknown as Router;
  const route = {} as ActivatedRoute;

  beforeEach(() => {
    jest.clearAllMocks();
    (router.createUrlTree as jest.Mock).mockReturnValue('tree');
    (router.serializeUrl as jest.Mock).mockReturnValue('/list/5');
    jest.spyOn(window, 'open').mockImplementation(() => null);
  });

  describe('createRowDetailUrl', () => {
    it('uses id by default', () => {
      expect(createRowDetailUrl(row)).toBe('5');
    });

    it('uses entityKey', () => {
      expect(createRowDetailUrl(row, 'code')).toBe('abc');
    });

    it('prefers customRoute', () => {
      const custom = new RowModel({ id: 5, code: 'abc' }, false, false, undefined, undefined, undefined, 'x', 'code');
      expect(createRowDetailUrl(custom, 'code')).toBe('x/abc');
    });
  });

  describe('navigateToRowDetail', () => {
    it('navigates relatively on plain event', () => {
      navigateToRowDetail(router, route, row, undefined, new MouseEvent('dblclick'));
      expect(router.navigate).toHaveBeenCalledWith(['5'], { relativeTo: route });
      expect(window.open).not.toHaveBeenCalled();
    });

    it('opens new window on ctrl', () => {
      navigateToRowDetail(router, route, row, undefined, new MouseEvent('click', { ctrlKey: true }));
      expect(window.open).toHaveBeenCalledWith('/list/5', '_blank');
      expect(router.navigate).not.toHaveBeenCalled();
    });

    it('opens new window on meta', () => {
      navigateToRowDetail(router, route, row, undefined, new MouseEvent('click', { metaKey: true }));
      expect(window.open).toHaveBeenCalledWith('/list/5', '_blank');
    });
  });

  it('openRowDetailInNewWindow builds url tree relative to route', () => {
    openRowDetailInNewWindow(router, route, row, 'code');
    expect(router.createUrlTree).toHaveBeenCalledWith(['abc'], { relativeTo: route });
    expect(window.open).toHaveBeenCalledWith('/list/5', '_blank');
  });

  describe('hasRowDetailRoute', () => {
    const make = (children: any[]) => ({ routeConfig: { path: 'list', children } } as unknown as ActivatedRoute);

    it('matches param route', () => {
      expect(hasRowDetailRoute(make([{ path: ':id' }]), '5')).toBe(true);
    });

    it('does not match without detail route', () => {
      expect(hasRowDetailRoute(make([{ path: 'other' }]), '5')).toBe(false);
    });
  });
});
