import { Routes } from '@angular/router';
import { PrimengTabViewFirstTabExampleComponent, PrimengTabViewSecondTabExampleComponent } from './examples';
import { TabViewDocComponent } from './tabview-doc.component';

export const routes: Routes = [
  {
    path: '',
    component: TabViewDocComponent,
    children: [
      { path: 'first', component: PrimengTabViewFirstTabExampleComponent },
      { path: 'second', component: PrimengTabViewSecondTabExampleComponent },
      { path: '', redirectTo: 'first', pathMatch: 'full' },
    ],
  },
];
