import { useEffect, useState } from "react";
import { api } from "../api/client";
import type { UserDto } from "../types/dto";
import { useAlert } from "./AlertContext";

type Props = {
    userId: string | null;
    isAdmin: boolean;
};

export function ProfileSection({ userId, isAdmin }: Props) {
    const { showAlert } = useAlert();
    const [user, setUser] = useState<UserDto | null>(null);
    const [loading, setLoading] = useState(false);
    const [isEditing, setIsEditing] = useState(false);
    const [firstName, setFirstName] = useState("");
    const [lastName, setLastName] = useState("");
    const [email, setEmail] = useState("");

    const fetchUser = async () => {
        if (!userId) return;
        setLoading(true);
        try {
            const endpoint = isAdmin ? `/users/${userId}` : `/users/me`;
            const data = await api<UserDto>(endpoint);
            setUser(data);
            setFirstName(data.firstName || "");
            setLastName(data.lastName || "");
            setEmail(data.email || "");
        } catch (e) {
            console.error("Error fetching user profile", e);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchUser();
    }, [userId]);

    const handleCancelEdit = () => {
        setIsEditing(false);
        if (user) {
            setFirstName(user.firstName || "");
            setLastName(user.lastName || "");
            setEmail(user.email || "");
        }
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        if (!userId || !user) return;

        try {
            await api(`/users/${userId}`, {
                method: "PUT",
                body: JSON.stringify({
                    ...user,
                    firstName,
                    lastName,
                    email
                })
            });
            setIsEditing(false);
            fetchUser();
            showAlert("Perfil Actualizado", "Los datos de tu perfil han sido guardados correctamente.", "success");
        } catch (e: any) {
            console.error(e);
            showAlert("Error al actualizar", e.message || "Ocurrió un error inesperado al guardar tu perfil.", "error");
        }
    };

    if (!userId) return null;

    return (
        <div className="bg-white rounded-xl shadow-sm border border-gray-200 overflow-hidden mb-8">
            <div className="px-6 py-5 border-b border-gray-200 bg-gray-50 flex justify-between items-center">
                <h3 className="text-lg leading-6 font-medium text-gray-900">Mi Perfil</h3>
                {!isEditing && (
                    <button
                        onClick={() => setIsEditing(true)}
                        className="text-sm font-medium text-blue-600 hover:text-blue-500 transition-colors"
                    >
                        Editar Datos
                    </button>
                )}
            </div>

            <div className="p-6">
                {loading ? (
                    <div className="animate-pulse flex space-x-4">
                        <div className="flex-1 space-y-4 py-1">
                            <div className="h-4 bg-gray-200 rounded w-1/4"></div>
                            <div className="h-4 bg-gray-200 rounded w-1/2"></div>
                        </div>
                    </div>
                ) : isEditing ? (
                    <form onSubmit={handleSubmit} className="space-y-4 max-w-2xl">
                        <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
                            <div>
                                <label className="block text-sm font-medium text-gray-700">Nombre</label>
                                <input
                                    type="text"
                                    value={firstName}
                                    onChange={e => setFirstName(e.target.value)}
                                    className="mt-1 block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-blue-500 focus:border-blue-500 sm:text-sm"
                                />
                            </div>
                            <div>
                                <label className="block text-sm font-medium text-gray-700">Apellido</label>
                                <input
                                    type="text"
                                    value={lastName}
                                    onChange={e => setLastName(e.target.value)}
                                    className="mt-1 block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-blue-500 focus:border-blue-500 sm:text-sm"
                                />
                            </div>
                        </div>
                        <div>
                            <label className="block text-sm font-medium text-gray-700">Email</label>
                            <input
                                type="email"
                                required
                                value={email}
                                onChange={e => setEmail(e.target.value)}
                                className="mt-1 block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-blue-500 focus:border-blue-500 sm:text-sm"
                            />
                        </div>

                        <div className="flex gap-3 pt-2">
                            <button
                                type="submit"
                                disabled={!email}
                                className={`py-2 px-4 border border-transparent rounded-md shadow-sm text-sm font-medium text-white transition-colors ${!email ? "bg-gray-300 cursor-not-allowed" : "bg-blue-600 hover:bg-blue-700"
                                    }`}
                            >
                                Guardar Cambios
                            </button>
                            <button
                                type="button"
                                onClick={handleCancelEdit}
                                className="bg-white py-2 px-4 border border-gray-300 rounded-md shadow-sm text-sm font-medium text-gray-700 hover:bg-gray-50 transition-colors"
                            >
                                Cancelar
                            </button>
                        </div>
                    </form>
                ) : user ? (
                    <dl className="grid grid-cols-1 sm:grid-cols-2 gap-x-4 gap-y-6">
                        <div className="sm:col-span-1">
                            <dt className="text-sm font-medium text-gray-500">Nombre Completo</dt>
                            <dd className="mt-1 text-sm text-gray-900">
                                {user.firstName || user.lastName
                                    ? `${user.firstName || ""} ${user.lastName || ""}`.trim()
                                    : <span className="text-gray-400 italic">No especificado</span>}
                            </dd>
                        </div>
                        <div className="sm:col-span-1">
                            <dt className="text-sm font-medium text-gray-500">Email</dt>
                            <dd className="mt-1 text-sm text-gray-900">{user.email}</dd>
                        </div>
                        <div className="sm:col-span-1">
                            <dt className="text-sm font-medium text-gray-500">Rol Sistema</dt>
                            <dd className="mt-1 text-sm text-gray-900 flex items-center">
                                <span className={`inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium ${user.role === "Admin" ? "bg-red-100 text-red-800" : "bg-green-100 text-green-800"
                                    }`}>
                                    {user.role}
                                </span>
                            </dd>
                        </div>
                    </dl>
                ) : (
                    <p className="text-sm text-gray-500 italic">No se pudo cargar la información del perfil.</p>
                )}
            </div>
        </div>
    );
}
