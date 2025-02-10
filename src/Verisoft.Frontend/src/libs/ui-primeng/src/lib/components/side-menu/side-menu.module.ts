import { NgModule, ModuleWithProviders } from '@angular/core';
import {
  MENU_TOKEN,
  MenuItem,
  SIDE_MENU_COMPONENT_TOKEN,
  SIDE_MENU_STATE_TOKEN,
  SideMenuModuleConfig,
  SideMenuProviderService,
  UnsubscribeComponent,
} from '@verisoft/ui-core';
import { SideMenuComponent } from './side-menu.component';

@NgModule({
  imports: [SideMenuComponent],
  exports: [SideMenuComponent],
  providers: [
    SideMenuProviderService,
    {
      provide: SIDE_MENU_COMPONENT_TOKEN,
      useExisting: SideMenuComponent,
    },
    {
      provide: SIDE_MENU_STATE_TOKEN,
      useValue: localStorage.getItem('SideMenuStateToken') ?? {
        expanded: [],
        minimalized: false,
      },
    },
  ],
})
export class SideMenuModule extends UnsubscribeComponent {
  constructor(private readonly service: SideMenuProviderService) {
    super();
  }

  static forRoot(
    config?: SideMenuModuleConfig
  ): ModuleWithProviders<SideMenuModule> {
    const moduleWithProvider: ModuleWithProviders<SideMenuModule> = {
      ngModule: SideMenuModule,
    };

    if (config?.items) {
      moduleWithProvider.providers = [
        {
          provide: MENU_TOKEN,
          useValue: config.items,
        },
      ];
    }

    return moduleWithProvider;
  }

  static forChild(menu: MenuItem[]): ModuleWithProviders<SideMenuModule> {
    return {
      ngModule: SideMenuModule,
      providers: [
        {
          provide: MENU_TOKEN,
          useValue: menu,
        },
      ],
    };
  }
}
