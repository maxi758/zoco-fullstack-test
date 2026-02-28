import { useState } from "react";
import { api } from "../api/client";
import { useAlert } from "./AlertContext";

type Props = {
    userId: string | null;
    isAdmin: boolean;
};

export function PasswordSection({ userId, isAdmin }: Props) {
    const { showAlert } = useAlert();
    const [isEditing, setIsEditing] = useState(false);
    const [password, setPassword] = useState("");
    const [confirmPassword, setConfirmPassword] = useState("");

    const handleCancelEdit = () => {
        setIsEditing(false);
        setPassword("");
        setConfirmPassword("");
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        if (!userId) return;

        if (password !== confirmPassword) {
            showAlert("Atención", "Las contraseñas no coinciden. Por favor, verifícalas.", "warning");
            return;
        }

        try {
            const endpoint = isAdmin ? `/users/${userId}` : `/users/me`;
            const user = await api<any>(endpoint);

            await api(`/users/${userId}`, {
                method: "PUT",
                body: JSON.stringify({
                    ...user,
                    password: password,
                    confirmPassword: confirmPassword
                })
            });
            setIsEditing(false);
            setPassword("");
            setConfirmPassword("");
            showAlert("Seguridad Actualizada", "La contraseña se actualizó correctamente.", "success");
        } catch (e: any) {
            console.error(e);
            showAlert("Error de Seguridad", e.message || "No se pudo actualizar la contraseña.", "error");
        }
    };

    if (!userId) return null;

    return (
        <div className="bg-white rounded-xl shadow-sm border border-gray-200 overflow-hidden mb-8">
            <div className="px-6 py-5 border-b border-gray-200 bg-gray-50 flex justify-between items-center">
                <h3 className="text-lg leading-6 font-medium text-gray-900">Seguridad</h3>
                {!isEditing && (
                    <button
                        onClick={() => setIsEditing(true)}
                        className="text-sm font-medium text-blue-600 hover:text-blue-500 transition-colors"
                    >
                        Cambiar Contraseña
                    </button>
                )}
            </div>

            <div className="p-6">
                {isEditing ? (
                    <form onSubmit={handleSubmit} className="space-y-4 max-w-2xl">
                        <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
                            <div>
                                <label className="block text-sm font-medium text-gray-700">Nueva Contraseña</label>
                                <input
                                    type="password"
                                    required
                                    value={password}
                                    onChange={e => setPassword(e.target.value)}
                                    className="mt-1 block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-blue-500 focus:border-blue-500 sm:text-sm"
                                />
                            </div>
                            <div>
                                <label className="block text-sm font-medium text-gray-700">Confirmar Contraseña</label>
                                <input
                                    type="password"
                                    required
                                    value={confirmPassword}
                                    onChange={e => setConfirmPassword(e.target.value)}
                                    className="mt-1 block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-blue-500 focus:border-blue-500 sm:text-sm"
                                />
                            </div>
                        </div>
                        <div className="flex gap-3 pt-2">
                            <button
                                type="submit"
                                disabled={!password || !confirmPassword}
                                className={`py-2 px-4 border border-transparent rounded-md shadow-sm text-sm font-medium text-white transition-colors ${!password || !confirmPassword ? "bg-gray-300 cursor-not-allowed" : "bg-red-600 hover:bg-red-700"
                                    }`}
                            >
                                Cambiar Contraseña
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
                ) : (
                    <p className="text-sm text-gray-500 italic">La contraseña de esta cuenta está protegida. Presiona "Cambiar Contraseña" para establecer una nueva.</p>
                )}
            </div>
        </div>
    );
}
