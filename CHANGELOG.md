# Changelog

## [Unreleased]

### Added
- Implementación completa de servicios de análisis predictivo
- Nuevos endpoints en VehicleController para análisis y predicciones
- Sistema de logging detallado en todos los servicios

### Changed
- Refactorización de VehicleService para usar tipos de Analysis.Core
- Optimización de consultas con AsNoTracking()
- Mejora en el manejo de errores y excepciones

### Fixed
- Corrección de referencias circulares en modelos
- Eliminación de tipos duplicados
- Actualización de namespaces para evitar ambigüedad

## [1.0.0] - 2024-03-19

### Added
- Implementación inicial del sistema IncliSafe
- Módulo de análisis de inclinación
- Sistema de predicción de mantenimiento
- Interfaz de usuario en Blazor WebAssembly

### Changed
- Migración a .NET 8
- Actualización de dependencias
- Mejora en la estructura del proyecto

### Security
- Implementación de autenticación JWT
- Protección de rutas con AuthorizeView
- Manejo seguro de credenciales 