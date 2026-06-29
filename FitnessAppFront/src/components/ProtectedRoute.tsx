import { Navigate } from "react-router-dom";
import type { ReactNode } from "react";
import { useAuth } from "../context/AuthContext";

export function ProtectedRoute({ children }: { children: ReactNode }) {
  const { isLoggedIn, loading } = useAuth();

  if (loading) return <div>Učitavanje...</div>;          
  if (!isLoggedIn) return <Navigate to="/login" replace />;  
  return <>{children}</>;
}