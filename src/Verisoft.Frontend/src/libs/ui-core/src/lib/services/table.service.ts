import { Injectable, signal } from "@angular/core";
import { TableSignal } from "../components";

@Injectable({
  providedIn: 'root'
})
export class TableService {
  // null = untouched since app start, distinct from a real forceReload() broadcast (bare Symbol()).
  readonly reload = signal<TableSignal | symbol | null>(null)

  /**
   * If name is set, reload a specific table with the set`[tableName]`
   *
   * If name is NOT set, reload all tables in the current DOM
   * */
  forceReload(name?: string): void {
    if (name) {
      this.reload.set({ name: name, symbol: Symbol() });
    } else {
      this.reload.set(Symbol());
    }
  }
}
