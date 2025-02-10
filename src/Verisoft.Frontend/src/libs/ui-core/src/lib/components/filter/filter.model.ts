import { InjectionToken } from '@angular/core';
import { ControlValueAccessor } from '@angular/forms';
import { GenericFieldDefinition } from '../generic-form';

export const FILTER_COMPONENT_TOKEN = new InjectionToken<FilterCore>(
    'FilterComponentToken'
);

export interface FilterCore extends ControlValueAccessor {
    fields: GenericFieldDefinition[];  
    title?: string;  
    fulltextFieldName: string;  
    showFulltext: boolean;  
    showFilters: boolean;  
    autoBind: boolean;  
    debounceTime?: number;
}
