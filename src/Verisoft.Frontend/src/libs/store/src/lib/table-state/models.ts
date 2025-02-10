import { DEFAULT_SEARCH_LIMIT, DEFAULT_SEARCH_PARAMS, Page, RequestParams } from "@verisoft/core";

export interface TableState<T> {
  dataLoading: boolean;
  requestParams: RequestParams<T>;
  gPage?: Page<T>;
  error?: string | null;
  selectedItems?: T[];
}

export const INITIAL_TABLE_STATE: TableState<any> = {
  gPage: {
    data: [],
    size: DEFAULT_SEARCH_LIMIT,
    total: 0.
  } as unknown as Page<any>,
  dataLoading: false,
  requestParams: DEFAULT_SEARCH_PARAMS,
  selectedItems: []
};