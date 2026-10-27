import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ERROR_PROVIDER_TOKEN } from '@verisoft/core';
import { of } from 'rxjs';
import { CalendarComponent } from './calendar.component';

describe('CalendarComponent', () => {
  let component: CalendarComponent;
  let fixture: ComponentFixture<CalendarComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CalendarComponent],
      providers: [
        { provide: ERROR_PROVIDER_TOKEN, useValue: { mapError: () => of(null) } }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(CalendarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
