import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RadioButtonComponent } from './radiobutton.component';

describe('RadioButtonComponent', () => {
  let component: RadioButtonComponent<any>;
  let fixture: ComponentFixture<RadioButtonComponent<any>>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RadioButtonComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(RadioButtonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
