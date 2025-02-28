import {
  Injectable,
  ViewContainerRef,
  Type,
  ComponentFactoryResolver,
  Injector,
  ComponentRef,
  ComponentFactory,
  OnInit,
  OnChanges,
  SimpleChanges,
  SimpleChange,
} from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class DynamicComponentFactoryService {
  constructor(private componentFactoryResolver: ComponentFactoryResolver) {}

  async createDynamicComponent<TComponent>(
    componentType: Type<TComponent>,
    viewContainerRef: ViewContainerRef,
    inputs: Partial<TComponent>,
    injector: Injector | undefined = undefined
  ) {
    const componentFactory =
      this.componentFactoryResolver.resolveComponentFactory(componentType);
    viewContainerRef.clear();
    const component = viewContainerRef.createComponent(
      componentFactory,
      undefined,
      injector
    );

    this.setComponentDataInt(componentFactory, component, inputs);
    this.fireComponentEvents(component.instance, inputs);
    return component;
  }

  setComponentData<TComponent>(
    component: ComponentRef<TComponent>,
    inputs: Partial<TComponent>
  ) {
    const factory = this.componentFactoryResolver.resolveComponentFactory(
      component.componentType
    );

    this.setComponentDataInt(factory, component, inputs);
  }

  private fireComponentEvents<TComponent>(instance: TComponent, inputs: Partial<TComponent>) {
    if (instance) {
      const onChangeComponent = instance as unknown as OnChanges;
      if (onChangeComponent.ngOnChanges && inputs) {
        const changeEventArgs = Object.keys(inputs).reduce((changes: SimpleChanges, key: string) => {
          const inputValue = (inputs as { [key: string]: unknown })[key];
          changes[key] = new SimpleChange(undefined, inputValue, true);
          return changes;
        }, {} as SimpleChanges);
        

        onChangeComponent.ngOnChanges(changeEventArgs);
      }

      const onInitComponent = instance as unknown as OnInit;
      if (onInitComponent.ngOnInit) {
        onInitComponent.ngOnInit();
      }
    }
  }

  private setComponentDataInt<TComponent>(
    factory: ComponentFactory<TComponent>,
    component: ComponentRef<TComponent>,
    inputs: Partial<TComponent>
  ) {
    if (inputs) {
      const propertyNames = factory.inputs.map((x) => x.propName);
      const inputsHash = new Set(propertyNames);
      Object.keys(inputs)
        .filter((x) => inputsHash.has(x))
        .forEach((x) => {
          (<{ [key: string]: unknown }>component.instance)[x] = (<
            { [key: string]: unknown }
          >inputs)[x];
        });
    }
  }
}
