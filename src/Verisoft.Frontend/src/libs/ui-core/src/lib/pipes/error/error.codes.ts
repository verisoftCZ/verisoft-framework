type ErrorCodesFn = (value?: any) => string;

export const ErrorCodesFns: { [key: string]: ErrorCodesFn } = {
  required: () => `This field is required!`,
  email: () => `Email is in wrong format!`,
  iban: () => `IBAN is in wrong format!`,
  lowStrength: () => `Password too weak!`,
  customPasswordRequirements: () => `Password does not meet minimal requirements.`,
  fieldsNotMatching: () => `Passwords are not the same!`,
  validationError: (value: any) => `${value}`,
};
