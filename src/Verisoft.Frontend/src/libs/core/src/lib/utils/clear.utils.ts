export class ClearUtils {

  static recursiveObjectAttributesTransformation(obj: any): void {
    ClearUtils.recursiveObjectAttributesTraversal(
      obj,
      ClearUtils.transformEmptyStringToNullStringFn
    );
  }
  static recursiveObjectAttributesDeletation(obj: any): void {
    ClearUtils.recursiveObjectAttributesTraversal(
      obj,
      ClearUtils.deleteEmptyStringToNullStringFn
    );
  }

  static transformEmptyStringToNullStringFn(obj: any, key: string) {
    if (typeof obj[key] === 'string' && obj[key] === '') {
      obj[key] = null;
    }
  }
  static deleteEmptyStringToNullStringFn(obj: any, key: string) {
    if (obj[key] === '' || obj[key] === null || obj[key] === undefined) {
      delete obj[key]
    }
  }


  static recursiveObjectAttributesTraversal(
    obj: any,
    transformationFn: (obj: any, key: string) => void
  ) {
    if (
      obj === null ||
      transformationFn === null ||
      typeof transformationFn !== 'function'
    ) {
      return;
    }

    Object.keys(obj).forEach((key) => {
      transformationFn(obj, key);

      if (typeof obj[key] === 'object') {
        this.recursiveObjectAttributesTraversal(obj[key], transformationFn);

        if (!Object.keys(obj[key]).length) {
          delete obj[key];
        }
      }
    });
  }

}
