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
    setError(""); 
    setMsg("");
    try {
      await userApi.update({ firstName, lastName, gender, birthDate });
      setMsg("Profil sačuvan.");
    } catch {
      setError("Greška pri izmeni.");
    }
  };

  const handleDelete = async () => {
    if (!confirm("Da li ste sigurni da želie trajno da obrišete nalog?")) return;
    try {
      await userApi.remove();
      await logout();          
      navigate("/register");   
    } catch {
      setError("Greška pri brisanju naloga.");
    }
  };

  return (
  <div className="page">
    <div className="card" style={{ maxWidth: 400, display: "grid", gap: 10 }}>
      <h2>Moj profil</h2>
      <p style={{ color: "#8a8460" }}>{user?.email}</p>

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
      {msg && <p className="success">{msg}</p>}
      {error && <p className="error">{error}</p>}

      <hr style={{ margin: "16px 0", border: "none", borderTop: "1px solid #e0d8a8" }} />
      <button onClick={handleDelete} style={{ background: "#e0a0a0", color: "#fff" }}>Obriši nalog</button>
    </div>
  </div>
    );
}