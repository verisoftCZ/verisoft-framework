import { CommonModule } from '@angular/common';
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
import { GovSizePipe } from '../../pipes';
import { Icons } from '../../icons';
@Component({
    selector: 'v-search',
    standalone: true,
    imports: [
    CommonModule,
    GovDesignSystemModule,
    GovSizePipe,
    ReactiveFormsModule,
],
    templateUrl: './search.component.html',
    styleUrl: './search.component.scss',
    changeDetection: ChangeDetectionStrategy.OnPush,
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
