import { Pipe, PipeTransform } from "@angular/core";
import { FieldSize, FieldSizeType } from "@verisoft/ui-core";

@Pipe({
    name: 'govSize',
    standalone: true,
})
export class GovSizePipe implements PipeTransform {
    transform(size: FieldSizeType | undefined): string | undefined {
        switch (size) {
            case FieldSize.small:
                return 's';
            case FieldSize.medium:
            case undefined:
                return 'm';
            case FieldSize.large:
                return 'l';
            case FieldSize.xl:
                return 'xl';
            default:
                return size;
        }
    }
}
