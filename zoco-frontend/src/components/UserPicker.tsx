import { useEffect, useState } from "react";
import { api } from "../api/client";
import type { UserDto } from "../types/dto";

type Props = {
    value: string | null;
    onChange: (val: string | null) => void;
};

export function UserPicker({ value, onChange }: Props) {
    const [users, setUsers] = useState<UserDto[]>([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        api<UserDto[]>("/users")
            .then(setUsers)
            .finally(() => setLoading(false));
    }, []);

    if (loading) return <div className="text-sm text-gray-500 animate-pulse">Cargando usuarios...</div>;

    return (
        <select
            value={value || ""}
            onChange={e => onChange(e.target.value || null)}
            className="block w-full pl-3 pr-10 py-2 text-base border-gray-300 focus:outline-none focus:ring-blue-500 focus:border-blue-500 sm:text-sm rounded-md shadow-sm"
        >
            <option value="">-- Seleccionar usuario --</option>
            {users.map(u => (
                <option key={u.id} value={u.id}>{u.email} ({u.role})</option>
            ))}
        </select>
    );
}
