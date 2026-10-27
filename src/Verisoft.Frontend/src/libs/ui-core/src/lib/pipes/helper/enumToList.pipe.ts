import { Pipe, PipeTransform } from "@angular/core";

@Pipe({
    name: 'enumToList',
    standalone: true,
})
export class EnumToListPipe implements PipeTransform {
    transform(data: any): any[] {
        return Object.keys(data)
            .filter(key => isNaN(Number(key)))
            .map(key => ({
                label: key,
                value: data[key as keyof typeof data],
            }));
    }
}