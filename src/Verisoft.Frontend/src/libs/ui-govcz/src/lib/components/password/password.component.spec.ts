import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ERROR_PROVIDER_TOKEN } from '@verisoft/core';
import { PasswordComponent } from './password.component';

describe('PasswordComponent', () => {
  let component: PasswordComponent;
  let fixture: ComponentFixture< PasswordComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PasswordComponent],
      providers: [
        { provide: ERROR_PROVIDER_TOKEN, useValue: {} }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(PasswordComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
