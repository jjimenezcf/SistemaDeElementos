﻿using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GestorDeElementos;
using Microsoft.EntityFrameworkCore;
using ModeloDeDto.Seguridad;
using ServicioDeDatos;
using ServicioDeDatos.Seguridad;
using Utilidades;

namespace GestoresDeNegocio.Seguridad
{
    public static partial class Joins
    {
        public static IQueryable<T> JoinDePuestosDeUnRol<T>(this IQueryable<T> registros, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros)
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

    static class FiltrosDePuestosDeUnRol
    {
        public static IQueryable<T> PuestosDeUnRol<T>(this IQueryable<T> registros, List<ClausulaDeFiltrado> filtros)
        where T : RolesDeUnPuestoDtm
        {
            foreach (ClausulaDeFiltrado filtro in filtros)
            {
                if (filtro.Clausula.ToLower() == nameof(RolesDeUnPuestoDtm.idPuesto).ToLower() ||
                    filtro.Clausula.ToLower() == "idElemento1".ToLower())
                    registros = registros.Where(x => x.idPuesto == filtro.Valor.Entero());

                if (filtro.Clausula.ToLower() == nameof(RolesDeUnPuestoDtm.IdRol).ToLower() ||
                    filtro.Clausula.ToLower() == "idElemento2".ToLower())
                    registros = registros.Where(x => x.IdRol == filtro.Valor.Entero());
            }

            return registros;
        }
    }
    static class OrdenacionDePuestosDeUnRol
    {
        public static IQueryable<RolesDeUnPuestoDtm> OrdenarPuestosDeUnRol(this IQueryable<RolesDeUnPuestoDtm> set, List<ClausulaDeOrdenacion> ordenacion)
        {
            if (ordenacion.Count == 0)
                return set.OrderBy(x => x.Rol.Nombre);
            return set;
        }
    }

    public class GestorDePuestosDeUnRol : GestorDeElementos<ContextoSe, RolesDeUnPuestoDtm, PuestosDeUnRolDto>
    {

        public class MapearPuestosDeUnRol : Profile
        {
            public MapearPuestosDeUnRol()
            {
                CreateMap<RolesDeUnPuestoDtm, PuestosDeUnRolDto>()
                    .ForMember(dto => dto.Puesto, dtm => dtm.MapFrom(dtm => dtm.Puesto.Nombre))
                    .ForMember(dto => dto.Rol, dtm => dtm.MapFrom(dtm => dtm.Rol.Nombre));

                CreateMap<PuestosDeUnRolDto, RolesDeUnPuestoDtm>();
            }
        }

        public GestorDePuestosDeUnRol(ContextoSe contexto, IMapper mapeador)
        : base(contexto, mapeador)
        {
            invertirMapeoDeRelacion = true;
        }

        internal static GestorDePuestosDeUnUsuario Gestor(ContextoSe contexto, IMapper mapeador)
        {
            return new GestorDePuestosDeUnUsuario(contexto, mapeador);
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

            return registros.JoinDePuestosDeUnRol(joins, parametros);
        }

        protected override IQueryable<RolesDeUnPuestoDtm> AplicarFiltros(IQueryable<RolesDeUnPuestoDtm> registros, List<ClausulaDeFiltrado> filtros, ParametrosDeNegocio parametros)
        {
            registros = base.AplicarFiltros(registros, filtros, parametros);

            if (HayFiltroPorId(registros))
                return registros;

            return registros.PuestosDeUnRol(filtros);
        }

        protected override IQueryable<RolesDeUnPuestoDtm> AplicarOrden(IQueryable<RolesDeUnPuestoDtm> registros, List<ClausulaDeOrdenacion> ordenacion)
        {
            registros = base.AplicarOrden(registros, ordenacion);
            return registros.OrdenarPuestosDeUnRol(ordenacion);
        }

        public List<RolDto> LeerRoles(int posicion, int cantidad, string filtro)
        {
            var gestor = GestorDeRoles.Gestor(Contexto, Mapeador);
            return GestorDeRoles.Leer(gestor, posicion, cantidad, filtro);
        }

        public List<PuestoDto> LeerPuestos(int posicion, int cantidad, string filtro)
        {
            var gestor = GestorDePuestosDeTrabajo.Gestor(Contexto, Mapeador);
            return GestorDePuestosDeTrabajo.Leer(gestor, posicion, cantidad, filtro);
        }

        //protected override void MapearDatosDeRelacion(RolesDeUnPuestoDtm registro, int idElemento1, int idElemento2)
        //{
        //    registro.idPuesto = idElemento1;
        //    registro.IdRol = idElemento2;
        //}
    }
}

