import { Navigate } from "react-router-dom";
import type { ReactNode } from "react";
import { useAuth } from "../context/AuthContext";

export function FemaleOnlyRoute({ children }: { children: ReactNode }) {
  const { user, loading } = useAuth();
  if (loading) return <div>Učitavanje...</div>;
  if (user?.gender !== "Female") return <Navigate to="/" replace />;  
  return <>{children}</>;
}