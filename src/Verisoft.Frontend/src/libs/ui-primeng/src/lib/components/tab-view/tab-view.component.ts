import {
  AfterContentInit,
  Component,
  ContentChildren,
  EventEmitter,
  Input,
  Output,
  QueryList,
} from '@angular/core';
import { TabViewCore, TabViewItemCore } from '@verisoft/ui-core';
import { MenuItem } from 'primeng/api';
import { TabViewItemComponent } from './tab-view-item.component';

@Component({
  selector: 'v-tab-view',
  templateUrl: './tab-view.component.html',
})
export class TabViewComponent 
  implements AfterContentInit, TabViewCore
{
  @ContentChildren(TabViewItemComponent)
  children!: QueryList<TabViewItemComponent>;

  @Input() items: TabViewItemCore[] = [];

  @Input() useRouting = false;

  @Input() activeIndex = 0;

  @Output() activeIndexChange = new EventEmitter<number>();

  mergedItems: TabViewItemCore[] = [];

  ngAfterContentInit(): void {
    this.mergedItems = [...this.items, ...this.children.toArray()];
  }

  protected activeItemChanged(item: MenuItem | number): void {
    let index: number;
    if (typeof item === 'number') {
      index = item;
    } else {
      index = this.items.indexOf(item as TabViewItemCore);
    }

    if (index != -1) {
      this.activeIndexChange.emit(index);
    }
  }
}
