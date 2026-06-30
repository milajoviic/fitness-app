import { useEffect, useState } from "react";
import { bodyMetricsApi } from "../api/bodyMetrics";
import type { BodyMetric } from "../types/bodyMetrics";

export function BodyMetricsPage() {
  const [bodyPart, setBodyPart] = useState("weight");
  const [metrics, setMetrics] = useState<BodyMetric[]>([]);
  const [value, setValue] = useState("");
  const [error, setError] = useState("");

  const load = async () => {
    setError("");
    try {
      setMetrics(await bodyMetricsApi.getByBodyPart(bodyPart));
    } catch {
      setError("Greška pri učitavanju.");
    }
  };

  useEffect(() => { load(); }, [bodyPart]);   

  const handleAdd = async () => {
    if (!value) return;
    try {
      await bodyMetricsApi.create({ bodyPart, value: Number(value) });
      setValue("");
      await load();
    } catch {
      setError("Greška pri dodavanju.");
    }
  };

  const handleDelete = async (m: BodyMetric) => {
    await bodyMetricsApi.remove(m.bodyPart, m.metricId);
    await load();
  };

  const unit = (part: string) => part === "weight" ? "kg" : "cm";

  const labelBP = (part: string) => {
    if (part === "weight") return "Težina";
    if (part === "waist") return "Struk";
    if (part === "hips") return "Kukovi";
    if (part === "chest") return "Grudi";
    return part;
  }

  return (
  <div className="page">
    <div className="card" style={{ maxWidth: 500, display: "grid", gap: 16 }}>
      <h2>Mere tela</h2>

      <div style={{ display: "flex", gap: 8, alignItems: "center" }}>
        <label>Mera:</label>
        <select value={bodyPart} onChange={(e) => setBodyPart(e.target.value)}>
          <option value="weight">Težina (kg)</option>
          <option value="waist">Struk (cm)</option>
          <option value="hips">Kukovi (cm)</option>
          <option value="chest">Grudi (cm)</option>
        </select>
      </div>

      <div style={{ display: "flex", gap: 8 }}>
        <input type="number" placeholder="vrednost" value={value} onChange={(e) => setValue(e.target.value)} />
        <button onClick={handleAdd}>Dodaj merenje</button>
      </div>
      {error && <p className="error">{error}</p>}

      <ul>
        {metrics.map((m) => (
          <li key={m.metricId} className="list-item">
            <span>
              {new Date(m.recordedAt).toLocaleDateString()}
              {" — "}
              {m.value} {unit(m.bodyPart)}
              {" — "}
              {labelBP(m.bodyPart)}
            </span>
            <button onClick={() => handleDelete(m)}>Obriši</button>
          </li>
        ))}
      </ul>
    </div>
  </div>
  );
}