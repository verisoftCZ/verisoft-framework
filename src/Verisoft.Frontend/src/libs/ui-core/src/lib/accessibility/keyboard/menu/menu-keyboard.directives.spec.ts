import { Component, signal } from '@angular/core';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { MenuKeyboardDirective } from './menu-keyboard.directive';
import { MenuKeyboardItemDirective } from './menu-keyboard-item.directive';
import { MenuKeyboardListDirective } from './menu-keyboard-list.directive';
import { MenuKeyboardTriggerDirective } from './menu-keyboard-trigger.directive';
import { MenuKeyboardItem } from './menu-keyboard.model';
import { MenuKeyboardFacade } from './menu-keyboard.facade';
import { MENU_KEYBOARD } from './menu-keyboard.token';
import {
  MENU_KEYBOARD_PORT,
  MenuKeyboardPort,
} from './menu-keyboard-port.token';

interface TestItem extends MenuKeyboardItem {
  id: string;
}

@Component({
  standalone: true,
  imports: [
    MenuKeyboardDirective,
    MenuKeyboardTriggerDirective,
    MenuKeyboardListDirective,
    MenuKeyboardItemDirective,
  ],
  template: `
    <div
      vMenuKeyboard
      [menuKeyboardItems]="items()"
      [menuKeyboardNavigation]="true"
      (menuKeyboardSelect)="onSelect($event)"
    >
      <button vMenuKeyboardTrigger type="button" data-testid="trigger">
        Trigger
      </button>
      <ul vMenuKeyboardList data-testid="list">
        @for (item of items(); track item.id) {
          <li role="none">
            <button
              vMenuKeyboardItem
              [menuKeyboardItem]="item"
              type="button"
              [attr.data-item-id]="item.id"
            >
              {{ item.id }}
            </button>
          </li>
        }
      </ul>
    </div>
  `,
})
class SingleMenuHostComponent {
  readonly items = signal<TestItem[]>([
    { id: 'a', label: 'A' },
    { id: 'b', label: 'B' },
  ]);

  selected: TestItem | null = null;

  onSelect(item: TestItem): void {
    this.selected = item;
  }
}

@Component({
  standalone: true,
  imports: [
    MenuKeyboardDirective,
    MenuKeyboardTriggerDirective,
    MenuKeyboardListDirective,
    MenuKeyboardItemDirective,
  ],
  template: `
    <div
      vMenuKeyboard
      [menuKeyboardItems]="items"
      [menuKeyboardNavigation]="true"
    >
      <button vMenuKeyboardTrigger type="button">Trigger</button>
      <ul vMenuKeyboardList>
        <li role="none">
          <button
            vMenuKeyboardItem
            [menuKeyboardItem]="currentItem"
            type="button"
            data-testid="item-host"
          >
            Item
          </button>
        </li>
      </ul>
    </div>
  `,
})
class ItemChangeHostComponent {
  readonly items: TestItem[] = [
    { id: 'a', label: 'A' },
    { id: 'b', label: 'B' },
  ];

  currentItem = this.items[0];
}

@Component({
  standalone: true,
  imports: [
    MenuKeyboardDirective,
    MenuKeyboardTriggerDirective,
    MenuKeyboardListDirective,
    MenuKeyboardItemDirective,
  ],
  template: `
    <div
      vMenuKeyboard
      [menuKeyboardItems]="firstItems"
      [menuKeyboardNavigation]="true"
    >
      <button vMenuKeyboardTrigger type="button" data-testid="first-trigger">
        First
      </button>
      <ul vMenuKeyboardList data-testid="first-list">
        <li role="none">
          <button
            vMenuKeyboardItem
            [menuKeyboardItem]="firstItems[0]"
            type="button"
          >
            First item
          </button>
        </li>
      </ul>
    </div>

    <div
      vMenuKeyboard
      [menuKeyboardItems]="secondItems"
      [menuKeyboardNavigation]="true"
    >
      <button vMenuKeyboardTrigger type="button" data-testid="second-trigger">
        Second
      </button>
      <ul vMenuKeyboardList data-testid="second-list">
        <li role="none">
          <button
            vMenuKeyboardItem
            [menuKeyboardItem]="secondItems[0]"
            type="button"
          >
            Second item
          </button>
        </li>
      </ul>
    </div>
  `,
})
class DualMenuHostComponent {
  readonly firstItems: TestItem[] = [{ id: 'first', label: 'First' }];
  readonly secondItems: TestItem[] = [{ id: 'second', label: 'Second' }];
}

describe('Menu keyboard directives', () => {
  let fixture: ComponentFixture<SingleMenuHostComponent>;
  let menuDirective: MenuKeyboardDirective<TestItem>;
  let menuKeyboardPort: jest.Mocked<MenuKeyboardPort>;

  const getMenuDirective = (): MenuKeyboardDirective<TestItem> =>
    fixture.debugElement
      .query(By.directive(MenuKeyboardDirective))
      .injector.get(MenuKeyboardDirective);

  const dispatchKeydown = (
    target: EventTarget,
    init: KeyboardEventInit,
    capture = true,
  ): KeyboardEvent => {
    const event = new KeyboardEvent('keydown', {
      bubbles: true,
      cancelable: true,
      ...init,
    });
    target.dispatchEvent(event);
    return event;
  };

  beforeEach(async () => {
    menuKeyboardPort = {
      open: jest.fn(async () => true),
      close: jest.fn(async () => undefined),
    };

    await TestBed.configureTestingModule({
      imports: [SingleMenuHostComponent],
      providers: [{ provide: MENU_KEYBOARD_PORT, useValue: menuKeyboardPort }],
    }).compileComponents();

    fixture = TestBed.createComponent(SingleMenuHostComponent);
    fixture.detectChanges();
    menuDirective = getMenuDirective();
  });

  afterEach(() => {
    if (!fixture.componentRef.destroyed) {
      fixture.destroy();
    }
  });

  it('sets trigger ARIA attributes', () => {
    const trigger = fixture.nativeElement.querySelector(
      '[data-testid="trigger"]',
    ) as HTMLButtonElement;

    expect(trigger.getAttribute('aria-haspopup')).toBe('menu');
    expect(trigger.getAttribute('aria-controls')).toBeTruthy();
    expect(trigger.getAttribute('aria-expanded')).toBe('false');

    menuDirective.handleOpenChange(true);
    fixture.detectChanges();

    expect(trigger.getAttribute('aria-expanded')).toBe('true');
  });

  it('sets list role and id', () => {
    const list = fixture.nativeElement.querySelector(
      '[data-testid="list"]',
    ) as HTMLUListElement;
    const trigger = fixture.nativeElement.querySelector(
      '[data-testid="trigger"]',
    ) as HTMLButtonElement;

    expect(list.getAttribute('role')).toBe('menu');
    expect(list.getAttribute('tabindex')).toBe('-1');
    expect(list.id).toBe(trigger.getAttribute('aria-controls'));
  });

  it('sets menuitem role and tabindex on active item only', () => {
    menuDirective.handleOpenChange(true);
    fixture.detectChanges();

    const buttons = Array.from(
      fixture.nativeElement.querySelectorAll('[data-item-id]'),
    ) as HTMLButtonElement[];

    expect(buttons[0].getAttribute('role')).toBe('menuitem');
    expect(buttons[0].tabIndex).toBe(0);
    expect(buttons[1].getAttribute('role')).toBe('menuitem');
    expect(buttons[1].tabIndex).toBe(-1);
  });

  it('focuses active menu item when focus request version changes', () => {
    const focusSpy = jest.spyOn(HTMLButtonElement.prototype, 'focus');
    const list = fixture.nativeElement.querySelector(
      '[data-testid="list"]',
    ) as HTMLUListElement;

    menuDirective.handleOpenChange(true);
    menuDirective.focusActiveItem();
    fixture.detectChanges();

    expect(focusSpy).toHaveBeenCalled();

    focusSpy.mockClear();
    dispatchKeydown(list, { key: 'ArrowDown', code: 'ArrowDown' });
    fixture.detectChanges();

    const secondButton = fixture.nativeElement.querySelector(
      '[data-item-id="b"]',
    ) as HTMLButtonElement;

    expect(secondButton.tabIndex).toBe(0);
    expect(focusSpy).toHaveBeenCalled();
    expect(focusSpy.mock.instances.at(-1)).toBe(secondButton);

    focusSpy.mockRestore();
  });

  it('handles trigger keydown locally without a global closed-menu listener', async () => {
    const addSpy = jest.spyOn(document, 'addEventListener');
    const menuDe = fixture.debugElement.query(
      By.directive(MenuKeyboardDirective),
    );
    const facade = menuDe.injector.get(
      MENU_KEYBOARD,
    ) as MenuKeyboardFacade<TestItem>;
    const triggerDe = fixture.debugElement.query(
      By.css('[data-testid="trigger"]'),
    );
    const event = {
      key: 'ArrowDown',
      code: 'ArrowDown',
      preventDefault: jest.fn(),
      stopPropagation: jest.fn(),
    } as unknown as KeyboardEvent;

    triggerDe.triggerEventHandler('keydown', event);
    await fixture.whenStable();
    fixture.detectChanges();

    expect(menuKeyboardPort.open).toHaveBeenCalled();
    expect(facade.focusedIndex()).toBe(0);
    expect(menuDirective.focusedIndex()).toBe(0);
    expect(
      addSpy.mock.calls.some(
        ([eventName, listener]) =>
          eventName === 'keydown' && listener !== undefined,
      ),
    ).toBe(false);

    addSpy.mockRestore();
  });

  it('binds document keydown listener only while menu is open', () => {
    const addSpy = jest.spyOn(document, 'addEventListener');
    const removeSpy = jest.spyOn(document, 'removeEventListener');

    menuDirective.handleOpenChange(true);
    fixture.detectChanges();

    expect(
      addSpy.mock.calls.some(([eventName]) => eventName === 'keydown'),
    ).toBe(true);

    menuDirective.handleOpenChange(false);
    fixture.detectChanges();

    expect(
      removeSpy.mock.calls.some(([eventName]) => eventName === 'keydown'),
    ).toBe(true);

    addSpy.mockRestore();
    removeSpy.mockRestore();
  });

  it('cleans up document listener when host is destroyed', () => {
    const removeSpy = jest.spyOn(document, 'removeEventListener');

    menuDirective.handleOpenChange(true);
    fixture.detectChanges();
    fixture.destroy();

    expect(
      removeSpy.mock.calls.some(([eventName]) => eventName === 'keydown'),
    ).toBe(true);

    removeSpy.mockRestore();
  });

  it('updates item registration when menuKeyboardItem input changes', async () => {
    await TestBed.resetTestingModule();
    await TestBed.configureTestingModule({
      imports: [ItemChangeHostComponent],
    }).compileComponents();

    const hostFixture = TestBed.createComponent(ItemChangeHostComponent);
    hostFixture.detectChanges();

    const facade = hostFixture.debugElement
      .query(By.directive(MenuKeyboardDirective))
      .injector.get(MENU_KEYBOARD) as MenuKeyboardFacade<TestItem>;
    const host = hostFixture.nativeElement.querySelector(
      '[data-testid="item-host"]',
    ) as HTMLElement;

    expect(facade.resolveItemFromTarget(host)).toEqual({ id: 'a', label: 'A' });

    hostFixture.componentInstance.currentItem = hostFixture.componentInstance.items[1];
    hostFixture.detectChanges();

    expect(facade.resolveItemFromTarget(host)).toEqual({ id: 'b', label: 'B' });
    expect(facade.resolveItemFromTarget(host)).not.toEqual({ id: 'a', label: 'A' });
  });

  it('applies role and tabindex to directive host element', () => {
    menuDirective.handleOpenChange(true);
    fixture.detectChanges();

    const activeItem = fixture.nativeElement.querySelector(
      '[data-item-id="a"]',
    ) as HTMLButtonElement;

    expect(activeItem.getAttribute('role')).toBe('menuitem');
    expect(activeItem.tabIndex).toBe(0);
  });

  it('keeps multiple menu instances isolated', async () => {
    await TestBed.resetTestingModule();
    await TestBed.configureTestingModule({
      imports: [DualMenuHostComponent],
    }).compileComponents();

    const dualFixture = TestBed.createComponent(DualMenuHostComponent);
    dualFixture.detectChanges();

    const menus = dualFixture.debugElement
      .queryAll(By.directive(MenuKeyboardDirective))
      .map((debugEl) => debugEl.injector.get(MenuKeyboardDirective<TestItem>));

    expect(menus).toHaveLength(2);

    const firstTrigger = dualFixture.nativeElement.querySelector(
      '[data-testid="first-trigger"]',
    ) as HTMLButtonElement;
    const secondList = dualFixture.nativeElement.querySelector(
      '[data-testid="second-list"]',
    ) as HTMLUListElement;

    menus[0].handleOpenChange(true);
    dualFixture.detectChanges();

    expect(menus[0].isOpen()).toBe(true);
    expect(menus[1].isOpen()).toBe(false);
    expect(firstTrigger.getAttribute('aria-expanded')).toBe('true');
    expect(secondList.id).not.toBe(
      firstTrigger.getAttribute('aria-controls'),
    );
  });
});
