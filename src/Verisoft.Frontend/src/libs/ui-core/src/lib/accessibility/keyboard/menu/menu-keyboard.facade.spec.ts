import { MenuKeyboardController } from './menu-keyboard.controller';
import { MenuKeyboardFacade } from './menu-keyboard.facade';

interface TestItem {
  id: string;
  disabled?: boolean;
}

describe('MenuKeyboardFacade', () => {
  const items: TestItem[] = [
    { id: 'a' },
    { id: 'b', disabled: true },
    { id: 'c' },
  ];

  let facade: MenuKeyboardFacade<TestItem>;
  let controller: MenuKeyboardController<TestItem>;
  let elementA: HTMLElement;
  let elementB: HTMLElement;

  beforeEach(() => {
    facade = new MenuKeyboardFacade<TestItem>();
    controller = new MenuKeyboardController({
      getItems: () => items,
      getItemId: (item) => item.id,
      onSelect: jest.fn(),
      onOpen: jest.fn(),
      onClose: jest.fn(),
      keyboardNavigation: true,
    });
    elementA = document.createElement('button');
    elementB = document.createElement('button');
  });

  it('registers and unregisters items by item reference', () => {
    facade.registerItem(items[0], elementA);
    expect(facade.resolveItemFromTarget(elementA)).toBe(items[0]);

    facade.unregisterItem(items[0], elementA);
    expect(facade.resolveItemFromTarget(elementA)).toBeNull();
  });

  it('unregisterItemElement removes all entries for the same element', () => {
    facade.registerItem(items[0], elementA);
    facade.registerItem(items[2], elementA);

    facade.unregisterItemElement(elementA);

    expect(facade.resolveItemFromTarget(elementA)).toBeNull();
  });

  it('unregisterItem ignores stale item reference for the same element', () => {
    facade.registerItem(items[0], elementA);
    facade.registerItem(items[1], elementA);

    facade.unregisterItem(items[0], elementA);

    expect(facade.resolveItemFromTarget(elementA)).toBe(items[1]);
  });

  it('reflects active item tabindex from focused index', () => {
    controller.handleOpenChange(true);
    controller.focusedIndex = 1;
    facade.publish(controller, items);

    expect(facade.getItemTabIndex(items[2])).toBe(0);
    expect(facade.getItemTabIndex(items[0])).toBe(-1);
    expect(facade.isItemActive(items[2])).toBe(true);
  });

  it('increments focusRequestVersion when open focus changes', () => {
    controller.handleOpenChange(true);
    controller.focusedIndex = 0;
    facade.publish(controller, items);
    const initialVersion = facade.focusRequestVersion();

    controller.focusedIndex = 2;
    facade.publish(controller, items);

    expect(facade.focusRequestVersion()).toBe(initialVersion + 1);
  });

  it('does not increment focusRequestVersion when menu is closed', () => {
    controller.focusedIndex = 0;
    facade.publish(controller, items);
    const initialVersion = facade.focusRequestVersion();

    controller.focusedIndex = 1;
    facade.publish(controller, items);

    expect(facade.focusRequestVersion()).toBe(initialVersion);
  });

  it('forwards trigger keydown requests to registered handler', () => {
    const handler = jest.fn();
    const event = new KeyboardEvent('keydown', { key: 'ArrowDown' });

    facade.setTriggerKeydownHandler(handler);
    facade.requestTriggerKeydown(event);

    expect(handler).toHaveBeenCalledWith(event);
  });

  it('registers trigger and list elements', () => {
    const trigger = document.createElement('button');
    const list = document.createElement('ul');

    facade.registerTrigger(trigger);
    facade.registerList(list);

    expect(facade.getTriggerElement()).toBe(trigger);
    expect(facade.getListElement()).toBe(list);

    facade.unregisterTrigger(trigger);
    facade.unregisterList(list);

    expect(facade.getTriggerElement()).toBeNull();
    expect(facade.getListElement()).toBeNull();
  });
});
