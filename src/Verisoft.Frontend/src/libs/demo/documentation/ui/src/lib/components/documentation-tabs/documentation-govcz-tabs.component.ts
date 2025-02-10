import { ChangeDetectionStrategy, Component, EventEmitter, Input, Output } from '@angular/core';
import { TabViewModule } from '@verisoft/ui-govcz';
import { DocumentationItem, DocumentationNavigationItem } from '../../models';
import { DocumentationNavigationComponent } from '../documentation-navigation/documentation-navigation.component';
import { DocumentationSectionComponent } from '../documentation-section/documentation-section.component';

@Component({
  selector: 'v-doc-govcz-tabs',
  standalone: true,
  imports: [
    TabViewModule,
    DocumentationSectionComponent,
    DocumentationNavigationComponent,
  ],
  templateUrl: './documentation-tabs.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DocumentationGovCzTabsComponent {
  @Input() activeIndex = 0;

  @Input() apiDocs!: DocumentationItem[];

  @Input() docs!: DocumentationItem[];

  @Input() menu!: DocumentationNavigationItem[];

  @Output() activeIndexChange = new EventEmitter<number>();
}
