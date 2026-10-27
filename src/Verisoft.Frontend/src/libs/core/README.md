# @verisoft/core

Core utilities and HTTP services for Verisoft Angular applications, providing foundational functionality for data access, HTTP communication, and utility operations.

## Installation

```bash
npm install @verisoft/core
```

## Features

- **BaseHttpService**: Generic HTTP service with CRUD operations
- **HTTP Models**: Type-safe request/response models
- **Utility Functions**: Common operations for arrays, objects, dates, and data manipulation
- **Storage Services**: Local storage abstraction
- **Error Handling**: Centralized error provider service

## Core Services

### BaseHttpService

Generic HTTP service providing standardized CRUD operations for REST APIs.

```typescript
import { Injectable } from '@angular/core';
import { BaseHttpService } from '@verisoft/core';

@Injectable({ providedIn: 'root' })
export class UserService extends BaseHttpService<User> {
  constructor() {
    super('users'); // Base endpoint path
  }
}

// Usage
export class UserComponent {
  constructor(private userService: UserService) {}

  loadUsers() {
    this.userService.fetchList({ page: 1, size: 10 })
      .subscribe(page => {
        console.log(page.data); // User[]
        console.log(page.total); // Total count
      });
  }

  getUser(id: number) {
    this.userService.get(id)
      .subscribe(user => console.log(user));
  }

  createUser(user: CreateUserDto) {
    this.userService.post(user)
      .subscribe(created => console.log(created));
  }

  updateUser(id: number, user: UpdateUserDto) {
    this.userService.put(id, user)
      .subscribe(updated => console.log(updated));
  }

  deleteUser(id: number) {
    this.userService.delete(id)
      .subscribe(() => console.log('Deleted'));
  }
}
```

### HTTP Models

Type-safe models for API communication:

```typescript
import { RequestParams, Page, SortDirection } from '@verisoft/core';

interface UserFilter {
  name?: string;
  email?: string;
  active?: boolean;
}

// Request parameters with filtering and sorting
const params: RequestParams<UserFilter> = {
  page: 1,
  size: 20,
  filter: {
    active: true,
    name: 'John'
  },
  sort: {
    field: 'createdAt',
    direction: SortDirection.DESC
  }
};

// Response pagination
interface UserPage extends Page<User> {
  data: User[];
  total: number;
  size: number;
}
```

### Utility Functions

Common utility operations for data manipulation:

```typescript
import { 
  ArrayUtils, 
  ObjectUtils, 
  DateUtils, 
  DataUtils, 
  ClearUtils 
} from '@verisoft/core';

// Array utilities
const uniqueItems = ArrayUtils.unique([1, 2, 2, 3]);
const groupedData = ArrayUtils.groupBy(users, 'department');

// Object utilities
const merged = ObjectUtils.merge(obj1, obj2);
const cloned = ObjectUtils.deepClone(originalObject);

// Date utilities
const formatted = DateUtils.format(new Date(), 'DD/MM/YYYY');
const isValid = DateUtils.isValid('2023-12-31');

// Data utilities
const filtered = DataUtils.filterBy(items, 'status', 'active');
const sorted = DataUtils.sortBy(items, 'name');

// Clear utilities
const cleaned = ClearUtils.removeEmpty(objectWithNulls);
```

### Storage Services

Local storage abstraction with type safety:

```typescript
import { LocalStorageService } from '@verisoft/core';

export class SettingsService {
  constructor(private storage: LocalStorageService) {}

  saveUserPreferences(prefs: UserPreferences) {
    this.storage.setItem('userPrefs', prefs);
  }

  getUserPreferences(): UserPreferences | null {
    return this.storage.getItem<UserPreferences>('userPrefs');
  }

  clearPreferences() {
    this.storage.removeItem('userPrefs');
  }
}
```

### Error Handling

Centralized error provider for consistent error management:

```typescript
import { ErrorProviderService } from '@verisoft/core';

@Component({...})
export class MyComponent {
  constructor(private errorProvider: ErrorProviderService) {}

  handleApiError(error: any) {
    this.errorProvider.handleError(error);
    // Automatically handles common HTTP errors
  }
}
```

## Configuration

### Base URL Configuration

Configure the base URL for HTTP services:

```typescript
import { BASE_URL_PATH } from '@verisoft/core';

@NgModule({
  providers: [
    {
      provide: BASE_URL_PATH,
      useValue: 'https://api.example.com/v1'
    }
  ]
})
export class AppModule {}
```

### Service Configuration

Customize BaseHttpService behavior:

```typescript
@Injectable()
export class CustomUserService extends BaseHttpService<User> {
  constructor() {
    super('users', {
      timeout: 10000,
      retries: 3,
      headers: {
        'Custom-Header': 'value'
      }
    });
  }

  // Override default methods
  protected override handleError(error: any) {
    // Custom error handling
    return super.handleError(error);
  }
}
```

## Advanced Usage

### Custom Request Parameters

Extend request parameters for specific needs:

```typescript
interface CustomRequestParams<T> extends RequestParams<T> {
  includeDeleted?: boolean;
  expand?: string[];
}

class ExtendedService extends BaseHttpService<MyEntity> {
  fetchWithOptions(params: CustomRequestParams<MyFilter>) {
    return this.httpClient.get<Page<MyEntity>>(
      this.buildUrl('', params)
    );
  }
}
```

### Response Transformation

Transform API responses:

```typescript
class TransformingService extends BaseHttpService<User> {
  fetchList(params: RequestParams<UserFilter>) {
    return super.fetchList(params).pipe(
      map(page => ({
        ...page,
        data: page.data.map(user => ({
          ...user,
          fullName: `${user.firstName} ${user.lastName}`
        }))
      }))
    );
  }
}
```

## Best Practices

1. **Extend BaseHttpService**: Create specific services for each entity
2. **Use Type Safety**: Define proper interfaces for filters and DTOs
3. **Handle Errors**: Implement proper error handling strategies
4. **Cache Responses**: Consider implementing caching for frequently accessed data
5. **Test Services**: Write unit tests for service methods

## Dependencies

- `@angular/common/http`
- `rxjs`
- `moment` (for date utilities)

## Contributing

This library is part of the Verisoft framework ecosystem. Follow the established patterns and ensure compatibility with other @verisoft packages.

## Running unit tests

Run `nx test core` to execute the unit tests.
This library was generated with [Nx](https://nx.dev).

## Running unit tests

Run `nx test core` to execute the unit tests.
