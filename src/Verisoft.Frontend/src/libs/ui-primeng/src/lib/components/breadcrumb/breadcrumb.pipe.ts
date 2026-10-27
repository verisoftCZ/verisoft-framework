import { Pipe, PipeTransform } from "@angular/core";
import { MenuItem as VerisoftMenuItem } from "@verisoft/ui-core";
import { MenuItem as PrimeMenuItem } from "primeng/api";

@Pipe({
  name: 'menuItem',
  standalone: true,
})
export class ConvertMenuItemPipe implements PipeTransform {
  transform(items: VerisoftMenuItem[]): PrimeMenuItem[] {
    return items.map(i => i as PrimeMenuItem);
  }
}