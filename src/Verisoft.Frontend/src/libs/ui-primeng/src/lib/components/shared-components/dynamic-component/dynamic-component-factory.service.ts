import {
  Injectable,
  ViewContainerRef,
  Type,
  Injector,
  ComponentRef,
  reflectComponentType,
} from '@angular/core';
import { ExtendedComponent } from '@verisoft/ui-core';
import { Subscription } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class DynamicComponentFactoryService {
  private readonly outputSubscriptions = new WeakMap<
    object,
    Map<string, Subscription>
  >();

  createDynamicComponent<TComponent>(
    componentType: Type<TComponent>,
    viewContainerRef: ViewContainerRef,
    inputs: ExtendedComponent<TComponent>,
    injector: Injector | undefined = undefined,
  ) {
    viewContainerRef.clear();
    const component = viewContainerRef.createComponent(componentType, {
      injector,
    });
    this.setComponentDataInt(componentType, component, inputs);
    this.fireComponentEvents(
      componentType,
      component.instance as TComponent & object,
      inputs,
    );
    return component;
  }

  cleanupComponent(instance: object): void {
    this.unsubscribeComponentEvents(instance);
  }

  setComponentData<TComponent>(
    component: ComponentRef<TComponent>,
    inputs: Partial<TComponent>,
  ) {
    this.setComponentDataInt(component.componentType, component, inputs);
  }

  private unsubscribeComponentEvents<TComponent>(instance: TComponent) {
    const subscriptions = this.outputSubscriptions.get(instance as object);

    if (subscriptions) {
      subscriptions.forEach((sub) => sub.unsubscribe());
      subscriptions.clear();
      this.outputSubscriptions.delete(instance as object);
    }
  }

  private fireComponentEvents<TComponent>(
    componentType: Type<TComponent>,
    instance: TComponent & object,
    inputs: ExtendedComponent<TComponent>,
  ) {
    if (!instance || typeof instance !== 'object') return;

    this.fireOutputComponentEvents(componentType, instance, inputs);
  }

  private setComponentDataInt<TComponent>(
    componentType: Type<TComponent>,
    component: ComponentRef<TComponent>,
    inputs: Partial<TComponent>,
  ) {
    if (inputs) {
      const propertyNames = this.getInputNames(componentType);
      const inputsHash = new Set(propertyNames);
      Object.keys(inputs)
        .filter((x) => inputsHash.has(x))
        .forEach((x) => {
          component.setInput(x, (<{ [key: string]: unknown }>inputs)[x]);
        });
    }
  }

  private fireOutputComponentEvents<TComponent>(
    componentType: Type<TComponent>,
    instance: TComponent & object,
    inputs: ExtendedComponent<TComponent>,
  ) {
    const outputNames = new Set(this.getOutputNames(componentType));
    const outputs = Object.keys(inputs ?? {}).filter((key) =>
      outputNames.has(key),
    );

    let subscriptions = this.outputSubscriptions.get(instance);
    if (!subscriptions) {
      subscriptions = new Map<string, Subscription>();
      this.outputSubscriptions.set(instance, subscriptions);
    }

    for (const outputKey of outputs) {
      const outputRef = (instance as Record<string, unknown>)[outputKey] as {
        subscribe?: (listener: (value: unknown) => void) => Subscription;
      };
      const callback = inputs[outputKey] as unknown;

      if (
        outputRef &&
        typeof outputRef.subscribe === 'function' &&
        typeof callback === 'function'
      ) {
        if (subscriptions.has(outputKey)) {
          subscriptions.get(outputKey)!.unsubscribe();
        }

        const subscription = outputRef.subscribe((value: unknown) =>
          (callback as (value: unknown) => void)(value),
        );
        subscriptions.set(outputKey, subscription);
      }
    }
  }

  private getInputNames<TComponent>(componentType: Type<TComponent>) {
    return (
      reflectComponentType(componentType)?.inputs.map(
        (input: { propName: string }) => input.propName,
      ) ?? []
    );
  }

  private getOutputNames<TComponent>(componentType: Type<TComponent>) {
    return (
      reflectComponentType(componentType)?.outputs.map(
        (output: { propName: string }) => output.propName,
      ) ?? []
    );
  }
}
