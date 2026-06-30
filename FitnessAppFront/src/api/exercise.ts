import api from "./axios";
import type { Exercise, CreateExerciseDto } from "../types/exercise";

export const exerciseApi = {
    getByWorkout: (workoutId: string): Promise<Exercise[]> => api.get<Exercise[]>(`/api/exercise/${workoutId}`).then((r) => r.data),
    create: (dto: CreateExerciseDto) => api.post("/api/exercise", dto),
    remove: (workoutId: string, excOrder: number) => api.delete("/api/exercise", {params: {workoutId, excOrder}}),
}