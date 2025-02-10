import { Type } from '@angular/core';
import { DOCS_BLOCK_TYPE, DOCS_CODE_LANG } from './constants.model';
import {
  DocumentationItem,
  DocumentationTableItem,
  IDocumentationCodeComponent,
} from './documentation-item.model';
import { getLangByFileName } from './functions.model';

export class DocumentationBuilder {
  items: DocumentationItem[] = [];

  addSection(config: (x: DocumentationSectionBuilder) => void) {
    const sectionBuilder = new DocumentationSectionBuilder();
    config?.(sectionBuilder);
    this.items = [...this.items, sectionBuilder.build()];
    return this;
  }

  addApiSection(config: (x: DocumentationApiSectionBuilder) => void) {
    const sectionBuilder = new DocumentationApiSectionBuilder();
    config?.(sectionBuilder);
    this.items = [...this.items, sectionBuilder.build()];
    return this;
  }

  build(): DocumentationItem[] {
    this.addLevelNumber(this.items, 0);
    return this.items;
  }

  private addLevelNumber(
    items: DocumentationItem[] | undefined,
    parent: number
  ) {
    if (items) {
      items.forEach((x) => {
        x.level = parent + 1;
        this.addLevelNumber(x.items, x.level);
      });
    }
  }
}

abstract class DocumentationItemBuilderBase<TBuilder> {
  protected item: DocumentationItem = {
    type: this.getType(),
  };

  abstract getType(): DOCS_BLOCK_TYPE;

  title(value: string): TBuilder {
    this.item.title = value;
    return this as unknown as TBuilder;
  }

  description(value: string): TBuilder {
    this.item.description = value;
    return this as unknown as TBuilder;
  }

  build(): DocumentationItem {
    return this.item;
  }
}

export class DocumentationSectionBuilder extends DocumentationItemBuilderBase<DocumentationSectionBuilder> {
  override getType(): DOCS_BLOCK_TYPE {
    return DOCS_BLOCK_TYPE.SECTION;
  }

  addCode(
    code: string,
    fileName: string | undefined = undefined,
    lang: DOCS_CODE_LANG | undefined = undefined
  ): DocumentationSectionBuilder {
    const codeBuilder = new DocumentationCodeBuilder();
    const codeItem = codeBuilder.addCode(code, fileName, lang).build();
    this.item.items = [...(this.item.items ?? []), codeItem];
    return this;
  }

  addCodeSection(
    config?: (x: DocumentationCodeBuilder) => void
  ): DocumentationSectionBuilder {
    const codeBuilder = new DocumentationCodeBuilder();
    config?.(codeBuilder);
    this.item.items = [...(this.item.items ?? []), codeBuilder.build()];
    return this;
  }

  addText(text: string): DocumentationSectionBuilder {
    const textItem: DocumentationItem = {
      type: DOCS_BLOCK_TYPE.TEXT,
      text: text,
    };
    this.item.items = [...(this.item.items ?? []), textItem];
    return this;
  }

  addTable(
    config?: (x: DocumentationTableBuilder) => void
  ): DocumentationSectionBuilder {
    const tableBuilder = new DocumentationTableBuilder();
    config?.(tableBuilder);
    this.item.items = [...(this.item.items ?? []), tableBuilder.build()];
    return this;
  }

  addCodeComponent<TComponent extends IDocumentationCodeComponent>(
    componentType: Type<TComponent>
  ): DocumentationSectionBuilder {
    const codeBuilder = new DocumentationCodeBuilder();
    const codeItem = codeBuilder.addCodeComponent(componentType).build();
    this.item.items = [...(this.item.items ?? []), codeItem];
    return this;
  }

  addSection(config: (x: DocumentationSectionBuilder) => void) {
    const sectionBuilder = new DocumentationSectionBuilder();
    config?.(sectionBuilder);
    this.item.items = [...(this.item.items ?? []), sectionBuilder.build()];
    return this;
  }
}

export class DocumentationApiSectionBuilder extends DocumentationSectionBuilder {
  override getType(): DOCS_BLOCK_TYPE {
    return DOCS_BLOCK_TYPE.API_SECTION;
  }
}
export class DocumentationTableBuilder extends DocumentationItemBuilderBase<DocumentationTableBuilder> {
  override getType(): DOCS_BLOCK_TYPE {
    return DOCS_BLOCK_TYPE.TABLE;
  }

  nameHeader(value: string) {
    this.ensureTable();
    if (this.item.table) {
      this.item.table.nameColumnHeader = value;
    }

    return this;
  }

  withType() {
    this.ensureTable();
    if (this.item.table) {
      this.item.table.showType = true;
    }

    return this;
  }

  withDefaultValue() {
    this.ensureTable();
    if (this.item.table) {
      this.item.table.showDefault = true;
    }

    return this;
  }

  withParameters() {
    this.ensureTable();
    if (this.item.table) {
      this.item.table.showParameters = true;
    }

    return this;
  }

  showParameters(visible = true): DocumentationTableBuilder {
    this.ensureTable();
    if (this.item.table) {
      this.item.table.showParameters = visible;
    }

    return this;
  }

  orderItems(value = true): DocumentationTableBuilder {
    this.ensureTable();
    if (this.item.table) {
      this.item.table.orderItems = value;
    }

    return this;
  }

  addItem(item: DocumentationTableItem): DocumentationTableBuilder {
    return this.addItems([item]);
  }

  addItems(items: DocumentationTableItem[]): DocumentationTableBuilder {
    this.ensureTable();
    if (this.item.table && items && items.length) {
      this.item.table.items = [...this.item.table.items, ...items];
    }

    return this;
  }

  override build(): DocumentationItem {
    if (this.item.table) {
      this.item.table.showDefault =
        this.item.table.showDefault === true ||
        this.item.table.items.some((x) => x.default != undefined);
      this.item.table.showType =
        this.item.table.showType === true ||
        this.item.table.items.some((x) => x.type != undefined);
      this.item.table.showParameters =
        this.item.table.showParameters === true ||
        this.item.table.items.some((x) => x.parameters != undefined);
    }
    return this.item;
  }

  private ensureTable() {
    this.item.table = this.item.table ?? {
      nameColumnHeader: 'Name',
      showDefault: false,
      showType: false,
      showParameters: false,
      orderItems: true,
      items: [],
    };
  }
}

export class DocumentationCodeBuilder extends DocumentationItemBuilderBase<DocumentationCodeBuilder> {
  override getType(): DOCS_BLOCK_TYPE {
    return DOCS_BLOCK_TYPE.CODE;
  }

  addCode(
    code: string,
    fileName: string | undefined = undefined,
    lang: DOCS_CODE_LANG | undefined = undefined,
    visible = true
  ): DocumentationCodeBuilder {
    this.ensureCode();
    fileName = fileName ?? 'index.ts';
    this.item.code?.files.push({
      content: code,
      language: lang ?? getLangByFileName(fileName),
      fileName: fileName,
      visible: visible,
    });

    return this;
  }

  addCodeComponent<TComponent extends IDocumentationCodeComponent>(
    componentType: Type<TComponent>
  ): DocumentationCodeBuilder {
    this.ensureCode();
    if (this.item.code) {
      this.item.code.componentType = componentType;
      const instanceOfComponent = new componentType();
      if (instanceOfComponent.code) {
        instanceOfComponent.code.files.forEach((x) => {
          this.addCode(x.content, x.fileName, x.language, x.visible);
        });
      }
    }

    return this;
  }

  buildCode() {
    this.ensureCode();
    return this.item.code;
  }

  private ensureCode() {
    this.item.code = this.item.code ?? { files: [] };
    this.item.code.files = this.item.code.files ?? [];
  }
}
