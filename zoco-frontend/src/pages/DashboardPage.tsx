import { useState } from "react";
import { useAuth } from "../auth/AuthContext";
import { ProfileSection } from "../components/ProfileSection";
import { PasswordSection } from "../components/PasswordSection";
import { StudiesSection } from "../components/StudiesSection";
import { AddressesSection } from "../components/AddressesSection";
import { UserPicker } from "../components/UserPicker";

export function DashboardPage() {
	const { role, userId, email, logout } = useAuth();
	const [targetUserId, setTargetUserId] = useState<string | null>(null);

	const effectiveUserId =
		role === "Admin" ? targetUserId : userId;

	return (
		<div className="max-w-5xl mx-auto my-8 px-4 sm:px-6 lg:px-8">
			<div className="bg-white shadow-sm rounded-xl p-6 mb-8 flex flex-col sm:flex-row justify-between items-start sm:items-center border border-gray-100">
				<div>
					<h2 className="text-3xl font-extrabold text-gray-900 tracking-tight">Dashboard</h2>
					<div className="mt-1 text-sm text-gray-500 font-medium">
						{email} <span className="mx-2 text-gray-300">|</span>
						<span className="inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium bg-blue-100 text-blue-800">
							{role}
						</span>
					</div>
				</div>
				<button
					onClick={logout}
					className="mt-4 sm:mt-0 inline-flex items-center px-4 py-2 border border-transparent text-sm font-medium rounded-md text-red-700 bg-red-100 hover:bg-red-200 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-red-500 transition-colors"
				>
					Cerrar Sesión
				</button>
			</div>

			{role === "Admin" && (
				<div className="bg-white shadow-sm rounded-xl p-6 mb-8 border border-gray-100">
					<div className="flex flex-col sm:flex-row justify-between items-start sm:items-center mb-6">
						<h3 className="text-lg leading-6 font-medium text-gray-900">Vista de Administrador</h3>
						<a
							href="/admin/users"
							className="mt-3 sm:mt-0 inline-flex items-center px-4 py-2 border border-transparent shadow-sm text-sm font-medium rounded-md text-white bg-gray-900 hover:bg-gray-800 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-gray-900 transition-colors"
						>
							Gestión completa de usuarios
						</a>
					</div>
					<div className="max-w-md">
						<label className="block text-sm font-medium text-gray-700 mb-2">Seleccionar usuario para ver sus datos</label>
						<UserPicker value={targetUserId} onChange={setTargetUserId} />
					</div>
					{!targetUserId && (
						<div className="mt-4 p-4 rounded-md bg-blue-50 border border-blue-100">
							<div className="flex">
								<div className="ml-3">
									<h3 className="text-sm font-medium text-blue-800">Atención</h3>
									<div className="mt-2 text-sm text-blue-700">
										<p>Selecciona un usuario de la lista desplegable de arriba para cargar y gestionar sus estudios y direcciones.</p>
									</div>
								</div>
							</div>
						</div>
					)}
				</div>
			)}

			<div className="grid grid-cols-1 lg:grid-cols-2 gap-8 mb-8">
				<ProfileSection userId={effectiveUserId} isAdmin={role === "Admin"} />
				<PasswordSection userId={effectiveUserId} isAdmin={role === "Admin"} />
			</div>

			<div className="grid grid-cols-1 lg:grid-cols-2 gap-8">
				<StudiesSection userId={effectiveUserId} isAdmin={role === "Admin"} />
				<AddressesSection userId={effectiveUserId} isAdmin={role === "Admin"} />
			</div>
		</div>
	);
}