import { useState } from "react";
import { useNavigate, Link } from "react-router-dom";
import { authApi } from "../api/auth";

export function RegisterPage() {
  const navigate = useNavigate();
  const [form, setForm] = useState({
    email: "", password: "", firstName: "", lastName: "",
    gender: "Female", birthDate: "",
  });
  const [error, setError] = useState("");

  const update = (field: string, value: string) => setForm({ ...form, [field]: value });

  const handleSubmit = async () => {
    setError("");
    try {
      await authApi.register(form);
      navigate("/login", {replace: true});                 
    } catch (err: any) {
      setError(err.response?.data ?? "Greška pri registraciji.");
    }
  };

  return (
  <div className="page">
    <div className="card" style={{ maxWidth: 360 }}>
      <h2>Registracija</h2>
      <div style={{ display: "grid", gap: 10 }}>
        <input placeholder="Email" value={form.email} onChange={(e) => update("email", e.target.value)} />
        <input placeholder="Lozinka" type="password" value={form.password} onChange={(e) => update("password", e.target.value)} />
        <input placeholder="Ime" value={form.firstName} onChange={(e) => update("firstName", e.target.value)} />
        <input placeholder="Prezime" value={form.lastName} onChange={(e) => update("lastName", e.target.value)} />
        <select value={form.gender} onChange={(e) => update("gender", e.target.value)}>
          <option value="Female">Žensko</option>
          <option value="Male">Muško</option>
        </select>
        <input type="date" value={form.birthDate} onChange={(e) => update("birthDate", e.target.value)} />
        <button onClick={handleSubmit}>Registruj se</button>
        {error && <p className="error">{error}</p>}
        <p>Već imaš nalog? <Link to="/login">Prijavi se</Link></p>
      </div>
    </div>
  </div>
  );
}