﻿using System;
using Enumerados;

namespace ModeloDeDto.Entorno
{

    public static class UsuariosPor
    {
        public static string NombreCompleto = nameof(NombreCompleto).ToLower();
        public static string Permisos = nameof(Permisos).ToLower();
        public static string AlgunUsuario = nameof(AlgunUsuario).ToLower();
        public static string eMail = nameof(eMail).ToLower();
    }

    [IUDto(AnchoEtiqueta = 20, AnchoSeparador = 5, MostrarExpresion = ExpresionElemento)]
    public class UsuarioDto : ElementoDto
    {
        internal const string ExpresionElemento = nameof(NombreCompleto);

        [IUPropiedad(
            VisibleEnGrid = true,
            VisibleEnEdicion = false,
            Etiqueta = "Usuario",
            Ordenar = true,
            OrdenarPor = nameof(UsuarioDto.Login),
            PorAnchoMnt = 35
         )
        ]
        public string NombreCompleto { get; set; } // => $"({Login}) {Apellido}, {Nombre}";

        [IUPropiedad(
            VisibleEnGrid = false,
            Etiqueta = "Usuario",
            Ayuda = "Usuario de conexión",
            Tipo = typeof(string),
            Fila = 1,
            Columna = 0,
            Ordenar = true,
            PorAnchoMnt = 25
            )
        ]
        public string Login { get; set; }


        [IUPropiedad(
            VisibleEnGrid = false,
            Etiqueta = "Apellidos",
            Ayuda = "Apellidos",
            Tipo = typeof(string),
            Fila = 3,
            Columna = 0,
            Ordenar = true,
            PorAnchoMnt = 45
            )
        ]
        public string Apellido { get; set; }


        [IUPropiedad(
            VisibleEnGrid = false,
            Etiqueta = "Nombre",
            Ayuda = "Nombre",
            Tipo = typeof(string),
            Fila = 2,
            Columna = 0,
            Posicion = 0
            )
        ]
        public string Nombre { get; set; }

        [IUPropiedad(
            Etiqueta = "eMail",
            Ayuda = "eMail",
            Tipo = typeof(string),
            Fila = 1,
            Columna = 1,
            Posicion = 0
            )
        ]
        public string eMail { get; set; }

        [IUPropiedad(
            Etiqueta = "Fecha de alta",
            EtiquetaGrid = "Alta",
            VisibleEnGrid = true,
            VisibleAlCrear = false,
            VisibleEnEdicion = true,
            VisibleAlConsultar = true,
            Tipo = typeof(DateTime),
            EditableAlEditar = false,
            Fila = 4,
            Columna = 0,
            Ordenar = true
            )
        ]
        public DateTime Alta { get; set; }

        //------------------------------------------------------------------------
        [IUPropiedad(
            Etiqueta = "Administrador",
            Ayuda = "indica si el usuario es administrador",
            VisibleEnGrid = false,
            Obligatorio = false,
            Fila = 6,
            Columna = 0,
            TipoDeControl = enumTipoControl.Check,
            ValorPorDefecto = false
            )
        ]
        public bool EsAdministrador { get; set; }


        [IUPropiedad(
            VisibleEnGrid = false,
            VisibleEnEdicion = true,
            Etiqueta = "Fotografía",
            Ayuda = "Seleccione un fichero",
            Tipo = typeof(int),
            TipoDeControl = enumTipoControl.Archivo,
            ExtensionesValidas = ".png, .jpg",
            UrlDelArchivo = nameof(Foto),
            Fila = 5,
            Columna = 0)]
        public int? IdArchivo { get; set; }

        [IUPropiedad(
            TipoDeControl = enumTipoControl.ImagenDelCanvas
            )]
        public string Foto { get; set; }

    }



}
