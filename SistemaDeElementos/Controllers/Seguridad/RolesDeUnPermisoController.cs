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
    public class RolesDeUnPermisoController : EntidadController<ContextoSe, PermisosDeUnRolDtm, RolesDeUnPermisoDto>
    {
        public RolesDeUnPermisoController(GestorDeRolesDeUnPermiso gestor, GestorDeErrores errores)
        : base
        (
          gestor,
          errores,
          new DescriptorDeRolesDeUnPermiso(ModoDescriptor.Mantenimiento)
        )
        {
        }

        [HttpPost]
        public IActionResult CrudRolesDeUnPermiso(string restrictor, string orden)
        {
            return ViewCrud();
        }

        protected override dynamic CargaDinamica(string claseElemento, int posicion, int cantidad, ClausulaDeFiltrado filtro)
        {
            if (claseElemento == nameof(PermisoDto))
                return GestorDePermisos.Gestor(GestorDeElementos.Contexto, GestorDeElementos.Mapeador).LeerPermisos(posicion, cantidad, new List<ClausulaDeFiltrado>() { filtro });

            if (claseElemento == nameof(RolDto))
                return GestorDeRoles.Gestor(GestorDeElementos.Contexto, GestorDeElementos.Mapeador).LeerRoles(posicion, cantidad, new List<ClausulaDeFiltrado>() { filtro });

            return base.CargaDinamica(claseElemento, posicion, cantidad, filtro);
        }

    }
}

