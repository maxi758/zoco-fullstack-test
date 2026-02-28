import { useEffect, useState } from "react";
import { api } from "../api/client";
import type { AddressDto } from "../types/dto";
import { useAlert } from "./AlertContext";

type Props = {
    userId: string | null;
    isAdmin: boolean;
};

export function AddressesSection({ userId, isAdmin }: Props) {
    const { showAlert } = useAlert();
    const [addresses, setAddresses] = useState<AddressDto[]>([]);
    const [loading, setLoading] = useState(false);
    const [isEditing, setIsEditing] = useState(false);
    const [editingAddressId, setEditingAddressId] = useState<number | null>(null);
    const [street, setStreet] = useState("");
    const [city, setCity] = useState("");
    const [stateField, setStateField] = useState("");
    const [zipCode, setZipCode] = useState("");
    const [country, setCountry] = useState("");

    const fetchAddresses = async () => {
        if (!userId) {
            setAddresses([]);
            return;
        }

        setLoading(true);
        try {
            const endpoint = isAdmin && userId ? `/addresses/user/${userId}` : `/addresses`;
            const data = await api<AddressDto[]>(endpoint);
            setAddresses(data);
        } catch (e) {
            console.error(e);
            showAlert("Error al cargar", "No se pudieron obtener las direcciones.", "error");
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchAddresses();
    }, [userId, isAdmin]);

    const handleEdit = (a: AddressDto) => {
        setEditingAddressId(a.id);
        setStreet(a.street);
        setCity(a.city);
        setStateField(a.state || "");
        setZipCode(a.zipCode);
        setCountry(a.country);
        setIsEditing(true);
    };

    const handleAdd = () => {
        setEditingAddressId(null);
        setStreet("");
        setCity("");
        setStateField("");
        setZipCode("");
        setCountry("");
        setIsEditing(true);
    };

    const handleCancelEdit = () => {
        setIsEditing(false);
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        try {
            const bodyObj: any = { userId, street, city, country, zipCode };
            if (stateField) {
                bodyObj.state = stateField;
            }
            const body = JSON.stringify(bodyObj);

            if (editingAddressId) {
                await api(`/addresses/${editingAddressId}`, { method: "PUT", body });
            } else {
                await api(`/addresses`, { method: "POST", body });
            }
            setIsEditing(false);
            fetchAddresses();
            showAlert("Éxito", "La dirección se ha guardado correctamente.", "success");
        } catch (e: any) {
            console.error(e);
            showAlert("Error", "Error al guardar la dirección: " + (e.message || ""), "error");
        }
    };

    const handleDelete = async (addressId: number) => {
        if (!window.confirm("¿Estás seguro de que deseas eliminar esta dirección?")) return;
        try {
            await api(`/addresses/${addressId}`, { method: "DELETE" });
            fetchAddresses();
            showAlert("Eliminada", "La dirección se eliminó correctamente.", "info");
        } catch (e: any) {
            console.error(e);
            showAlert("Error", "Error al eliminar la dirección: " + (e.message || ""), "error");
        }
    };

    if (!userId && isAdmin) return null;

    return (
        <div className="bg-white rounded-xl shadow-sm border border-gray-200 overflow-hidden mb-8">
            <div className="px-6 py-5 border-b border-gray-200 bg-gray-50 flex justify-between items-center">
                <h3 className="text-lg leading-6 font-medium text-gray-900">Direcciones</h3>
                {!isEditing && (
                    <button
                        onClick={handleAdd}
                        className="inline-flex items-center px-3 py-1.5 border border-transparent text-xs font-medium rounded-md shadow-sm text-white bg-blue-600 hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500 transition-colors"
                    >
                        Añadir Dirección
                    </button>
                )}
            </div>

            <div className="p-6">
                {loading ? (
                    <div className="animate-pulse flex space-x-4">
                        <div className="flex-1 space-y-4 py-1">
                            <div className="h-4 bg-gray-200 rounded w-3/4"></div>
                            <div className="h-4 bg-gray-200 rounded w-1/2"></div>
                        </div>
                    </div>
                ) : isEditing ? (
                    <form onSubmit={handleSubmit} className="space-y-4">
                        <div>
                            <label className="block text-sm font-medium text-gray-700">Calle</label>
                            <input
                                type="text"
                                required
                                value={street}
                                onChange={e => setStreet(e.target.value)}
                                className="mt-1 block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-blue-500 focus:border-blue-500 sm:text-sm"
                            />
                        </div>
                        <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
                            <div>
                                <label className="block text-sm font-medium text-gray-700">Ciudad</label>
                                <input
                                    type="text"
                                    required
                                    value={city}
                                    onChange={e => setCity(e.target.value)}
                                    className="mt-1 block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-blue-500 focus:border-blue-500 sm:text-sm"
                                />
                            </div>
                            <div>
                                <label className="block text-sm font-medium text-gray-700">Provincia/Estado (Opcional)</label>
                                <input
                                    type="text"
                                    value={stateField}
                                    onChange={e => setStateField(e.target.value)}
                                    className="mt-1 block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-blue-500 focus:border-blue-500 sm:text-sm"
                                />
                            </div>
                        </div>
                        <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
                            <div>
                                <label className="block text-sm font-medium text-gray-700">Código Postal</label>
                                <input
                                    type="text"
                                    required
                                    value={zipCode}
                                    onChange={e => setZipCode(e.target.value)}
                                    className="mt-1 block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-blue-500 focus:border-blue-500 sm:text-sm"
                                />
                            </div>
                            <div>
                                <label className="block text-sm font-medium text-gray-700">País</label>
                                <input
                                    type="text"
                                    required
                                    value={country}
                                    onChange={e => setCountry(e.target.value)}
                                    className="mt-1 block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-blue-500 focus:border-blue-500 sm:text-sm"
                                />
                            </div>
                        </div>
                        <div className="flex gap-3 pt-2">
                            <button
                                type="submit"
                                disabled={!street || !city || !zipCode || !country}
                                className={`inline-flex justify-center py-2 px-4 border border-transparent shadow-sm text-sm font-medium rounded-md text-white transition-colors ${(!street || !city || !zipCode || !country) ? "bg-gray-300 cursor-not-allowed" :
                                    "bg-blue-600 hover:bg-blue-700"
                                    }`}
                            >
                                {editingAddressId ? "Guardar Cambios" : "Crear Dirección"}
                            </button>
                            <button
                                type="button"
                                onClick={handleCancelEdit}
                                className="bg-white py-2 px-4 border border-gray-300 rounded-md shadow-sm text-sm font-medium text-gray-700 hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500 transition-colors"
                            >
                                Cancelar
                            </button>
                        </div>
                    </form>
                ) : addresses.length === 0 ? (
                    <p className="text-sm text-gray-500 italic">No hay direcciones registradas para este usuario.</p>
                ) : (
                    <ul className="divide-y divide-gray-200">
                        {addresses.map(a => (
                            <li key={a.id} className="py-4 flex justify-between items-start">
                                <div>
                                    <p className="text-sm font-medium text-gray-900">{a.street}</p>
                                    <p className="text-sm text-gray-500">
                                        {a.city}{a.state ? `, ${a.state}` : ""}, {a.zipCode} - {a.country}
                                    </p>
                                </div>
                                <div className="flex space-x-3 ml-4">
                                    <button
                                        onClick={() => handleEdit(a)}
                                        className="text-sm font-medium text-blue-600 hover:text-blue-500 transition-colors"
                                    >
                                        Editar
                                    </button>
                                    <button
                                        onClick={() => handleDelete(a.id)}
                                        className="text-sm font-medium text-red-600 hover:text-red-500 transition-colors"
                                    >
                                        Eliminar
                                    </button>
                                </div>
                            </li>
                        ))}
                    </ul>
                )}
            </div>
        </div>
    );
}
