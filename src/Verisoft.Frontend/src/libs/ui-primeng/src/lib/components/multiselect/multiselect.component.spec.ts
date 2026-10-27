import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ERROR_PROVIDER_TOKEN } from '@verisoft/core';
import { of } from 'rxjs';
import { MultiselectComponent } from './multiselect.component';

describe('MultiselectComponent', () => {
  let component: MultiselectComponent<any>;
  let fixture: ComponentFixture<MultiselectComponent<any>>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MultiselectComponent],
      providers: [
        { provide: ERROR_PROVIDER_TOKEN, useValue: { mapError: () => of(null) } }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(MultiselectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
