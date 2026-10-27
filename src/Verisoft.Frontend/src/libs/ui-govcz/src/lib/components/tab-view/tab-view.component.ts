import { NgTemplateOutlet } from '@angular/common';
import {
  AfterContentInit,
  ChangeDetectorRef,
  Component,
  ContentChildren,
  EventEmitter,
  inject,
  Input,
  Output,
  QueryList,
  ViewEncapsulation,
} from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { TranslatePipe } from '@ngx-translate/core';
import { TabViewItemCore } from '@verisoft/ui-core';
import { TabViewItemComponent } from './tab-view-item.component';

@Component({
  selector: 'v-tab-view',
  imports: [NgTemplateOutlet, RouterOutlet, RouterLink, RouterLinkActive, TranslatePipe],
  templateUrl: './tab-view.component.html',
  styleUrls: ['./tab-view.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class TabViewComponent implements AfterContentInit {

  @ContentChildren(TabViewItemComponent) tabs!: QueryList<TabViewItemComponent>;

  @Input() highlight = false;
  @Input() useRouting = false;
  @Input() items: TabViewItemCore[] = [];
  @Input() activeIndex = 0;

  @Output() activeIndexChange = new EventEmitter<number>();

  private readonly cd = inject(ChangeDetectorRef);

  ngAfterContentInit(): void {
    this.initTabs();
  }

  initTabs(): void {
    this.items = [...this.tabs.toArray(), ...this.items];
    this.cd.detectChanges();
  }

  changeTabIndex(index: number) {
    if (this.activeIndex !== index) {
      this.activeIndex = index;
      this.activeIndexChange.emit(this.activeIndex);
    }

    this.activeIndexChange.emit(index);
    this.cd.detectChanges();
  }

  setTabActive(options: TabViewItemComponent[]): void {
    options.forEach((_, index) => {
      index === this.activeIndex;
    });
    this.cd.detectChanges();
  }
}