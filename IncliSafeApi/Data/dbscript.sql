
USE IncliSafe;

CREATE TABLE Usuarios (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    Password NVARCHAR(100) NOT NULL,
    Rol NVARCHAR(50),
    Activo BIT DEFAULT 1
);

CREATE TABLE Vehiculos (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Placa NVARCHAR(10) NOT NULL UNIQUE,
    Marca NVARCHAR(100) NOT NULL,
    Modelo NVARCHAR(50) NOT NULL,
    Año INT NOT NULL CHECK (Año BETWEEN 1900 AND 2100),
    Estado NVARCHAR(50),
    Activo BIT DEFAULT 1
);

CREATE TABLE Inspecciones (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Fecha DATETIME NOT NULL DEFAULT GETDATE(),
    Estado NVARCHAR(50),
    Observaciones NVARCHAR(MAX),
    VehiculoId INT NOT NULL FOREIGN KEY REFERENCES Vehiculos(Id),
    UsuarioId INT NOT NULL FOREIGN KEY REFERENCES Usuarios(Id)
);

CREATE INDEX idx_vehiculos_placa ON Vehiculos(Placa);
CREATE INDEX idx_usuarios_email ON Usuarios(Email);
CREATE INDEX idx_inspecciones_fecha ON Inspecciones(Fecha);

-- Usuario admin con password admin123 (hash generado para 'admin123')
INSERT INTO Usuarios (Nombre, Email, Password, Rol, Activo)
VALUES ('Admin', 'admin@inclisafe.com', 'AQAAAAEAACcQAAAAEBqqjV2JZP+3QR8VBexE02FXRTXkL1pXMoGn+FkvcZV8dQtIvEFgTF42pDUvLPmP6Q==', 'Admin', 1);
