import { NgTemplateOutlet } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { GovDesignSystemModule } from '@gov-design-system-ce/angular';
import { TranslateModule } from '@ngx-translate/core';
import { TAB_VIEW_COMPONENT_TOKEN } from '@verisoft/ui-core';
import { TabViewItemComponent } from './tab-view-item.component';
import { TabViewComponent } from './tab-view.component';

@NgModule({
  imports: [
    RouterOutlet,
    NgTemplateOutlet,
    GovDesignSystemModule,
    RouterLinkActive,
    RouterLink,
    TranslateModule
  ],
  declarations: [TabViewComponent, TabViewItemComponent],
  exports: [TabViewComponent, TabViewItemComponent],
  providers: [
    { provide: TAB_VIEW_COMPONENT_TOKEN, useExisting: TabViewComponent },
  ],
})
export class TabViewModule {}
