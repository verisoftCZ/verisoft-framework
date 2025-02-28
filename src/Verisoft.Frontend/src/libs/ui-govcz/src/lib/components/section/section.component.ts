import { CommonModule } from '@angular/common';
import {
    ChangeDetectionStrategy,
    Component,
    Input,
    NO_ERRORS_SCHEMA,
} from '@angular/core';
import { GovDesignSystemModule } from '@gov-design-system-ce/angular';
import {
    FieldSizeType,
    FieldSize,
    SECTION_COMPONENT_TOKEN,
    SectionCore
} from '@verisoft/ui-core';
import { GovSizePipe } from '../../pipes';
import { Icons } from '../../icons';

@Component({
    selector: 'v-section',
    standalone: true,
    imports: [
        CommonModule,
        GovDesignSystemModule,
        GovSizePipe
    ],
    templateUrl: './section.component.html',
    styleUrl: './section.component.scss',
    changeDetection: ChangeDetectionStrategy.OnPush,
    providers: [
        {
            provide: SECTION_COMPONENT_TOKEN,
            useExisting: SectionComponent,
        },
    ],
    schemas: [
        NO_ERRORS_SCHEMA
    ],
})
export class SectionComponent
    implements SectionCore
{
    @Input() title!: string;
    @Input() badge!: string;
    @Input() showContent = false;
    @Input() backgroundColor!: string;
    @Input() annotation!: string;
    @Input() identifier!: string;
    @Input() icon!: string;
    @Input() iconType!: string;
    @Input() size: FieldSizeType = FieldSize.medium;
    @Input() badgeSlot: 'prefix' | 'sufix' = 'sufix';
    @Input() open = false;

    icons = Icons;
}
