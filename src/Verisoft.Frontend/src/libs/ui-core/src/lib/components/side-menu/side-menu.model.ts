import { InjectionToken } from '@angular/core';

export const MENU_TOKEN = new InjectionToken<MenuItem[]>(
  'MENU'
);

export const LOGO_ROUTER_ROUTE = new InjectionToken<string>(
  'LOGO_ROUTER_ROUTE'
);

export const SIDE_MENU_COMPONENT_TOKEN = new InjectionToken<SideMenuCore>(
  'SideMenuComponentToken'
);

export const SIDE_MENU_STATE_TOKEN = new InjectionToken<SideMenuState>(
  'SideMenuStateToken'
);

export interface SideMenuState {
  expanded: string[];
  minimalized: boolean;
}

export interface MenuItem {
  id?: string;
  label: string;
  icon?: string;
  expanded?: boolean;
  data?: MenuItemData;
  tooltip?: string;
  url?: string;
  visible?: boolean;
  type?: string;
  children?: MenuItem[];
  routerLink?: any;
  class?: string;
}

type ExtendableType = {
  [key: string]:
    | string
    | number
    | boolean
    | string[]
    | number[]
    | boolean[]
    | undefined;
};

export interface MenuItemData extends ExtendableType {
  roles?: string[];
  permissions?: string[];
}

export interface SideMenuModuleConfig {
  items?: MenuItem[];
}

export interface SideMenuCore {
  items: MenuItem[];
  logoUrl: string;
  userName: string;
  userRole: any | any[] | undefined;
}