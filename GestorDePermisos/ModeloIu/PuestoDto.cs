﻿using Gestor.Elementos.ModeloIu;

namespace Gestor.Elementos.Seguridad
{
    [IUDto(AnchoEtiqueta = 20
      , AnchoSeparador = 5)]
    public class PuestoDto : Elemento
    {
        [IUPropiedad(
            Etiqueta = "Puesto",
            Ayuda = "Nombre al puesto de trabajo",
            Tipo = typeof(string),
            Fila = 0,
            Columna = 0,
            Ordenar = true,
            PorAnchoMnt = 50
            )
        ]
        public string Nombre { get; set; }


        [IUPropiedad(
            Etiqueta = "Descripción",
            Ayuda = "Descripción del puesto de trabajo",
            Tipo = typeof(string),
            Fila = 0,
            Columna = 0,
            Ordenar = true,
            PorAnchoMnt = 50
            )
        ]
        public string Descripcion { get; set; }
    }
}
