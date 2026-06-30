import { useEffect, useState } from "react";
import { healthApi } from "../api/health";
import type { Health } from "../types/health";

export function HealthPage() {
  const [health, setHealth] = useState<Health | null>(null);
  const [bmi, setBmi] = useState<number | null>(null);
  const [error, setError] = useState("");

  const [height, setHeight] = useState("");
  const [weight, setWeight] = useState("");
  const [notes, setNotes] = useState("");
  const [chronic, setChronic] = useState("Diabetes");

  const load = async () => {
    setError("");
    try {
      const h = await healthApi.get();
      setHealth(h);
      setHeight(h.height?.toString() ?? "");
      setWeight(h.weight?.toString() ?? "");
      setNotes(h.notes ?? "");
    } catch (err: any) {
      if (err.response?.status === 404) {
        setHealth(null);   
      } else {
        setError("Greška pri učitavanju.");
      }
    }
    try {
      setBmi(await healthApi.getBmi());
    } catch {
      setBmi(null);
    }
  };

  useEffect(() => { load(); }, []);

  const saveHeight = async () => { await healthApi.setHeight(Number(height)); await load(); };
  const saveWeight = async () => { await healthApi.setWeight(Number(weight)); await load(); };
  const saveNotes  = async () => { await healthApi.setNotes(notes); await load(); };
  const addChronic = async () => { await healthApi.addChronic(chronic); await load(); };

  const row = { display: "flex", gap: 8, alignItems: "center" } as const;

  return (
    <div style={{ maxWidth: 500, margin: "40px auto", display: "grid", gap: 16 }}>
      <h2>Zdravstveni profil</h2>
      {error && <p style={{ color: "red" }}>{error}</p>}

      {bmi !== null && (
        <div style={{ padding: 12, background: "#f0f8ff", borderRadius: 8 }}>
          <strong>BMI: {bmi}</strong>
        </div>
      )}

      <div style={row}>
        <span style={{ width: 80 }}>Visina (cm):</span>
        <input type="number" value={height} onChange={(e) => setHeight(e.target.value)} />
        <button onClick={saveHeight}>Sačuvaj</button>
      </div>
      <div style={row}>
        <span style={{ width: 80 }}>Težina (kg):</span>
        <input type="number" value={weight} onChange={(e) => setWeight(e.target.value)} />
        <button onClick={saveWeight}>Sačuvaj</button>
      </div>
      <div style={row}>
        <span style={{ width: 80 }}>Beleške:</span>
        <input style={{ flex: 1 }} value={notes} onChange={(e) => setNotes(e.target.value)} />
        <button onClick={saveNotes}>Sačuvaj</button>
      </div>

      <div>
        <strong>Hronične bolesti:</strong>
        <ul>{(health?.chronicCond ?? []).map((c, i) => <li key={i}>{c}</li>)}</ul>
        <div style={row}>
          <select value={chronic} onChange={(e) => setChronic(e.target.value)}>
            <option value="Diabetes">Dijabetes</option>
            <option value="Asthma">Astma</option>
            <option value="Depression">Depresija</option>
            <option value="Anxiety">Anksioznost</option>
            <option value="Arthritis">Artritis</option>
          </select>
          <button onClick={addChronic}>Dodaj</button>
        </div>
      </div>
    </div>
  );
}