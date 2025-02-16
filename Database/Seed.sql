/*
Script de inicialización de IncliSafe
- Borra y recrea la base de datos
- Crea las tablas necesarias
- Inserta datos de prueba para un año completo
*/

USE master;
GO

-- Borrar todas las bases de datos relacionadas si existen
IF EXISTS (SELECT * FROM sys.databases WHERE name = 'IncliSafe')
BEGIN
    ALTER DATABASE IncliSafe SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE IncliSafe;
END

IF EXISTS (SELECT * FROM sys.databases WHERE name = 'IncliSafeTest')
BEGIN
    ALTER DATABASE IncliSafeTest SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE IncliSafeTest;
END

IF EXISTS (SELECT * FROM sys.databases WHERE name = 'IncliSafeDev')
BEGIN
    ALTER DATABASE IncliSafeDev SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE IncliSafeDev;
END
GO

-- Crear nueva base de datos
CREATE DATABASE IncliSafe;
GO

USE IncliSafe;
GO

-- Configuración inicial
SET NOCOUNT ON;
SET XACT_ABORT ON;
GO

-- Crear tablas principales
CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Email NVARCHAR(256) UNIQUE NOT NULL,
    UserName NVARCHAR(256) UNIQUE NOT NULL,
    PasswordHash NVARCHAR(MAX) NOT NULL,
    SecurityStamp NVARCHAR(MAX),
    EmailConfirmed BIT DEFAULT 1,
    PhoneNumber NVARCHAR(50),
    PhoneNumberConfirmed BIT DEFAULT 0,
    TwoFactorEnabled BIT DEFAULT 0,
    LockoutEnabled BIT DEFAULT 0,
    AccessFailedCount INT DEFAULT 0,
    Role NVARCHAR(50) NOT NULL,
    CreatedAt DATETIME2 DEFAULT GETDATE(),
    LastLogin DATETIME2,
    CompanyName NVARCHAR(100),
    Address NVARCHAR(200),
    FleetSize INT,
    IndustryType NVARCHAR(50),
    PreferredLanguage NVARCHAR(10) DEFAULT 'es',
    NotificationPreferences NVARCHAR(MAX)
);

CREATE TABLE Vehicles (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    LicensePlate NVARCHAR(20) NOT NULL,
    Brand NVARCHAR(50) NOT NULL,
    Model NVARCHAR(50) NOT NULL,
    Year INT NOT NULL,
    VehicleType NVARCHAR(50) NOT NULL,
    Status NVARCHAR(50) NOT NULL,
    LastInspectionDate DATE,
    NextInspectionDate DATE,
    UserId INT NOT NULL,
    CreatedAt DATETIME2 DEFAULT GETDATE(),
    LastUpdate DATETIME2,
    Capacity DECIMAL(10,2),
    SerialNumber NVARCHAR(100),
    Notes NVARCHAR(MAX),
    FOREIGN KEY (UserId) REFERENCES Users(Id),
    CONSTRAINT UQ_LicensePlate UNIQUE (LicensePlate)
);

CREATE TABLE DobackDevices (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    SerialNumber NVARCHAR(100) UNIQUE NOT NULL,
    VehicleId INT,
    InstallationDate DATE,
    LastCalibrationDate DATE,
    NextCalibrationDate DATE,
    FirmwareVersion NVARCHAR(50),
    Status NVARCHAR(50),
    FOREIGN KEY (VehicleId) REFERENCES Vehicles(Id)
);

CREATE TABLE DobackFiles (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ReportId INT,
    OriginalFileName NVARCHAR(500),
    StoragePath NVARCHAR(500),
    FileSize BIGINT,
    UploadDate DATETIME2 DEFAULT GETDATE(),
    ProcessedDate DATETIME2,
    ProcessingStatus NVARCHAR(50),
    FileHash NVARCHAR(64),
    FOREIGN KEY (ReportId) REFERENCES DobackReports(Id)
);

CREATE TABLE DobackReports (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    VehicleId INT NOT NULL,
    DeviceId INT NOT NULL,
    ReportDate DATETIME2 NOT NULL,
    FilePath NVARCHAR(500),
    -- Datos del sensor Doback
    AccelerationX DECIMAL(10,2),  -- ax
    AccelerationY DECIMAL(10,2),  -- ay
    AccelerationZ DECIMAL(10,2),  -- az
    GyroscopeX DECIMAL(10,2),     -- gx
    GyroscopeY DECIMAL(10,2),     -- gy
    GyroscopeZ DECIMAL(10,2),     -- gz
    Roll DECIMAL(10,2),           -- roll
    Pitch DECIMAL(10,2),          -- pitch
    Yaw DECIMAL(10,2),            -- yaw
    TimeAntWifi DECIMAL(10,2),    -- timeantwifi
    USCycle1 DECIMAL(10,2),       -- usciclo1
    USCycle2 DECIMAL(10,2),       -- usciclo2
    USCycle3 DECIMAL(10,2),       -- usciclo3
    USCycle4 DECIMAL(10,2),       -- usciclo4
    USCycle5 DECIMAL(10,2),       -- usciclo5
    StabilityIndex DECIMAL(10,2), -- si
    AccMagnitude DECIMAL(10,2),   -- accmag
    MicrosCleanCAN DECIMAL(10,2), -- microslimpiarcan
    MicrosSD DECIMAL(10,2),       -- microsds
    ErrorsCAN INT,                -- erroresCAN
    Speed DECIMAL(10,2),          -- speed
    Steer DECIMAL(10,2),          -- steer
    ProcessedStatus NVARCHAR(50),
    CreatedAt DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (VehicleId) REFERENCES Vehicles(Id),
    FOREIGN KEY (DeviceId) REFERENCES DobackDevices(Id)
);

CREATE TABLE Maintenance (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    VehicleId INT NOT NULL,
    MaintenanceDate DATETIME2 NOT NULL,
    Type NVARCHAR(50) NOT NULL,
    Description NVARCHAR(MAX),
    Cost DECIMAL(10,2),
    Status NVARCHAR(50) NOT NULL,
    Technician NVARCHAR(100),
    WorkOrderNumber NVARCHAR(50),
    PartsReplaced NVARCHAR(MAX),
    CreatedAt DATETIME2 DEFAULT GETDATE(),
    CompletedAt DATETIME2,
    FOREIGN KEY (VehicleId) REFERENCES Vehicles(Id)
);

CREATE TABLE Alerts (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    VehicleId INT NOT NULL,
    ReportId INT,
    AlertType NVARCHAR(50) NOT NULL,
    Severity NVARCHAR(20) NOT NULL,
    Description NVARCHAR(MAX),
    CreatedAt DATETIME2 DEFAULT GETDATE(),
    ResolvedAt DATETIME2,
    Status NVARCHAR(50),
    FOREIGN KEY (VehicleId) REFERENCES Vehicles(Id),
    FOREIGN KEY (ReportId) REFERENCES DobackReports(Id)
);

CREATE TABLE PredictiveAnalysis (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    VehicleId INT,
    AnalysisDate DATETIME2,
    PredictionType NVARCHAR(50),
    PredictionValue DECIMAL(10,4),
    Confidence DECIMAL(5,2),
    FactorsConsidered NVARCHAR(MAX),
    RecommendedActions NVARCHAR(MAX),
    FOREIGN KEY (VehicleId) REFERENCES Vehicles(Id)
);

-- Índices optimizados
CREATE INDEX IX_Vehicles_UserId ON Vehicles(UserId);
CREATE INDEX IX_DobackReports_VehicleId ON DobackReports(VehicleId);
CREATE INDEX IX_DobackReports_DeviceId ON DobackReports(DeviceId);
CREATE INDEX IX_Maintenance_VehicleId ON Maintenance(VehicleId);
CREATE INDEX IX_Alerts_VehicleId ON Alerts(VehicleId);
CREATE INDEX IX_DobackReports_Date ON DobackReports(ReportDate);
CREATE INDEX IX_Maintenance_Date ON Maintenance(MaintenanceDate);

-- Insertar usuarios base
INSERT INTO Users (Email, UserName, PasswordHash, SecurityStamp, Role, CompanyName)
VALUES 
('admin@inclisafe.com', 'admin', 'AQAAAAIAAYagAAAAEP8Vs+gT5H1Lk2uQTQE7yFyFJWxrxBFVe+QJcyQFXgA4VtKJ7WdZWSHEPZE8Qw==', NEWID(), 'Admin', 'IncliSafe Admin'),
('inspector@inclisafe.com', 'inspector', 'AQAAAAIAAYagAAAAEP8Vs+gT5H1Lk2uQTQE7yFyFJWxrxBFVe+QJcyQFXgA4VtKJ7WdZWSHEPZE8Qw==', NEWID(), 'Inspector', 'IncliSafe Inspection'),
('prueba@inclisafe.com', 'prueba', 'AQAAAAIAAYagAAAAEP8Vs+gT5H1Lk2uQTQE7yFyFJWxrxBFVe+QJcyQFXgA4VtKJ7WdZWSHEPZE8Qw==', NEWID(), 'User', 'Bomberos Madrid Central');

-- Declarar tablas temporales para condiciones
DECLARE @WeatherConditions TABLE (Condition NVARCHAR(50));
INSERT INTO @WeatherConditions VALUES 
('Sunny'), ('Rainy'), ('Cloudy'), ('Windy'), ('Snow');

DECLARE @RoadTypes TABLE (Type NVARCHAR(50));
INSERT INTO @RoadTypes VALUES 
('Urban'), ('Highway'), ('Mountain'), ('Off-road'), ('Industrial');

-- Variables para generación de datos
DECLARE @UserId INT = (SELECT Id FROM Users WHERE UserName = 'prueba');
DECLARE @StartDate DATE = DATEADD(YEAR, -1, GETDATE());
DECLARE @CurrentDate DATE = @StartDate;
DECLARE @WeekCounter INT = 1;

-- Insertar flota de bomberos
INSERT INTO Vehicles (LicensePlate, Brand, Model, Year, VehicleType, Status, UserId, SerialNumber)
VALUES 
('B-001-FD', 'Mercedes-Benz', 'Atego 1529F', 2022, 'Fire Truck', 'Active', @UserId, 'MB22-001'),
('B-002-FD', 'Rosenbauer', 'AT3', 2021, 'Fire Truck', 'Active', @UserId, 'RB21-002'),
('B-003-FD', 'Scania', 'P410', 2022, 'Water Tanker', 'Active', @UserId, 'SC22-003'),
('B-004-FD', 'MAN', 'TGM 15.290', 2021, 'Rescue Truck', 'Active', @UserId, 'MN21-004'),
('B-005-FD', 'Mercedes-Benz', 'Actros 3341', 2020, 'Heavy Rescue', 'Maintenance', @UserId, 'MB20-005'),
('B-006-FD', 'Volvo', 'FMX 500', 2022, 'Fire Truck', 'Active', @UserId, 'VO22-006'),
('B-007-FD', 'Iveco', 'Eurocargo', 2021, 'Equipment Truck', 'Active', @UserId, 'IV21-007'),
('B-008-FD', 'Scania', 'P380', 2020, 'Ladder Truck', 'Active', @UserId, 'SC20-008'),
('B-009-FD', 'MAN', 'TGS 33.400', 2022, 'Water Tanker', 'Active', @UserId, 'MN22-009'),
('B-010-FD', 'Mercedes-Benz', 'Arocs 2645', 2021, 'Heavy Rescue', 'Active', @UserId, 'MB21-010');

-- Insertar dispositivos Doback
INSERT INTO DobackDevices (SerialNumber, VehicleId, InstallationDate, LastCalibrationDate, NextCalibrationDate, FirmwareVersion, Status)
SELECT 
    'DOB-' + SerialNumber,
    Id,
    DATEADD(MONTH, -12, GETDATE()),
    DATEADD(MONTH, -6, GETDATE()),
    DATEADD(MONTH, 6, GETDATE()),
    '2.1.0',
    'Active'
FROM Vehicles
WHERE UserId = @UserId;

-- Generar reportes Doback para todo el año
WHILE @CurrentDate <= GETDATE()
BEGIN
    -- Generar 2 reportes semanales por vehículo
    INSERT INTO DobackReports (
        VehicleId,
        DeviceId,
        ReportDate,
        FilePath,
        AccelerationX,
        AccelerationY,
        AccelerationZ,
        GyroscopeX,
        GyroscopeY,
        GyroscopeZ,
        Roll,
        Pitch,
        Yaw,
        TimeAntWifi,
        USCycle1,
        USCycle2,
        USCycle3,
        USCycle4,
        USCycle5,
        StabilityIndex,
        AccMagnitude,
        MicrosCleanCAN,
        MicrosSD,
        ErrorsCAN,
        Speed,
        Steer,
        ProcessedStatus
    )
    SELECT 
        v.Id,
        d.Id,
        DATEADD(DAY, (ABS(CHECKSUM(NEWID())) % 3), @CurrentDate),
        'reports/' + v.LicensePlate + '/' + FORMAT(@CurrentDate, 'yyyy-MM-dd') + '_' + 
            CAST((ROW_NUMBER() OVER (PARTITION BY v.Id ORDER BY (SELECT NULL))) AS NVARCHAR(10)) + '.txt',
        -- Valores basados en el ejemplo proporcionado
        11.96 + (RAND() - 0.5) * 2,     -- ax: variación alrededor de 11.96
        8.54 + (RAND() - 0.5) * 2,      -- ay: variación alrededor de 8.54
        1012 + (RAND() - 0.5) * 1,      -- az: variación alrededor de 1012
        (RAND() - 0.5) * 100,           -- gx: rango -50 a 50
        (RAND() - 0.5) * 100,           -- gy: rango -50 a 50
        (RAND() - 0.5) * 100,           -- gz: rango -50 a 50
        CASE 
            WHEN v.VehicleType = 'Ladder Truck' THEN -0.7 + (RAND() - 0.5) * 0.5
            ELSE -0.5 + (RAND() - 0.5) * 0.3
        END,                            -- roll: valores más extremos para camiones escalera
        0.5 + (RAND() - 0.5) * 0.2,    -- pitch
        (RAND() - 0.5) * 0.1,          -- yaw
        CAST(DATEDIFF(SECOND, @StartDate, @CurrentDate) AS DECIMAL(10,2)) * 1000, -- timeantwifi
        20000 + (RAND() - 0.5) * 1000, -- usciclo1
        20000 + (RAND() - 0.5) * 1000, -- usciclo2
        20000,                         -- usciclo3
        20000,                         -- usciclo4
        20000,                         -- usciclo5
        0.98 + (RAND() - 0.5) * 0.1,   -- si: índice de estabilidad
        1012 + (RAND() - 0.5) * 1,     -- accmag
        4 + (RAND() * 2),              -- microslimpiarcan
        (RAND() * 1000),               -- microsds
        0,                             -- erroresCAN
        CASE 
            WHEN v.VehicleType IN ('Water Tanker', 'Heavy Rescue') THEN (RAND() * 60)
            ELSE (RAND() * 80)
        END,                           -- speed: velocidad según tipo de vehículo
        (RAND() - 0.5) * 2,           -- steer
        'Processed'
    FROM Vehicles v
    JOIN DobackDevices d ON v.Id = d.VehicleId
    WHERE v.UserId = @UserId
    CROSS JOIN (SELECT 1 AS ReportNum UNION SELECT 2) AS Reports;

    SET @CurrentDate = DATEADD(WEEK, 1, @CurrentDate);
    SET @WeekCounter = @WeekCounter + 1;
END;

-- Generar alertas basadas en los reportes
INSERT INTO Alerts (VehicleId, ReportId, AlertType, Severity, Description, Status)
SELECT 
    VehicleId,
    Id AS ReportId,
    CASE 
        WHEN StabilityIndex < 0.5 THEN 'Critical Stability'
        WHEN StabilityIndex < 0.7 THEN 'Stability Warning'
        ELSE 'Maintenance Due'
    END AS AlertType,
    CASE 
        WHEN StabilityIndex < 0.5 THEN 'High'
        WHEN StabilityIndex < 0.7 THEN 'Medium'
        ELSE 'Low'
    END AS Severity,
    CASE 
        WHEN StabilityIndex < 0.5 THEN 'Riesgo crítico de estabilidad detectado'
        WHEN StabilityIndex < 0.7 THEN 'Advertencia de estabilidad'
        ELSE 'Mantenimiento preventivo recomendado'
    END AS Description,
    CASE 
        WHEN StabilityIndex < 0.5 THEN 'Open'
        ELSE 'Resolved'
    END AS Status
FROM DobackReports
WHERE StabilityIndex < 0.8;

-- Generar mantenimientos basados en alertas
INSERT INTO Maintenance (VehicleId, MaintenanceDate, Type, Description, Cost, Status, Technician, WorkOrderNumber)
SELECT 
    VehicleId,
    CreatedAt,
    CASE Severity
        WHEN 'High' THEN 'Emergency'
        WHEN 'Medium' THEN 'Corrective'
        ELSE 'Preventive'
    END,
    Description,
    CASE Severity
        WHEN 'High' THEN 2000 + (RAND() * 1000)
        WHEN 'Medium' THEN 800 + (RAND() * 400)
        ELSE 300 + (RAND() * 200)
    END,
    'Completed',
    'Técnico ' + CAST((ABS(CHECKSUM(NEWID())) % 5 + 1) AS VARCHAR(1)),
    'WO-' + FORMAT(CreatedAt, 'yyyyMMdd') + '-' + CAST((ROW_NUMBER() OVER (ORDER BY CreatedAt)) AS VARCHAR(4))
FROM Alerts
WHERE Severity IN ('High', 'Medium')
OR (Severity = 'Low' AND (ABS(CHECKSUM(NEWID())) % 2 = 0));

-- Generar análisis predictivo basado en histórico
INSERT INTO PredictiveAnalysis (
    VehicleId,
    AnalysisDate,
    PredictionType,
    PredictionValue,
    Confidence,
    FactorsConsidered,
    RecommendedActions
)
SELECT 
    v.Id,
    GETDATE(),
    'StabilityRisk',
    CASE 
        WHEN AVG(dr.StabilityIndex) < 0.6 THEN 0.8
        WHEN AVG(dr.StabilityIndex) < 0.7 THEN 0.6
        ELSE 0.3
    END,
    0.85 + (RAND() * 0.10),
    CONCAT(
        'Factores analizados: ',
        'Índice de estabilidad promedio: ', FORMAT(AVG(dr.StabilityIndex), 'N2'),
        ', Alertas totales: ', COUNT(DISTINCT a.Id),
        ', Mantenimientos: ', COUNT(DISTINCT m.Id),
        ', Condiciones climáticas más frecuentes: ', STRING_AGG(DISTINCT dr.WeatherConditions, ', ')
    ),
    CASE 
        WHEN AVG(dr.StabilityIndex) < 0.6 THEN 'URGENTE: Programar revisión completa. Alto riesgo de inestabilidad.'
        WHEN AVG(dr.StabilityIndex) < 0.7 THEN 'Revisar sistema de suspensión y estabilidad en próximo mantenimiento'
        ELSE 'Mantener monitoreo regular. Vehículo en condiciones estables.'
    END
FROM Vehicles v
LEFT JOIN DobackReports dr ON v.Id = dr.VehicleId
LEFT JOIN Alerts a ON v.Id = a.VehicleId
LEFT JOIN Maintenance m ON v.Id = m.VehicleId
WHERE v.UserId = @UserId
GROUP BY v.Id;

-- Nuevas tablas para ML y Knowledge Base

-- Tabla para patrones globales identificados por ML
CREATE TABLE MLPatterns (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    PatternType NVARCHAR(50) NOT NULL,
    PatternName NVARCHAR(100) NOT NULL,
    Description NVARCHAR(MAX),
    DetectionLogic NVARCHAR(MAX),
    Confidence DECIMAL(5,2),
    OccurrenceCount INT,
    FirstDetected DATETIME2,
    LastDetected DATETIME2,
    Status NVARCHAR(50),
    ValidationStatus NVARCHAR(50),
    CreatedAt DATETIME2 DEFAULT GETDATE()
);

-- Tabla para instancias de patrones detectados
CREATE TABLE MLPatternInstances (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    PatternId INT,
    VehicleId INT,
    ReportId INT,
    DetectionDate DATETIME2,
    ConfidenceScore DECIMAL(5,2),
    ContextData NVARCHAR(MAX),
    Outcome NVARCHAR(50),
    ValidatedBy INT, -- Usuario que validó
    FOREIGN KEY (PatternId) REFERENCES MLPatterns(Id),
    FOREIGN KEY (VehicleId) REFERENCES Vehicles(Id),
    FOREIGN KEY (ReportId) REFERENCES DobackReports(Id),
    FOREIGN KEY (ValidatedBy) REFERENCES Users(Id)
);

-- Base de conocimiento global
CREATE TABLE KnowledgeBase (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Category NVARCHAR(50),
    Title NVARCHAR(200),
    Description NVARCHAR(MAX),
    Solution NVARCHAR(MAX),
    VehicleTypes NVARCHAR(MAX), -- Tipos de vehículos aplicables
    Severity NVARCHAR(20),
    SuccessRate DECIMAL(5,2),
    TimesToApplied INT,
    CreatedBy INT,
    LastUpdatedBy INT,
    CreatedAt DATETIME2 DEFAULT GETDATE(),
    LastUpdatedAt DATETIME2,
    FOREIGN KEY (CreatedBy) REFERENCES Users(Id),
    FOREIGN KEY (LastUpdatedBy) REFERENCES Users(Id)
);

-- Tabla para relacionar incidentes con conocimiento
CREATE TABLE IncidentKnowledge (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    AlertId INT,
    KnowledgeBaseId INT,
    AppliedDate DATETIME2,
    SuccessfulResolution BIT,
    ResolutionNotes NVARCHAR(MAX),
    TimeToResolve INT, -- minutos
    FOREIGN KEY (AlertId) REFERENCES Alerts(Id),
    FOREIGN KEY (KnowledgeBaseId) REFERENCES KnowledgeBase(Id)
);

-- Tabla para métricas de ML
CREATE TABLE MLMetrics (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    MetricDate DATE,
    VehicleType NVARCHAR(50),
    PatternType NVARCHAR(50),
    AccuracyScore DECIMAL(5,2),
    PrecisionScore DECIMAL(5,2),
    RecallScore DECIMAL(5,2),
    F1Score DECIMAL(5,2),
    SampleSize INT,
    ModelVersion NVARCHAR(20),
    Notes NVARCHAR(MAX)
);

-- Tabla para tendencias globales
CREATE TABLE GlobalTrends (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    TrendType NVARCHAR(50),
    VehicleType NVARCHAR(50),
    StartDate DATE,
    EndDate DATE,
    TrendValue DECIMAL(10,2),
    ChangeRate DECIMAL(5,2),
    Significance DECIMAL(5,2),
    Description NVARCHAR(MAX),
    DetectionMethod NVARCHAR(50)
);

-- Insertar patrones iniciales de ML
INSERT INTO MLPatterns (PatternType, PatternName, Description, DetectionLogic, Confidence, Status)
VALUES 
('Stability', 'High Roll Pattern', 'Patrón de alto riesgo de vuelco en curvas', 
'Roll > 0.7 AND Speed > 40 AND Steer > 1.5', 0.85, 'Active'),

('Maintenance', 'Brake Wear Pattern', 'Indicadores tempranos de desgaste de frenos',
'AccelerationZ pattern variation > 15% over 2 weeks', 0.75, 'Active'),

('Performance', 'Engine Stress Pattern', 'Patrón de estrés del motor en pendientes',
'GyroscopeY > 30 AND AccelerationX > 15 sustained', 0.80, 'Active');

-- Insertar conocimiento base inicial
INSERT INTO KnowledgeBase (
    Category, 
    Title, 
    Description, 
    Solution, 
    VehicleTypes, 
    Severity,
    SuccessRate
)
VALUES 
('Stability', 
'Riesgo de vuelco en curvas cerradas',
'Patrones de inestabilidad detectados en curvas cerradas a velocidad moderada',
'1. Reducir velocidad en 20% en curvas
2. Mantener ángulo de giro constante
3. Evitar cambios bruscos de dirección',
'Fire Truck, Water Tanker, Heavy Rescue',
'High',
0.95),

('Maintenance',
'Desgaste prematuro de frenos',
'Indicadores de desgaste acelerado del sistema de frenos',
'1. Inspección inmediata del sistema
2. Ajuste de presión hidráulica
3. Verificación de distribución de peso',
'All',
'Medium',
0.88);

-- Modificar la generación de alertas para incluir ML
INSERT INTO MLPatternInstances (PatternId, VehicleId, ReportId, DetectionDate, ConfidenceScore)
SELECT 
    (SELECT TOP 1 Id FROM MLPatterns WHERE PatternType = 'Stability'),
    dr.VehicleId,
    dr.Id,
    dr.ReportDate,
    0.85 + (RAND() * 0.10)
FROM DobackReports dr
WHERE ABS(dr.Roll) > 0.7 
AND dr.Speed > 40;

-- Actualizar el resumen final para incluir métricas de ML
SELECT 
    v.LicensePlate,
    v.VehicleType,
    COUNT(DISTINCT mpi.Id) AS MLPatternsDetected,
    AVG(mpi.ConfidenceScore) AS AvgMLConfidence,
    COUNT(DISTINCT ik.Id) AS KnowledgeBaseApplications,
    FORMAT(AVG(CAST(ik.SuccessfulResolution AS DECIMAL)), 'P1') AS KnowledgeSuccessRate
FROM Vehicles v
LEFT JOIN MLPatternInstances mpi ON v.Id = mpi.VehicleId
LEFT JOIN Alerts a ON v.Id = a.VehicleId
LEFT JOIN IncidentKnowledge ik ON a.Id = ik.AlertId
WHERE v.UserId = @UserId
GROUP BY v.LicensePlate, v.VehicleType;

-- Mostrar tendencias globales
INSERT INTO GlobalTrends (
    TrendType,
    VehicleType,
    StartDate,
    EndDate,
    TrendValue,
    ChangeRate,
    Significance,
    Description
)
SELECT 
    'Stability',
    v.VehicleType,
    MIN(dr.ReportDate),
    MAX(dr.ReportDate),
    AVG(dr.StabilityIndex),
    (MAX(dr.StabilityIndex) - MIN(dr.StabilityIndex)) / NULLIF(MIN(dr.StabilityIndex), 0) * 100,
    0.95,
    'Tendencia de estabilidad por tipo de vehículo'
FROM Vehicles v
JOIN DobackReports dr ON v.Id = dr.VehicleId
GROUP BY v.VehicleType;

-- Actualizar el resumen final para incluir más métricas relevantes
SELECT 'Resumen detallado de la flota:' AS Info;

SELECT 
    v.LicensePlate,
    v.Brand + ' ' + v.Model AS Vehicle,
    v.VehicleType,
    COUNT(dr.Id) AS TotalReports,
    FORMAT(AVG(dr.StabilityIndex), 'N2') AS AvgStability,
    FORMAT(MAX(dr.MaxSpeed), 'N1') + ' km/h' AS MaxSpeed,
    FORMAT(AVG(dr.LoadWeight), 'N0') + ' kg' AS AvgLoad,
    SUM(dr.WarningCount) AS Warnings,
    COUNT(DISTINCT m.Id) AS Maintenances,
    FORMAT(SUM(m.Cost), 'C', 'es-ES') AS TotalCost,
    COUNT(DISTINCT a.Id) AS Alerts,
    FORMAT(MAX(pa.PredictionValue), 'P1') AS RiskLevel,
    FORMAT(MAX(pa.Confidence), 'P1') AS Confidence,
    STRING_AGG(DISTINCT dr.WeatherConditions, ', ') AS WeatherConditions
FROM Vehicles v
LEFT JOIN DobackReports dr ON v.Id = dr.VehicleId
LEFT JOIN Maintenance m ON v.Id = m.VehicleId
LEFT JOIN Alerts a ON v.Id = a.VehicleId
LEFT JOIN PredictiveAnalysis pa ON v.Id = pa.VehicleId
WHERE v.UserId = @UserId
GROUP BY v.LicensePlate, v.Brand, v.Model, v.VehicleType
ORDER BY AVG(dr.StabilityIndex);

-- Mostrar estadísticas generales
SELECT 
    'Estadísticas Globales' AS Category,
    COUNT(DISTINCT v.Id) AS TotalVehicles,
    COUNT(DISTINCT dr.Id) AS TotalReports,
    FORMAT(AVG(dr.StabilityIndex), 'N2') AS FleetAvgStability,
    FORMAT(SUM(m.Cost), 'C', 'es-ES') AS TotalMaintenanceCost,
    COUNT(DISTINCT a.Id) AS TotalAlerts
FROM Vehicles v
LEFT JOIN DobackReports dr ON v.Id = dr.VehicleId
LEFT JOIN Maintenance m ON v.Id = m.VehicleId
LEFT JOIN Alerts a ON v.Id = a.VehicleId
WHERE v.UserId = @UserId;

-- Nuevas tablas para análisis avanzado

-- 1. Telemetría Avanzada
CREATE TABLE TelemetryAnalysis (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    VehicleId INT,
    AnalysisDate DATETIME2,
    RouteComplexity DECIMAL(5,2),
    DrivingScore DECIMAL(5,2),
    RiskFactors NVARCHAR(MAX),
    TerrainType NVARCHAR(50),
    WeatherImpact DECIMAL(5,2),
    EmergencyResponseTime INT,
    CreatedAt DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (VehicleId) REFERENCES Vehicles(Id)
);

CREATE TABLE DrivingPatterns (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    VehicleId INT,
    PatternDate DATETIME2,
    AccelerationPattern NVARCHAR(MAX),
    BrakingPattern NVARCHAR(MAX),
    TurningPattern NVARCHAR(MAX),
    SpeedPattern NVARCHAR(MAX),
    RiskLevel DECIMAL(5,2),
    CreatedAt DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (VehicleId) REFERENCES Vehicles(Id)
);

-- 2. Machine Learning Avanzado
CREATE TABLE MLModels (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ModelName NVARCHAR(100),
    ModelType NVARCHAR(50),
    Parameters NVARCHAR(MAX),
    TrainingData NVARCHAR(MAX),
    Accuracy DECIMAL(5,2),
    LastTrainingDate DATETIME2,
    Version NVARCHAR(20),
    Status NVARCHAR(50),
    CreatedAt DATETIME2 DEFAULT GETDATE()
);

CREATE TABLE MLFeatureImportance (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ModelId INT,
    FeatureName NVARCHAR(100),
    Importance DECIMAL(5,2),
    Description NVARCHAR(MAX),
    CreatedAt DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (ModelId) REFERENCES MLModels(Id)
);

-- 3. Sistema de Recomendaciones
CREATE TABLE RecommendationEngine (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    VehicleType NVARCHAR(50),
    Scenario NVARCHAR(100),
    RecommendationType NVARCHAR(50),
    RecommendationText NVARCHAR(MAX),
    ConfidenceScore DECIMAL(5,2),
    SuccessRate DECIMAL(5,2),
    ApplicabilityConditions NVARCHAR(MAX),
    CreatedAt DATETIME2 DEFAULT GETDATE(),
    LastUpdated DATETIME2
);

-- 4. Análisis Predictivo Mejorado
CREATE TABLE DetailedPredictions (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    VehicleId INT,
    PredictionDate DATETIME2,
    ComponentType NVARCHAR(50),
    FailureProbability DECIMAL(5,2),
    TimeToFailure INT,
    MaintenanceUrgency NVARCHAR(20),
    CostImplication DECIMAL(10,2),
    CreatedAt DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (VehicleId) REFERENCES Vehicles(Id)
);

-- 5. Integración Climática
CREATE TABLE WeatherImpactAnalysis (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    LocationId NVARCHAR(50),
    WeatherCondition NVARCHAR(50),
    VehicleType NVARCHAR(50),
    PerformanceImpact DECIMAL(5,2),
    SafetyThreshold DECIMAL(5,2),
    RecommendedActions NVARCHAR(MAX),
    CreatedAt DATETIME2 DEFAULT GETDATE()
);

-- 6. Análisis de Rutas
CREATE TABLE RouteAnalysis (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    VehicleId INT,
    RouteDate DATETIME2,
    StartPoint GEOGRAPHY,
    EndPoint GEOGRAPHY,
    RouteComplexity DECIMAL(5,2),
    RiskHotspots NVARCHAR(MAX),
    OptimalSpeed DECIMAL(5,2),
    SafetyScore DECIMAL(5,2),
    CreatedAt DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (VehicleId) REFERENCES Vehicles(Id)
);

-- 7. Sistema de Alertas Avanzado
CREATE TABLE AdvancedAlertSystem (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    AlertTemplate NVARCHAR(MAX),
    TriggerConditions NVARCHAR(MAX),
    Priority INT,
    AutoActions NVARCHAR(MAX),
    NotificationRules NVARCHAR(MAX),
    CreatedAt DATETIME2 DEFAULT GETDATE(),
    LastModified DATETIME2
);

-- Índices optimizados para las nuevas tablas
CREATE INDEX IX_TelemetryAnalysis_VehicleDate ON TelemetryAnalysis(VehicleId, AnalysisDate);
CREATE INDEX IX_DrivingPatterns_VehicleDate ON DrivingPatterns(VehicleId, PatternDate);
CREATE INDEX IX_DetailedPredictions_VehicleDate ON DetailedPredictions(VehicleId, PredictionDate);
CREATE INDEX IX_RouteAnalysis_VehicleDate ON RouteAnalysis(VehicleId, RouteDate);
CREATE INDEX IX_MLModels_Status ON MLModels(Status);
CREATE INDEX IX_WeatherImpactAnalysis_Location ON WeatherImpactAnalysis(LocationId, WeatherCondition);

-- Insertar datos de ejemplo para ML
INSERT INTO MLModels (ModelName, ModelType, Parameters, Accuracy, Status, Version)
VALUES 
('StabilityPredictor', 'RandomForest', 
'{"n_estimators": 100, "max_depth": 10}', 0.92, 'Active', '1.0.0'),
('MaintenancePredictor', 'GradientBoosting',
'{"learning_rate": 0.1, "n_estimators": 150}', 0.89, 'Active', '1.0.0');

-- Insertar recomendaciones iniciales
INSERT INTO RecommendationEngine (
    VehicleType, 
    Scenario, 
    RecommendationType, 
    RecommendationText, 
    ConfidenceScore
)
VALUES 
('Fire Truck', 'High Roll Risk', 'Safety',
'Reducir velocidad en 30% en curvas con radio < 100m', 0.95),
('Water Tanker', 'Steep Descent', 'Operation',
'Mantener velocidad constante y evitar frenados bruscos', 0.92);

-- Actualizar el resumen final para incluir nuevas métricas
SELECT 
    v.LicensePlate,
    v.VehicleType,
    COUNT(dr.Id) AS TotalReports,
    AVG(ta.DrivingScore) AS AvgDrivingScore,
    MAX(dp.RiskLevel) AS MaxRiskLevel,
    COUNT(DISTINCT ra.Id) AS AnalyzedRoutes,
    AVG(ra.SafetyScore) AS AvgSafetyScore,
    FORMAT(MAX(dp.FailureProbability), 'P1') AS MaxFailureProb
FROM Vehicles v
LEFT JOIN DobackReports dr ON v.Id = dr.VehicleId
LEFT JOIN TelemetryAnalysis ta ON v.Id = ta.VehicleId
LEFT JOIN DrivingPatterns dp ON v.Id = dp.VehicleId
LEFT JOIN RouteAnalysis ra ON v.Id = ra.VehicleId
WHERE v.UserId = @UserId
GROUP BY v.LicensePlate, v.VehicleType
ORDER BY AVG(ta.DrivingScore) DESC;

GO 