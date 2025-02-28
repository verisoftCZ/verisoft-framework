import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ActionButtonGroupComponent } from './action-button-group.component';

describe('ActionButtonGroupComponent', () => {
  let component: ActionButtonGroupComponent;
  let fixture: ComponentFixture<ActionButtonGroupComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ActionButtonGroupComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(ActionButtonGroupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
