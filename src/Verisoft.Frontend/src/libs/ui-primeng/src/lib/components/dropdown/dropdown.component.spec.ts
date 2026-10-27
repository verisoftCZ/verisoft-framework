import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ERROR_PROVIDER_TOKEN } from '@verisoft/core';
import { DropdownComponent } from './dropdown.component';

describe('DropdownComponent', () => {
  let component: DropdownComponent<any>;
  let fixture: ComponentFixture<DropdownComponent<any>>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DropdownComponent],
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

    fixture = TestBed.createComponent(DropdownComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

