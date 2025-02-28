import { Observable } from "rxjs";

export interface NotificableProperty {
  propertyChanged: Observable<unknown>;
}
