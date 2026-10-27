# @verisoft/ui-primeng

A comprehensive Angular UI component library built on top of PrimeNG that provides enterprise-grade components with modern design and extensive functionality. This library combines the power of PrimeNG with Verisoft's design patterns to deliver consistent, feature-rich user interfaces for business applications.

## Overview

The `@verisoft/ui-primeng` library extends `@verisoft/ui-core` and leverages PrimeNG components to provide a complete set of UI components suitable for enterprise applications. It offers advanced data components, rich form controls, and sophisticated layout elements with built-in theming and internationalization support.

## Features

### Components
- **Form Controls**: Advanced text fields, dropdowns, checkboxes, radio buttons, calendars, sliders
- **Data Components**: Feature-rich tables with pagination, sorting, filtering, and export
- **Navigation**: Breadcrumbs, side menus, tabs, stepper components with progress tracking
- **Input Groups**: Complex input combinations with buttons, icons, and validation
- **Feedback**: Toast notifications, confirm dialogs, loading indicators, tooltips
- **Layout**: Headers, page headers, sections with collapsible content
- **Advanced**: Multi-select with search, number inputs with formatting, password strength

### Key Features
- ✅ **PrimeNG Integration**: Built on top of the mature PrimeNG component library
- ✅ **Enterprise Ready**: Components designed for complex business applications
- ✅ **Rich Data Handling**: Advanced table features with virtual scrolling and lazy loading
- ✅ **Theming Support**: Multiple built-in themes with customization options
- ✅ **Internationalization**: Full i18n support with @ngx-translate integration
- ✅ **Accessibility**: WCAG 2.1 compliant with keyboard navigation support
- ✅ **Form Integration**: Deep Angular Reactive Forms integration with validation
- ✅ **Performance**: Optimized for large datasets and complex UIs

## Installation

```bash
npm install @verisoft/ui-primeng
```

### Peer Dependencies

Make sure you have the following peer dependencies installed:

```bash
npm install @verisoft/core @verisoft/ui-core primeng @angular/core @angular/common @angular/forms @angular/platform-browser @angular/router rxjs @ngx-translate/core uuid
```

### Styling Setup

Add PrimeNG CSS theme to your `angular.json` or import in your global styles:

```json
// angular.json
"styles": [
  "node_modules/primeng/resources/themes/saga-blue/theme.css",
  "node_modules/primeng/resources/primeng.min.css",
  "node_modules/primeicons/primeicons.css"
]
```

Or in your global `styles.scss`:

```scss
@import 'primeng/resources/themes/saga-blue/theme.css';
@import 'primeng/resources/primeng.min.css';
@import 'primeicons/primeicons.css';
```

## Quick Start

1. **Import individual components** in your Angular application:

```typescript
import { 
  ButtonComponent, 
  TextfieldComponent, 
  FormFieldComponent 
} from '@verisoft/ui-primeng';

@Component({
  imports: [ButtonComponent, TextfieldComponent, FormFieldComponent],
  // ...
})
export class MyComponent { }
```

2. **Use components** in your templates:

```html
<v-button [primary]="true" icon="pi pi-check">Save Changes</v-button>
<v-form-field label="Business Email">
  <v-textfield formControlName="email" placeholder="Enter email address"></v-textfield>
</v-form-field>
```

## Usage Examples

### Advanced Form with Validation
```typescript
// Component
import { FormBuilder, Validators } from '@angular/forms';
import { 
  ButtonComponent, 
  TextfieldComponent, 
  DropdownComponent, 
  NumberInputComponent, 
  CalendarComponent, 
  MultiselectComponent, 
  TextareaComponent, 
  FormFieldComponent 
} from '@verisoft/ui-primeng';

@Component({
  imports: [
    ButtonComponent, 
    TextfieldComponent, 
    DropdownComponent, 
    NumberInputComponent, 
    CalendarComponent, 
    MultiselectComponent, 
    TextareaComponent, 
    FormFieldComponent
  ],
  // ...
})
export class BusinessFormComponent {
  form = this.fb.group({
    companyName: ['', [Validators.required, Validators.minLength(3)]],
    industry: ['', Validators.required],
    employees: [null, [Validators.required, Validators.min(1)]],
    foundedDate: [null, Validators.required],
    features: [[]],
    description: ['', Validators.maxLength(500)]
  });

  industries = [
    { label: 'Technology', value: 'tech' },
    { label: 'Healthcare', value: 'health' },
    { label: 'Finance', value: 'finance' }
  ];

  features = [
    { label: 'Online Payments', value: 'payments' },
    { label: 'Customer Support', value: 'support' },
    { label: 'Analytics', value: 'analytics' }
  ];

  constructor(private fb: FormBuilder) {}
}
```

```html
<!-- Template -->
<form [formGroup]="form">
  <v-form-field label="Company Name" [required]="true">
    <v-textfield formControlName="companyName" placeholder="Enter company name"></v-textfield>
  </v-form-field>
  
  <v-form-field label="Industry" [required]="true">
    <v-dropdown formControlName="industry" [options]="industries" placeholder="Select industry"></v-dropdown>
  </v-form-field>
  
  <v-form-field label="Number of Employees" [required]="true">
    <v-number-input formControlName="employees" [min]="1" [max]="10000"></v-number-input>
  </v-form-field>
  
  <v-form-field label="Founded Date" [required]="true">
    <v-calendar formControlName="foundedDate" [showIcon]="true"></v-calendar>
  </v-form-field>
  
  <v-form-field label="Required Features">
    <v-multiselect formControlName="features" [options]="features" [filter]="true"></v-multiselect>
  </v-form-field>
  
  <v-form-field label="Description">
    <v-textarea formControlName="description" [rows]="4" [maxLength]="500"></v-textarea>
  </v-form-field>
  
  <v-button [primary]="true" type="submit" [disabled]="form.invalid">Submit Application</v-button>
</form>
```

### Advanced Data Table
```typescript
// Component
import { TableComponent, PageHeaderComponent, ButtonComponent } from '@verisoft/ui-primeng';

@Component({
  imports: [TableComponent, PageHeaderComponent, ButtonComponent],
  // ...
})
export class DataManagementComponent {
  employees = [
    { id: 1, name: 'John Doe', department: 'IT', salary: 75000, hireDate: new Date('2020-01-15') },
    { id: 2, name: 'Jane Smith', department: 'HR', salary: 68000, hireDate: new Date('2019-03-20') }
  ];

  columns = [
    { field: 'name', header: 'Employee Name', sortable: true, filter: true },
    { field: 'department', header: 'Department', sortable: true, filter: true },
    { field: 'salary', header: 'Salary', sortable: true, filter: true, type: 'currency' },
    { field: 'hireDate', header: 'Hire Date', sortable: true, filter: true, type: 'date' }
  ];

  exportColumns = this.columns.map(col => ({ title: col.header, dataKey: col.field }));

  exportPdf() {
    // Export functionality
  }

  exportExcel() {
    // Export functionality
  }
}
```

```html
<!-- Template -->
<v-page-header title="Employee Management">
  <v-button icon="pi pi-download" label="Export PDF" (click)="exportPdf()"></v-button>
  <v-button icon="pi pi-file-excel" label="Export Excel" (click)="exportExcel()"></v-button>
</v-page-header>

<v-table 
  [data]="employees" 
  [columns]="columns"
  [sortable]="true"
  [filterable]="true"
  [pagination]="true"
  [rows]="10"
  [exportFilename]="'employees'"
  [globalFilterFields]="['name', 'department']">
  
  <ng-template pTemplate="caption">
    <div class="table-header">
      <span>Employee Directory</span>
      <span class="p-input-icon-left">
        <i class="pi pi-search"></i>
        <input pInputText type="text" (input)="table.filterGlobal($event.target.value, 'contains')" placeholder="Global Search" />
      </span>
    </div>
  </ng-template>
</v-table>
```

### Stepper Component for Multi-Step Process
```typescript
// Component
import { StepperComponent, ButtonComponent } from '@verisoft/ui-primeng';

@Component({
  imports: [StepperComponent, ButtonComponent],
  // ...
})
export class OnboardingComponent {
  activeIndex = 0;
  
  steps = [
    { label: 'Personal Info' },
    { label: 'Company Details' },
    { label: 'Preferences' },
    { label: 'Confirmation' }
  ];

  nextStep() {
    if (this.activeIndex < this.steps.length - 1) {
      this.activeIndex++;
    }
  }

  prevStep() {
    if (this.activeIndex > 0) {
      this.activeIndex--;
    }
  }
}
```

```html
<!-- Template -->
<v-stepper [(activeIndex)]="activeIndex" [steps]="steps">
  <v-step>
    <!-- Personal Info Form -->
    <h3>Personal Information</h3>
    <!-- Form fields -->
  </v-step>
  
  <v-step>
    <!-- Company Details Form -->
    <h3>Company Details</h3>
    <!-- Form fields -->
  </v-step>
  
  <v-step>
    <!-- Preferences Form -->
    <h3>Preferences</h3>
    <!-- Form fields -->
  </v-step>
  
  <v-step>
    <!-- Confirmation -->
    <h3>Review & Confirm</h3>
    <!-- Summary -->
  </v-step>
</v-stepper>

<div class="step-actions">
  <v-button label="Back" (click)="prevStep()" [disabled]="activeIndex === 0"></v-button>
  <v-button label="Next" (click)="nextStep()" [disabled]="activeIndex === steps.length - 1" [primary]="true"></v-button>
</div>
```

## Theming

The library supports multiple PrimeNG themes. You can switch themes by changing the CSS import:

```scss
// Light themes
@import 'primeng/resources/themes/saga-blue/theme.css';      // Default
@import 'primeng/resources/themes/saga-green/theme.css';
@import 'primeng/resources/themes/saga-orange/theme.css';

// Dark themes
@import 'primeng/resources/themes/vela-blue/theme.css';
@import 'primeng/resources/themes/vela-green/theme.css';
@import 'primeng/resources/themes/arya-blue/theme.css';
```

## Internationalization

The library includes built-in support for internationalization:

```typescript
// app.module.ts
import { TranslateModule } from '@ngx-translate/core';

@NgModule({
  imports: [
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient]
      }
    })
  ]
})
export class AppModule { }
```

## API Documentation

For detailed component APIs, properties, and methods, please refer to:
- Component documentation with IntelliSense
- [PrimeNG Documentation](https://primeng.org/) for underlying component features
- Verisoft component extensions and customizations

## Development

### Running unit tests

Run `nx test ui-primeng` to execute the unit tests.

### Building the library

Run `nx build ui-primeng` to build the library.

### Linting

Run `nx lint ui-primeng` to run ESLint on the library.

## Performance Tips

- Use virtual scrolling for large datasets in tables
- Implement lazy loading for dropdowns with many options
- Use OnPush change detection strategy for better performance
- Consider pagination for tables with extensive data

## Contributing

This library is part of the Verisoft framework. Please follow the established coding standards and ensure compatibility with PrimeNG updates.

## License

Copyright © Verisoft. All rights reserved.
