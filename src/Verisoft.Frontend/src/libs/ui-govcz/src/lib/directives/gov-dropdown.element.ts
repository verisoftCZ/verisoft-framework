export type GovDropdownElement = HTMLElement & {
  getState: () => Promise<boolean>;
  toggle: () => Promise<void>;
};
