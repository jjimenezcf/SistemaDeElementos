﻿using ServicioDeDatos;
using Gestor.Errores;
using Microsoft.AspNetCore.Mvc;
using MVCSistemaDeElementos.Descriptores;
using ServicioDeDatos.Seguridad;
using GestoresDeNegocio.Seguridad;
using ModeloDeDto.Seguridad;
using GestorDeElementos;
using System.Collections.Generic;

namespace MVCSistemaDeElementos.Controllers
{
    public class PermisosDeUnRolController : EntidadController<ContextoSe, PermisosDeUnRolDtm, PermisosDeUnRolDto>
    {
         public PermisosDeUnRolController(GestorDePermisosDeUnRol gestor, GestorDeErrores errores)
         : base
         (
           gestor,
           errores,
           new DescriptorDePermisosDeUnRol(ModoDescriptor.Mantenimiento)
         )
        {
        }

        [HttpPost]
        public IActionResult CrudPermisosDeUnRol(string restrictor, string orden)
        {
            return ViewCrud();
        }

        protected override dynamic CargaDinamica(string claseElemento, int posicion, int cantidad, ClausulaDeFiltrado filtro)
        {
            if (claseElemento == nameof(PermisoDto))
                return GestorDePermisos.Gestor(GestorDeElementos.Contexto, GestorDeElementos.Mapeador).LeerPermisos(posicion, cantidad, new List<ClausulaDeFiltrado>() { filtro });

            if (claseElemento == nameof(RolDto))
                GestorDeRoles.Gestor(GestorDeElementos.Contexto, GestorDeElementos.Mapeador).LeerRoles(posicion, cantidad, new List<ClausulaDeFiltrado>() { filtro });

            return base.CargaDinamica(claseElemento, posicion, cantidad, filtro);
        }

    }
}
