import { EventEmitter, Injectable, Output } from '@angular/core';
import { DialogData } from '../components';
@Injectable({
  providedIn: 'root',
})
export class DialogService {
  @Output() showEvent: EventEmitter<DialogData> =
    new EventEmitter<DialogData>();

  showDialog(data: DialogData): void {
    this.showEvent.emit(data);
  }
}
