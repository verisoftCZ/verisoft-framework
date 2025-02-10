export function isFilterEmpty<T>(filter: Partial<T> | undefined) {
  if (filter == undefined) {
    return true;
  }

  return !Object.entries(filter).some((x) => x[1] != undefined);
}
