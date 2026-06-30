export interface Workout{
    userId: string;
    workoutDate: string;
    workoutId: string;
    typeOfWorkout: string;
    isRestDay: boolean;
    notes: string | null;
}
export interface CreateWorkoutDto{
    workoutDate: string;
    typeOfWorkout: string;
    isRestDay: boolean;
    notes: string | null;
}
export interface UpdateWorkoutDto{
    workoutDate: string;
    workoutId: string;
    typeOfWorkout: string;
    isRestDay: boolean;
    notes: string | null;
}