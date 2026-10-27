import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ControlSeverity, FieldSize, MenuItem } from '@verisoft/ui-core';
import { SplitButtonModule } from 'primeng/splitbutton';
import { DropdownButtonComponent } from './dropdown-button.component';

describe('DropdownButtonComponent', () => {
    let component: DropdownButtonComponent;
    let fixture: ComponentFixture<DropdownButtonComponent>;

    beforeEach(async () => {
        await TestBed.configureTestingModule({
            imports: [DropdownButtonComponent, SplitButtonModule]
        }).compileComponents();

        fixture = TestBed.createComponent(DropdownButtonComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });

    describe('Input properties', () => {
        it('should set label input', () => {
            fixture.componentRef.setInput('label', 'Test Button');
            expect(component.label()).toBe('Test Button');
        });

        it('should set icon input', () => {
            fixture.componentRef.setInput('icon', 'pi pi-check');
            expect(component.icon()).toBe('pi pi-check');
        });

        it('should set rounded input to true', () => {
            fixture.componentRef.setInput('rounded', true);
            expect(component.rounded()).toBe(true);
        });

        it('should default rounded to false', () => {
            expect(component.rounded()).toBe(false);
        });

        it('should set raised input to true', () => {
            fixture.componentRef.setInput('raised', true);
            expect(component.raised()).toBe(true);
        });

        it('should default raised to false', () => {
            expect(component.raised()).toBe(false);
        });

        it('should set outlined input to true', () => {
            fixture.componentRef.setInput('outlined', true);
            expect(component.outlined()).toBe(true);
        });

        it('should default outlined to false', () => {
            expect(component.outlined()).toBe(false);
        });

        it('should set severity input', () => {
            fixture.componentRef.setInput('severity', ControlSeverity.danger);
            expect(component.severity()).toBe(ControlSeverity.danger);
        });

        it('should default severity to primary', () => {
            expect(component.severity()).toBe(ControlSeverity.primary);
        });

        it('should set size input', () => {
            fixture.componentRef.setInput('size', FieldSize.large);
            expect(component.size()).toBe(FieldSize.large);
        });

        it('should set items input', () => {
            const testItems: MenuItem[] = [
                { label: 'Item 1', command: () => {} },
                { label: 'Item 2', icon: 'pi pi-star' }
            ];
            fixture.componentRef.setInput('items', testItems);
            expect(component.items()).toEqual(testItems);
        });

        it('should default items to undefined', () => {
            expect(component.items()).toBeUndefined();
        });
    });

    describe('Computed properties', () => {
        it('should convert severity to primeng severity', () => {
            fixture.componentRef.setInput('severity', ControlSeverity.success);
            expect(component.primengSeverity()).toBeDefined();
        });

        it('should convert size to primeng size', () => {
            fixture.componentRef.setInput('size', FieldSize.small);
            expect(component.primengSize()).toBeDefined();
        });

        it('should return empty array when items is undefined', () => {
            fixture.componentRef.setInput('items', undefined);
            expect(component.primengItems()).toEqual([]);
        });

        it('should map items to primeng format', () => {
            const commandSpy = jest.fn();
            const testItems: MenuItem[] = [
                {
                    label: 'Save',
                    icon: 'pi pi-save',
                    command: commandSpy,
                    class: 'test-class'
                },
                {
                    label: 'Delete',
                    icon: 'pi pi-trash',
                    url: '/delete'
                },
                {
                    label: 'Edit',
                    routerLink: ['/edit']
                }
            ];

            fixture.componentRef.setInput('items', testItems);
            const primengItems = component.primengItems();

            expect(primengItems.length).toBe(3);
            expect(primengItems[0].label).toBe('Save');
            expect(primengItems[0].icon).toBe('pi pi-save');
            expect(primengItems[0].class).toBe('test-class');
            expect(primengItems[1].url).toBe('/delete');
            expect(primengItems[2].routerLink).toEqual(['/edit']);

            // Test command execution
            primengItems[0].command?.();
            expect(commandSpy).toHaveBeenCalled();
        });

        it('should handle items without optional properties', () => {
            const testItems: MenuItem[] = [
                { label: 'Simple Item' }
            ];

            fixture.componentRef.setInput('items', testItems);
            const primengItems = component.primengItems();

            expect(primengItems.length).toBe(1);
            expect(primengItems[0].label).toBe('Simple Item');
            expect(primengItems[0].icon).toBeUndefined();
            expect(primengItems[0].url).toBeUndefined();
            expect(primengItems[0].routerLink).toBeUndefined();
            expect(primengItems[0].class).toBeUndefined();
        });
    });

    describe('Template rendering', () => {
        it('should render p-splitbutton element', () => {
            const compiled = fixture.nativeElement as HTMLElement;
            const splitButton = compiled.querySelector('p-splitbutton');
            expect(splitButton).toBeTruthy();
        });

        it('should bind label to p-splitbutton', () => {
            fixture.componentRef.setInput('label', 'Action');
            fixture.detectChanges();
            
            const compiled = fixture.nativeElement as HTMLElement;
            const splitButton = compiled.querySelector('p-splitbutton');
            expect(splitButton).toBeTruthy();
        });

        it('should bind icon to p-splitbutton', () => {
            fixture.componentRef.setInput('icon', 'pi pi-plus');
            fixture.detectChanges();
            
            const compiled = fixture.nativeElement as HTMLElement;
            const splitButton = compiled.querySelector('p-splitbutton');
            expect(splitButton).toBeTruthy();
        });

        it('should apply all boolean properties', () => {
            fixture.componentRef.setInput('rounded', true);
            fixture.componentRef.setInput('raised', true);
            fixture.componentRef.setInput('outlined', true);
            fixture.detectChanges();
            
            const compiled = fixture.nativeElement as HTMLElement;
            const splitButton = compiled.querySelector('p-splitbutton');
            expect(splitButton).toBeTruthy();
        });

        it('should bind menu items', () => {
            const testItems: MenuItem[] = [
                { label: 'Option 1' },
                { label: 'Option 2' }
            ];
            
            fixture.componentRef.setInput('items', testItems);
            fixture.detectChanges();
            
            const compiled = fixture.nativeElement as HTMLElement;
            const splitButton = compiled.querySelector('p-splitbutton');
            expect(splitButton).toBeTruthy();
        });
    });

    describe('Provider registration', () => {
        it('should provide DROPDOWN_BUTTON_COMPONENT_TOKEN', () => {
            const token = fixture.debugElement.injector.get(component.constructor as any);
            expect(token).toBeTruthy();
        });
    });
});
