export function sortBy<TEntity, TProperty>(
  items: TEntity[],
  selector: (item: TEntity) => TProperty,
  ascending = true
): TEntity[] {
  return [...items].sort((item, other) => {
    const value = selector(item);
    const otherValue = selector(other);
    const direction = ascending ? 1 : -1;
    if (value == undefined && other == undefined) {
      return 0;
    }

    if (value == undefined) {
      return -1 * direction;
    }
    if (otherValue == undefined) {
      return 1 * direction;
    }

    if (value < otherValue) {
      return -1 * direction;
    }

    if (value > otherValue) {
      return 1 * direction;
    }

    return 0;
  });
}

export function getValueByPath<T, TValue>(
  obj: T,
  path: string | undefined,
): TValue | undefined {
  if (path == undefined || path == ''){
    return obj as unknown as TValue;
  }

  return path.split('.').reduce((acc, key) => {
    if (acc && typeof acc === 'object' && key in acc) {
      return (acc as Record<string, TValue>)[key];
    }
    return undefined;
  }, obj as unknown) as TValue;
}

export function cloneDeep<T>(obj: T): T {
  if (obj === null || typeof obj !== 'object') {
    return obj;
  }

  if (obj instanceof Date) {
    return new Date(obj.getTime()) as any;
  }

  if (Array.isArray(obj)) {
    return obj.map((item) => cloneDeep(item)) as any;
  }

  if (obj instanceof Map) {
    return new Map(Array.from(obj.entries()).map(([key, value]) => [key, cloneDeep(value)])) as any;
  }

  if (obj instanceof Set) {
    return new Set(Array.from(obj.values()).map((value) => cloneDeep(value))) as any;
  }

  const clonedObj = {} as T;
  for (const key in obj) {
    if (Object.prototype.hasOwnProperty.call(obj, key)) {
      clonedObj[key] = cloneDeep(obj[key]);
    }
  }

  return clonedObj;
}
