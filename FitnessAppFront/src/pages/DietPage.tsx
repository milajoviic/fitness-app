import { useEffect, useState } from "react";
import { dietApi } from "../api/diet";
import type { Diet } from "../types/diet";

// danasnji datum kao "YYYY-MM-DD"
const today = () => new Date().toISOString().slice(0, 10);

export function DietPage() {
  const [day, setDay] = useState(today());
  const [diet, setDiet] = useState<Diet | null>(null);
  const [allDays, setAllDays] = useState<Diet[]>([]);   // istorija svih dana
  const [error, setError] = useState("");

  // polja za unos
  const [breakfast, setBreakfast] = useState("");
  const [lunch, setLunch] = useState("");
  const [dinner, setDinner] = useState("");
  const [calories, setCalories] = useState("");
  const [snack, setSnack] = useState("");
  const [supplement, setSupplement] = useState("Creatine");

  // ucitaj izabrani dan
  const load = async () => {
    setError("");
    try {
      const d = await dietApi.getByDay(day);
      setDiet(d);
      setBreakfast(d.breakfast ?? "");
      setLunch(d.lunch ?? "");
      setDinner(d.dinner ?? "");
      setCalories(d.calories?.toString() ?? "");
    } catch (err: any) {
      if (err.response?.status === 404) {
        setDiet(null);
        setBreakfast(""); setLunch(""); setDinner(""); setCalories("");
      } else {
        setError("Greška pri učitavanju.");
      }
    }
  };

  // ucitaj sve dane (istorija)
  const loadAllDays = async () => {
    try {
      setAllDays(await dietApi.getAll());
    } catch {
      // ako ne uspe, ostavi praznu listu
    }
  };

  useEffect(() => { load(); }, [day]);          // kad se promeni dan
  useEffect(() => { loadAllDays(); }, []);      // svi dani jednom, pri otvaranju

  // svaki "sacuvaj" posalje svoj PUT pa osvezi i dan i istoriju
  const saveBreakfast = async () => { await dietApi.setBreakfast(day, breakfast); await load(); await loadAllDays(); };
  const saveLunch     = async () => { await dietApi.setLunch(day, lunch); await load(); await loadAllDays(); };
  const saveDinner    = async () => { await dietApi.setDinner(day, dinner); await load(); await loadAllDays(); };
  const saveCalories  = async () => { await dietApi.setCalories(day, Number(calories)); await load(); await loadAllDays(); };

  const handleAddSnack = async () => {
    if (!snack) return;
    await dietApi.addSnack(day, snack);
    setSnack("");
    await load();
    await loadAllDays();
  };
  const handleRemoveSnack = async (s: string) => { await dietApi.removeSnack(day, s); await load(); await loadAllDays(); };

  const handleAddSupplement = async () => {
    await dietApi.addSupplement(day, supplement);
    await load();
    await loadAllDays();
  };
  const handleRemoveSupplement = async (s: string) => { await dietApi.removeSupplement(day, s); await load(); await loadAllDays(); };
  const handleDeleteDay = async (dayToDelete: string) => {
    if (!confirm("Obrisati sve uneto za ovaj dan?")) return;
    await dietApi.deleteDay(dayToDelete.slice(0, 10));   
    await load();          
    await loadAllDays();   
  };

  const row = { display: "flex", gap: 8, alignItems: "center" } as const;

  return (
    <div className="page">
      <div className="card" style={{ display: "grid", gap: 16 }}>
        <h2>Ishrana</h2>

        <div style={row}>
          <label>Dan:</label>
          <input type="date" value={day} onChange={(e) => setDay(e.target.value)} />
        </div>
        {error && <p className="error">{error}</p>}

        {/* OBROCI */}
        <div style={row}>
          <span style={{ width: 90 }}>Doručak:</span>
          <input style={{ flex: 1 }} value={breakfast} onChange={(e) => setBreakfast(e.target.value)} />
          <button onClick={saveBreakfast}>Sačuvaj</button>
        </div>
        <div style={row}>
          <span style={{ width: 90 }}>Ručak:</span>
          <input style={{ flex: 1 }} value={lunch} onChange={(e) => setLunch(e.target.value)} />
          <button onClick={saveLunch}>Sačuvaj</button>
        </div>
        <div style={row}>
          <span style={{ width: 90 }}>Večera:</span>
          <input style={{ flex: 1 }} value={dinner} onChange={(e) => setDinner(e.target.value)} />
          <button onClick={saveDinner}>Sačuvaj</button>
        </div>
        <div style={row}>
          <span style={{ width: 90 }}>Kalorije:</span>
          <input type="number" style={{ flex: 1 }} value={calories} onChange={(e) => setCalories(e.target.value)} />
          <button onClick={saveCalories}>Sačuvaj</button>
        </div>

        {/* UZINE */}
        <div>
          <strong>Užine:</strong>
          <ul style={{ margin: "8px 0" }}>
            {(diet?.snacks ?? []).map((s, i) => (
              <li key={i} className="list-item"><span>{s}</span><button onClick={() => handleRemoveSnack(s)}>ukloni</button></li>
            ))}
          </ul>
          <div style={row}>
            <input placeholder="nova užina" value={snack} onChange={(e) => setSnack(e.target.value)} />
            <button onClick={handleAddSnack}>Dodaj</button>
          </div>
        </div>

        {/* DODACI */}
        <div>
          <strong>Dodaci:</strong>
          <ul style={{ margin: "8px 0" }}>
            {(diet?.supplements ?? []).map((s, i) => (
              <li key={i} className="list-item"><span>{s}</span><button onClick={() => handleRemoveSupplement(s)}>ukloni</button></li>
            ))}
          </ul>
          <div style={row}>
            <select value={supplement} onChange={(e) => setSupplement(e.target.value)}>
              <option value="Creatine">Kreatin</option>
              <option value="Protein">Protein</option>
              <option value="VitaminD">Vitamin D</option>
            </select>
            <button onClick={handleAddSupplement}>Dodaj</button>
          </div>
        </div>

        {/* ISTORIJA PO DANIMA */}
        <div style={{ marginTop: 24, borderTop: "1px solid #e0d8a8", paddingTop: 16 }}>
          <h3>Istorija po danima</h3>
          {allDays.length === 0 && <p style={{ color: "#8a8460" }}>Još nema unetih dana.</p>}

          <div style={{ display: "grid", gap: 12 }}>
            {allDays.map((d) => (
              <div key={d.logDay} style={{ padding: 12, background: "rgba(255,255,255,0.7)", borderRadius: 8 }}>
                <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center" }}>
                <strong>{new Date(d.logDay).toLocaleDateString()}</strong>
                <button onClick={() => handleDeleteDay(d.logDay)}>Obriši dan</button>
              </div>
              <div style={{ fontSize: 14, marginTop: 4 }}>
                {d.breakfast && <div>Doručak: {d.breakfast}</div>}
                {d.lunch && <div>Ručak: {d.lunch}</div>}
                {d.dinner && <div>Večera: {d.dinner}</div>}
                {d.calories != null && d.calories > 0 && <div>Kalorije: {d.calories}</div>}
                {d.snacks && d.snacks.length > 0 && <div>Užine: {d.snacks.join(", ")}</div>}
                {d.supplements && d.supplements.length > 0 && <div>Dodaci: {d.supplements.join(", ")}</div>}
              </div>
            </div>
            ))}
          </div>
        </div>

      </div>
    </div>
  );
}