import { QueryList } from '@angular/core';
import { debounceTime, map, merge, startWith, switchMap } from 'rxjs';
import { NotificableProperty } from './notificable-property.model';

export function queryListChanged<TEntity>(list: QueryList<TEntity>) {
  return list.changes.pipe(
    startWith({}),
    switchMap(() => {
      const actionPropertyChanges$ = list
        .toArray()
        .filter((action) => (<NotificableProperty>action).propertyChanged)
        .map((action) => (<NotificableProperty>action).propertyChanged);

      return merge(...actionPropertyChanges$).pipe(
        startWith({}),
        map(() => list.toArray())
      );
    }),
    debounceTime(50)
  );
}
