-- Crear la base de datos
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'IncliSafeDB')
BEGIN
    CREATE DATABASE IncliSafeDB;
END
GO

USE IncliSafeDB;
GO

-- Crear esquemas
IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'Analysis')
BEGIN
    EXEC('CREATE SCHEMA Analysis')
END
GO

IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'Vehicle')
BEGIN
    EXEC('CREATE SCHEMA Vehicle')
END
GO

IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'Maintenance')
BEGIN
    EXEC('CREATE SCHEMA Maintenance')
END
GO

-- Tabla de Vehículos
CREATE TABLE Vehicle.Vehicles (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(500),
    Type NVARCHAR(50) NOT NULL,
    Status NVARCHAR(50) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    LastMaintenanceDate DATETIME2,
    NextMaintenanceDate DATETIME2,
    TotalMileage DECIMAL(10,2),
    CONSTRAINT UQ_Vehicles_Name UNIQUE (Name)
);
GO

-- Tabla de Sensores
CREATE TABLE Vehicle.Sensors (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    VehicleId INT NOT NULL,
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(500),
    Type NVARCHAR(50) NOT NULL,
    Location NVARCHAR(100),
    Status NVARCHAR(50) NOT NULL,
    LastReading DATETIME2,
    LastValue DECIMAL(10,2),
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Sensors_Vehicles FOREIGN KEY (VehicleId) REFERENCES Vehicle.Vehicles(Id),
    CONSTRAINT UQ_Sensors_VehicleName UNIQUE (VehicleId, Name)
);
GO

-- Tabla de Análisis
CREATE TABLE Analysis.Analyses (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    VehicleId INT NOT NULL,
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(500),
    Type NVARCHAR(50) NOT NULL,
    Score DECIMAL(5,2) NOT NULL,
    AnalyzedAt DATETIME2 NOT NULL,
    AnalysisDate DATETIME2 NOT NULL,
    StabilityScore DECIMAL(5,2) NOT NULL,
    SafetyScore DECIMAL(5,2) NOT NULL,
    MaintenanceScore DECIMAL(5,2) NOT NULL,
    EfficiencyScore DECIMAL(5,2) NOT NULL,
    Notes NVARCHAR(MAX),
    Severity NVARCHAR(50) NOT NULL,
    Confidence DECIMAL(5,2) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Analyses_Vehicles FOREIGN KEY (VehicleId) REFERENCES Vehicle.Vehicles(Id)
);
GO

-- Tabla de Datos de Análisis
CREATE TABLE Analysis.AnalysisData (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    AnalysisId INT NOT NULL,
    Timestamp DATETIME2 NOT NULL,
    Value DECIMAL(10,2) NOT NULL,
    SensorId INT NOT NULL,
    MetricType NVARCHAR(50),
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_AnalysisData_Analyses FOREIGN KEY (AnalysisId) REFERENCES Analysis.Analyses(Id),
    CONSTRAINT FK_AnalysisData_Sensors FOREIGN KEY (SensorId) REFERENCES Vehicle.Sensors(Id)
);
GO

-- Tabla de Métricas Adicionales de Análisis
CREATE TABLE Analysis.AnalysisAdditionalMetrics (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    AnalysisDataId INT NOT NULL,
    MetricKey NVARCHAR(100) NOT NULL,
    MetricValue DECIMAL(10,2) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_AnalysisAdditionalMetrics_AnalysisData FOREIGN KEY (AnalysisDataId) REFERENCES Analysis.AnalysisData(Id)
);
GO

-- Tabla de Recomendaciones de Análisis
CREATE TABLE Analysis.AnalysisRecommendations (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    AnalysisId INT NOT NULL,
    Recommendation NVARCHAR(MAX) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_AnalysisRecommendations_Analyses FOREIGN KEY (AnalysisId) REFERENCES Analysis.Analyses(Id)
);
GO

-- Tabla de Parámetros de Análisis
CREATE TABLE Analysis.AnalysisParameters (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    AnalysisId INT NOT NULL,
    ParameterKey NVARCHAR(100) NOT NULL,
    ParameterValue NVARCHAR(MAX) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_AnalysisParameters_Analyses FOREIGN KEY (AnalysisId) REFERENCES Analysis.Analyses(Id)
);
GO

-- Tabla de Anomalías
CREATE TABLE Analysis.Anomalies (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    VehicleId INT NOT NULL,
    AnalysisId INT,
    DetectedAt DATETIME2 NOT NULL,
    Type NVARCHAR(50) NOT NULL,
    Severity NVARCHAR(50) NOT NULL,
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(MAX) NOT NULL,
    Score DECIMAL(5,2) NOT NULL,
    ExpectedValue DECIMAL(10,2) NOT NULL,
    ActualValue DECIMAL(10,2) NOT NULL,
    Deviation DECIMAL(10,2) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Anomalies_Vehicles FOREIGN KEY (VehicleId) REFERENCES Vehicle.Vehicles(Id),
    CONSTRAINT FK_Anomalies_Analyses FOREIGN KEY (AnalysisId) REFERENCES Analysis.Analyses(Id)
);
GO

-- Tabla de Parámetros de Anomalías
CREATE TABLE Analysis.AnomalyParameters (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    AnomalyId INT NOT NULL,
    ParameterKey NVARCHAR(100) NOT NULL,
    ParameterValue NVARCHAR(MAX) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_AnomalyParameters_Anomalies FOREIGN KEY (AnomalyId) REFERENCES Analysis.Anomalies(Id)
);
GO

-- Tabla de Predicciones
CREATE TABLE Analysis.Predictions (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    VehicleId INT NOT NULL,
    AnalysisId INT,
    PredictedAt DATETIME2 NOT NULL,
    Type NVARCHAR(50) NOT NULL,
    Probability DECIMAL(5,2) NOT NULL,
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(MAX) NOT NULL,
    PredictedValue DECIMAL(10,2) NOT NULL,
    ValidUntil DATETIME2 NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Predictions_Vehicles FOREIGN KEY (VehicleId) REFERENCES Vehicle.Vehicles(Id),
    CONSTRAINT FK_Predictions_Analyses FOREIGN KEY (AnalysisId) REFERENCES Analysis.Analyses(Id)
);
GO

-- Tabla de Parámetros de Predicciones
CREATE TABLE Analysis.PredictionParameters (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    PredictionId INT NOT NULL,
    ParameterKey NVARCHAR(100) NOT NULL,
    ParameterValue NVARCHAR(MAX) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_PredictionParameters_Predictions FOREIGN KEY (PredictionId) REFERENCES Analysis.Predictions(Id)
);
GO

-- Índices para optimizar consultas frecuentes
CREATE INDEX IX_Vehicles_Status ON Vehicle.Vehicles(Status);
CREATE INDEX IX_Vehicles_LastMaintenanceDate ON Vehicle.Vehicles(LastMaintenanceDate);
CREATE INDEX IX_Vehicles_NextMaintenanceDate ON Vehicle.Vehicles(NextMaintenanceDate);

CREATE INDEX IX_Sensors_VehicleId ON Vehicle.Sensors(VehicleId);
CREATE INDEX IX_Sensors_Status ON Vehicle.Sensors(Status);
CREATE INDEX IX_Sensors_LastReading ON Vehicle.Sensors(LastReading);

CREATE INDEX IX_Analyses_VehicleId ON Analysis.Analyses(VehicleId);
CREATE INDEX IX_Analyses_Type ON Analysis.Analyses(Type);
CREATE INDEX IX_Analyses_AnalyzedAt ON Analysis.Analyses(AnalyzedAt);
CREATE INDEX IX_Analyses_Severity ON Analysis.Analyses(Severity);

CREATE INDEX IX_AnalysisData_AnalysisId ON Analysis.AnalysisData(AnalysisId);
CREATE INDEX IX_AnalysisData_SensorId ON Analysis.AnalysisData(SensorId);
CREATE INDEX IX_AnalysisData_Timestamp ON Analysis.AnalysisData(Timestamp);

CREATE INDEX IX_Anomalies_VehicleId ON Analysis.Anomalies(VehicleId);
CREATE INDEX IX_Anomalies_Type ON Analysis.Anomalies(Type);
CREATE INDEX IX_Anomalies_Severity ON Analysis.Anomalies(Severity);
CREATE INDEX IX_Anomalies_DetectedAt ON Analysis.Anomalies(DetectedAt);

CREATE INDEX IX_Predictions_VehicleId ON Analysis.Predictions(VehicleId);
CREATE INDEX IX_Predictions_Type ON Analysis.Predictions(Type);
CREATE INDEX IX_Predictions_PredictedAt ON Analysis.Predictions(PredictedAt);
CREATE INDEX IX_Predictions_ValidUntil ON Analysis.Predictions(ValidUntil);

-- Procedimientos almacenados para operaciones comunes
CREATE OR ALTER PROCEDURE Analysis.InsertAnalysis
    @VehicleId INT,
    @Name NVARCHAR(100),
    @Description NVARCHAR(500),
    @Type NVARCHAR(50),
    @Score DECIMAL(5,2),
    @AnalyzedAt DATETIME2,
    @AnalysisDate DATETIME2,
    @StabilityScore DECIMAL(5,2),
    @SafetyScore DECIMAL(5,2),
    @MaintenanceScore DECIMAL(5,2),
    @EfficiencyScore DECIMAL(5,2),
    @Notes NVARCHAR(MAX),
    @Severity NVARCHAR(50),
    @Confidence DECIMAL(5,2)
AS
BEGIN
    SET NOCOUNT ON;
    
    INSERT INTO Analysis.Analyses (
        VehicleId, Name, Description, Type, Score, AnalyzedAt, AnalysisDate,
        StabilityScore, SafetyScore, MaintenanceScore, EfficiencyScore,
        Notes, Severity, Confidence
    )
    VALUES (
        @VehicleId, @Name, @Description, @Type, @Score, @AnalyzedAt, @AnalysisDate,
        @StabilityScore, @SafetyScore, @MaintenanceScore, @EfficiencyScore,
        @Notes, @Severity, @Confidence
    );

    SELECT SCOPE_IDENTITY() AS Id;
END
GO

CREATE OR ALTER PROCEDURE Analysis.InsertAnomaly
    @VehicleId INT,
    @AnalysisId INT,
    @DetectedAt DATETIME2,
    @Type NVARCHAR(50),
    @Severity NVARCHAR(50),
    @Name NVARCHAR(100),
    @Description NVARCHAR(MAX),
    @Score DECIMAL(5,2),
    @ExpectedValue DECIMAL(10,2),
    @ActualValue DECIMAL(10,2),
    @Deviation DECIMAL(10,2)
AS
BEGIN
    SET NOCOUNT ON;
    
    INSERT INTO Analysis.Anomalies (
        VehicleId, AnalysisId, DetectedAt, Type, Severity, Name, Description,
        Score, ExpectedValue, ActualValue, Deviation
    )
    VALUES (
        @VehicleId, @AnalysisId, @DetectedAt, @Type, @Severity, @Name, @Description,
        @Score, @ExpectedValue, @ActualValue, @Deviation
    );

    SELECT SCOPE_IDENTITY() AS Id;
END
GO

CREATE OR ALTER PROCEDURE Analysis.InsertPrediction
    @VehicleId INT,
    @AnalysisId INT,
    @PredictedAt DATETIME2,
    @Type NVARCHAR(50),
    @Probability DECIMAL(5,2),
    @Name NVARCHAR(100),
    @Description NVARCHAR(MAX),
    @PredictedValue DECIMAL(10,2),
    @ValidUntil DATETIME2
AS
BEGIN
    SET NOCOUNT ON;
    
    INSERT INTO Analysis.Predictions (
        VehicleId, AnalysisId, PredictedAt, Type, Probability, Name, Description,
        PredictedValue, ValidUntil
    )
    VALUES (
        @VehicleId, @AnalysisId, @PredictedAt, @Type, @Probability, @Name, @Description,
        @PredictedValue, @ValidUntil
    );

    SELECT SCOPE_IDENTITY() AS Id;
END
GO

-- Función para calcular el estado general de un vehículo
CREATE OR ALTER FUNCTION Vehicle.CalculateVehicleStatus
(
    @VehicleId INT
)
RETURNS NVARCHAR(50)
AS
BEGIN
    DECLARE @Status NVARCHAR(50);
    DECLARE @LastAnalysisScore DECIMAL(5,2);
    DECLARE @HasCriticalAnomalies BIT;
    DECLARE @NextMaintenanceDate DATETIME2;

    -- Obtener la última puntuación de análisis
    SELECT TOP 1 @LastAnalysisScore = Score
    FROM Analysis.Analyses
    WHERE VehicleId = @VehicleId
    ORDER BY AnalyzedAt DESC;

    -- Verificar si hay anomalías críticas
    SELECT @HasCriticalAnomalies = CASE WHEN COUNT(*) > 0 THEN 1 ELSE 0 END
    FROM Analysis.Anomalies
    WHERE VehicleId = @VehicleId
    AND Severity = 'Critical'
    AND DetectedAt >= DATEADD(DAY, -7, GETDATE());

    -- Obtener la fecha del próximo mantenimiento
    SELECT @NextMaintenanceDate = NextMaintenanceDate
    FROM Vehicle.Vehicles
    WHERE Id = @VehicleId;

    -- Determinar el estado
    IF @LastAnalysisScore IS NULL
        SET @Status = 'Unknown'
    ELSE IF @HasCriticalAnomalies = 1
        SET @Status = 'Critical'
    ELSE IF @LastAnalysisScore < 60
        SET @Status = 'Warning'
    ELSE IF @NextMaintenanceDate IS NOT NULL AND @NextMaintenanceDate <= DATEADD(DAY, 7, GETDATE())
        SET @Status = 'MaintenanceNeeded'
    ELSE
        SET @Status = 'Normal';

    RETURN @Status;
END
GO

-- Trigger para actualizar el estado del vehículo
CREATE OR ALTER TRIGGER Vehicle.UpdateVehicleStatus
ON Vehicle.Vehicles
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE v
    SET Status = Vehicle.CalculateVehicleStatus(v.Id)
    FROM Vehicle.Vehicles v
    INNER JOIN inserted i ON v.Id = i.Id;
END
GO 