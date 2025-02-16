🚀 IncliSafe - Sistema de Análisis Doback
=======================================

📋 Descripción
-------------
Plataforma especializada en el análisis de datos Doback para monitoreo 
de estabilidad vehicular. Procesa archivos de telemetría para generar 
análisis en tiempo real y detectar patrones de comportamiento críticos.

⚡ Funcionalidades Core
--------------------
📊 Análisis de Datos
  • Procesamiento de archivos Doback (.txt)
  • Análisis de estabilidad en tiempo real
  • Cálculo de índices de rendimiento
  • Detección de eventos críticos

📈 Visualización
  • Gráficas de aceleración (X,Y,Z)
  • Visualización de orientación
  • Índices de estabilidad
  • Exportación de resultados

🔔 Alertas y Monitoreo
  • Notificaciones en tiempo real
  • Umbrales configurables
  • Registro de eventos
  • Histórico de alertas

🛠️ Tecnologías
------------
• .NET 8
• Blazor WebAssembly
• SQL Server 2022
• MudBlazor UI
• ECharts
• SignalR

⚙️ Requisitos
-----------
• .NET 8 SDK
• SQL Server 2022
• 8GB RAM
• Navegador moderno
• VS 2022

📦 Instalación
------------
1️⃣ Repositorio:
   git clone https://github.com/[org]/inclisafe.git

2️⃣ Dependencias:
   dotnet restore
   dotnet build

3️⃣ Database:
   dotnet ef database update

4️⃣ Ejecución:
   dotnet watch run

🔧 Configuración
-------------
appsettings.json:
{
  "ConnectionStrings": {
    "Default": "Server=...;Database=IncliSafe;"
  },
  "Doback": {
    "SamplingRate": 100,
    "BufferSize": 1024
  }
}

📱 Módulos
--------
• Análisis Doback
• Monitoreo
• Reportes
• Configuración
• Administración

🔄 DevOps
-------
• GitHub Actions
• Tests (xUnit)
• SonarQube
• Azure Deploy

📅 Roadmap
--------
2024-Q1
  • Optimización de análisis
  • Mejoras en UI/UX
  • Nuevos reportes

2024-Q2
  • API REST
  • Más sensores
  • Documentación

💬 Soporte
--------
• docs.inclisafe.com
• soporte@inclisafe.com
• GitHub Issues

⚖️ Licencia
---------
Propietario
© 2024 IncliSafe 