import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ActivatedRoute, Router, provideRouter } from '@angular/router';
import { RowModel } from '@verisoft/ui-core';
import { TableComponent } from './table.component';

describe('TableComponent', () => {
  let component: TableComponent<any>;
  let fixture: ComponentFixture<TableComponent<any>>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TableComponent],
      providers: [provideRouter([])]
    }).compileComponents();

    fixture = TestBed.createComponent(TableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  describe('row navigation', () => {
    let router: Router;
    const row = new RowModel({ id: 7 }, false, false);

    afterEach(() => jest.restoreAllMocks());

    beforeEach(() => {
      router = TestBed.inject(Router);
      jest.spyOn(router, 'navigate').mockResolvedValue(true);
      jest.spyOn(window, 'open').mockImplementation(() => null);
      component.route = { routeConfig: { path: 'list', children: [{ path: ':id' }] } } as unknown as ActivatedRoute;
    });

    it('double click navigates to detail', () => {
      component.onRowDoubleClick(row);
      expect(router.navigate).toHaveBeenCalledWith(['7'], { relativeTo: component.route });
    });

    it('double click does nothing without detail route', () => {
      component.route = { routeConfig: { path: 'list', children: [] } } as unknown as ActivatedRoute;
      component.onRowDoubleClick(row);
      expect(router.navigate).not.toHaveBeenCalled();
    });

    it('double click emits rowDoubleClick instead of navigating', () => {
      const spy = jest.fn();
      component.rowDoubleClick.subscribe(spy);
      component.onRowDoubleClick(row);
      expect(spy).toHaveBeenCalledWith({ id: 7 });
      expect(router.navigate).not.toHaveBeenCalled();
    });

    it('middle mouse button opens new window', () => {
      const tree = jest.spyOn(router, 'createUrlTree');
      component.onRowMouseDown(row, new MouseEvent('mousedown', { button: 1 }));
      expect(tree).toHaveBeenCalledWith(['7'], { relativeTo: component.route });
      expect(window.open).toHaveBeenCalledWith(expect.any(String), '_blank');
    });

    it('left mouse button does not open new window', () => {
      component.onRowMouseDown(row, new MouseEvent('mousedown', { button: 0 }));
      expect(window.open).not.toHaveBeenCalled();
    });

    it('disableCustomClicks blocks everything', () => {
      component.disableCustomClicks = true;
      component.onRowDoubleClick(row);
      component.onRowMouseDown(row, new MouseEvent('mousedown', { button: 1 }));
      expect(router.navigate).not.toHaveBeenCalled();
      expect(window.open).not.toHaveBeenCalled();
    });
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
