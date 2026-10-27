import { Pipe, PipeTransform } from "@angular/core";
import { GenericFieldDefinition, GenericFieldType } from "../generic-form";
import { FeatureListColumnDefinition } from "./feature-list-page.model";

@Pipe({
    name: 'featureListColumn',
})
export class FeatureListFilterPipe<T> implements PipeTransform {
    transform(value: FeatureListColumnDefinition<T>[] | undefined): GenericFieldDefinition[] | undefined {
        if (!value){
            return undefined;
        }

        return value.filter(x => x.filter).map((x, index) => ({
            ...(x.filterOptions ?? {}),
            name: x.id,
            label: typeof x.headerName === 'function'? x.headerName(x.id, index) : x.headerName ?? '',
            type: x.type as GenericFieldType,
        }));
    }
}