export enum ControlSeverity {
  success = 'success',
  info = 'info',
  warning = 'warning',
  danger = 'danger',
  help = 'help',
  primary = 'primary',
  secondary = 'secondary',
  contrast = 'contrast',
}

export enum GovControlSeverity {
  primary = 'primary',
  secondary = 'secondary',
  neutral = 'neutral',
  error = 'error',
  success = 'success',
  warning = 'warning',
}

export enum GovButtonType {
  solid = 'solid',
  outlined = "outlined",
  base = "base",
  link = "link"
}

export enum IconPosition {
  left = 'left',
  right = 'right',
}

export enum SlotPosition {
  top = 'top',
  bottom = 'bottom',
}

export enum Position{
  top = 'top',
  bottom = 'bottom',
  left = 'left',
  right = 'right',
}

export enum FieldSize {
  small = 'small',
  medium = 'medium',
  large = 'large',
  xl = 'xl',
}

export enum FieldAlign {
  left = 'left',
  center = 'center',
  right = 'right',
}

export enum FieldType {
  text = 'text',
  number = 'number',
  password = 'password',
  search = 'search',
  date = 'date',
}

export enum LayoutType {
  horizontal = 'horizontal',
  vertical = 'vertical',
}

export type ControlSeverityType = keyof typeof ControlSeverity;
export type GovControlSeverityType = keyof typeof GovControlSeverity;
export type GovButtonTypeType = keyof typeof GovButtonType;
export type IconPositionType = keyof typeof IconPosition;
export type SlotPositionType = keyof typeof SlotPosition;
export type PositionType = keyof typeof Position;
export type FieldSizeType = keyof typeof FieldSize;
export type FieldAlignType = keyof typeof FieldAlign;
export type FieldTypeType = keyof typeof FieldType;
export type LayoutTypeType = keyof typeof LayoutType;
