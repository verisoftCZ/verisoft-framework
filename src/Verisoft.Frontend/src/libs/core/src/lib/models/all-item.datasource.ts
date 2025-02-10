import { Observable } from 'rxjs';

export interface AllItemDatasource<T> {
  getData$: () => Observable<T[]>
}
