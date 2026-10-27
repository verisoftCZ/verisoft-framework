# @verisoft/ui-core

A foundational Angular library that provides core UI functionalities, directives, interfaces, and base classes for building consistent user interfaces. This library serves as the architectural foundation for all UI component libraries within the Verisoft ecosystem, offering shared utilities, base components, and common interfaces without implementing concrete UI components.

## Overview

The `@verisoft/ui-core` library provides essential infrastructure and utilities that enable other UI libraries to build consistent, accessible, and well-structured components. It includes base classes, interfaces, directives, services, and utilities that standardize component behavior across the entire Verisoft UI ecosystem.

## Features

### Core Infrastructure
- **Base Classes**: Abstract base classes for form components, input controls, and lifecycle management
- **Component Interfaces**: TypeScript interfaces defining component contracts and APIs
- **Tokens & Providers**: Injection tokens for component registration and dependency injection
- **Type Definitions**: Comprehensive type definitions for component properties and events

### Directives
- **Form Directives**: Enhanced form behavior, validation helpers, and form state management
- **Utility Directives**: Common DOM manipulation, event handling, and accessibility helpers
- **Validation Directives**: Custom validators and validation feedback mechanisms

### Services & Utilities
- **Format Services**: Date, number, currency, and text formatting utilities
- **Screen Service**: Responsive design utilities and screen size detection
- **Menu Service**: Navigation and menu state management
- **Common Services**: Shared utilities for HTTP, storage, and application state

### Base Components
- **UnsubscribeComponent**: Automatic subscription cleanup and memory leak prevention
- **BaseFormComponent**: Foundation for all form-related functionality
- **BaseInputComponent**: Core input control behavior and validation integration

### Key Features
- ✅ **Architecture Foundation**: Provides the structural foundation for all UI libraries
- ✅ **Type Safety**: Full TypeScript support with comprehensive type definitions
- ✅ **Extensibility**: Designed to be extended by specialized UI component libraries
- ✅ **Consistency**: Ensures consistent behavior across all UI components
- ✅ **Performance**: Optimized base classes with OnPush change detection support
- ✅ **Accessibility**: Built-in accessibility patterns and ARIA support
- ✅ **Form Integration**: Deep Angular Reactive Forms integration and validation
- ✅ **State Management**: Integration with `@verisoft/store` for application state

## Installation

```bash
npm install @verisoft/ui-core
```

### Peer Dependencies

Make sure you have the following peer dependencies installed:

```bash
npm install @verisoft/core @verisoft/store @angular/core @angular/common @angular/forms @angular/platform-browser
```

## Quick Start

1. **Import base classes and interfaces** in your component library:

```typescript
import { 
  BaseFormInputComponent, 
  ButtonCore, 
  TextfieldCore,
  BUTTON_COMPONENT_TOKEN 
} from '@verisoft/ui-core';

@Component({
  selector: 'my-button',
  providers: [{
    provide: BUTTON_COMPONENT_TOKEN,
    useExisting: MyButtonComponent
  }]
})
export class MyButtonComponent extends BaseFormInputComponent implements ButtonCore {
  // Component implementation
}
```

2. **Use services and utilities**:

```typescript
import { ScreenSizeService, FormatService } from '@verisoft/ui-core';

@Component({
  // ...
})
export class MyComponent {
  constructor(
    private screenService: ScreenSizeService,
    private formatService: FormatService
  ) {}

  get isMobile() {
    return this.screenService.isMobile();
  }

  formatCurrency(value: number) {
    return this.formatService.currency(value);
  }
}
```

## Usage Examples

### Creating a Custom Component with Base Classes
```typescript
import { 
  BaseFormInputComponent, 
  TextfieldCore, 
  TEXTFIELD_COMPONENT_TOKEN,
  FieldSizeType,
  FieldTypeType 
} from '@verisoft/ui-core';
import { Component, Input, Optional, Self } from '@angular/core';
import { ControlValueAccessor, NgControl } from '@angular/forms';

@Component({
  selector: 'custom-textfield',
  template: `
    <input 
      [type]="type"
      [value]="value"
      [disabled]="disabled"
      (input)="onInput($event)"
      (blur)="onBlur()"
      [class]="getInputClasses()"
    />
  `,
  providers: [{
    provide: TEXTFIELD_COMPONENT_TOKEN,
    useExisting: CustomTextfieldComponent
  }]
})
export class CustomTextfieldComponent 
  extends BaseFormInputComponent 
  implements ControlValueAccessor, TextfieldCore {

  @Input() type: FieldTypeType = 'text';
  @Input() size: FieldSizeType = 'medium';

  constructor(@Optional() @Self() ngControl: NgControl) {
    super(ngControl);
  }

  protected getInputClasses(): string {
    return `input input--${this.size} ${this.disabled ? 'input--disabled' : ''}`;
  }
}
```

### Using Core Services
```typescript
import { 
  ScreenSizeService, 
  SideMenuService, 
  FormatService 
} from '@verisoft/ui-core';

@Component({
  selector: 'my-responsive-component',
  template: `
    <div [class]="containerClass">
      <span>{{ formattedPrice }}</span>
      <button (click)="toggleMenu()" *ngIf="!isMobile">Menu</button>
    </div>
  `
})
export class ResponsiveComponent {
  constructor(
    private screenService: ScreenSizeService,
    private menuService: SideMenuService,
    private formatService: FormatService
  ) {}

  get isMobile(): boolean {
    return this.screenService.isMobile();
  }

  get containerClass(): string {
    return this.isMobile ? 'container-mobile' : 'container-desktop';
  }

  get formattedPrice(): string {
    return this.formatService.currency(99.99, 'EUR');
  }

  toggleMenu(): void {
    this.menuService.toggle();
  }
}
```

### Implementing Component Interfaces
```typescript
import { ButtonCore, IconPositionType } from '@verisoft/ui-core';

export class MyButtonComponent implements ButtonCore {
  @Input() primary: boolean = false;
  @Input() disabled: boolean = false;
  @Input() icon?: string;
  @Input() iconPosition: IconPositionType = 'left';
  @Input() loading: boolean = false;
  @Input() size: 'small' | 'medium' | 'large' = 'medium';

  // Implement required methods from ButtonCore interface
  onClick(): void {
    if (!this.disabled && !this.loading) {
      // Handle click
    }
  }
}
```

## Architecture

### Base Classes
- **BaseFormComponent**: Foundation for all form-related components with validation integration
- **BaseFormInputComponent**: Base class for form input controls with ControlValueAccessor implementation
- **UnsubscribeComponent**: Automatic subscription management and memory leak prevention
- **BaseInputControls**: Core input control behavior and common properties

### Component Interfaces
- **ButtonCore**: Interface defining button component contract
- **TextfieldCore**: Interface for text input components
- **DropdownCore**: Interface for dropdown/select components
- **TableCore**: Interface for data table components
- **FormFieldCore**: Interface for form field wrapper components

### Injection Tokens
- **BUTTON_COMPONENT_TOKEN**: Token for registering button implementations
- **TEXTFIELD_COMPONENT_TOKEN**: Token for registering textfield implementations
- **DROPDOWN_COMPONENT_TOKEN**: Token for registering dropdown implementations
- **TABLE_COMPONENT_TOKEN**: Token for registering table implementations

### Services
- **ScreenSizeService**: Responsive design utilities and breakpoint detection
- **SideMenuService**: Navigation menu state management
- **FormatService**: Formatting utilities for dates, numbers, and currencies
- **ValidationService**: Custom validation logic and error handling

### Type Definitions
- **FieldSizeType**: Standardized size options ('small' | 'medium' | 'large')
- **FieldTypeType**: Input field types ('text' | 'email' | 'password' | etc.)
- **IconPositionType**: Icon positioning ('left' | 'right')
- **ControlSeverityType**: Validation state types ('success' | 'warning' | 'error')

## API Documentation

For detailed component APIs, properties, and methods, please refer to the component documentation or use your IDE's IntelliSense for comprehensive inline documentation.

## API Documentation

For detailed APIs, interfaces, and base class documentation, please refer to:
- TypeScript definitions and IntelliSense in your IDE
- Base class documentation for extension patterns
- Interface definitions for component contracts
- Service documentation for utility functions

## Development

### Running unit tests

Run `nx test ui-core` to execute the unit tests.

### Building the library

Run `nx build ui-core` to build the library.

### Linting

Run `nx lint ui-core` to run ESLint on the library.

## Extending the Library

The `ui-core` library is designed to be extended by other UI component libraries. Here's how to create a new UI library:

### Creating a Component Library
```typescript
// 1. Extend base classes
import { BaseFormInputComponent, ButtonCore } from '@verisoft/ui-core';

export class MyButtonComponent extends BaseFormInputComponent implements ButtonCore {
  // Your implementation
}

// 2. Provide component tokens
@Component({
  providers: [{
    provide: BUTTON_COMPONENT_TOKEN,
    useExisting: MyButtonComponent
  }]
})

// 3. Use core services
constructor(
  private screenService: ScreenSizeService,
  private formatService: FormatService
) {
  super();
}
```

### Best Practices
- Always extend `BaseFormInputComponent` for form controls
- Implement the appropriate Core interface for your component type
- Use injection tokens to register your components
- Leverage core services for consistent behavior
- Follow the established type definitions and naming conventions

## Contributing

This library is part of the Verisoft framework. Please follow the established coding standards and architectural patterns when contributing.

## License

Copyright © Verisoft. All rights reserved.
