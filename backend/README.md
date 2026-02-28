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

### 2. Crear la Base de Datos

Toda la estructura necesaria de la base de datos (tablas, relaciones y un usuario Administrador inicial para pruebas) está provista en el archivo `Script.sql` ubicado en la raíz de este repositorio.

Existen dos formas de preparar la base de datos:

**Opción A: Ejecutar el Script SQL (Recomendado)**
1. Abre SQL Server Management Studio (SSMS) o Azure Data Studio.
2. Conéctate a tu instancia local de SQL Server.
3. Abre el archivo `Script.sql` y ejecútalo. Esto creará la BD `ZocoTestDb` e insertará datos de prueba.

**Opción B: Usar las Migraciones de Entity Framework**
Si prefieres que EF Core genere la base de datos automáticamente:
```bash
# Ejecutar en la misma carpeta donde está el .sln
dotnet ef database update
```

**🔑 Credenciales de Prueba (Administrador)**
Si utilizas el `Script.sql` de arriba o ejecutas las migraciones, el `DbSeeder` generará una cuenta de administrador por defecto, lista para probar el Panel de Control:
- **Email:** `admin@zoco.local`
- **Password:** `Admin123!`

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
