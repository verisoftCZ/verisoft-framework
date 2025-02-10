import { ComponentFixture, TestBed } from '@angular/core/testing';
import { GenericFieldComponent } from './generic-field.component';

describe('GenericFieldComponent', () => {
  let component: GenericFieldComponent<string>;
  let fixture: ComponentFixture<GenericFieldComponent<string>>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GenericFieldComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(GenericFieldComponent<string>);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
