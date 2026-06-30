import { useEffect, useState } from "react";
import { dietApi } from "../api/diet";
import type { Diet } from "../types/diet";

const today = () => new Date().toISOString().slice(0, 10);

export function DietPage() {
  const [day, setDay] = useState(today());
  const [diet, setDiet] = useState<Diet | null>(null);
  const [error, setError] = useState("");

  const [breakfast, setBreakfast] = useState("");
  const [lunch, setLunch] = useState("");
  const [dinner, setDinner] = useState("");
  const [calories, setCalories] = useState("");
  const [snack, setSnack] = useState("");
  const [supplement, setSupplement] = useState("Creatine");

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

  useEffect(() => { load(); }, [day]);   

  const saveBreakfast = async () => { await dietApi.setBreakfast(day, breakfast); await load(); };
  const saveLunch     = async () => { await dietApi.setLunch(day, lunch); await load(); };
  const saveDinner    = async () => { await dietApi.setDinner(day, dinner); await load(); };
  const saveCalories  = async () => { await dietApi.setCalories(day, Number(calories)); await load(); };

  const handleAddSnack = async () => {
    if (!snack) return;
    await dietApi.addSnack(day, snack);
    setSnack("");
    await load();
  };
  const handleRemoveSnack = async (s: string) => { await dietApi.removeSnack(day, s); await load(); };

  const handleAddSupplement = async () => {
    await dietApi.addSupplement(day, supplement);
    await load();
  };
  const handleRemoveSupplement = async (s: string) => { await dietApi.removeSupplement(day, s); await load(); };

  const row = { display: "flex", gap: 8, alignItems: "center" } as const;

  return (
    <div style={{ maxWidth: 600, margin: "40px auto", display: "grid", gap: 16 }}>
      <h2>Ishrana</h2>

      <div style={row}>
        <label>Dan:</label>
        <input type="date" value={day} onChange={(e) => setDay(e.target.value)} />
      </div>
      {error && <p style={{ color: "red" }}>{error}</p>}

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

      <div>
        <strong>Užine:</strong>
        <ul>
          {(diet?.snacks ?? []).map((s, i) => (
            <li key={i}>{s} <button onClick={() => handleRemoveSnack(s)}>ukloni</button></li>
          ))}
        </ul>
        <div style={row}>
          <input placeholder="nova užina" value={snack} onChange={(e) => setSnack(e.target.value)} />
          <button onClick={handleAddSnack}>Dodaj</button>
        </div>
      </div>

      <div>
        <strong>Dodaci:</strong>
        <ul>
          {(diet?.supplements ?? []).map((s, i) => (
            <li key={i}>{s} <button onClick={() => handleRemoveSupplement(s)}>ukloni</button></li>
          ))}
        </ul>
        <div style={row}>
          <select value={supplement} onChange={(e) => setSupplement(e.target.value)}>
            <option value="Antioxidants">Antioksidanti</option>
            <option value="Protein">Protein</option>
            <option value="CreatineMono">Kreatin Monohidrat</option>
            <option value="Caffeine">Kofein</option>
            <option value="Iron">Gvožđe</option>
          </select>
          <button onClick={handleAddSupplement}>Dodaj</button>
        </div>
      </div>
    </div>
  );
}