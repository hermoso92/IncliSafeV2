using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using IncliSafe.Shared.Models.Entities;

namespace IncliSafeApi.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Usuarios.Any())
                return;

            var admin = new Usuario
            {
                Nombre = "Administrador",
                Username = "admin",
                Email = "admin@inclisafe.com",
                Rol = "Administrador",
                Activo = true,
                CreatedAt = DateTime.UtcNow
            };

            context.Usuarios.Add(admin);
            context.SaveChanges();
        }
    }
} 