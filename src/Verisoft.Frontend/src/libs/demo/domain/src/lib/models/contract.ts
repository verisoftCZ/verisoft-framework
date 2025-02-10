export interface Contract {
    id: number;
    name: string;
    dateSigned: Date;
    validTo: Date;
    validFrom: Date;
    type: number;
    state: number;
    createdBy: string;
    createdAt: Date;
    updatedBy: string;
    updatedAt: Date;
}

export interface ContractListData {
    total: number;
    data: Contract[];
}