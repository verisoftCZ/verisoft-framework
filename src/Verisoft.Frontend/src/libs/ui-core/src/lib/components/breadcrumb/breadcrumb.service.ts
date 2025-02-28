import { EventEmitter, Injectable, Output } from '@angular/core';
import { Breadcrumb } from './breadcrumb.model';

@Injectable({
  providedIn: 'root',
})
export class BreadcrumbService {
  @Output() routeChange: EventEmitter<Breadcrumb> = new EventEmitter();
}
