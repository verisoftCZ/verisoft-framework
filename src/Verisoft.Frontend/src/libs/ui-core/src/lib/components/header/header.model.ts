import { InjectionToken } from '@angular/core';
import { MenuItem } from 'primeng/api';

export const HEADER_COMPONENT_TOKEN = new InjectionToken<HeaderCore>(
    'HeaderComponentToken'
);

export const SETTINGS_MENU = new InjectionToken<MenuItem[]>(
    'SETTINGS_MENU'
  );

export interface HeaderCore {
    title: string;
    userName: string;
    logoUrl: string;
    userRole: any | any[] | undefined;
    menuRef: HTMLDivElement;
}
