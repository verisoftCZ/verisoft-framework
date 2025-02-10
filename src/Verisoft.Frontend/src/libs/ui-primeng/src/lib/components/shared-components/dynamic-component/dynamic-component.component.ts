import {
  AfterViewInit,
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  inject,
  Injector,
  Input,
  OnChanges,
  Type,
  ViewChild,
  ViewContainerRef,
} from '@angular/core';
import { DynamicComponentFactoryService } from './dynamic-component-factory.service';

@Component({
  selector: 'v-dynamic-component',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `<ng-container #dynamicContainer></ng-container>`,
})
export class DynamicComponent<TComponent> implements AfterViewInit, OnChanges {
  @Input() componentType!: Type<TComponent>;

  @Input() data!: Partial<TComponent>;

  @ViewChild('dynamicContainer', { read: ViewContainerRef, static: true })
  container!: ViewContainerRef;

  private factoryServices = inject(DynamicComponentFactoryService);

  private changeDetectorRef = inject(ChangeDetectorRef);

  private injector = inject(Injector);

  ngOnChanges(): void {
    this.crateComponent();
  }

  ngAfterViewInit(): void {
    this.crateComponent();
    this.changeDetectorRef.detectChanges();
  }

  private crateComponent() {
    if (this.container) {
      this.factoryServices.createDynamicComponent(
        this.componentType,
        this.container,
        this.data,
        this.injector
      );
    }
  }
}
