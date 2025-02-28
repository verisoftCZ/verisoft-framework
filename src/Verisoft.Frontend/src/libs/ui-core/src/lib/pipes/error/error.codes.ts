type ErrorCodesFn = (value?: any) => string;

export const ErrorCodesFns: { [key: string]: ErrorCodesFn } = {
  required: () => `This field is required!`,
  validationError: (value: any) => `Validation error: ${value}`,
};
