import { useEffect, useState } from "react";
import { api } from "../api/client";
import type { StudyDto } from "../types/dto";
import { useAlert } from "./AlertContext";

type Props = {
    userId: string | null;
    isAdmin: boolean;
};

export function StudiesSection({ userId, isAdmin }: Props) {
    const { showAlert } = useAlert();
    const [studies, setStudies] = useState<StudyDto[]>([]);
    const [loading, setLoading] = useState(false);

    // form state
    const [isEditing, setIsEditing] = useState(false);
    const [editingStudyId, setEditingStudyId] = useState<number | null>(null);
    const [title, setTitle] = useState("");
    const [institution, setInstitution] = useState("");
    const [startDate, setStartDate] = useState("");
    const [endDate, setEndDate] = useState("");

    const fetchStudies = async () => {
        if (!userId) {
            setStudies([]);
            return;
        }
        setLoading(true);
        try {
            const endpoint = isAdmin && userId ? `/studies/user/${userId}` : `/studies`;
            const data = await api<StudyDto[]>(endpoint);
            setStudies(data);
        } catch (e) {
            console.error(e);
            showAlert("Error al cargar", "No se pudieron obtener los estudios.", "error");
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchStudies();
    }, [userId, isAdmin]);

    const handleEdit = (study: StudyDto) => {
        setEditingStudyId(study.id);
        setTitle(study.title);
        setInstitution(study.institution);
        setStartDate(study.startDate?.substring(0, 10) || "");
        setEndDate(study.endDate?.substring(0, 10) || "");
        setIsEditing(true);
    };

    const handleAdd = () => {
        setEditingStudyId(null);
        setTitle("");
        setInstitution("");
        setStartDate("");
        setEndDate("");
        setIsEditing(true);
    };

    const handleCancel = () => {
        setIsEditing(false);
        setEditingStudyId(null);
        setTitle("");
        setInstitution("");
        setStartDate("");
        setEndDate("");
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        try {
            const bodyObj: any = { userId, title, institution, startDate };
            if (endDate) {
                bodyObj.endDate = endDate;
            }
            const body = JSON.stringify(bodyObj);

            if (editingStudyId) {
                await api(`/studies/${editingStudyId}`, { method: "PUT", body });
            } else {
                await api(`/studies`, { method: "POST", body });
            }
            setIsEditing(false);
            fetchStudies();
            showAlert("Éxito", "El estudio se ha guardado correctamente.", "success");
        } catch (e: any) {
            console.error(e);
            showAlert("Error", "Error al guardar el estudio: " + (e.message || ""), "error");
        }
    };

    const handleDelete = async (studyId: number) => {
        if (!window.confirm("¿Estás seguro de que deseas eliminar este estudio?")) return;
        try {
            await api(`/studies/${studyId}`, { method: "DELETE" });
            fetchStudies();
            showAlert("Eliminado", "El estudio se eliminó correctamente.", "info");
        } catch (e: any) {
            console.error(e);
            showAlert("Error", "Error al eliminar el estudio: " + (e.message || ""), "error");
        }
    };

    if (!userId && isAdmin) return null;

    return (
        <div className="bg-white rounded-xl shadow-sm border border-gray-200 overflow-hidden mb-8">
            <div className="px-6 py-5 border-b border-gray-200 bg-gray-50 flex justify-between items-center">
                <h3 className="text-lg leading-6 font-medium text-gray-900">Estudios</h3>
                {!isEditing && (
                    <button
                        onClick={handleAdd}
                        className="inline-flex items-center px-3 py-1.5 border border-transparent text-xs font-medium rounded-md shadow-sm text-white bg-blue-600 hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500 transition-colors"
                    >
                        Añadir Estudio
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
                        <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
                            <div>
                                <label className="block text-sm font-medium text-gray-700">Institución</label>
                                <input
                                    type="text"
                                    required
                                    value={institution}
                                    onChange={e => setInstitution(e.target.value)}
                                    className="mt-1 block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-blue-500 focus:border-blue-500 sm:text-sm"
                                    placeholder="Ej. Universidad de Buenos Aires"
                                />
                            </div>
                            <div>
                                <label className="block text-sm font-medium text-gray-700">Título / Grado obtenido</label>
                                <input
                                    type="text"
                                    required
                                    value={title}
                                    onChange={e => setTitle(e.target.value)}
                                    className="mt-1 block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-blue-500 focus:border-blue-500 sm:text-sm"
                                    placeholder="Ej. Ingeniería en Sistemas"
                                />
                            </div>
                        </div>
                        <div className="grid grid-cols-2 gap-4">
                            <div>
                                <label className="block text-sm font-medium text-gray-700">Ingreso</label>
                                <input
                                    required type="date" value={startDate} onChange={e => setStartDate(e.target.value)}
                                    className="mt-1 block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-blue-500 focus:border-blue-500 sm:text-sm"
                                />
                            </div>
                            <div>
                                <label className="block text-sm font-medium text-gray-700">Egreso <span className="text-gray-400 font-normal">(Opcional)</span></label>
                                <input
                                    type="date" value={endDate} onChange={e => setEndDate(e.target.value)}
                                    className="mt-1 block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-blue-500 focus:border-blue-500 sm:text-sm"
                                />
                            </div>
                        </div>
                        <div className="flex gap-3 pt-2">
                            <button
                                type="submit"
                                disabled={!title || !institution || !startDate}
                                className={`inline-flex justify-center py-2 px-4 border border-transparent shadow-sm text-sm font-medium rounded-md text-white transition-colors ${(!title || !institution || !startDate) ? "bg-gray-300 cursor-not-allowed" :
                                    "bg-blue-600 hover:bg-blue-700"
                                    }`}
                            >
                                {editingStudyId ? "Guardar Cambios" : "Crear Estudio"}
                            </button>
                            <button
                                type="button"
                                onClick={handleCancel}
                                className="bg-white py-2 px-4 border border-gray-300 rounded-md shadow-sm text-sm font-medium text-gray-700 hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500 transition-colors"
                            >
                                Cancelar
                            </button>
                        </div>
                    </form>
                ) : studies.length === 0 ? (
                    <p className="text-sm text-gray-500 italic">No hay estudios registrados para este usuario.</p>
                ) : (
                    <ul className="divide-y divide-gray-200">
                        {studies.map(study => (
                            <li key={study.id} className="py-4 flex justify-between items-start">
                                <div>
                                    <p className="text-sm font-medium text-gray-900">{study.title}</p>
                                    <p className="text-sm text-gray-500">{study.institution}</p>
                                    <p className="text-xs text-gray-400 mt-1">
                                        {study.startDate?.substring(0, 10)} {study.endDate ? ` a ${study.endDate.substring(0, 10)}` : "(Actualidad)"}
                                    </p>
                                </div>
                                <div className="flex space-x-3 ml-4">
                                    <button
                                        onClick={() => handleEdit(study)}
                                        className="text-sm font-medium text-blue-600 hover:text-blue-500 transition-colors"
                                    >
                                        Editar
                                    </button>
                                    <button
                                        onClick={() => handleDelete(study.id)}
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
