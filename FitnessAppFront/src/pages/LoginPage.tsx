import { useState } from "react";
import { useNavigate, Link } from "react-router-dom";
import { useAuth } from "../context/AuthContext";

export function LoginPage() {
  const { login } = useAuth();
  const navigate = useNavigate();
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");

  const handleSubmit = async () => {
    setError("");
    try {
      await login({ email, password });
      navigate("/");                       
    } catch {
      setError("Pogrešan email ili lozinka.");
    }
  };

  return (
    <div style={{ maxWidth: 320, margin: "80px auto", display: "grid", gap: 8 }}>
      <h2>Prijava</h2>
      <input placeholder="Email" value={email} onChange={(e) => setEmail(e.target.value)} />
      <input placeholder="Lozinka" type="password" value={password} onChange={(e) => setPassword(e.target.value)} />
      <button onClick={handleSubmit}>Prijavi se</button>
      {error && <p style={{ color: "red" }}>{error}</p>}
      <p>Nemaš nalog? <Link to="/register">Registruj se</Link></p>
    </div>
  );
}