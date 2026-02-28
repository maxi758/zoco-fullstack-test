// API client configuration
export const API_BASE_URL = import.meta.env.VITE_API_BASE_URL || 'https://localhost:7247/api';

export function getToken() {
    return sessionStorage.getItem("token");
}

export async function api<T>(path: string, options: RequestInit = {}): Promise<T> {
    const token = getToken();

    const headers: HeadersInit = {
        "Content-Type": "application/json",
        ...(options.headers || {}),
        ...(token ? { Authorization: `Bearer ${token}` } : {}),
    };

    const res = await fetch(`${API_BASE_URL}${path}`, { ...options, headers });

    if (!res.ok) {
        let msg = `Error ${res.status}`;
        try {
            const body = await res.json();
            msg = body?.message || msg;
        } catch { }
        throw new Error(msg);
    }

    if (res.status === 204) return undefined as T;

    const text = await res.text();
    if (!text) return undefined as T;

    try {
        return JSON.parse(text) as T;
    } catch {
        return text as any as T;
    }
}
