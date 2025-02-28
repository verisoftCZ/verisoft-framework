import { ComponentFixture, TestBed } from '@angular/core/testing';
import { TristatecheckboxComponent } from './tristatecheckbox.component';

describe('TristatecheckboxComponent', () => {
  let component: TristatecheckboxComponent;
  let fixture: ComponentFixture<TristatecheckboxComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TristatecheckboxComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(TristatecheckboxComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
