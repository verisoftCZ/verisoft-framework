import { AfterContentInit, ChangeDetectionStrategy, Component, ContentChildren, EventEmitter, inject, Input, Output, QueryList } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { TabViewCore, TabViewItemCore } from '@verisoft/ui-core';
import { TabViewItemComponent } from './tab-view-item.component';

@Component({
  standalone: false,
  selector: 'v-tab-view',
  templateUrl: './tab-view.component.html',
  styleUrls: [
    "./tab-view.component.scss"
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TabViewComponent implements AfterContentInit, TabViewCore {
  router = inject(Router);
  private activatedRoute = inject(ActivatedRoute);

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

  protected activeItemChanged(event: CustomEvent): void {
    const clickedIdentifier = event.detail?.identifier;
    this.activeIndexChange.emit(clickedIdentifier);
  }

  protected handleTabClick(event: CustomEvent) {
    this.activeItemChanged(event);
    if (this.useRouting && event.detail?.triggerIdentifier) {
      const targetPath = event.detail.triggerIdentifier;
      
      this.router.navigate([targetPath], {
        relativeTo: this.activatedRoute.parent,
        skipLocationChange: false
      });
    }
  }
}
