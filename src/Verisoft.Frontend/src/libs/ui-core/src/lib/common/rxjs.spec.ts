import { QueryList } from '@angular/core';
import { skip, Subject } from 'rxjs';
import { NotificableProperty } from './notificable-property.model';
import { queryListChanged } from './rxjs';

describe('queryListChanged', () => {
  let queryList: QueryList<NotificableProperty>;

  beforeEach(() => {
    queryList = new QueryList<NotificableProperty>();
  });

  it('should emit initial value', (done) => {
    queryListChanged(queryList).subscribe((result) => {
      expect(result).toEqual([]);
      done();
    });
  });

  it('should emit when propertyChanged emits', (done) => {
    const propertyChanged = new Subject<void>();
    const item = { propertyChanged } as NotificableProperty;
    queryList.reset([item]);
    queryList.notifyOnChanges();

    queryListChanged(queryList)
      .pipe(skip(1))
      .subscribe((result) => {
        expect(result).toEqual([item]);
        done();
      });

    setTimeout(() => {
      propertyChanged.next();
    }, 60);
  });

  it('should debounce emissions', (done) => {
    const propertyChanged = new Subject<void>();
    const item = { propertyChanged } as NotificableProperty;
    queryList.reset([item]);
    queryList.notifyOnChanges();

    const emittedValues: NotificableProperty[][] = [];
    queryListChanged(queryList).subscribe((result) => {
      emittedValues.push(result);
    });

    propertyChanged.next();
    propertyChanged.next();

    setTimeout(() => {
      expect(emittedValues.length).toBe(1);
      expect(emittedValues[0]).toEqual([item]);
      done();
    }, 100);
  });
});
