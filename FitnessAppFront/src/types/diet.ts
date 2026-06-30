export interface Diet{
    userId: string;
    logDay: string;              
    breakfast: string | null;
    lunch: string | null;
    dinner: string | null;
    snacks: string[] | null;
    supplements: string[] | null;   
    calories: number | null;
}