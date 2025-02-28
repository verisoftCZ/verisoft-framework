import { Directive, HostListener, Input } from "@angular/core";

@Directive({
    // eslint-disable-next-line @angular-eslint/directive-selector
    selector: 'v-button[useShortCut]',
    exportAs: 'useShortCut',
    standalone: true,
  })
  export class ButtonShortCutDirective
  {
     @Input() shortCutFn?: () => void;
     @Input() shortCutKey!: string;

     private keyMap: { [key: string]: boolean } = {};

     @HostListener('document:keydown', ['$event'])
     onKeyDown(event: KeyboardEvent) {
       this.keyMap[event.key.toLowerCase()] = true;
       this.checkShortcut();
     }
   
     @HostListener('document:keyup', ['$event'])
     onKeyUp(event: KeyboardEvent) {
       this.keyMap[event.key.toLowerCase()] = false;
     }

     private checkShortcut() {
       if (this.shortCutKey && this.shortCutFn) {
         const keys = this.shortCutKey.toLowerCase().split('+');
         const isShortcutPressed = keys.every((key) => this.keyMap[key]);   
         if (isShortcutPressed) {
           this.shortCutFn?.();
           keys.forEach((key) => (this.keyMap[key] = false));
         }
       }
     }
}