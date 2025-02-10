export interface DetailState<T> {
  loaded: boolean;
  item?: T;
  formState?: FormState;
  error?: string | null;
}

export interface ChosenRowsState<T> { 
  rows: Array<T>;
}

export const INITIAL_CHOSEN_ROW_STATE: ChosenRowsState<any> = {
  rows: []
}

export const INITIAL_DETAIL_STATE: DetailState<any> = {
  loaded: false,
};

export interface SaveItemState {
  saveInProgress: boolean;
  error?: string | null;
}

export const INITIAL_SAVE_ITEM_STATE: SaveItemState = {
  saveInProgress: false,
};

export interface FormState {
  dirty: boolean;
  valid: boolean;
}
