import { ChangeDetectorRef, Directive, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UnsubscribeComponent } from '../unsubscribe.component';
import { FieldSize, FieldSizeType } from '../../common';
import { takeUntil } from 'rxjs';
import { PageHeaderService } from './page-header.service';

@Directive({})
export class PageHeaderCoreComponent
  extends UnsubscribeComponent
  implements OnInit
{
  @Input() title!: string;
  @Input() subtitle: string | undefined;
  @Input() showBackButton!: boolean;
  @Input() size: FieldSizeType = FieldSize.small;

constructor(
    readonly router: Router,
    readonly cdr: ChangeDetectorRef,
    readonly headerService: PageHeaderService
  ) {
    super();
  }

  ngOnInit(): void {
    this.headerService.pageHeader
      .pipe(takeUntil(this.destroyed$))
      .subscribe((x) => {
        this.title = x.title;
        this.subtitle = x.subtitle;
        this.showBackButton = x.showBackButton ?? false;
        this.cdr.detectChanges();
      });
  }

  protected locationBack() {
    history.back();
  }
}
