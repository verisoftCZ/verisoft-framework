import { InjectionToken } from '@angular/core';
import { MenuItem } from '../side-menu/side-menu.model';

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
    fixed: boolean;
    userAvatar: string;
    menuRef: HTMLDivElement;
}
