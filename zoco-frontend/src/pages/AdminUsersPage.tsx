import { useEffect, useState } from "react";
import { api } from "../api/client";
import type { UserDto, Role } from "../types/dto";
import { useAlert } from "../components/AlertContext";

export function AdminUsersPage() {
    const { showAlert } = useAlert();
    const [users, setUsers] = useState<UserDto[]>([]);
    const [loading, setLoading] = useState(true);
    const [editingUser, setEditingUser] = useState<UserDto | null>(null);

    const fetchUsers = async () => {
        setLoading(true);
        try {
            const data = await api<UserDto[]>("/users");
            setUsers(data);
        } catch (e) {
            console.error("Error fetching users", e);
            showAlert("Error al cargar", "No se pudieron obtener los usuarios.", "error");
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchUsers();
    }, []);

    const handleDelete = async (id: string) => {
        if (!confirm("¿Eliminar usuario permanentemente?")) return;
        try {
            await api(`/ users / ${id} `, { method: "DELETE" });
            fetchUsers();
            showAlert("Usuario Eliminado", "El usuario ha sido borrado exitosamente del sistema.", "info");
        } catch (e: any) {
            showAlert("Error al eliminar", e.message || "No se pudo eliminar el usuario.", "error");
        }
    };

    const handleUpdate = async (e: React.FormEvent) => {
        e.preventDefault();
        if (!editingUser) return;
        try {
            await api(`/ users / ${editingUser.id} `, {
                method: "PUT",
                body: JSON.stringify(editingUser)
            });
            setEditingUser(null);
            fetchUsers();
            showAlert("Usuario Actualizado", "Los datos del usuario han sido modificados correctamente.", "success");
        } catch (e: any) {
            showAlert("Error al actualizar", e.message || "No se pudo actualizar la información del usuario.", "error");
        }
    };

    return (
        <div className="max-w-7xl mx-auto my-8 px-4 sm:px-6 lg:px-8">
            <div className="flex flex-col sm:flex-row justify-between items-start sm:items-center mb-8">
                <div>
                    <h2 className="text-3xl font-extrabold text-gray-900 tracking-tight">Gestión de Usuarios</h2>
                    <p className="mt-1 text-sm text-gray-500">Panel de administración para visualizar, editar y eliminar usuarios del sistema.</p>
                </div>
                <a
                    href="/"
                    className="mt-4 sm:mt-0 inline-flex items-center text-sm font-medium text-blue-600 hover:text-blue-500 transition-colors"
                >
                    <span aria-hidden="true" className="mr-2">←</span> Volver al Dashboard
                </a>
            </div>

            {loading ? (
                <div className="animate-pulse flex space-x-4 p-6 bg-white rounded-xl shadow-sm border border-gray-200">
                    <div className="flex-1 space-y-4 py-1">
                        <div className="h-4 bg-gray-200 rounded w-1/4"></div>
                        <div className="space-y-3">
                            <div className="h-8 bg-gray-200 rounded"></div>
                            <div className="h-8 bg-gray-200 rounded"></div>
                            <div className="h-8 bg-gray-200 rounded"></div>
                        </div>
                    </div>
                </div>
            ) : (
                <div className="bg-white shadow-sm rounded-xl border border-gray-200 overflow-hidden">
                    <div className="overflow-x-auto">
                        <table className="min-w-full divide-y divide-gray-200">
                            <thead className="bg-gray-50">
                                <tr>
                                    <th scope="col" className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">ID</th>
                                    <th scope="col" className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Nombre</th>
                                    <th scope="col" className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Email</th>
                                    <th scope="col" className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Rol</th>
                                    <th scope="col" className="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase tracking-wider">Acciones</th>
                                </tr>
                            </thead>
                            <tbody className="bg-white divide-y divide-gray-200">
                                {users.map(u => (
                                    <tr key={u.id} className="hover:bg-gray-50 transition-colors">
                                        <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500 font-mono" title={u.id}>
                                            {u.id.substring(0, 8)}...
                                        </td>
                                        <td className="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">
                                            {u.firstName || u.lastName ? `${u.firstName || ""} ${u.lastName || ""} `.trim() : <span className="text-gray-400 italic">No especificado</span>}
                                        </td>
                                        <td className="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">
                                            {u.email}
                                        </td>
                                        <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                                            <span className={`inline - flex items - center px - 2.5 py - 0.5 rounded - full text - xs font - medium ${u.role === "Admin" ? "bg-red-100 text-red-800" : "bg-green-100 text-green-800"
                                                } `}>
                                                {u.role}
                                            </span>
                                        </td>
                                        <td className="px-6 py-4 whitespace-nowrap text-right text-sm font-medium">
                                            <button
                                                onClick={() => setEditingUser({ ...u })}
                                                className="text-blue-600 hover:text-blue-900 transition-colors mr-4"
                                            >
                                                Editar
                                            </button>
                                            <button
                                                onClick={() => handleDelete(u.id)}
                                                className="text-red-600 hover:text-red-900 transition-colors"
                                            >
                                                Eliminar
                                            </button>
                                        </td>
                                    </tr>
                                ))}
                                {users.length === 0 && (
                                    <tr>
                                        <td colSpan={5} className="px-6 py-10 text-center text-sm text-gray-500 italic">
                                            No hay usuarios registrados en el sistema.
                                        </td>
                                    </tr>
                                )}
                            </tbody>
                        </table>
                    </div>
                </div>
            )}

            {editingUser && (
                <div className="mt-8 bg-white shadow-sm rounded-xl border border-gray-200 p-6 max-w-2xl">
                    <h3 className="text-lg leading-6 font-medium text-gray-900 mb-5">Editar Usuario</h3>
                    <form onSubmit={handleUpdate} className="space-y-5">
                        <div className="grid grid-cols-1 sm:grid-cols-2 gap-5">
                            <div>
                                <label className="block text-sm font-medium text-gray-700 mb-1">Nombre</label>
                                <input
                                    type="text"
                                    value={editingUser.firstName || ""}
                                    onChange={e => setEditingUser({ ...editingUser, firstName: e.target.value })}
                                    className="block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-blue-500 focus:border-blue-500 sm:text-sm"
                                />
                            </div>
                            <div>
                                <label className="block text-sm font-medium text-gray-700 mb-1">Apellido</label>
                                <input
                                    type="text"
                                    value={editingUser.lastName || ""}
                                    onChange={e => setEditingUser({ ...editingUser, lastName: e.target.value })}
                                    className="block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-blue-500 focus:border-blue-500 sm:text-sm"
                                />
                            </div>
                        </div>
                        <div className="grid grid-cols-1 sm:grid-cols-2 gap-5">
                            <div>
                                <label className="block text-sm font-medium text-gray-700 mb-1">Email</label>
                                <input
                                    type="email"
                                    required
                                    value={editingUser.email}
                                    onChange={e => setEditingUser({ ...editingUser, email: e.target.value })}
                                    className="block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-blue-500 focus:border-blue-500 sm:text-sm"
                                />
                            </div>
                            <div>
                                <label className="block text-sm font-medium text-gray-700 mb-1">Rol</label>
                                <select
                                    value={editingUser.role}
                                    onChange={e => setEditingUser({ ...editingUser, role: e.target.value as Role })}
                                    className="block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-blue-500 focus:border-blue-500 sm:text-sm"
                                >
                                    <option value="User">User</option>
                                    <option value="Admin">Admin</option>
                                </select>
                            </div>
                        </div>
                        <div className="flex justify-end gap-3 pt-4 border-t border-gray-100">
                            <button
                                type="button"
                                onClick={() => setEditingUser(null)}
                                className="bg-white py-2 px-4 border border-gray-300 rounded-md shadow-sm text-sm font-medium text-gray-700 hover:bg-gray-50 transition-colors"
                            >
                                Cancelar
                            </button>
                            <button
                                type="submit"
                                disabled={!editingUser.email}
                                className={`py - 2 px - 4 border border - transparent rounded - md shadow - sm text - sm font - medium text - white transition - colors ${!editingUser.email ? "bg-gray-300 cursor-not-allowed" : "bg-blue-600 hover:bg-blue-700"
                                    } `}
                            >
                                Guardar Cambios
                            </button>
                        </div>
                    </form>
                </div>
            )}
        </div>
    );
}
