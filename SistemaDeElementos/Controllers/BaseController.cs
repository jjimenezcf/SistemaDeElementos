﻿using Gestor.Errores;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServicioDeDatos;

namespace MVCSistemaDeElementos.Controllers
{
    public enum EstadoPeticion { Ok, Error }
    public class Resultado
    {
        public EstadoPeticion Estado { get; set; }
        public string Mensaje { get; set; }
        public string consola { get; set; }
        public int Total { get; set; } = 0;
        public dynamic Datos { get; set; }
    }

    public class ResultadoHtml : Resultado
    {
        public string Html { get; set; }
    }

    public class BaseController : Controller
    {
        protected GestorDeErrores GestorDeErrores { get; }
        public ILogger Logger { get; set; }
        protected DatosDeConexion DatosDeConexion { get; set; }

        public BaseController(GestorDeErrores gestorDeErrores)
        {
            GestorDeErrores = gestorDeErrores;
        }
    }
}
