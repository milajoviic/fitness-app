export interface Period{
    userId: string;
    startDate: string;
    endDate: string | null;
    notes: string | null;
}
export interface PeriodInputDto {
  startDate: string;        
  endDate: string | null;
  notes: string | null;
}