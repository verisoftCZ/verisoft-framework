import {
  InteractionAction,
  LinearNavigationAction,
} from '../core/keyboard-action.model';

export type MenuTriggerAction = 'open' | 'openToLast';

export type MenuOpenKeyboardAction = LinearNavigationAction | InteractionAction;

export type MenuKeyboardAction = MenuOpenKeyboardAction | MenuTriggerAction;
