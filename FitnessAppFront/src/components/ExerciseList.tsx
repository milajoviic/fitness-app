import { useEffect, useState } from "react";
import { exerciseApi } from "../api/exercise";
import type { Exercise } from "../types/exercise";

export function ExerciseList({ workoutId }: { workoutId: string }) {
  const [exercises, setExercises] = useState<Exercise[]>([]);
  const [error, setError] = useState("");

  const [name, setName] = useState("");
  const [reps, setReps] = useState("");
  const [sets, setSets] = useState("");
  const [restMinutes, setRestMinutes] = useState("");
  const [weightKg, setWeightKg] = useState("");

  const load = async () => {
    setError("");
    try {
      setExercises(await exerciseApi.getByWorkout(workoutId));
    } catch {
      setError("Greška pri učitavanju vežbi.");
    }
  };

  useEffect(() => { load(); }, [workoutId]);

  const handleAdd = async () => {
    if (!name) { setError("Unesi naziv vežbe."); return; }
    try {
      await exerciseApi.create({
        workoutId,
        name,
        reps: Number(reps) || 0,
        sets: Number(sets) || 0,
        restMinutes: Number(restMinutes) || 0,
        weightKg: Number(weightKg) || 0,
      });
      setName(""); setReps(""); setSets(""); setRestMinutes(""); setWeightKg("");
      await load();
    } catch {
      setError("Greška pri dodavanju vežbe.");
    }
  };

  const handleDelete = async (e: Exercise) => {
    await exerciseApi.remove(e.workoutId, e.excOrder);
    await load();
  };

  return (
  <div style={{ marginTop: 12, paddingLeft: 16, borderLeft: "3px solid #f5d020" }}>
    {error && <p className="error">{error}</p>}

    {exercises.length === 0 && <p style={{ color: "#8a8460" }}>Nema vežbi.</p>}
    <ol style={{ display: "grid", gap: 6, margin: "8px 0" }}>
      {exercises.map((e) => (
        <li key={e.excOrder} style={{ display: "flex", justifyContent: "space-between", alignItems: "center" }}>
          <span>{e.name} — {e.sets}×{e.reps}, {e.weightKg}kg, pauza {e.restMinutes}min</span>
          <button onClick={() => handleDelete(e)}>Ukloni</button>
        </li>
      ))}
    </ol>

    <div style={{ display: "flex", gap: 6, flexWrap: "wrap", marginTop: 8 }}>
        <label style={{ display: "grid", gap: 4, fontSize: 13 }}>
            Naziv
            <input value={name} onChange={(e) => setName(e.target.value)} />
        </label>
        <label style={{ display: "grid", gap: 4, fontSize: 13 }}>
            Broj serija
            <input value={sets} onChange={(e) => setSets(e.target.value)} />
        </label>
        <label style={{ display: "grid", gap: 4, fontSize: 13 }}>
            Br. ponavljanja
            <input value={reps} onChange={(e) => setReps(e.target.value)} />
        </label>
        <label style={{ display: "grid", gap: 4, fontSize: 13 }}>
            Kg
            <input value={weightKg} onChange={(e) => setWeightKg(e.target.value)} />
        </label>
        <label style={{ display: "grid", gap: 4, fontSize: 13 }}>
            Pauza
            <input value={restMinutes} onChange={(e) => setRestMinutes(e.target.value)} />
        </label>
      <button onClick={handleAdd}>Dodaj vežbu</button>
    </div>
  </div>
    );
}