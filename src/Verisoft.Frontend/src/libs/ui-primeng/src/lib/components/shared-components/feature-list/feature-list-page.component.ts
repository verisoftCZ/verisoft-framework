import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import {
  FEATURE_LIST_PAGE_CONFIG_PROPERTY,
  FeatureListPageConfig,
} from './feature-list-page.model';
import { FeatureListComponent } from './feature-list.component';

@Component({
  selector: 'v-feature-list-page',
  standalone: true,
  imports: [FeatureListComponent],
  template: `
    <v-feature-list
      [title]="config.title"
      [tableName]="config.tableName"
      [showAdd]="config.showAdd ?? true"
      [showDownload]="config.showDownload ?? true"
      [showDelete]="config.showDelete ?? true"
    >
    </v-feature-list>
  `,
})
export class FeatureListPageComponent {
  config!: FeatureListPageConfig;

  constructor(activatedRoute: ActivatedRoute) {
    this.config =
      activatedRoute.snapshot.data[FEATURE_LIST_PAGE_CONFIG_PROPERTY];
  }
}
