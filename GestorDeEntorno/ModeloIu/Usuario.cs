﻿using Gestor.Elementos.ModeloIu;
using System;
using System.Linq;

namespace Gestor.Elementos.Entorno
{

    public static class UsuariosPor
    {
       public static string NombreCompleto = nameof(NombreCompleto).ToLower();
       public static string Permisos = nameof(Permisos).ToLower();
    }

    public class UsuarioDto : Elemento
    {
        [IUCreacion(
            Etiqueta = "Usuario",
            Ayuda = "Usuario de conexión", 
            Tipo = typeof(string), 
            Visible = true, 
            Fila = 0, 
            Columna = 0
            )
        ]
        public string Login { get; set; }


        [IUCreacion(
            Etiqueta = "Nombre de usuario",
            Ayuda = "Introducir el nombre de usuario (Apellidos y Nombre)",
            Tipo = typeof(string),
            Visible = true,
            Fila = 1,
            Columna = 0
            )
        ]
        public string Apellido { get; set; }


        [IUCreacion(
            Ayuda = "Nombre",
            Tipo = typeof(string),
            Visible = true,
            Fila = 1,
            Columna = 1,
            Posicion = 0
            )
        ]
        public string Nombre { get; set; }


        [IUCreacion(
            Etiqueta = "Fecha de alta",
            Tipo = typeof(DateTime),
            Visible = true,
            Editable = false,
            ValorPorDefecto="AlMostrar(GetDate())",
            Fila = 2,
            Columna = 1
            )
        ]
        public DateTime Alta { get; set; }

    }



}
