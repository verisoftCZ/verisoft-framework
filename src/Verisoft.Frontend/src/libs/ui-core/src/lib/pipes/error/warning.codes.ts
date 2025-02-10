type WarningCodesFn = (value?: any) => string;

export const WarningCodesFns: { [key: string]: WarningCodesFn } = {
  validationWarning: (value: any) => `${value}`,
};
