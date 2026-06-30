export interface Health{
    userId: string;
    height: number | null;
    weight: number | null;
    notes: string | null;
    chronicCond: string[] | null;
}