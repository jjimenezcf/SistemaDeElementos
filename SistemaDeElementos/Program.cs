﻿using System;
using Gestor.Elementos.Entorno;
using Gestor.Elementos.Seguridad;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
namespace MVCSistemaDeElementos
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var servidorWeb = CreateWebHostBuilder(args).Build();
            CrearBdSiNoExiste(servidorWeb);
            servidorWeb.Run();
        }

        private static void CrearBdSiNoExiste(IWebHost sevidorWeb)
        {
            var scope = sevidorWeb.Services.CreateScope();
            var services = scope.ServiceProvider;
            IniciarContextoDeEntorno(services);
            IniciarContextoDeSeguro(services);
        }

        private static void IniciarContextoDeSeguro(IServiceProvider services)
        {
            var ctoPermisos = services.GetRequiredService<CtoPermisos>();
            try
            {
                ctoPermisos.Database.Migrate();
                ctoPermisos.IniciarTraza();
            }
            catch (Exception ex)
            {
                Gestor.Errores.GestorDeErrores.EnviaError("Error al inicializar la BD.", ex);
                throw new Exception($"Error al conectarse al contexto {ctoPermisos.GetType().Name}", ex);
            }
            finally
            {
                if (ctoPermisos != null)
                    ctoPermisos.CerrarTraza();
            }
        }

        private static void IniciarContextoDeEntorno(IServiceProvider services)
        {
            var ctoEntorno = services.GetRequiredService<CtoEntorno>();
            var gestorDeMenus = services.GetRequiredService<GestorDeMenus>();
            var gestorDeVistasMvc = services.GetRequiredService<GestorDeVistasMvc>();
            try
            {
                ctoEntorno.Database.Migrate();
                ctoEntorno.IniciarTraza();
                CtoEntorno.NuevaVersion(ctoEntorno);
                CtoEntorno.InicializarMaestros(ctoEntorno, gestorDeMenus, gestorDeVistasMvc);
            }
            catch (Exception ex)
            {
                Gestor.Errores.GestorDeErrores.EnviaError("Error al inicializar la BD.", ex);
                throw new Exception($"Error al conectarse al contexto {ctoEntorno.GetType().Name}", ex);
            }
            finally
            {
                if (ctoEntorno != null)
                    ctoEntorno.CerrarTraza();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                   .UseStartup<Startup>();
    }
}
