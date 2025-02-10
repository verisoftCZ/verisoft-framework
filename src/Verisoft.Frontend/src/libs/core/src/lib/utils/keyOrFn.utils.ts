export function keyOrFn(keyOrFn: ((row: any) => string | undefined) | string, row: any){
    if (keyOrFn instanceof Function) {
        return keyOrFn(row);
    }
    else if (typeof keyOrFn === 'string') {
        const value = row[keyOrFn];
        if (value){
            return value;
        }
    }
    else if (typeof keyOrFn === 'boolean') {
      return keyOrFn;
  }
    return '';
}
