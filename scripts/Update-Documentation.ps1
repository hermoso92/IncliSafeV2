# Documentation Management Script
# Author: Claude AI
# Version: 1.0.0

$ErrorActionPreference = 'Stop'
$VerbosePreference = 'Continue'

# Configuration
$config = @{
    DocsPath = "..\docs"
    ApiPath = "..\IncliSafeApi"
    ClientPath = "..\IncliSafe.Client"
    SharedPath = "..\IncliSafe.Shared"
}

# Create documentation structure
function Initialize-Documentation {
    Write-Verbose "Initializing documentation structure..."
    
    @(
        "$($config.DocsPath)\api",
        "$($config.DocsPath)\client",
        "$($config.DocsPath)\database",
        "$($config.DocsPath)\guides",
        "$($config.DocsPath)\architecture"
    ) | ForEach-Object {
        if (-not (Test-Path $_)) {
            New-Item -ItemType Directory -Path $_ -Force
        }
    }
}

# Generate API documentation
function Update-ApiDocumentation {
    Write-Verbose "Updating API documentation..."
    
    # Create API endpoints documentation
    $controllers = Get-ChildItem -Path "$($config.ApiPath)\Controllers" -Filter "*Controller.cs"
    $apiDocs = @"
# API Documentation

## Endpoints

"@
    
    foreach ($controller in $controllers) {
        $content = Get-Content $controller.FullName
        $apiDocs += "`n### $($controller.BaseName)`n`n"
        
        foreach ($line in $content) {
            if ($line -match "\[Http(Get|Post|Put|Delete)\]") {
                $apiDocs += "- $($matches[0])`n"
            }
        }
    }
    
    Set-Content -Path "$($config.DocsPath)\api\endpoints.md" -Value $apiDocs
}

# Generate database documentation
function Update-DatabaseDocumentation {
    Write-Verbose "Updating database documentation..."
    
    # Document database context and models
    $models = Get-ChildItem -Path "$($config.SharedPath)\Models" -Filter "*.cs"
    $dbDocs = @"
# Database Documentation

## Models

"@
    
    foreach ($model in $models) {
        $content = Get-Content $model.FullName
        $dbDocs += "`n### $($model.BaseName)`n`n"
        
        foreach ($line in $content) {
            if ($line -match "public\s+\w+\s+\w+\s*{\s*get;\s*set;\s*}") {
                $dbDocs += "- $line`n"
            }
        }
    }
    
    Set-Content -Path "$($config.DocsPath)\database\models.md" -Value $dbDocs
}

# Generate architecture documentation
function Update-ArchitectureDocumentation {
    Write-Verbose "Updating architecture documentation..."
    
    $archDocs = @"
# IncliSafe Architecture

## Overview
IncliSafe is a modern web application built with:
- Blazor WebAssembly for the client-side
- .NET 8 Web API for the backend
- SQL Server for data storage
- SignalR for real-time communications

## Project Structure
- IncliSafe.Client: Blazor WebAssembly frontend
- IncliSafeApi: .NET 8 backend API
- IncliSafe.Shared: Shared models and utilities
- IncliSafe.Tests: Unit and integration tests

## Key Components
1. Authentication & Authorization
   - JWT-based authentication
   - Role-based authorization
   - Secure token storage

2. Real-time Features
   - SignalR hubs for live updates
   - WebSocket connections
   - Event-driven architecture

3. Data Access
   - Entity Framework Core
   - Repository pattern
   - CQRS implementation

4. UI Components
   - MudBlazor component library
   - Responsive design
   - Progressive Web App (PWA)

## Security Measures
- HTTPS enforcement
- XSS protection
- CSRF prevention
- SQL injection protection
- Input validation
"@
    
    Set-Content -Path "$($config.DocsPath)\architecture\overview.md" -Value $archDocs
}

# Generate user guides
function Update-UserGuides {
    Write-Verbose "Updating user guides..."
    
    $guides = @(
        @{
            Title = "Getting Started"
            Content = @"
# Getting Started with IncliSafe

## Prerequisites
- .NET 8 SDK
- SQL Server 2022
- Modern web browser

## Installation
1. Clone the repository
2. Run \`dotnet restore\`
3. Configure the database connection
4. Run \`dotnet run\`

## First Steps
1. Create an admin account
2. Configure system settings
3. Add users and roles
4. Start monitoring
"@
        },
        @{
            Title = "Development Guide"
            Content = @"
# Development Guide

## Setup Development Environment
1. Install Visual Studio 2022
2. Install SQL Server Management Studio
3. Configure user secrets
4. Set up Git hooks

## Coding Standards
- Use C# coding conventions
- Follow SOLID principles
- Write unit tests
- Document public APIs

## Deployment
1. Build release version
2. Run database migrations
3. Deploy to production
4. Monitor logs
"@
        }
    )
    
    foreach ($guide in $guides) {
        $filename = $guide.Title -replace '\s+', '-'
        Set-Content -Path "$($config.DocsPath)\guides\$filename.md" -Value $guide.Content
    }
}

# Update README
function Update-MainReadme {
    Write-Verbose "Updating main README..."
    
    $readme = @"
# IncliSafe Documentation

## Overview
Welcome to the IncliSafe documentation. This documentation is automatically generated and updated.

## Contents
1. [API Documentation](api/endpoints.md)
2. [Database Documentation](database/models.md)
3. [Architecture Overview](architecture/overview.md)
4. [User Guides](guides/)

## Quick Links
- [Getting Started](guides/Getting-Started.md)
- [Development Guide](guides/Development-Guide.md)
- [API Endpoints](api/endpoints.md)

## Contributing
Please read our [Development Guide](guides/Development-Guide.md) before contributing.

## Support
For support, please contact the development team.
"@
    
    Set-Content -Path "$($config.DocsPath)\README.md" -Value $readme
}

# Main execution flow
try {
    Initialize-Documentation
    Update-ApiDocumentation
    Update-DatabaseDocumentation
    Update-ArchitectureDocumentation
    Update-UserGuides
    Update-MainReadme
    
    Write-Host "Documentation updated successfully!" -ForegroundColor Green
}
catch {
    Write-Error "Error updating documentation: $_"
    exit 1
} 