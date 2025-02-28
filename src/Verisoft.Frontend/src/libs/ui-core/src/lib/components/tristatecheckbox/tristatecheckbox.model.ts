import { InjectionToken } from '@angular/core';
import { BaseFormCore } from '../base-form';

export const TRISTATE_CHECKBOX_COMPONENT_TOKEN = new InjectionToken<TristateCheckboxCore>(
    'TristateCheckboxComponentToken'
);

// eslint-disable-next-line @typescript-eslint/no-empty-interface
export interface TristateCheckboxCore extends BaseFormCore {}
