import api from "./axios";
import type { Workout, CreateWorkoutDto, UpdateWorkoutDto } from "../types/workout";

export const workoutsApi = {
    getAll: (): Promise<Workout[]> => api.get<Workout[]>("/api/userworkout").then((r)=>r.data),
    create: (dto: CreateWorkoutDto): Promise<Workout> => api.post<Workout>("/api/userworkout", dto).then((r) => r.data),
    update: (dto: UpdateWorkoutDto): Promise<void> => api.put("/api/userworkout", dto).then(() => {}),
    remove: (workoutDate: string, workoutId: string): Promise<void> => api.delete("/api/userworkout", {params: {workoutDate, workoutId}}).then(() => {}),
};