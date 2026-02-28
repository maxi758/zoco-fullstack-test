import React, { createContext, useContext, useMemo, useState } from "react";
import { api } from "../api/client";

type AuthState = {
    token: string | null;
    role: "Admin" | "User" | null;
    userId: string | null;
    email: string | null;
};

type LoginRequest = { email: string; password: string };
type LoginResponse = { token: string; role: string; userId: string; email: string };

type AuthContextValue = AuthState & {
    login: (req: LoginRequest) => Promise<void>;
    logout: () => void;
};

const AuthContext = createContext<AuthContextValue | null>(null);

export function AuthProvider({ children }: { children: React.ReactNode }) {
    const [token, setToken] = useState<string | null>(sessionStorage.getItem("token"));
    const [role, setRole] = useState<AuthState["role"]>((sessionStorage.getItem("role") as any) || null);
    const [userId, setUserId] = useState<string | null>(sessionStorage.getItem("userId"));
    const [email, setEmail] = useState<string | null>(sessionStorage.getItem("email"));

    async function login(req: LoginRequest) {
        const res = await api<LoginResponse>("/auth/login", {
            method: "POST",
            body: JSON.stringify(req),
        });

        sessionStorage.setItem("token", res.token);
        sessionStorage.setItem("role", res.role);
        sessionStorage.setItem("userId", res.userId);
        sessionStorage.setItem("email", res.email);

        setToken(res.token);
        setRole(res.role as any);
        setUserId(res.userId);
        setEmail(res.email);
    }

    function logout() {
        sessionStorage.removeItem("token");
        sessionStorage.removeItem("role");
        sessionStorage.removeItem("userId");
        sessionStorage.removeItem("email");
        setToken(null);
        setRole(null);
        setUserId(null);
        setEmail(null);
    }

    const value = useMemo(
        () => ({ token, role, userId, email, login, logout }),
        [token, role, userId, email]
    );

    return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}

export function useAuth() {
    const ctx = useContext(AuthContext);
    if (!ctx) throw new Error("AuthProvider missing");
    return ctx;
}