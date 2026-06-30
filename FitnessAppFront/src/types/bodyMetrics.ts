export interface BodyMetric{
    userId: string;
    bodyPart: string;
    metricId: string;       
    recordedAt: string;     
    value: number;
}
export interface CreateBodyMetricDto {
    bodyPart: string;
    value: number;
}