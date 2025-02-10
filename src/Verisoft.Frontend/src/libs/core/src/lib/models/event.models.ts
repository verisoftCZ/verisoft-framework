import { RequestParams } from "./base-http.models";

export type LazyLoadEvent = RequestParams<unknown>;

export interface FilterEvent {
    filter?: string;
}