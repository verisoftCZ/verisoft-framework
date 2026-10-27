# @verisoft/store

NgRx-based state management library for Verisoft Angular applications, providing standardized patterns for handling application state, including table/list views, detail forms, and entity relationships.

## Installation

```bash
npm install @verisoft/store
```

## Features

- **Detail State Management**: Handle entity detail forms with validation and persistence
- **Table State Management**: Manage paginated lists with filtering, sorting, and selection
- **Binding State Management**: Handle entity relationships and associations
- **Type-Safe**: Full TypeScript support with generic types
- **NgRx Integration**: Built on @ngrx/store and @ngrx/effects

## State Modules

### Detail State

Manages individual entity details, forms, and CRUD operations.

```typescript
import { 
  createDetailReducers,
  createInitDetailAction,
  createLoadDetailSuccessAction,
  createSaveDetailAction,
  DetailState 
} from '@verisoft/store';

// State interface
interface UserDetailState extends DetailState<User> {
  // Additional state properties if needed
}

// Create reducer
const userDetailReducer = createDetailReducers<User>(
  'userDetail', // Repository name
  INITIAL_USER_DETAIL_STATE
);

// Actions usage
export class UserDetailComponent {
  constructor(private store: Store) {}

  loadUser(id: string) {
    this.store.dispatch(
      createInitDetailAction('userDetail')({ obj: id })
    );
  }

  saveUser() {
    this.store.dispatch(
      createSaveDetailAction('userDetail')()
    );
  }
}
```

### Table State

Manages paginated lists with filtering, sorting, and item selection.

```typescript
import { 
  createTablePageReducers,
  createGetPageTableAction,
  createFilterPageTableAction,
  TableState 
} from '@verisoft/store';
import { RequestParams } from '@verisoft/core';

// State interface
interface UserTableState extends TableState<User> {
  // Additional state properties if needed
}

// Create reducer
const userTableReducer = createTablePageReducers<User>(
  'userTable', // Repository name
  INITIAL_USER_TABLE_STATE
);

// Actions usage
export class UserListComponent {
  constructor(private store: Store) {}

  loadPage(page: number) {
    this.store.dispatch(
      createGetPageTableAction('userTable')({ 
        page,
        filter: this.currentFilter,
        sort: this.currentSort 
      })
    );
  }

  applyFilter(filter: UserFilter) {
    this.store.dispatch(
      createFilterPageTableAction('userTable')({ filter })
    );
  }

  selectItems(selectedItems: User[]) {
    this.store.dispatch(
      createSelectItemsTableAction('userTable')({ selectedItems })
    );
  }
}
```

### Binding State

Manages relationships between entities (e.g., user roles, project members).

```typescript
import { 
  createBindingsEffects,
  createAddBindingAction,
  createEditBindingAction,
  createDeleteBindingAction 
} from '@verisoft/store';

// Usage in effects
@Injectable()
export class UserRoleEffects {
  constructor(
    private actions$: Actions,
    private userRoleService: UserRoleService,
    private store: Store
  ) {}

  // Create binding effects
  userRoleBindings$ = createBindingsEffects(
    'userRoles',           // Repository name
    this.actions$,         // Actions observable
    this.userDetail$,      // Parent entity observable
    'role',                // Singular name
    this.userRoleService.addRoles.bind(this.userRoleService),
    this.userRoleService.deleteRoles.bind(this.userRoleService),
    this.snackBar,
    this.translateService,
    this.tableService
  );
}
```

## Complete Example

### Setting up Feature State

```typescript
import { createFeatureSelector, createSelector } from '@ngrx/store';
import { 
  DetailState, 
  TableState,
  createDetailReducers,
  createTablePageReducers 
} from '@verisoft/store';

// Feature state interface
interface UserFeatureState {
  userDetail: DetailState<User>;
  userTable: TableState<User>;
}

// Reducers
const userDetailReducer = createDetailReducers<User>('userDetail');
const userTableReducer = createTablePageReducers<User>('userTable');

// Feature selector
const selectUserFeature = createFeatureSelector<UserFeatureState>('users');

// Selectors
export const selectUserDetail = createSelector(
  selectUserFeature,
  state => state.userDetail
);

export const selectUserTable = createSelector(
  selectUserFeature,
  state => state.userTable
);

export const selectCurrentUser = createSelector(
  selectUserDetail,
  state => state.item
);

export const selectUserList = createSelector(
  selectUserTable,
  state => state.gPage?.data || []
);
```

### Component Integration

```typescript
import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { 
  createInitDetailAction,
  createSaveDetailAction,
  createGetPageTableAction 
} from '@verisoft/store';

@Component({
  template: `
    <div *ngIf="user$ | async as user">
      <form [formGroup]="userForm" (ngSubmit)="saveUser()">
        <!-- Form controls -->
      </form>
    </div>
    
    <div *ngIf="users$ | async as users">
      <table>
        <tr *ngFor="let user of users">
          <td>{{ user.name }}</td>
          <td>{{ user.email }}</td>
        </tr>
      </table>
    </div>
  `
})
export class UserManagementComponent implements OnInit {
  user$ = this.store.select(selectCurrentUser);
  users$ = this.store.select(selectUserList);
  loading$ = this.store.select(selectUserTable).pipe(
    map(state => state.dataLoading)
  );

  constructor(private store: Store) {}

  ngOnInit() {
    this.loadUsers();
  }

  loadUsers() {
    this.store.dispatch(
      createGetPageTableAction('userTable')({ 
        page: 1,
        size: 20 
      })
    );
  }

  loadUser(id: string) {
    this.store.dispatch(
      createInitDetailAction('userDetail')({ obj: id })
    );
  }

  saveUser() {
    this.store.dispatch(
      createSaveDetailAction('userDetail')()
    );
  }
}
```

### Form Integration with DetailStore Directive

```typescript
// Component using the directive
@Component({
  template: `
    <form 
      v-baseForm 
      #userForm="baseForm"
      v-useDetailStore
      [form]="userForm"
      detailsRepository="userDetail"
      ngrxFeatureKey="users"
      [detailId]="userId">
      
      <input 
        type="text" 
        formControlName="name" 
        placeholder="User Name">
      
      <input 
        type="email" 
        formControlName="email" 
        placeholder="Email">
      
      <button type="submit" [disabled]="!userForm.valid">
        Save User
      </button>
    </form>
  `
})
export class UserFormComponent {
  @Input() userId?: string;
}
```

## State Models

### DetailState Interface

```typescript
interface DetailState<T> {
  loaded: boolean;                    // Data load status
  item?: T;                          // Current entity
  formState?: FormState;             // Form validation state
  error?: string | null;             // Error message
  saveItemState: SaveItemState;      // Save operation state
  backendValidationErrors: BackendValidationError[]; // Server validation
}

interface FormState {
  dirty: boolean;   // Form has changes
  valid: boolean;   // Form is valid
}

interface SaveItemState {
  saveInProgress: boolean;  // Save operation in progress
  error?: string | null;    // Save error
}
```

### TableState Interface

```typescript
interface TableState<T> {
  dataLoading: boolean;           // Data loading status
  requestParams: RequestParams<T>; // Current request parameters
  gPage?: Page<T>;               // Paginated data
  error?: string | null;         // Error message
  selectedItems?: T[];           // Selected table items
}
```

## Available Actions

### Detail Actions

```typescript
// Initialize detail (load or create new)
createInitDetailAction(repository)({ obj: id })
createInitNewDetailAction(repository)()

// Data operations
createLoadDetailSuccessAction(repository)({ item })
createLoadDetailFailureAction(repository)({ error })

// Form operations
createUpdateDetailAction(repository)({ item })
createUpdateFormStateAction(repository)({ formState })

// Save operations
createSaveDetailAction(repository)()
createSaveDetailSuccessAction(repository)({ item })
createSaveDetailFailureAction(repository)({ error })

// State management
createResetStateAction(repository)()
```

### Table Actions

```typescript
// Data loading
createGetPageTableAction(repository)({ page, filter, sort })
createDataLoadSuccessTableAction(repository)({ gPage })
createDataLoadErrorTableAction(repository)({ error })

// Filtering and sorting
createFilterPageTableAction(repository)({ filter })
createStaticFilterTableAction(repository)({ filter })
createResetTableFilterAction(repository)()

// Selection and pagination
createSelectItemsTableAction(repository)({ selectedItems })
createChangePageSizeTableAction(repository)({ size })

// State management
createDestroyTableAction(repository)()
```

### Binding Actions

```typescript
// Binding operations
createAddBindingAction(repository)({ bindings })
createEditBindingAction(repository)({ binding })
createDeleteBindingAction(repository)({ bindingIds })

// Success/failure actions
createBindingModifiedSuccessAction(repository)()
createBindingModifiedFailureAction(repository)({ error })
```

## Best Practices

1. **Repository Naming**: Use consistent repository names across your application
2. **State Structure**: Keep state normalized and avoid deep nesting
3. **Selectors**: Create memoized selectors for computed state
4. **Effects**: Handle side effects (API calls) in NgRx effects
5. **Error Handling**: Implement proper error handling for all operations
6. **Type Safety**: Use TypeScript generics for type-safe state management

## Dependencies

- `@ngrx/store`
- `@ngrx/effects`
- `@verisoft/core` (for HTTP models)

## Contributing

This library is part of the Verisoft framework ecosystem. Follow the established patterns and ensure compatibility with other @verisoft packages.

## Running unit tests

Run `nx test store` to execute the unit tests.

This library was generated with [Nx](https://nx.dev).

## Running unit tests

Run `nx test store` to execute the unit tests.
