-- 1. Crear la Base de Datos
CREATE DATABASE ZocoTestDb;
GO
USE ZocoTestDb;
GO

-- 2. Crear Tabla de Usuarios (La principal)
CREATE TABLE Users (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    FirstName NVARCHAR(100) NULL,
    LastName NVARCHAR(100) NULL,
    Email NVARCHAR(255) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(MAX) NOT NULL, -- Hasheado (bcrypt/pbkdf2)
    Role NVARCHAR(20) NOT NULL DEFAULT 'User', -- 'User' o 'Admin'
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);
GO

-- 3. Crear Tabla de Direcciones (1:N hacia Users)
CREATE TABLE Addresses (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId UNIQUEIDENTIFIER NOT NULL,
    Street NVARCHAR(200) NOT NULL,
    City NVARCHAR(100) NOT NULL,
    State NVARCHAR(100) NULL,
    ZipCode NVARCHAR(20) NOT NULL,
    Country NVARCHAR(100) NOT NULL,
    CONSTRAINT FK_Addresses_Users FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
);
GO

-- 4. Crear Tabla de Estudios (1:N hacia Users)
CREATE TABLE Studies (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId UNIQUEIDENTIFIER NOT NULL,
    Title NVARCHAR(200) NOT NULL,
    Institution NVARCHAR(200) NOT NULL,
    StartDate DATETIME2 NOT NULL,
    EndDate DATETIME2 NULL, -- Puede ser null si aún estudia ahí
    CONSTRAINT FK_Studies_Users FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
);
GO

-- 5. Crear Tabla de Sesiones (1:N hacia Users)
CREATE TABLE SessionLogs (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId UNIQUEIDENTIFIER NOT NULL,
    FechaInicio DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    FechaFin DATETIME2 NULL, -- Se actualiza al desloguearse o por expiración (Token)
    IpAddress NVARCHAR(45) NULL,
    CONSTRAINT FK_SessionLogs_Users FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
);
GO

-- 6. Insertar Usuario Administrador Inicial Requerido (Para poder probar AdminUsersPage)
-- NOTA: El hash debe coincidir con una clave conocida (ej. pass123), esto es ilustrativo
INSERT INTO Users (Id, FirstName, LastName, Email, PasswordHash, Role)
VALUES 
(NEWID(), 'Admin', 'Zoco', 'admin@zoco.com', 'AQAAAAEAACcQAAAA...[TU_HASH_BCRYPT]...', 'Admin');
GO
