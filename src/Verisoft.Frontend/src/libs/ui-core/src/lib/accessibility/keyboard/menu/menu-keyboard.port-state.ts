export type MenuKeyboardPortOperationState = 'idle' | 'opening' | 'closing';

export type MenuKeyboardPortIntent =
  | { type: 'open' }
  | { type: 'close'; restoreFocus: boolean };
