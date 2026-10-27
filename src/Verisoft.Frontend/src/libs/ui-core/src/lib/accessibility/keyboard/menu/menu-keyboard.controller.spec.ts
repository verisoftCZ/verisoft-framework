import { MenuKeyboardController } from './menu-keyboard.controller';

interface TestItem {
  id: string;
  disabled?: boolean;
}

describe('MenuKeyboardController', () => {
  const items: TestItem[] = [{ id: 'a' }, { id: 'b' }, { id: 'c' }];

  let onSelect: jest.Mock;
  let onOpen: jest.Mock;
  let onClose: jest.Mock;
  let controller: MenuKeyboardController<TestItem>;

  beforeEach(() => {
    onSelect = jest.fn();
    onOpen = jest.fn();
    onClose = jest.fn();

    controller = new MenuKeyboardController({
      getItems: () => items,
      getItemId: (item) => item.id,
      onSelect,
      onOpen,
      onClose,
      keyboardNavigation: true,
    });
  });

  const keydown = (code: string, target: Partial<KeyboardEvent> = {}) =>
    ({
      code,
      key: code,
      preventDefault: jest.fn(),
      stopPropagation: jest.fn(),
      ...target,
    }) as unknown as KeyboardEvent;

  it('opens menu on ArrowDown from trigger', () => {
    const event = keydown('ArrowDown');
    controller.onTriggerKeydown(event);

    expect(event.preventDefault).toHaveBeenCalled();
    expect(onOpen).toHaveBeenCalled();
    expect(controller.isOpen).toBe(false);
    expect(controller.focusedIndex).toBe(0);

    controller.handleOpenChange(true);
    expect(controller.isOpen).toBe(true);
  });

  it('does not open menu when there are no actionable items', () => {
    const emptyController = new MenuKeyboardController({
      getItems: () => [],
      onSelect,
      onOpen,
      onClose,
      keyboardNavigation: true,
    });

    const event = keydown('ArrowDown');
    emptyController.onTriggerKeydown(event);

    expect(onOpen).not.toHaveBeenCalled();
    expect(event.preventDefault).not.toHaveBeenCalled();
  });

  it('opens menu on ArrowUp with focus on last item', () => {
    controller.onTriggerKeydown(keydown('ArrowUp'));
    expect(controller.focusedIndex).toBe(2);
  });

  it('navigates with arrow keys when open', () => {
    controller.handleOpenChange(true);
    controller.focusedIndex = 0;

    const down = keydown('ArrowDown');
    controller.onMenuKeydown(down);
    expect(controller.focusedIndex).toBe(1);

    const up = keydown('ArrowUp');
    controller.onMenuKeydown(up);
    expect(controller.focusedIndex).toBe(0);
  });

  it('wraps focus from last to first item', () => {
    controller.handleOpenChange(true);
    controller.focusedIndex = 2;

    controller.onMenuKeydown(keydown('ArrowDown'));
    expect(controller.focusedIndex).toBe(0);
  });

  it('selects focused item on Enter and closes menu', () => {
    controller.handleOpenChange(true);
    controller.focusedIndex = 1;

    controller.onMenuKeydown(keydown('Enter'));

    expect(onSelect).toHaveBeenCalledWith(items[1]);
    expect(onClose).toHaveBeenCalledWith(true);
    expect(controller.isOpen).toBe(true);

    controller.handleOpenChange(false);
    expect(controller.isOpen).toBe(false);
  });

  it('closes menu on Escape and restores focus', () => {
    controller.handleOpenChange(true);

    controller.onMenuKeydown(keydown('Escape'));

    expect(onClose).toHaveBeenCalledWith(true);
    expect(controller.isOpen).toBe(true);

    controller.handleOpenChange(false);
    expect(controller.isOpen).toBe(false);
  });

  it('closes menu on Tab without preventing default or restoring focus', () => {
    controller.handleOpenChange(true);

    const tab = keydown('Tab');
    controller.onMenuKeydown(tab);

    expect(onClose).toHaveBeenCalledWith(false);
    expect(tab.preventDefault).not.toHaveBeenCalled();
    expect(tab.stopPropagation).not.toHaveBeenCalled();
  });

  it('jumps to first and last item with Home and End', () => {
    controller.handleOpenChange(true);
    controller.focusedIndex = 1;

    controller.onMenuKeydown(keydown('Home'));
    expect(controller.focusedIndex).toBe(0);

    controller.onMenuKeydown(keydown('End'));
    expect(controller.focusedIndex).toBe(2);
  });

  it('skips disabled items in actionable list', () => {
    const disabledItems: TestItem[] = [
      { id: 'a' },
      { id: 'b', disabled: true },
      { id: 'c' },
    ];

    const disabledController = new MenuKeyboardController({
      getItems: () => disabledItems,
      getItemId: (item) => item.id,
      onSelect,
      onOpen,
      onClose,
      keyboardNavigation: true,
    });

    disabledController.handleOpenChange(true);
    disabledController.focusedIndex = 0;
    disabledController.onMenuKeydown(keydown('ArrowDown'));

    expect(disabledController.focusedIndex).toBe(1);
    disabledController.onMenuKeydown(keydown('Enter'));
    expect(onSelect).toHaveBeenCalledWith(disabledItems[2]);
  });

  it('uses initialActiveItemId when opening menu', () => {
    const customController = new MenuKeyboardController({
      getItems: () => items,
      getItemId: (item) => item.id,
      onSelect,
      onOpen,
      onClose,
      keyboardNavigation: {
        initialActiveItemId: 'c',
      },
    });

    customController.handleOpenChange(true);
    expect(customController.focusedIndex).toBe(2);
  });

  it('preserves focused index when handleOpenChange(true) is called twice', () => {
    controller.onTriggerKeydown(keydown('ArrowUp'));
    expect(controller.focusedIndex).toBe(2);

    controller.handleOpenChange(true);
    expect(controller.focusedIndex).toBe(2);

    controller.handleOpenChange(true);
    expect(controller.focusedIndex).toBe(2);
  });

  it('resets to first item when reopening without keyboard preset', () => {
    controller.handleOpenChange(true);
    controller.focusedIndex = 2;
    controller.handleOpenChange(false);

    controller.handleOpenChange(true);
    expect(controller.focusedIndex).toBe(0);
  });

  it('does not keep open state when menu has no actionable items', () => {
    const emptyController = new MenuKeyboardController({
      getItems: () => [{ id: 'a', disabled: true }],
      onSelect,
      onOpen,
      onClose,
      keyboardNavigation: true,
    });

    emptyController.handleOpenChange(true);
    expect(emptyController.isOpen).toBe(false);
  });

  it('sets active item by reference for hover sync', () => {
    const disabledItems: TestItem[] = [
      { id: 'a' },
      { id: 'b', disabled: true },
      { id: 'c' },
    ];

    const disabledController = new MenuKeyboardController({
      getItems: () => disabledItems,
      getItemId: (item) => item.id,
      onSelect,
      onOpen,
      onClose,
      keyboardNavigation: true,
    });

    disabledController.handleOpenChange(true);
    disabledController.setActiveItem(disabledItems[2]);

    expect(disabledController.focusedIndex).toBe(1);
    expect(disabledController.isItemFocused(disabledItems[2])).toBe(true);
  });

  it('ignores keyboard when navigation is disabled', () => {
    const disabledController = new MenuKeyboardController({
      getItems: () => items,
      onSelect,
      onOpen,
      onClose,
      keyboardNavigation: false,
    });

    const event = keydown('ArrowDown');
    disabledController.onTriggerKeydown(event);

    expect(onOpen).not.toHaveBeenCalled();
    expect(event.preventDefault).not.toHaveBeenCalled();
  });

  it('ignores keyboard when navigation input is undefined', () => {
    const disabledController = new MenuKeyboardController({
      getItems: () => items,
      onSelect,
      onOpen,
      onClose,
    });

    const event = keydown('ArrowDown');
    disabledController.onTriggerKeydown(event);

    expect(onOpen).not.toHaveBeenCalled();
    expect(disabledController.isNavigationEnabled()).toBe(false);
  });

  it('uses custom close keys', () => {
    const customController = new MenuKeyboardController({
      getItems: () => items,
      onSelect,
      onOpen,
      onClose,
      keyboardNavigation: {
        closeKeys: ['Escape'],
        dismissKeys: [],
      },
    });

    customController.handleOpenChange(true);

    const tab = keydown('Tab');
    customController.onMenuKeydown(tab);
    expect(onClose).not.toHaveBeenCalled();

    customController.onMenuKeydown(keydown('Escape'));
    expect(onClose).toHaveBeenCalledWith(true);
  });

  it('does not wrap focus when loop is disabled', () => {
    const noLoopController = new MenuKeyboardController({
      getItems: () => items,
      onSelect,
      onOpen,
      onClose,
      keyboardNavigation: {
        loop: false,
      },
    });

    noLoopController.handleOpenChange(true);
    noLoopController.focusedIndex = 2;

    noLoopController.onMenuKeydown(keydown('ArrowDown'));
    expect(noLoopController.focusedIndex).toBe(2);
  });

  it('cancels pending keyboard open state', () => {
    controller.onTriggerKeydown(keydown('ArrowDown'));
    expect(controller.focusedIndex).toBe(0);

    controller.cancelPendingOpen();

    expect(controller.focusedIndex).toBe(-1);
  });
});
