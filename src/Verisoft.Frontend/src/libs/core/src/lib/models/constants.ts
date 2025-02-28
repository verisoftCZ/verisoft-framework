import { LazyLoadEvent } from './event.models';

export const DEFAULT_PAGE_SIZE = 50;

export const DEFAULT_LAZYLOAD_OPTIONS: LazyLoadEvent = {
  offset: 0,
  limit: DEFAULT_PAGE_SIZE,
};
