# Prueba Técnica - Full Stack Developer (Backend .NET)

Este proyecto corresponde a la parte Backend de la solución desarrollada para la prueba técnica. Es una API RESTful construida en **.NET 6+** siguiendo una arquitectura multicapa (Controllers, Services, Repositories), utilizando **Entity Framework Core** para el acceso a datos y asegurada mediante **Autenticación JWT** y políticas basadas en Roles.

## 🛠️ Tecnologías Utilizadas

- **Framework:** .NET Core 6+ / ASP.NET Core Web API
- **Base de Datos:** SQL Server
- **ORM:** Entity Framework Core
- **Seguridad:** Autenticación JWT y Middleware de Autorización por Roles (`[Authorize]`)
- **Documentación:** Swagger (OpenAPI) integrado y asegurado

## 🏗️ Requisitos Funcionales Implementados

- **Autenticación:** Endpoints para Login y Registro. Generación de claims por rol y registro de la hora de inicio y fin de la sesión mediante la tabla `SessionLogs`.
- **Gestión de Recursos Relacionados:** CRUD completo para Usuarios, Direcciones y Estudios con validación unívoca (un usuario regular solo puede consultar, editar y borrar sus propios registros, asegurando la propiedad de la entidad).
- **Control de Acceso basado en Roles (RBAC):** Privilegios elevados para el rol `Admin`, el cual puede saltarse las restricciones de pertenencia de registros para gestionar todo el sistema.
- **Auditoría e Integridad:** Estructura modular, inyección de dependencias estricta y uso de DTOs para no exponer los modelos de base de datos de Entity Framework hacia el exterior.

## ⚙️ Requisitos previos

- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0) o superior instalado.
- Servidor local de **SQL Server** o SQL Server Express (usualmente `(localdb)\MSSQLLocalDB` o `.\SQLEXPRESS`).

## 🚀 Instrucciones de Ejecución Local

### 1. Configurar la Base de Datos (`appsettings.json`)

En la raíz del proyecto backend, debes abrir o modificar el archivo `appsettings.json` o `appsettings.Development.json` para asegurarte de que la cadena de conexión (`DefaultConnection`) apunte a tu instancia local de SQL Server, y que las claves JWT correspondan a tus requerimientos de seguridad.

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.\\SQLEXPRESS;Database=ZocoTestDb;Trusted_Connection=True;MultipleActiveResultSets=true;Encrypt=False"
  },
  "JwtSettings": {
    "Key": "A_Very_Long_And_Secure_Development_Secret_Key_1234567890",
    "Issuer": "ZocoTestIssuer",
    "Audience": "ZocoTestAudience"
  }
}
```

### 2. Aplicar las Migraciones y crear la Base de Datos

El repositorio ya contiene la carpeta de `Migrations`. Para generar automáticamente las bases de datos y las tablas de Usuarios, Roles, Sesiones, Direcciones y Estudios, ejecuta el siguiente comando en la consola del administrador de paquetes (Visual Studio) o utilizando la CLI de EF Tools:

```bash
# Si usas la CLI de .NET:
dotnet ef database update
```
*(También se provee un script `Script.sql` en la raíz en caso de que prefieras crear la base manualmente).*

### 3. Ejecutar el Proyecto

Compila e inicia el proyecto por terminal (o presionando F5 en Visual Studio).

```bash
dotnet run
```

La consola generará las rutas HTTP y HTTPS por defecto del servidor Kestrel.

### 4. Acceder al Swagger UI

Con la API corriendo, navega en tu explorador hacia:
`https://localhost:<TU_PUERTO>/swagger`

Allí podrás ver todos los controladores autodocumentados e ingresar tu Token en el candado superior (`Authorize`) para efectuar peticiones a los Endpoints protegidos.
