import { EventEmitter } from '@angular/core';
import { ControlSeverityType, FieldSizeType, NotificableProperty } from '../../common';

export interface ActionButton extends NotificableProperty {
  disabled: boolean;
  toolTip?: string;
  id?: string;
  icon?: string;
  outlined: boolean;
  raised: boolean;
  severity?: ControlSeverityType;
  label?: string;
  size?: FieldSizeType;
  click: EventEmitter<MouseEvent>;
}
