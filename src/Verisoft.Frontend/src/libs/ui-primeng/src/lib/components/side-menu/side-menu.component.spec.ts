import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { SIDE_MENU_STATE_TOKEN } from '@verisoft/ui-core';
import { BehaviorSubject } from 'rxjs';
import { SideMenuComponent } from './side-menu.component';

describe('SideMenuComponent', () => {
  let component: SideMenuComponent;
  let fixture: ComponentFixture<SideMenuComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SideMenuComponent],
      providers: [
        provideRouter([]),
        { 
            provide: SIDE_MENU_STATE_TOKEN, 
            useValue: {
                compact: new BehaviorSubject(false),
                open: new BehaviorSubject(true),
                mode: new BehaviorSubject('static'),
                mobileOpen: new BehaviorSubject(false)
            } 
        }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(SideMenuComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
