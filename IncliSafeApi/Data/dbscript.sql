
CREATE TABLE IF NOT EXISTS Usuarios (
    Id SERIAL PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    Email VARCHAR(100) NOT NULL UNIQUE,
    Password VARCHAR(100) NOT NULL,
    Rol VARCHAR(50),
    Activo BOOLEAN DEFAULT true
);

CREATE TABLE IF NOT EXISTS Vehiculos (
    Id SERIAL PRIMARY KEY,
    Placa VARCHAR(10) NOT NULL UNIQUE,
    Marca VARCHAR(100) NOT NULL,
    Modelo VARCHAR(50) NOT NULL,
    Año INTEGER NOT NULL CHECK (Año BETWEEN 1900 AND 2100),
    Estado VARCHAR(50),
    Activo BOOLEAN DEFAULT true
);

CREATE TABLE IF NOT EXISTS Inspecciones (
    Id SERIAL PRIMARY KEY,
    Fecha TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    Estado VARCHAR(50),
    Observaciones TEXT,
    VehiculoId INTEGER NOT NULL REFERENCES Vehiculos(Id),
    UsuarioId INTEGER NOT NULL REFERENCES Usuarios(Id)
);

-- Índices para mejorar el rendimiento
CREATE INDEX IF NOT EXISTS idx_vehiculos_placa ON Vehiculos(Placa);
CREATE INDEX IF NOT EXISTS idx_usuarios_email ON Usuarios(Email);
CREATE INDEX IF NOT EXISTS idx_inspecciones_fecha ON Inspecciones(Fecha);

-- Usuario admin por defecto (password: admin123)
INSERT INTO Usuarios (Nombre, Email, Password, Rol, Activo)
VALUES ('Admin', 'admin@inclisafe.com', 'AQAAAAEAACcQAAAAEJk6Hk/r3cNvnKQTJ8TmWiN+C5C0sQqbVKYuXJVDhDjLCMHo1SdKQ8MNXP0/N0gR8A==', 'Admin', true)
ON CONFLICT (Email) DO NOTHING;
