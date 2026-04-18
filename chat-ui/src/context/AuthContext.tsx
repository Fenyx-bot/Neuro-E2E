import React, { createContext, useContext, useState, useEffect } from "react";
import { authCurrentUser } from "../api/auth";
import { User } from "../types";

interface AuthContextType {
  token: string | null;
  login: (token: string) => void;
  logout: () => void;
  isAuthenticated: boolean;
  fetchCurrentUser: () => void;
  user: User | null;
}

const AuthContext = createContext<AuthContextType | null>(null);

export const AuthProvider: React.FC<{ children: React.ReactNode }> = ({
  children,
}) => {
  const [token, setToken] = useState<string | null>(
    localStorage.getItem("token"),
  );

  const [user, setUser] = useState<User | null>(null);

  const login = (newToken: string) => {
    setToken(newToken);
    localStorage.setItem("token", newToken);

    fetchCurrentUser();
  };

  const logout = () => {
    setToken(null);
    localStorage.removeItem("token");
  };

  const fetchCurrentUser = async () => {
    if (user) {
      return;
    }

    if (!token) {
      return;
    }

    const fetchedUser = await authCurrentUser();
    setUser(fetchedUser);

    console.log("Current user: ", user);
  };

  const value = {
    token,
    login,
    fetchCurrentUser,
    user,
    logout,
    isAuthenticated: !!token,
  };

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
};

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error("useAuth must be used within an AuthProvider");
  }
  return context;
};
