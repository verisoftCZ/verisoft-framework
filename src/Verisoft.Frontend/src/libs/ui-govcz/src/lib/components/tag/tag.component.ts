import {
    ChangeDetectionStrategy,
    Component,
    input,
} from "@angular/core";
import { GovDesignSystemModule } from "@gov-design-system-ce/angular";
import { TranslateModule } from "@ngx-translate/core";
import {
    ControlSeverity,
    ControlSeverityType,
    FieldSize,
    FieldSizeType,
    TagCore,
    TAG_COMPONENT_TOKEN,
    TagVariant,
} from "@verisoft/ui-core";
import { GovColorPipe, GovSizePipe } from "../../pipes";
import { GovTagType } from "./tag.model";

@Component({
    selector: "v-tag",
    styleUrl: "./tag.component.scss",
    templateUrl: './tag.component.html',
    imports: [
        GovDesignSystemModule,
        GovSizePipe,
        GovColorPipe,
        TranslateModule
    ],
    changeDetection: ChangeDetectionStrategy.OnPush,
    providers: [
        {
            provide: TAG_COMPONENT_TOKEN,
            useExisting: TagComponent,
        }
    ],
})
export class TagComponent implements TagCore {
    label = input<string | undefined>();
    severity = input<ControlSeverityType | undefined>(ControlSeverity.primary);
    variant = input<TagVariant>(GovTagType.bold);
    size = input<FieldSizeType>(FieldSize.medium);
}

