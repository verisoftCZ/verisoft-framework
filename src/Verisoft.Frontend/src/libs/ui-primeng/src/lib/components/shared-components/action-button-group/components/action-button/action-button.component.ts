import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  inject,
  Input,
  OnChanges,
  Output,
  SimpleChanges,
} from '@angular/core';
import {
  ACTION_BUTTON_GROUP_COMPONENT_TOKEN,
  ActionButton,
  ActionButtonGroupCore,
  ControlSeverityType,
  FieldSizeType,
} from '@verisoft/ui-core';
import { TooltipModule } from 'primeng/tooltip';
import { Observable, Subject } from 'rxjs';
import { ButtonComponent } from '../../../../button';
import { Icons } from '../../../../../icons';

@Component({
  selector: 'v-action-button',
  standalone: true,
  imports: [ButtonComponent, TooltipModule],
  templateUrl: './action-button.component.html',
  styleUrl: './action-button.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ActionButtonComponent implements ActionButton, OnChanges {
  @Input() disabled = false;
  @Input() toolTip?: string;
  @Input() id?: string;
  @Input() icon?: string;
  @Input() outlined = false;
  @Input() raised = false;
  @Input() severity?: ControlSeverityType;
  @Input() label?: string;
  @Input() size?: FieldSizeType;
  // eslint-disable-next-line @angular-eslint/no-output-native
  @Output() click = new EventEmitter<MouseEvent>();

  icons = Icons

  private propertyChangeSubject = new Subject();

  propertyChanged: Observable<unknown> =
    this.propertyChangeSubject.asObservable();

  buttonGroup: ActionButtonGroupCore | null = inject<ActionButtonGroupCore>(
    ACTION_BUTTON_GROUP_COMPONENT_TOKEN,
    {
      optional: true,
    }
  );

  ngOnChanges(changes: SimpleChanges): void {
    const isValueChange = Object.keys(changes).some(
      (x) => !changes[x].firstChange
    );
    if (isValueChange) {
      this.propertyChangeSubject.next(null);
    }
  }

  handleClick(event: MouseEvent) {
    event.stopPropagation();
    this.click.emit(event);
  }
}
