import {
  AfterViewInit,
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  ComponentRef,
  inject,
  Injector,
  Input,
  OnChanges,
  OnDestroy,
  Type,
  ViewChild,
  ViewContainerRef,
} from '@angular/core';
import { ExtendedComponent } from '@verisoft/ui-core';
import { DynamicComponentFactoryService } from './dynamic-component-factory.service';

@Component({
  selector: 'v-dynamic-component',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `<ng-container #dynamicContainer></ng-container>`,
})
export class DynamicComponent<TComponent>
  implements AfterViewInit, OnChanges, OnDestroy {
  @Input() componentType!: Type<TComponent>;

  @Input() data!: ExtendedComponent<TComponent>;

  @ViewChild('dynamicContainer', { read: ViewContainerRef, static: true })
  container!: ViewContainerRef;

  private factoryServices = inject(DynamicComponentFactoryService);

  private changeDetectorRef = inject(ChangeDetectorRef);

  private injector = inject(Injector);

  private componentRef?: ComponentRef<TComponent>;

  ngOnChanges(): void {
    this.createComponent();
  }

  ngAfterViewInit(): void {
    this.createComponent();
    this.changeDetectorRef.detectChanges();
  }

  ngOnDestroy(): void {
    this.destroyCurrentComponent();
  }

  private createComponent() {
    if (this.container) {
      this.destroyCurrentComponent();
      this.componentRef = this.factoryServices.createDynamicComponent(
        this.componentType,
        this.container,
        this.data,
        this.injector
      );
    }
  }

  private destroyCurrentComponent() {
    if (this.componentRef) {
      this.factoryServices.cleanupComponent(this.componentRef.instance as object);
      this.componentRef.destroy();
      this.componentRef = undefined;
    }
  }
}
