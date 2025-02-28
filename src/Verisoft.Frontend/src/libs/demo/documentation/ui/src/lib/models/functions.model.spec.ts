import { DOCS_CODE_LANG } from "./constants.model";
import { getLangByFileName } from "./functions.model";

describe('Functions - getLangByFileName', () => {
    test('Should return typescript by ts extension', () =>{
        const fileName = "index.ts";

        const type = getLangByFileName(fileName);

        expect(type).toBe(DOCS_CODE_LANG.TS);
    });

    test('Should return typescript when file extension is ts and file name contains multiple dots', () =>{
        const fileName = "my.file.ts";

        const type = getLangByFileName(fileName);

        expect(type).toBe(DOCS_CODE_LANG.TS);
    });

    test('Should return typescript when file extension is ts and file extension is upper cased', () =>{
        const fileName = "my.file.TS";

        const type = getLangByFileName(fileName);

        expect(type).toBe(DOCS_CODE_LANG.TS);
    });
})