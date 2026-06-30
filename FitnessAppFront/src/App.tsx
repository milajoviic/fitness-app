import { Routes, Route, Link } from "react-router-dom";
import { LoginPage } from "./pages/LoginPage";
import { RegisterPage } from "./pages/RegisterPage";
import { ProtectedRoute } from "./components/ProtectedRoute";
import { useAuth } from "./context/AuthContext";
import { WorkoutsPage } from "./pages/WorkoutPage";
import { DietPage } from "./pages/DietPage";
import { BodyMetricsPage } from "./pages/BodyMetricsPage";
import { HealthPage } from "./pages/HealthPage";
import { PeriodPage } from "./pages/PeriodPage";
import { ProfilePage } from "./pages/ProfilePage";

function HomePage() {
  const { user, logout } = useAuth();
  return (
    <div style={{ padding: 40 }}>
      <h1>Zdravo, {user?.firstName}!</h1>
      <nav style={{ display: "grid", gap: 8, margin: "16px 0" }}>
        <Link to="/workouts">Treninzi</Link>
        <Link to="/diet">Ishrana</Link>
        <Link to="/body-metrics">Mere tela</Link>
        <Link to="/health">Zdravlje</Link>
        {user?.gender!=="Male" && (
          <Link to="/period">Ciklus</Link>
        )}
        <Link to="/profile">Moj profil</Link>
      </nav>
      <button onClick={logout}>Odjavi se</button>
    </div>
  );
}

export default function App() {
  return (
     <Routes>
      <Route path="/login" element={<LoginPage />} />
      <Route path="/register" element={<RegisterPage />} />
      <Route path="/" element={<ProtectedRoute><HomePage /></ProtectedRoute>} />
      <Route path="/workouts" element={<ProtectedRoute><WorkoutsPage /></ProtectedRoute>} />
      <Route path="/diet" element={<ProtectedRoute><DietPage /></ProtectedRoute>} />
      <Route path="/body-metrics" element={<ProtectedRoute><BodyMetricsPage /></ProtectedRoute>} />
      <Route path="/health" element={<ProtectedRoute><HealthPage /></ProtectedRoute>} />
      <Route path="/period" element={<ProtectedRoute><PeriodPage /></ProtectedRoute>} />
      <Route path="/profile" element={<ProtectedRoute><ProfilePage /></ProtectedRoute>} />
    </Routes>
  );
}