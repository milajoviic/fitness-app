import { useEffect, useState } from "react";
import { periodApi } from "../api/period";
import type { Period } from "../types/period";

export function PeriodPage() {
  const [periods, setPeriods] = useState<Period[]>([]);
  const [phase, setPhase] = useState<string | null>(null);
  const [error, setError] = useState("");

  const [startDate, setStartDate] = useState("");
  const [endDate, setEndDate] = useState("");
  const [notes, setNotes] = useState("");

  const load = async () => {
    setError("");
    try {
      setPeriods(await periodApi.getAll());
    } catch {
      setError("Greška pri učitavanju.");
    }
    try {
      setPhase(await periodApi.getPhase());
    } catch {
      setPhase(null);   
    }
  };

  useEffect(() => { load(); }, []);

  const handleAdd = async () => {
    if (!startDate) { setError("Unesi datum početka."); return; }
    try {
      await periodApi.create({
        startDate,
        endDate: endDate || null,
        notes: notes || null,
      });
      setStartDate(""); setEndDate(""); setNotes("");
      await load();
    } catch (err: any) {
      setError(err.response?.data ?? "Greška pri dodavanju.");
    }
  };

  const handleDelete = async (p: Period) => {
    await periodApi.remove(p.startDate);
    await load();
  };

  return (
    <div style={{ maxWidth: 500, margin: "40px auto", display: "grid", gap: 16 }}>
      <h2>Menstrualni ciklus</h2>

      {phase && (
        <div style={{ padding: 12, background: "#fff0f5", borderRadius: 8 }}>
          Trenutna faza: <strong>{phase}</strong>
        </div>
      )}

      <div style={{ display: "grid", gap: 8, padding: 16, border: "1px solid #ddd", borderRadius: 8 }}>
        <h3>Dodaj menstruaciju</h3>
        <label>Početak: <input type="date" value={startDate} onChange={(e) => setStartDate(e.target.value)} /></label>
        <label>Kraj (opciono): <input type="date" value={endDate} onChange={(e) => setEndDate(e.target.value)} /></label>
        <input placeholder="Beleške (opciono)" value={notes} onChange={(e) => setNotes(e.target.value)} />
        <button onClick={handleAdd}>Dodaj</button>
      </div>
      {error && <p style={{ color: "red" }}>{error}</p>}

      <ul style={{ listStyle: "none", padding: 0, display: "grid", gap: 6 }}>
        {periods.map((p) => (
          <li key={p.startDate} style={{ display: "flex", justifyContent: "space-between", padding: 8, border: "1px solid #eee", borderRadius: 6 }}>
            <span>
              {new Date(p.startDate).toLocaleDateString()}
              {p.endDate && ` — ${new Date(p.endDate).toLocaleDateString()}`}
              {p.notes && <em> ({p.notes})</em>}
            </span>
            <button onClick={() => handleDelete(p)}>Obriši</button>
          </li>
        ))}
      </ul>
    </div>
  );
}