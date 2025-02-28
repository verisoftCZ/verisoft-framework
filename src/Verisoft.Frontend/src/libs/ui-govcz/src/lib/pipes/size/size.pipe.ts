import { Pipe, PipeTransform } from "@angular/core";
import { FieldSizeType } from "@verisoft/ui-core";

@Pipe({
    name: 'govSize',
    standalone: true,
})
export class GovSizePipe implements PipeTransform {
    transform(size: FieldSizeType | undefined): string | undefined {
        if (size) {
            return size.substring(0, 1);
        }

        if (typeof(size) === 'undefined') {
            return 'm';
        }

        return size;
    }
}
