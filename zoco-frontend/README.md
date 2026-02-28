# Prueba Técnica - Full Stack Developer (Frontend React)

Este proyecto corresponde a la parte Frontend de la solución desarrollada para la prueba técnica. Es una aplicación React interactivada, construida con Vite, TypeScript y estilizada con Tailwind CSS. Permite a los usuarios y administradores gestionar perfiles, direcciones y estudios.

🔗 **Link del Deploy (Vercel):** [https://zoco-fullstack.vercel.app/](https://zoco-fullstack.vercel.app/)

## 🛠️ Tecnologías Utilizadas

- **Core:** React 18, TypeScript, Vite
- **Enrutamiento:** React Router DOM v6
- **Estilos:** Tailwind CSS v4
- **Estado y Contexto:** React Context API (`AuthContext`, `AlertContext`)
- **Peticiones HTTP:** Cliente Fetch Nativo encapsulado
- **Persistencia de sesión:** `sessionStorage` (JWT Tokens)

## 🏗️ Estructura del Proyecto

```
src/
├── api/             # Cliente HTTP y configuración de endpoints
├── auth/            # Contexto de Autenticación y Rutas Protegidas
├── components/      # Componentes UI reutilizables (Modales, Secciones de perfil)
├── pages/           # Vistas principales (Login, Registro, Dashboard, Admin)
├── types/           # Interfaces y tipos de TypeScript (DTOs)
├── App.tsx          # Configuración global de Providers y Router
└── main.tsx         # Punto de entrada de la aplicación
```

## 🚀 Funcionalidades Clave implementadas

1. **Autenticación Completa:** Login y Registro integrados con el backend. Guardado seguro del JWT en `sessionStorage`.
2. **Dashboard Dinámico:** 
   - Los Usuarios Regulares pueden visualizar y modificar libremente sus datos personales (Perfil, Contraseña, Direcciones, Estudios).
   - Los Administradores (`Role: Admin`) tienen acceso a un "User Picker" para tomar el control de cualquier usuario y gestionar la información en su nombre.
3. **Panel de Administrador Dedicado (`/admin/users`):** Interfaz exclusiva para visualizar a todos los usuarios registrados, modificar sus datos y eliminarlos.
4. **Protección de Rutas:** Implementación de `<ProtectedRoute>` que restringe el acceso según estado de autenticación y rol.
5. **UI/UX Consistente:** Diseño responsivo 100% Mobile-First y uso de Modales Globales basados en Tailwind en lugar de los `alert()` nativos del navegador.

## ⚙️ Variables de Entorno

Para ejecutar este proyecto, necesitas configurar las variables de entorno. Crea un archivo `.env` en la raíz del proyecto (puedes basarte en `.env.example`) y agrega:

```env
VITE_API_BASE_URL=https://localhost:7247/api
```
*(Asegúrate de cambiar este puerto si tu backend en .NET se ejecuta en uno diferente).*

## 🏃 Instrucciones de Ejecución Local

1. Asegúrate de tener **Node.js** (v18 o superior) instalado en tu computadora.
2. Clona el repositorio y abre una terminal en la carpeta principal del frontend.
3. Instala las dependencias del proyecto:
   ```bash
   npm install
   ```
4. Inicia el servidor de desarrollo:
   ```bash
   npm run dev
   ```
5. Abre en tu navegador la dirección indicada en la terminal (por defecto suele ser `http://localhost:5173/`).
