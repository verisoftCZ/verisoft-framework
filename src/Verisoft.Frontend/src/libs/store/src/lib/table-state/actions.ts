import { createAction, props } from '@ngrx/store';
import { Page, Sort } from '@verisoft/core';

enum TablePageAction {
  GET_PAGE = 'Get page',
  CREATE_STATIC_FILTER = 'Create static filter',
  DATA_LOAD_SUCCESS = 'Data load success',
  DATA_LOAD_ERROR = 'Data load error',
  REFRESH_PAGE = 'Refresh page',
  FILTER_PAGE = 'Filter page',
  CHANGE_PAGE_SIZE = 'Change page size',
  SORT_PAGE = 'Sort page',
  DESTROY = 'Destroy',
  SELECT_ITEMS = 'Select items',
  REMOVE_RANGE = 'Remove range',
}

export function createGetPageTableAction<T = any>(tableRepository: string) {
  return createAction(
    `${tableRepository} ${TablePageAction.GET_PAGE}`,
    props<{
      page: number;
      size: number;
      id?: string | null;
      filter?: Partial<T>;
      sort?: Sort[];
    }>()
  );
}

export function createStaticFilterTableAction<T = any>(
  tableRepository: string
) {
  return createAction(
    `${tableRepository} ${TablePageAction.CREATE_STATIC_FILTER}`,
    props<{
      filter?: Partial<T>;
    }>()
  );
}

export function createDataLoadSuccessTableAction<T>(tableRepository: string) {
  return createAction(
    `${tableRepository} ${TablePageAction.DATA_LOAD_SUCCESS}`,
    props<{ gPage: Page<T> }>()
  );
}

export function createDataLoadErrorTableAction(tableRepository: string) {
  return createAction(
    `${tableRepository} ${TablePageAction.DATA_LOAD_ERROR}`,
    props<{ error: any }>()
  );
}

export function createChangePageSizeTableAction(tableRepository: string) {
  return createAction(
    `${tableRepository} ${TablePageAction.CHANGE_PAGE_SIZE}`,
    props<{ size: number }>()
  );
}

// TODO: use action in delete item effect
export function createRefreshPageTableAction(tableRepository: string) {
  return createAction(`${tableRepository} ${TablePageAction.REFRESH_PAGE}`);
}

export function createFilterPageTableAction<T>(tableRepository: string) {
  return createAction(
    `${tableRepository} ${TablePageAction.REFRESH_PAGE}`,
    props<{ filter: Partial<T> }>()
  );
}

export function createResetTableFilterAction(tableRepository: string) {
  return createAction(`${tableRepository} ${TablePageAction.REFRESH_PAGE}`);
}

export function createDestroyTableAction(tableRepository: string) {
  return createAction(`${tableRepository} ${TablePageAction.DESTROY}`);
}

export function createSelectItemsTableAction<T>(tableRepository: string) {
  return createAction(
    `${tableRepository} ${TablePageAction.SELECT_ITEMS}`,
    props<{ selectedItems: T[] }>()
  );
}

export function createRemoveRangeTableAction(tableRepository: string) {
  return createAction(`${tableRepository} ${TablePageAction.REMOVE_RANGE}`
  );
}