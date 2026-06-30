import api from "./axios";
import type { BodyMetric, CreateBodyMetricDto } from "../types/bodyMetrics";

export const bodyMetricsApi = {
    create: (dto: CreateBodyMetricDto) => api.post("/api/bodymetrics", dto).then((r) => r.data),
    getByBodyPart: (bodyPart: string): Promise<BodyMetric[]> => api.get<BodyMetric[]>(`/api/bodymetrics/${bodyPart}`).then((r)=>r.data),
    remove: (bodyPart: string, metricId: string) => api.delete(`/api/bodymetrics/${bodyPart}/${metricId}`),
}