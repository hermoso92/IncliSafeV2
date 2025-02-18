-- Crear la base de datos si no existe
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'IncliSafeDB')
BEGIN
    CREATE DATABASE IncliSafeDB;
END
GO

USE IncliSafeDB;
GO

-- Habilitar tablas temporales a nivel de base de datos
IF NOT EXISTS (SELECT 1 FROM sys.database_temporal_period_settings)
BEGIN
    ALTER DATABASE IncliSafeDB SET TEMPORAL_HISTORY_RETENTION ON;
END
GO

-- Asegurar que las tablas temporales están habilitadas
IF NOT EXISTS (SELECT 1 FROM sys.change_tracking_databases WHERE database_id = DB_ID())
BEGIN
    ALTER DATABASE IncliSafeDB
    SET CHANGE_TRACKING = ON
    (CHANGE_RETENTION = 2 DAYS, AUTO_CLEANUP = ON)
END
GO

-- Configurar el aislamiento de instantáneas
ALTER DATABASE IncliSafeDB SET ALLOW_SNAPSHOT_ISOLATION ON;
GO

-- Configurar el control de versiones optimista
ALTER DATABASE IncliSafeDB SET READ_COMMITTED_SNAPSHOT ON;
GO

-- Configurar la recuperación simple para mejor rendimiento
ALTER DATABASE IncliSafeDB SET RECOVERY SIMPLE;
GO

-- Crear índices después de que las tablas existan
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'DobackData')
BEGIN
    IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_DobackData_Timestamp')
    CREATE NONCLUSTERED INDEX [IX_DobackData_Timestamp] ON [dbo].[DobackData] ([Timestamp]);
END
GO

IF EXISTS (SELECT * FROM sys.tables WHERE name = 'DobackAnalyses')
BEGIN
    IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_DobackAnalysis_VehicleId_Timestamp')
    CREATE NONCLUSTERED INDEX [IX_DobackAnalysis_VehicleId_Timestamp] 
    ON [dbo].[DobackAnalyses] ([VehicleId], [Timestamp]);
END
GO

IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Notifications')
BEGIN
    IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Notifications_UserId_CreatedAt')
    CREATE NONCLUSTERED INDEX [IX_Notifications_UserId_CreatedAt] 
    ON [dbo].[Notifications] ([UserId], [CreatedAt]);
END
GO 