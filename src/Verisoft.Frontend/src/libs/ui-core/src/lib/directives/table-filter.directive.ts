import {
  ChangeDetectorRef,
  Directive,
  inject,
  Input,
  OnChanges,
  OnDestroy,
  SimpleChanges,
} from '@angular/core';
import { debounceTime, Subject, Subscription } from 'rxjs';
import { DEFAULT_DEBOUNCE_TIME, setComponentProperties } from '../common';
import { FilterCore, TABLE_COMPONENT_TOKEN, TableCore } from '../components';

@Directive({
  // eslint-disable-next-line @angular-eslint/directive-selector
  selector: 'v-table[useFilter]',
  standalone: true,
  exportAs: 'tableFilterDirective',
})
export class TableFilterDirective<T> implements OnChanges, OnDestroy {
  @Input({ required: true }) filterComponent!: FilterCore;

  private filterChange$ = new Subject<T>();
  private subscription: Subscription | undefined = undefined;

  private tableComponent = inject<TableCore<T>>(TABLE_COMPONENT_TOKEN);

  private changeDetectorRef = inject(ChangeDetectorRef);

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['filterComponent']) {
      this.unRegister();
      this.register();
    }
  }

  private onFilterChange(value: T): void {
    if (this.tableComponent?.filter !== value) {
      setComponentProperties(this.tableComponent, {
        filter: value,
        currentPage: 1,
      });
      
      this.changeDetectorRef.detectChanges();
    }
  }

  ngOnDestroy(): void {
    this.unRegister();
  }

  private unRegister() {
    this.subscription?.unsubscribe();
  }

  private register() {
    this.filterComponent.registerOnChange((value: T) => {
      this.filterChange$.next(value);
    });

    this.subscription = this.filterChange$
      .pipe(
        debounceTime(
          this.filterComponent.debounceTime ? 0 : DEFAULT_DEBOUNCE_TIME
        )
      )
      .subscribe((value) => this.onFilterChange(value));
  }
}
