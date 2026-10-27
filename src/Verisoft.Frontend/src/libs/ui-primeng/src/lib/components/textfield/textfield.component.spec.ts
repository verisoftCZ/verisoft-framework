import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ERROR_PROVIDER_TOKEN } from '@verisoft/core';
import { of } from 'rxjs';
import { TextfieldComponent } from './textfield.component';

describe('TextfieldComponent', () => {
  let component: TextfieldComponent;
  let fixture: ComponentFixture<TextfieldComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TextfieldComponent],
      providers: [
        { provide: ERROR_PROVIDER_TOKEN, useValue: { mapError: () => of(null) } }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(TextfieldComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
