import { Routes } from '@angular/router';
import { TabViewFirstTabExampleComponent, TabViewSecondTabExampleComponent } from './examples';
import { TabViewDocComponent } from './tabview-doc.component';

export const routes: Routes = [
  {
    path: '',
    component: TabViewDocComponent,
    children: [
      { path: 'first', component: TabViewFirstTabExampleComponent },
      { path: 'second', component: TabViewSecondTabExampleComponent },
      { path: '**', redirectTo: 'first', pathMatch: 'full' },
    ],
  },
];
