import { Observable } from "rxjs";

export interface PreventUnsavedChangesCore {
  canDeactivate: () => Observable<boolean> | Promise<boolean> | boolean;
}
