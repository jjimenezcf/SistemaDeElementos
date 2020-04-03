﻿using AutoMapper;
using System.Linq;
using System.Collections.Generic;
using Utilidades;
using Gestor.Elementos.ModeloIu;

namespace Gestor.Elementos.Seguridad
{

    static class FiltrosDePermisos
    {
        public static IQueryable<T> FiltroPorNombre<T>(this IQueryable<T> registros, List<ClausulaDeFiltrado> filtros) where T : PermisoDtm
        {
            foreach (ClausulaDeFiltrado filtro in filtros)
                if (filtro.Propiedad.ToLower() == PermisoPor.Nombre)
                    return registros.Where(x => x.Nombre.Contains(filtro.Valor));

            return registros;
        }

        public static IQueryable<T> FiltroPorRol<T>(this IQueryable<T> registros, List<ClausulaDeFiltrado> filtros) where T : PermisoDtm
        {
            foreach (ClausulaDeFiltrado filtro in filtros)
                if (filtro.Propiedad.ToLower() == PermisoPor.PermisoDeUnRol)
                {
                    var listaIds = filtro.Valor.ListaEnteros();
                    foreach (int id in listaIds)
                    {
                        registros = registros.Where(x => x.Roles.Any(i => i.IdPermiso == id));
                    }
                }

            return registros;

        }
    }


    public class GestorDePermisos : GestorDeElementos<CtoSeguridad, PermisoDtm, PermisoDto>
    {
        public class MapearPermiso : Profile
        {
            public MapearPermiso()
            {
                CreateMap<PermisoDtm, PermisoDto>();
                CreateMap<PermisoDto,PermisoDtm>();
            }
        }

        public GestorDePermisos(CtoSeguridad contexto, IMapper mapeador)
            : base(contexto, mapeador)
        {
            
        }
               
        protected override PermisoDtm LeerConDetalle(int Id)
        {
            return null;
        }

        protected override IQueryable<PermisoDtm> AplicarFiltros(IQueryable<PermisoDtm> registros, List<ClausulaDeFiltrado> filtros, ParametrosDeNegocio parametros)
        {
            foreach (var f in filtros)
                if (f.Propiedad == FiltroPor.Id)
                  return base.AplicarFiltros(registros, filtros, parametros);

            return registros
                .FiltroPorNombre(filtros)
                .FiltroPorRol(filtros);
        }

    }
}
