import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { ERROR_PROVIDER_TOKEN } from '@verisoft/core';
import { FilterComponent } from './filter.component';

describe('VerisoftTableFilterComponent', () => {
  let component: FilterComponent<any>;
  let fixture: ComponentFixture<FilterComponent<any>>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FilterComponent],
      providers: [
        provideRouter([]),
        { provide: ERROR_PROVIDER_TOKEN, useValue: {} }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(FilterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
