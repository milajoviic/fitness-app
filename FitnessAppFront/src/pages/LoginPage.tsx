import { useState } from "react";
import { useNavigate, Link } from "react-router-dom";
import { Navigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";

export function LoginPage() {
  const { login, isLoggedIn } = useAuth();
  if(isLoggedIn) return <Navigate to="/" replace />;
  const navigate = useNavigate();
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");

  const handleSubmit = async () => {
    setError("");
    try {
      await login({ email, password });
      navigate("/", {replace: true});                       
    } catch {
      setError("Pogrešan email ili lozinka.");
    }
  };

  return (
  <div className="page">
    <div className="card" style={{ maxWidth: 360 }}>
      <h2>Prijava</h2>
      <div style={{ display: "grid", gap: 10 }}>
        <input placeholder="Email" value={email} onChange={(e) => setEmail(e.target.value)} />
        <input placeholder="Lozinka" type="password" value={password} onChange={(e) => setPassword(e.target.value)} />
        <button onClick={handleSubmit}>Prijavi se</button>
        {error && <p className="error">{error}</p>}
        <p>Nemaš nalog? <Link to="/register">Registruj se</Link></p>
      </div>
    </div>
  </div>
);
}