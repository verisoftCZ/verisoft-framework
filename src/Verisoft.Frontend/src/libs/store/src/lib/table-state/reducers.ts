/* eslint-disable @typescript-eslint/no-explicit-any */
import { ActionCreator, ReducerTypes, createReducer, on } from '@ngrx/store';
import {
  createGetPageTableAction,
  createStaticFilterTableAction,
  createDataLoadSuccessTableAction,
  createDataLoadErrorTableAction,
  createFilterPageTableAction,
  createDestroyTableAction,
  createSelectItemsTableAction,
  createChangePageSizeTableAction,
  createResetTableFilterAction,
} from './actions';
import { INITIAL_TABLE_STATE, TableState } from './models';

export function createTablePageReducers<
  T = any,
  TState extends TableState<T> = TableState<T>
>(
  tableRepository: string,
  initialState: TState = INITIAL_TABLE_STATE as unknown as TState,
  ...ons: ReducerTypes<TState, readonly ActionCreator[]>[]
) {
  return createReducer(
    initialState,
    on(
      createGetPageTableAction(tableRepository),
      (state, { page, id, filter, sort }) => {
        return {
          ...state,
          requestParams: {
            ...state.requestParams,
            filter: filter ?? state.requestParams.filter,
            sort: sort ?? state.requestParams.sort,
            id: id ?? state.requestParams.id,
            page,
          },
          dataLoading: true,
          submitted: true,
        };
      }
    ),

    on(createStaticFilterTableAction(tableRepository), (state, { filter }) => {
      return {
        ...state,
        requestParams: {
          ...state.requestParams,
          filter: {
            ...state.requestParams?.filter,
            filter,
          },
        },
      };
    }),

    on(createResetTableFilterAction(tableRepository), (state) => {
      return {
        ...state,
        requestParams: {
          ...state.requestParams,
          filter: undefined,
          page: 1
        },
      }
    }),

    on(
      createDataLoadSuccessTableAction(tableRepository),
      (state, { gPage }) => {
        return {
          ...state,
          gPage,
          dataLoading: false,
        };
      }
    ),

    on(createDataLoadErrorTableAction(tableRepository), (state, { error }) => {
      return {
        ...state,
        error,
        dataLoading: false,
      };
    }),

    on(createFilterPageTableAction(tableRepository), (state, { filter }) => {
      return {
        ...state,
        requestParams: {
          ...state.requestParams,
          filter: filter,
          page: 1,
        },
        dataLoading: true,
      };
    }),

    on(createDestroyTableAction(tableRepository), () => {
      return {
        ...(initialState as any),
      };
    }),

    on(
      createSelectItemsTableAction(tableRepository),
      (state, { selectedItems }) => {
        return {
          ...state,
          selectedItems,
        };
      }
    ),

    on(createChangePageSizeTableAction(tableRepository), (state, { size }) => {
      return {
        ...state,
        requestParams: {
          ...state.requestParams,
          size,
        }
      }
    }),
    ...ons
  );
}
