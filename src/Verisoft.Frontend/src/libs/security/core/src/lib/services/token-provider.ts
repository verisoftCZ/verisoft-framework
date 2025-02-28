import { Observable } from "rxjs";

export interface TokenProvider {
    getToken(): Observable<string | undefined>;
  }