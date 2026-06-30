import api from "./axios";
import type { Period, PeriodInputDto } from "../types/period";

export const periodApi = {
    getAll: (): Promise<Period[]> => api.get<Period[]>("/api/period").then((r) => r.data),
    getPhase: (): Promise<string> => api.get<string>("/api/period/phase").then((r) => r.data),
    create: (dto: PeriodInputDto) => api.post("/api/period", dto),
    remove: (start: string) => api.delete("/api/period", {params: { start }}),
}