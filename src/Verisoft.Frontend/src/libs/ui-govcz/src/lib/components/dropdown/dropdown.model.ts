export interface DropDownItemData<T> {
    label: string;
    value: string | number;
    item: T;
    selected: boolean;
}