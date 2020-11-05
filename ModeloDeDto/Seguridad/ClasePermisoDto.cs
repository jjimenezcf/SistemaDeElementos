﻿

namespace ModeloDeDto.Seguridad
{
    [IUDto]
    public class ClasePermisoDto: ElementoDto
    {
        [IUPropiedad(
            Etiqueta = "Clase",
            Ayuda = "Clase de permiso",
            Ordenar = true
            )
        ]
        public string Nombre { get; set; }
    }
}