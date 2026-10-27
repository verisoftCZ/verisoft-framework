import { NgTemplateOutlet } from '@angular/common';
import {
  AfterContentInit,
  Component,
  ContentChildren,
  EventEmitter,
  Input,
  Output,
  QueryList,
} from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';
import { TAB_VIEW_COMPONENT_TOKEN, TabViewCore, TabViewItemCore } from '@verisoft/ui-core';
import { MenuItem } from 'primeng/api';
import { TabsModule } from 'primeng/tabs';
import { TabViewItemComponent } from './tab-view-item.component';
@Component({
  selector: 'v-tab-view',
  templateUrl: './tab-view.component.html',
  imports: [RouterOutlet, RouterLink, TabsModule, NgTemplateOutlet],
  providers: [
    { provide: TAB_VIEW_COMPONENT_TOKEN, useExisting: TabViewComponent }],

})
export class TabViewComponent
  implements AfterContentInit, TabViewCore {
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
