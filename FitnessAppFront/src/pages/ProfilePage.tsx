import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { userApi } from "../api/user";
import { useAuth } from "../context/AuthContext";

export function ProfilePage() {
  const { user, logout } = useAuth();
  const navigate = useNavigate();
  const [error, setError] = useState("");
  const [msg, setMsg] = useState("");

  const [firstName, setFirstName] = useState(user?.firstName ?? "");
  const [lastName, setLastName] = useState(user?.lastName ?? "");
  const [gender, setGender] = useState(user?.gender ?? "Female");
  const [birthDate, setBirthDate] = useState(user?.birthDate?.slice(0, 10) ?? "");

  const handleUpdate = async () => {
    setError(""); setMsg("");
    try {
      await userApi.update({ firstName, lastName, gender, birthDate });
      setMsg("Profil sačuvan.");
    } catch {
      setError("Greška pri izmeni.");
    }
  };

  const handleDelete = async () => {
    if (!confirm("Sigurno želiš da obrišeš nalog? Ovo je trajno.")) return;
    try {
      await userApi.remove();
      await logout();          
      navigate("/register");   
    } catch {
      setError("Greška pri brisanju naloga.");
    }
  };

  return (
    <div style={{ maxWidth: 400, margin: "40px auto", display: "grid", gap: 8 }}>
      <h2>Moj profil</h2>
      <p style={{ color: "#888" }}>{user?.email}</p>

      <label>Ime: <input value={firstName} onChange={(e) => setFirstName(e.target.value)} /></label>
      <label>Prezime: <input value={lastName} onChange={(e) => setLastName(e.target.value)} /></label>
      <label>Pol:
        <select value={gender} onChange={(e) => setGender(e.target.value)}>
          <option value="Female">Žensko</option>
          <option value="Male">Muško</option>
        </select>
      </label>
      <label>Datum rođenja: <input type="date" value={birthDate} onChange={(e) => setBirthDate(e.target.value)} /></label>

      <button onClick={handleUpdate}>Sačuvaj izmene</button>
      {msg && <p style={{ color: "green" }}>{msg}</p>}
      {error && <p style={{ color: "red" }}>{error}</p>}

      <hr style={{ margin: "16px 0" }} />
      <button onClick={handleDelete} style={{ color: "red" }}>Obriši nalog</button>
    </div>
  );
}