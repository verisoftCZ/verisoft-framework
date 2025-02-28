import { NgTemplateOutlet } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { TAB_VIEW_COMPONENT_TOKEN } from '@verisoft/ui-core';
import { TabMenuModule } from 'primeng/tabmenu';
import { TabViewModule as PrimeTabViewModule } from 'primeng/tabview';
import { TabMenuPipe } from './tab-menu.pipe';
import { TabViewItemComponent } from './tab-view-item.component';
import { TabViewComponent } from './tab-view.component';

@NgModule({
  imports: [RouterOutlet, TabMenuModule, PrimeTabViewModule, NgTemplateOutlet],
  declarations: [TabViewComponent, TabViewItemComponent, TabMenuPipe],
  exports: [TabViewComponent, TabViewItemComponent],
  providers: [
    { provide: TAB_VIEW_COMPONENT_TOKEN, useExisting: TabViewComponent },
  ],
})
export class TabViewModule {}
