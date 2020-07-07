﻿using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Gestor.Elementos.Entorno;
using GestorDeSeguridad.ModeloIu;
using Microsoft.EntityFrameworkCore;
using ServicioDeDatos;
using ServicioDeDatos.Entorno;
using ServicioDeDatos.Seguridad;
using Utilidades;

namespace Gestor.Elementos.Seguridad
{
    public static partial class Joins
    {
        public static IQueryable<T> JoinDeRolesDeUnPuesto<T>(this IQueryable<T> registros, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros)
        where T : RolesDeUnPuestoDtm
        {
            foreach (ClausulaDeJoin join in joins)
            {
                if (join.Dtm == typeof(RolDtm))
                    registros = registros.Include(p => p.Rol);
                if (join.Dtm == typeof(PuestoDtm))
                    registros = registros.Include(p => p.Puesto);
            }

            return registros;
        }
    }

    static class FiltrosDeRolesDeUnPuesto
    {

        public static IQueryable<T> AnadirFiltros<T>(this IQueryable<T> registros, List<ClausulaDeFiltrado> filtros)
        where T : RolesDeUnPuestoDtm
        {
            foreach (ClausulaDeFiltrado filtro in filtros)
            {
                if (filtro.Clausula.ToLower() == nameof(RolesDeUnPuestoDtm.idPuesto).ToLower())
                {
                    registros = registros.Where(p => p.idPuesto == filtro.Valor.Entero());
                }
            }

            return registros;
        }
    }

    public class GestorDeRolesDeUnPuesto : GestorDeElementos<ContextoSe, RolesDeUnPuestoDtm, RolesDeUnPuestoDto>
    {

        public class MapearClasePermiso : Profile
        {
            public MapearClasePermiso()
            {
                CreateMap<RolesDeUnPuestoDtm, RolesDeUnPuestoDto>()
                    .ForMember(dto => dto.Puesto, dtm => dtm.MapFrom(dtm => dtm.Puesto.Nombre))
                    .ForMember(dto => dto.Rol, dtm => dtm.MapFrom(dtm => dtm.Rol.Nombre));

                CreateMap<RolesDeUnPuestoDto, RolesDeUnPuestoDtm>();
            }
        }

        public GestorDeRolesDeUnPuesto(ContextoSe contexto, IMapper mapeador)
        : base(contexto, mapeador)
        {


        }

        internal static GestorDePuestosDeUnUsuario Gestor(IMapper mapeador)
        {
            var contexto = ContextoSe.ObtenerContexto();
            return (GestorDePuestosDeUnUsuario)CrearGestor<GestorDePuestosDeUnUsuario>(() => new GestorDePuestosDeUnUsuario(contexto, mapeador));
        }

        protected override void DefinirJoins(List<ClausulaDeFiltrado> filtros, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros)
        {
            base.DefinirJoins(filtros, joins, parametros);
            joins.Add(new ClausulaDeJoin { Dtm = typeof(RolDtm) });
            joins.Add(new ClausulaDeJoin { Dtm = typeof(PuestoDtm) });
        }

        protected override IQueryable<RolesDeUnPuestoDtm> AplicarJoins(IQueryable<RolesDeUnPuestoDtm> registros, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros)
        {
            registros = base.AplicarJoins(registros, joins, parametros);

            return registros.JoinDeRolesDeUnPuesto(joins, parametros);
        }

        protected override IQueryable<RolesDeUnPuestoDtm> AplicarFiltros(IQueryable<RolesDeUnPuestoDtm> registros, List<ClausulaDeFiltrado> filtros, ParametrosDeNegocio parametros)
        {
            registros = base.AplicarFiltros(registros, filtros, parametros);

            if (HayFiltroPorId(registros))
                return registros;

            return registros.AnadirFiltros(filtros);
        }

        public dynamic LeerRoles(int posicion, int cantidad, string filtro)
        {
            var gestor = GestorDeRoles.Gestor(Mapeador); var filtros = new List<ClausulaDeFiltrado>();
            if (!filtro.IsNullOrEmpty())
                filtros.Add(new ClausulaDeFiltrado { Criterio = CriteriosDeFiltrado.contiene, Clausula = nameof(RolDto.Nombre), Valor = filtro });

            var clasesDtm = gestor.LeerRegistros(posicion, cantidad, filtros);
            return gestor.MapearElementos(clasesDtm).ToList();
        }

        public dynamic LeerPuestos(int posicion, int cantidad, string filtro)
        {
            var gestor = GestorDePuestosDeTrabajo.Gestor(Mapeador);
            var filtros = new List<ClausulaDeFiltrado>();
            if (!filtro.IsNullOrEmpty())
                filtros.Add(new ClausulaDeFiltrado { Criterio = CriteriosDeFiltrado.contiene, Clausula = nameof(PuestoDto.Nombre), Valor = filtro });

            var clasesDtm = gestor.LeerRegistros(posicion, cantidad, filtros);
            return gestor.MapearElementos(clasesDtm).ToList();
        }
    }
}
