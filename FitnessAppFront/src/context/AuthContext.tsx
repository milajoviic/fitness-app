import { createContext, useContext, useEffect, useState } from "react";
import type { ReactNode } from "react";
import { authApi } from "../api/auth";
import { tokenStorage } from "../api/tokenStorage";
import type { CurrentUser, LoginDto } from "../types/auth";

interface AuthContextType {
  user: CurrentUser | null;
  isLoggedIn: boolean;
  loading: boolean;
  login: (dto: LoginDto) => Promise<void>;
  logout: () => Promise<void>;
  refreshUser: () => Promise<void>; 
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export function AuthProvider({ children }: { children: ReactNode }) {
  const [user, setUser] = useState<CurrentUser | null>(null);
  const [isLoggedIn, setIsLoggedIn] = useState(!!tokenStorage.getAccess());
  const [loading, setLoading] = useState(true);   

 
  useEffect(() => {
    if (!tokenStorage.getAccess()) {
      setLoading(false);
      return;
    }
    authApi.me()
      .then((u) => { setUser(u); setIsLoggedIn(true); })
      .catch(() => { tokenStorage.clear(); setIsLoggedIn(false); })  
      .finally(() => setLoading(false));
  }, []);

  const login = async (dto: LoginDto) => {
    await authApi.login(dto);          
    setIsLoggedIn(true);
    try { setUser(await authApi.me()); } catch { }
  };

  const logout = async () => {
    await authApi.logout();
    setUser(null);
    setIsLoggedIn(false);
  };
  const refreshUser = async () => {
    try{
      setUser(await authApi.me());
    }
    catch{
    }
  };

  return (
    <AuthContext.Provider value={{ user, isLoggedIn, loading, login, logout, refreshUser}}>
      {children}
    </AuthContext.Provider>
  );
}

export function useAuth() {
  const ctx = useContext(AuthContext);
  if (!ctx) throw new Error("useAuth mora biti unutar AuthProvider-a");
  return ctx;
}