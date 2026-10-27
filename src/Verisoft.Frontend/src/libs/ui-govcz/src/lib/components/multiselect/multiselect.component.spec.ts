import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ERROR_PROVIDER_TOKEN } from '@verisoft/core';
import { MultiselectComponent } from './multiselect.component';

describe('MultiselectComponent', () => {
  let component: MultiselectComponent<any>;
  let fixture: ComponentFixture<MultiselectComponent<any>>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MultiselectComponent],
      providers: [
        {
          provide: ERROR_PROVIDER_TOKEN,
          useValue: {
            mapError: jest.fn(),
            errors: {},
          },
        },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(MultiselectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

