
import {
    ChangeDetectionStrategy,
    Component,
    EventEmitter,
    Input,
    Output,
} from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { GovDesignSystemModule } from '@gov-design-system-ce/angular';
import { FieldSizeType, FieldSize } from '@verisoft/ui-core';
import { Icons } from '../../icons';
import { GovSizePipe } from '../../pipes';

@Component({
    selector: 'v-search',
    imports: [
    GovDesignSystemModule,
    GovSizePipe,
    ReactiveFormsModule
],
    templateUrl: './search.component.html',
    styleUrl: './search.component.scss',
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class SearchComponent {

    @Input() placeholder!: string;

    @Input() size: FieldSizeType = FieldSize.medium;

    @Output() searchTerm = new EventEmitter<string>();

    value!: string;

    icons = Icons

    setValue(event: any) {
        this.value = event.detail.value;
    }

    search() {
        this.searchTerm.emit(this.value);
    }
}
