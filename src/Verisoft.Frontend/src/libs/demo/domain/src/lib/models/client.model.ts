export interface Client {
  name: string;
  tradeId: string;
  vatId: string;
  representative: string;
  tradeRegisterEntry: string;
  numberOfEmployees: number;
  description: string;
  contactNote: string;
  accountingSystemClientId: string;
  websiteUrl: string;
  recruitmentUrl: string;
  companyActivity: string;
  taxDomicile: 'AFG';
  parentClientId: number;
  id: number;
  createdBy: string;
  createdAt: Date;
  updatedBy: string;
  updatedAt: Date;
}
