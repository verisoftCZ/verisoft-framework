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
  path: string
): TValue | undefined {
  return path.split('.').reduce((acc, key) => {
    if (acc && typeof acc === 'object' && key in acc) {
      return (acc as Record<string, TValue>)[key];
    }
    return undefined;
  }, obj as unknown) as TValue;
}
