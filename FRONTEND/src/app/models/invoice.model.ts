export interface Invoice {
  id: number;
  dDBeleg_RecordID?: number;
  liegNR: number;
  propertyName: string;
  beleg_ID?: number;
  beleg_Datum?: number;
  beleg_DatumFormat?: Date;
  krediNR?: number;
  kreditor?: string;
  betrag?: number;
  rG_Betrag?: number;
  b_User?: string;
  b_Datum?: string;
  b_Valuta?: string;
  bezahlt?: number;
  filePath?: string;
  katNR?: number;
  kat?: string;
}
