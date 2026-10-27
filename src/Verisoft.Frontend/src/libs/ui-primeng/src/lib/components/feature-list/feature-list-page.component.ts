import { Component, inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FEATURE_LIST_PAGE_CONFIG_PROPERTY, FeatureListPageConfig } from '@verisoft/ui-core';
import { FeatureListComponent } from './feature-list.component';

@Component({
    selector: 'v-feature-list-page',
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
  `
})
export class FeatureListPageComponent {
  config!: FeatureListPageConfig;

  private readonly activatedRoute = inject(ActivatedRoute);

  constructor() {
    this.config =
      this.activatedRoute.snapshot.data[FEATURE_LIST_PAGE_CONFIG_PROPERTY];
  }
}
