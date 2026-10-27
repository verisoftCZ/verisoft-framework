import { Component } from '@angular/core';
import {
  DOCS_CODE_LANG,
  DocumentationCodeBuilder,
} from '@verisoft/demo-documentation-ui';
import { MenuItem } from '@verisoft/ui-core';
import { DropdownButtonComponent, Icons } from '@verisoft/ui-primeng';

const template = `
<div class="row">
  <div class="col-12">
    <v-dropdown-button 
      label="Basic Dropdown" 
      [icon]="icons.settings"
      [items]="basicItems">
    </v-dropdown-button>
  </div>
</div>
`;

@Component({
    selector: 'v-doc-primeng-dropdown-button-basic-example',
    imports: [DropdownButtonComponent],
    template: template
})
export class PrimengDropdownButtonBasicExampleComponent {
  icons = Icons;
  
  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .addCode(`
export class ExampleComponent {
  icons = Icons;

  basicItems: MenuItem[] = [
    {
      id: 'new',
      label: 'New',
      icon: this.icons.add,
      command: () => this.onNew()
    },
    {
      id: 'edit',
      label: 'Edit',
      icon: this.icons.edit,
      command: () => this.onEdit()
    },
    {
      id: 'delete',
      label: 'Delete',
      icon: this.icons.delete,
      command: () => this.onDelete()
    }
  ];

  onNew() {
    alert('New action');
  }

  onEdit() {
    alert('Edit action');
  }

  onDelete() {
    alert('Delete action');
  }
}`, DOCS_CODE_LANG.TS)
    .buildCode();

  basicItems: MenuItem[] = [
    {
      id: 'new',
      label: 'New',
      icon: this.icons.add,
      command: () => this.onNew()
    },
    {
      id: 'edit',
      label: 'Edit',
      icon: this.icons.edit,
      command: () => this.onEdit()
    },
    {
      id: 'delete',
      label: 'Delete',
      icon: this.icons.delete,
      command: () => this.onDelete()
    }
  ];

  onNew() {
    alert('New action');
  }

  onEdit() {
    alert('Edit action');
  }

  onDelete() {
    alert('Delete action');
  }
}
