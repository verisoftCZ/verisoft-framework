# @verisoft/ui-govcz

A comprehensive Angular UI component library that provides components specifically designed to comply with Czech Government Design System (gov.cz) standards. This library offers a complete set of form controls, navigation components, and layout elements that follow accessibility guidelines and government design standards.

## Overview

The `@verisoft/ui-govcz` library is built on top of `@verisoft/ui-core` and provides specialized components for Czech government applications. It includes custom styling, icons, and behavior patterns that align with official gov.cz design specifications.

## Features

### Components
- **Form Controls**: Input fields, dropdowns, checkboxes, radio buttons, switches, calendars
- **Navigation**: Breadcrumbs, side menu, tabs, stepper components
- **Data Display**: Tables with filtering and sorting, tooltips
- **Feedback**: Snackbars, confirm dialogs, error handling, loaders
- **Layout**: Headers, page headers, sections, button groups
- **Specialized**: Search components, multiselect, password fields

### Key Features
- ✅ **Gov.cz Compliance**: All components follow Czech government design standards
- ✅ **Accessibility**: WCAG 2.1 compliant components
- ✅ **Form Integration**: Seamless integration with Angular Reactive Forms
- ✅ **Security**: Built-in security features via `@verisoft/security-core`
- ✅ **State Management**: Integration with `@verisoft/store`
- ✅ **Customizable**: Flexible theming and styling options

## Installation

```bash
npm install @verisoft/ui-govcz
```

### Peer Dependencies

Make sure you have the following peer dependencies installed:

```bash
npm install @verisoft/ui-core @verisoft/core @verisoft/security-core @angular/core @angular/common @angular/forms
```

## Quick Start

1. **Import individual components** in your Angular application:

```typescript
import { 
  ButtonComponent, 
  TextfieldComponent, 
  FormFieldComponent 
} from '@verisoft/ui-govcz';

@Component({
  imports: [ButtonComponent, TextfieldComponent, FormFieldComponent],
  // ...
})
export class MyComponent { }
```

2. **Use components** in your templates:

```html
<v-button [primary]="true">Government Action</v-button>
<v-form-field label="Official Document">
  <v-textfield formControlName="document"></v-textfield>
</v-form-field>
```

## Usage Examples

### Basic Form
```typescript
// Component
import { FormBuilder, Validators } from '@angular/forms';
import { ButtonComponent, TextfieldComponent, FormFieldComponent } from '@verisoft/ui-govcz';

@Component({
  imports: [ButtonComponent, TextfieldComponent, FormFieldComponent],
  // ...
})
export class MyFormComponent {
  form = this.fb.group({
    name: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]]
  });

  constructor(private fb: FormBuilder) {}
}
```

```html
<!-- Template -->
<form [formGroup]="form">
  <v-form-field label="Full Name" [required]="true">
    <v-textfield formControlName="name"></v-textfield>
  </v-form-field>
  
  <v-form-field label="Email Address" [required]="true">
    <v-textfield formControlName="email" type="email"></v-textfield>
  </v-form-field>
  
  <v-button [primary]="true" type="submit">Submit</v-button>
</form>
```

### Data Table
```html
<v-table 
  [data]="tableData" 
  [columns]="columns"
  [sortable]="true"
  [filterable]="true">
</v-table>
```

## API Documentation

For detailed component APIs, properties, and methods, please refer to the component documentation or use your IDE's IntelliSense for inline documentation.

## Development

### Running unit tests

Run `nx test ui-govcz` to execute the unit tests.

### Building the library

Run `nx build ui-govcz` to build the library.

### Linting

Run `nx lint ui-govcz` to run ESLint on the library.

## Contributing

This library is part of the Verisoft framework. Please follow the established coding standards and ensure all components maintain gov.cz design compliance.

## License

Copyright © Verisoft. All rights reserved.
