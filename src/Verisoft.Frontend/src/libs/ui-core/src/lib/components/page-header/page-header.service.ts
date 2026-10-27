import { EventEmitter, Injectable, Output } from '@angular/core';
import { PageHeaderCore } from './page-header.model';

@Injectable({
  providedIn: 'root',
})
export class PageHeaderService {
  @Output() pageHeader = new EventEmitter<PageHeaderCore>();
}
