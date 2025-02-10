import {
  ChangeDetectionStrategy,
  Component,
  inject,
  Input,
  OnChanges,
  SimpleChanges,
} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import {
  API_PREFIX,
  DOC_PREFIX,
  DOCS_CONFIG,
  DocumentationCofig,
  DocumentationItem,
  DocumentationNavigationItem,
  getApiDocs,
  getMainDocs,
  getMenuItems,
} from '../../models';
import { DocumentationHeaderComponent } from '../documentation-header/documentation-header.component';
import { DocumentationGovCzTabsComponent } from '../documentation-tabs/documentation-govcz-tabs.component';
import { DocumentationPrimengTabsComponent } from '../documentation-tabs/documentation-primeng-tabs.component';

@Component({
  selector: 'v-doc-page',
  standalone: true,
  imports: [
    DocumentationPrimengTabsComponent,
    DocumentationGovCzTabsComponent,
    DocumentationHeaderComponent,
  ],
  templateUrl: './documentation-page.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DocumentationPageComponent implements OnChanges {
  @Input() title!: string;

  @Input() description!: string;

  @Input() items?: DocumentationItem[];

  activeIndex = 0;

  apiDocs!: DocumentationItem[];

  docs!: DocumentationItem[];

  menu!: DocumentationNavigationItem[];

  config = inject<DocumentationCofig>(DOCS_CONFIG);

  constructor(activativeRoute: ActivatedRoute, private router: Router) {
    activativeRoute.fragment.subscribe((fragment) => {
      this.activeIndex = fragment?.startsWith('api_') ? 1 : 0;
      this.navigateToPage(fragment);
    });
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['items']) {
      this.apiDocs = getApiDocs(this.items);
      this.docs = getMainDocs(this.items);
      this.menu = getMenuItems(this.docs);
    }
  }

  changeTab(index: number): void {
    this.activeIndex = index;
    this.router.navigate([], {
      fragment: this.activeIndex === 0 ? DOC_PREFIX : API_PREFIX,
    });
  }

  private navigateToPage(navigationId: string | null) {
    if (navigationId) {
      setTimeout(() => {
        const element = document.getElementById(navigationId);
        if (element) {
          element.scrollIntoView({ behavior: 'smooth', block: 'start' });
        }
      });
    }
  }
}
