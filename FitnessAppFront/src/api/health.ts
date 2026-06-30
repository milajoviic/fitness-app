import api from "./axios";
import type { Health } from "../types/health";

const rawJson = { headers: { "Content-Type": "application/json" } };

export const healthApi = {
    get: (): Promise<Health> => api.get<Health>("/api/userhealth").then((r) => r.data),
    getBmi: (): Promise<number> => api.get<number>("/api/userhealth/bmi").then((r) => r.data),
    setHeight: (height: number) =>  api.put("/api/userhealth/height", JSON.stringify(height), rawJson),
    setWeight: (weight: number) => api.put("/api/userhealth/weight", JSON.stringify(weight), rawJson),
    setNotes: (notes: string) => api.put("/api/userhealth/notes", JSON.stringify(notes), rawJson),

    addChronic: (cond: string) => api.put("/api/userhealth/chronic", JSON.stringify(cond), rawJson),

    deleteNotes: () => api.delete("/api/userhealth/notes"),
    deleteProfile: () => api.delete("/api/userhealth"),

}