import { useEffect, useState } from "react";
import { workoutsApi } from "../api/workouts";
import type { Workout } from "../types/workout";
import { ExerciseList } from "../components/ExerciseList";

export function WorkoutsPage() {
  const [workouts, setWorkouts] = useState<Workout[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  const [workoutDate, setWorkoutDate] = useState("");
  const [typeOfWorkout, setTypeOfWorkout] = useState("Cardio");
  const [isRestDay, setIsRestDay] = useState(false);
  const [notes, setNotes] = useState("");
  const [openWorkoutId, setOpenWorkoutId] = useState<string | null>(null);

  const load = async () => {
    setLoading(true);
    setError("");
    try {
      setWorkouts(await workoutsApi.getAll());
    } catch {
      setError("Greška pri učitavanju treninga.");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => { load(); }, []);   

  const handleCreate = async () => {
    if (!workoutDate) { setError("Unesi datum."); return; }
    try {
      await workoutsApi.create({
        workoutDate,
        typeOfWorkout,
        isRestDay,
        notes: notes || null,
      });
      setWorkoutDate(""); setNotes(""); setIsRestDay(false);   
      await load();                                            
    } catch {
      setError("Greška pri dodavanju.");
    }
  };

  const handleDelete = async (w: Workout) => {
    try {
      await workoutsApi.remove(w.workoutDate, w.workoutId);
      await load();
    } catch {
      setError("Greška pri brisanju.");
    }
  };

  return (
    <div style={{ maxWidth: 600, margin: "40px auto" }}>
      <h2>Moji treninzi</h2>

      <div style={{ display: "grid", gap: 8, marginBottom: 24, padding: 16, border: "1px solid #ddd", borderRadius: 8 }}>
        <h3>Dodaj trening</h3>
        <input type="date" value={workoutDate} onChange={(e) => setWorkoutDate(e.target.value)} />
        <select value={typeOfWorkout} onChange={(e) => setTypeOfWorkout(e.target.value)}>
          <option value="Cardio">Kardio</option>
          <option value="Aerobic">Aerobik</option>
          <option value="Strength">Trening snage</option>
          <option value="Flexibility">Fleksibilnost</option>
        </select>
        <label>
          <input type="checkbox" checked={isRestDay} onChange={(e) => setIsRestDay(e.target.checked)} /> Dan odmora
        </label>
        <input placeholder="Beleške (opciono)" value={notes} onChange={(e) => setNotes(e.target.value)} />
        <button onClick={handleCreate}>Dodaj</button>
      </div>

      {loading && <p>Učitavanje...</p>}
      {error && <p style={{ color: "red" }}>{error}</p>}
      {!loading && workouts.length === 0 && <p>Još nema treninga.</p>}

      <ul style={{ listStyle: "none", padding: 0, display: "grid", gap: 8 }}>
        {workouts.map((w) => (
          <li key={w.workoutId} style={{ padding: 12, border: "1px solid #eee", borderRadius: 8 }}>
            <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center" }}>
              <span>
                <strong>{new Date(w.workoutDate).toLocaleDateString()}</strong>
                {" — "}{w.isRestDay ? "Dan odmora" : w.typeOfWorkout}
                {w.notes && <em> ({w.notes})</em>}
              </span>
              <span style={{ display: "flex", gap: 8 }}>
                {!w.isRestDay && (
                <button onClick={() => setOpenWorkoutId(openWorkoutId === w.workoutId ? null : w.workoutId)}>
                {openWorkoutId === w.workoutId ? "Sakrij vežbe" : "Vežbe"}
                </button>
                )}
                <button onClick={() => handleDelete(w)}>Obriši</button>
              </span>
            </div>
            {openWorkoutId === w.workoutId && <ExerciseList workoutId={w.workoutId} />}
          </li>
        ))}
      </ul>
    </div>
  );
}