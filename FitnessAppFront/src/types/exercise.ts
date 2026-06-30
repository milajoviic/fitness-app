export interface Exercise {
  workoutId: string;
  excOrder: number;
  name: string;
  reps: number;
  restMinutes: number;
  sets: number;
  weightKg: number;
}

export interface CreateExerciseDto {
  workoutId: string;
  name: string;
  reps: number;
  restMinutes: number;
  sets: number;
  weightKg: number;
}