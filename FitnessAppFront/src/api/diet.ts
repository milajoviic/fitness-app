import api from "./axios";
import type { Diet } from "../types/diet"; 

const rawJson = {headers: {"Content-Type": "application/json"}};
export const dietApi = {
    getByDay: (day: string): Promise<Diet> => api.get<Diet>(`/api/userdiet/${day}`).then((r) => r.data),
    getAll: (): Promise<Diet[]> => api.get<Diet[]>("/api/userdiet").then((r) => r.data),

    setBreakfast: (day: string, text: string) => api.put(`/api/userdiet/${day}/breakfast`, JSON.stringify(text), rawJson),
    setLunch: (day: string, text: string) => api.put(`/api/userdiet/${day}/lunch`, JSON.stringify(text), rawJson),
    setDinner: (day: string, text: string) => api.put(`/api/userdiet/${day}/dinner`, JSON.stringify(text), rawJson),

    setCalories: (day: string, calories: number) => api.put(`/api/userdiet/${day}/calories`, JSON.stringify(calories), rawJson),

    addSnack: (day: string, snack: string) => api.put(`/api/userdiet/${day}/snack`, JSON.stringify(snack), rawJson),
    removeSnack: (day: string, snack: string) => api.delete(`/api/userdiet/${day}/snack`, { data: JSON.stringify(snack), ...rawJson }),

    addSupplement: (day: string, supp: string) => api.put(`/api/userdiet/${day}/supplement`, JSON.stringify(supp), rawJson),
    removeSupplement: (day: string, supp: string) => api.delete(`/api/userdiet/${day}/supplement`, { data: JSON.stringify(supp), ...rawJson }),

    deleteDay: (day: string) => api.delete(`/api/userdiet/${day}`),
}