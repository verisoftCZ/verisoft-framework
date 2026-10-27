export interface DetailState<T> {
  loaded: boolean;
  item?: T;
  formState?: FormState;
  error?: string | null;
  saveItemState: SaveItemState;
  backendValidationErrors: BackendValidationError[];
}

export interface BackendValidationError {
  code: string;
  message: string;
  parameters: string;
}

export interface ChosenRowsState<T> { 
  rows: Array<T>;
}

export interface SaveItemState {
  saveInProgress: boolean;
  error?: string | null;
}

export const INITIAL_SAVE_ITEM_STATE: SaveItemState = {
  saveInProgress: false,
};

export const INITIAL_CHOSEN_ROW_STATE: ChosenRowsState<any> = {
  rows: []
}

export const INITIAL_DETAIL_STATE: DetailState<any> = {
  loaded: false,
  saveItemState: INITIAL_SAVE_ITEM_STATE,
  backendValidationErrors: [],
};

export interface FormState {
  dirty: boolean;
  valid: boolean;
}
