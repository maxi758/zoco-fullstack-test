Zoco FullStack Technical Test — Backend (.NET 8)

API REST desarrollada con .NET 8, Entity Framework Core (Code First), SQL Server / LocalDB, autenticación mediante JWT (Bearer) y documentación interactiva con Swagger.

🛠 Stack Tecnológico

.NET 8 (ASP.NET Core Web API)

Entity Framework Core

SQL Server / LocalDB

JWT Authentication (Bearer)

Swagger (Swashbuckle)

📋 Requisitos

.NET SDK 8.x

SQL Server o LocalDB instalado

Visual Studio 2022 (opcional)

## 🌀 Quickstart (desde 0)
1. Clonar:
git clone https://github.com/maxi758/zoco-fullstack-test.git cd zoco-fullstack-test
2. Restaurar paquetes:
dotnet restore

⚙ Configuración

Configurar variables (editar `zoco.API/appsettings.Development.json` o usar variables de entorno):
- `ConnectionStrings:Default` — cadena de conexión
- `Jwt:Key` — 32+ caracteres
- `Jwt:Issuer`, `Jwt:Audience`, `Jwt:ExpiresMinutes`

🗄 Base de Datos (Migraciones)
Crear migraciones / aplicar DB:
- CLI:
  ```
  dotnet tool install --global dotnet-ef
  dotnet ef migrations add InitialCreate --project zoco.Infrastructure --startup-project zoco.API
  dotnet ef database update --project zoco.Infrastructure --startup-project zoco.API
  ```
- Visual Studio: abrir solución y en __Package Manager Console__ seleccionar `zoco.Infrastructure` y ejecutar:
  ```
  Add-Migration InitialCreate
  Update-Database
  ```
▶ Ejecutar la API
Desde Visual Studio

Presionar F5

Desde consola
dotnet run --project zoco.API

Swagger estará disponible en:

https://localhost:<PUERTO>/swagger
## 🔐 Uso básico (JWT)
1. `POST /api/auth/register` — registrar usuario  
2. `POST /api/auth/login` — obtener JWT  
3. En Swagger: Authorize → `Bearer <TOKEN>`  
4. Endpoints protegidos devuelven 401/403 según autorización

Para acciones sobre recursos de otros usuarios (p. ej. crear estudio para otro user) se usa el mismo endpoint con `?userId=<USER_ID>` cuando aplique; los servicios validan permisos.

Logout

POST /api/auth/logout

👥 Roles

El sistema tiene dos roles:

Usuario (User)

Puede gestionar únicamente su propio perfil.

Puede gestionar únicamente sus propios estudios y direcciones.

Administrador (Admin)

Puede gestionar cualquier usuario.

Puede gestionar estudios y direcciones de cualquier usuario.

🔄 Convertir un Usuario en Admin (para pruebas)

Ejecutar en la base de datos:

UPDATE Users
SET Role = 1
WHERE Email = 'admin@email.com';

Luego volver a hacer login para obtener un nuevo token con rol Admin.

📌 Endpoints Principales
👤 Usuarios

GET /api/users/me

GET /api/users (Admin)

GET /api/users/{id} (Admin)

PUT /api/users/{id}

Usuario puede modificar solo su propio usuario

Admin puede modificar cualquier usuario

DELETE /api/users/{id}

🎓 Estudios

GET /api/studies

POST /api/studies

GET /api/studies/{id}

PUT /api/studies/{id}

DELETE /api/studies/{id}


🏠 Direcciones

GET /api/addresses

POST /api/addresses

GET /api/addresses/{id}

PUT /api/addresses/{id}

DELETE /api/addresses/{id}


🧠 Manejo de Errores

La API implementa un middleware global para manejo de excepciones:

401 → No autenticado

403 → No autorizado

404 → Recurso no encontrado

400 → Error de validación o datos inválidos

500 → Error inesperado del servidor

📂 Arquitectura

La solución está estructurada en capas:

API → Controllers + Middleware

Application → Services + DTOs + Interfaces

Domain → Entidades y enums

Infrastructure → Repositorios + DbContext

Se utiliza inyección de dependencias para desacoplar lógica de negocio y acceso a datos.

✅ Funcionalidades Implementadas

Autenticación con JWT

Registro y Login

Logout con registro de sesiones (SessionLogs)

CRUD completo de Usuarios

CRUD completo de Estudios

CRUD completo de Direcciones

Control de acceso basado en roles

Validación de propiedad de recursos

Índice único en Email

Documentación con Swagger

Arquitectura en capas (Controller / Service / Repository)

📌 Notas Finales

Este proyecto fue desarrollado como prueba técnica fullstack, priorizando:

Correcta separación de responsabilidades

Seguridad mediante JWT

Control de acceso por rol

Código claro y mantenible

Buenas prácticas REST