import { InjectionToken } from '@angular/core';
import { IconPositionType } from '../../common';
import { ActionButton } from './action-button.model';

export const ACTION_BUTTON_GROUP_COMPONENT_TOKEN =
  new InjectionToken<ActionButtonGroupCore>('ActionButtonGroupComponentToken');

export interface ActionButtonGroupCore {
  maxItems: number;
  maxItemsMobile: number;
  items: ActionButton[];
  menuIconPos: IconPositionType;
  menuIcon: string;
  label?: string;
}
