export type ExtendedComponent<TComponent> = 
    Partial<TComponent> & { [key: string]: (value: any) => void; };