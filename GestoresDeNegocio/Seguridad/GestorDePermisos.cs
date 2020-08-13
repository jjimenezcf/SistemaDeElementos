﻿using AutoMapper;
using System.Linq;
using System.Collections.Generic;
using Utilidades;
using Microsoft.EntityFrameworkCore;
using System;
using Gestor.Errores;
using ServicioDeDatos.Seguridad;
using ServicioDeDatos;
using ModeloDeDto.Seguridad;
using GestorDeElementos;

namespace GestoresDeNegocio.Seguridad
{
    public static partial class Joins
    {
        public static IQueryable<T> AplicarJoinsDePermisos<T>(this IQueryable<T> registros, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros) where T : PermisoDtm
        {
            foreach (ClausulaDeJoin join in joins)
            {
                if (join.Dtm == typeof(ClasePermisoDtm))
                    registros = registros.Include(p => p.Clase);

                if (join.Dtm == typeof(TipoPermisoDtm))
                    registros = registros.Include(p => p.Tipo);
            }

            return registros;
        }
    }

    static class FiltrosPermiso
    {
        public static IQueryable<T> FiltrarPorUsuario<T>(this IQueryable<T> registros, List<ClausulaDeFiltrado> filtros) where T : PermisoDtm
        {
            foreach (ClausulaDeFiltrado filtro in filtros)
                if (filtro.Clausula.ToLower() == PermisoPor.PermisosDeUnUsuario)
                {
                    var listaIds = filtro.Valor.ListaEnteros();
                    foreach (int id in listaIds)
                    {
                        registros = registros.Where(p => p.Usuarios.Any(up => up.IdUsua == id && up.IdPermiso == p.Id));
                    }
                }

            return registros;
        }

        public static IQueryable<T> FiltroPorRol<T>(this IQueryable<T> registros, List<ClausulaDeFiltrado> filtros) where T : PermisoDtm
        {
            foreach (ClausulaDeFiltrado filtro in filtros)
                if (filtro.Clausula.ToLower() == PermisoPor.PermisoDeUnRol)
                {
                    var listaIds = filtro.Valor.ListaEnteros();
                    foreach (int id in listaIds)
                    {
                        registros = registros.Where(x => x.Roles.Any(i => i.IdPermiso == id));
                    }
                }

            return registros;

        }
        public static IQueryable<T> FiltroPorClase<T>(this IQueryable<T> registros, List<ClausulaDeFiltrado> filtros) where T : PermisoDtm
        {
            foreach (ClausulaDeFiltrado filtro in filtros)
                if (filtro.Clausula.ToLower() == nameof(PermisoDtm.Clase).ToLower())
                {
                    registros = registros.Where(x => x.IdClase == filtro.Valor.Entero());
                }

            return registros;
        }
        public static IQueryable<T> FiltroPorTipo<T>(this IQueryable<T> registros, List<ClausulaDeFiltrado> filtros) where T : PermisoDtm
        {
            foreach (ClausulaDeFiltrado filtro in filtros)
                if (filtro.Clausula.ToLower() == nameof(PermisoDtm.Tipo).ToLower())
                {
                    registros = registros.Where(x => x.IdTipo == filtro.Valor.Entero());
                }

            return registros;
        }


    }
    static class PermisosRegOrd
    {
        public static IQueryable<PermisoDtm> Orden(this IQueryable<PermisoDtm> set, List<ClausulaDeOrdenacion> ordenacion)
        {
            if (ordenacion.Count == 0)
                return set.OrderBy(x => x.Nombre);

            foreach (var orden in ordenacion)
            {
                if (orden.Propiedad == nameof(PermisoDtm.Nombre).ToLower())
                    set = orden.Modo == ModoDeOrdenancion.ascendente
                        ? set.OrderBy(x => x.Nombre)
                        : set.OrderByDescending(x => x.Nombre);
                
                if (orden.Propiedad == nameof(PermisoDtm.Clase).ToLower())
                    set = orden.Modo == ModoDeOrdenancion.ascendente
                        ? set.OrderBy(x => x.Clase)
                        : set.OrderByDescending(x => x.Clase);

                if (orden.Propiedad == nameof(PermisoDtm.Tipo).ToLower())
                    set = orden.Modo == ModoDeOrdenancion.ascendente
                        ? set.OrderBy(x => x.Tipo)
                        : set.OrderByDescending(x => x.Tipo);
            }

            return set;
        }
    }

    public class GestorDePermisos : GestorDeElementos<ContextoSe, PermisoDtm, PermisoDto>
    {
        public class MapearPermiso : Profile
        {
            public MapearPermiso()
            {
                CreateMap<PermisoDtm, PermisoDto>()
                .ForMember(dto => dto.Clase, dtm => dtm.MapFrom(dtm => dtm.Clase.Nombre))
                .ForMember(dto => dto.Tipo, dtm => dtm.MapFrom(dtm => dtm.Tipo.Nombre));

                CreateMap<PermisoDto, PermisoDtm>();

                CreateMap<ClasePermisoDtm, ClasePermisoDto>();

            }
        }

        public GestorDePermisos(ContextoSe contexto, IMapper mapeador)
            : base(contexto, mapeador)
        {

        }

        internal static GestorDePermisos Gestor(ContextoSe contexto, IMapper mapeador)
        {
            return new GestorDePermisos(contexto, mapeador);
        }

        protected override IQueryable<PermisoDtm> AplicarFiltros(IQueryable<PermisoDtm> registros, List<ClausulaDeFiltrado> filtros, ParametrosDeNegocio parametros)
        {
            registros = base.AplicarFiltros(registros, filtros, parametros);

            if (HayFiltroPorId(registros))
                return registros;

            return registros
                .FiltrarPorUsuario(filtros)
                .FiltroPorRol(filtros)
                .FiltroPorTipo(filtros)
                .FiltroPorClase(filtros);
        }


        protected override IQueryable<PermisoDtm> AplicarOrden(IQueryable<PermisoDtm> registros, List<ClausulaDeOrdenacion> ordenacion)
        {
            registros = base.AplicarOrden(registros, ordenacion);
            return registros.Orden(ordenacion);
        }

        protected override void DefinirJoins(List<ClausulaDeFiltrado> filtros, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros)
        {
            base.DefinirJoins(filtros, joins, parametros);

            joins.Add(new ClausulaDeJoin { Dtm = typeof(ClasePermisoDtm) });
            joins.Add(new ClausulaDeJoin { Dtm = typeof(TipoPermisoDtm) });
        }
        protected override IQueryable<PermisoDtm> AplicarJoins(IQueryable<PermisoDtm> registros, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros)
        {
            return registros.AplicarJoinsDePermisos(joins, parametros);
        }

        public List<ClasePermisoDto> LeerClases()
        {
            return LeerClases(0,-1,"");
        }

        public List<ClasePermisoDto> LeerClases(int posicion, int cantidad, string valorDeFiltro)
        {
            var gestor = GestorDeClaseDePermisos.Gestor(Contexto, Mapeador); 
            var filtros = new List<ClausulaDeFiltrado>();
            if (!valorDeFiltro.IsNullOrEmpty())
                filtros.Add(new ClausulaDeFiltrado { Criterio = CriteriosDeFiltrado.contiene, Clausula = nameof(ClasePermisoDtm.Nombre), Valor = valorDeFiltro });

            var clasesDtm = gestor.LeerRegistros(posicion, cantidad, filtros);
            return gestor.MapearElementos(clasesDtm).ToList();
        }

        public List<TipoPermisoDto> LeerTipos()
        {
            return LeerTipos(0, -1, "");
        }

        public List<TipoPermisoDto> LeerTipos(int posicion, int cantidad, string valorDeFiltro)
        {
            var gestor = GestorDeTipoPermiso.Gestor(Contexto, Mapeador);
            var filtros = new List<ClausulaDeFiltrado>();
            if (!valorDeFiltro.IsNullOrEmpty())
                filtros.Add(new ClausulaDeFiltrado { Criterio = CriteriosDeFiltrado.contiene, Clausula = nameof(TipoPermisoDtm.Nombre), Valor = valorDeFiltro });

            var tiposDtm = gestor.LeerRegistros(posicion, cantidad, filtros);
            return gestor.MapearElementos(tiposDtm).ToList();
        }

        protected override void AntesMapearRegistroParaEliminar(PermisoDto elemento, ParametrosDeNegocio opciones)
        {
            base.AntesMapearRegistroParaEliminar(elemento, opciones);

            var gestor = GestorDeRolesDePermisos.Gestor(Contexto, Mapeador);
            var filtro = new ClausulaDeFiltrado { Clausula = nameof(RolesDeUnPermisoDtm.IdPermiso), Criterio = CriteriosDeFiltrado.igual, Valor = elemento.Id.ToString() };
            var filtros = new List<ClausulaDeFiltrado> {filtro};
            var r = gestor.LeerRegistros(0, 1, filtros);
            if (r.Count > 0)
            {
                var roles = "";
                foreach (var r1 in r)
                    roles = $"{(roles == "" ? "" : $"{roles},")} {r1.Rol.Nombre}";

                Exception exc = GestorDeErrores.MostrarExcepcion(excepcioMostrar: $"El permiso está incluido en {(r.Count == 1 ? "el rol" : "los roles") }: '{roles}'");
                throw exc;
            }
        }

        internal PermisoDtm Crear(string nombrePermiso, TipoPermisoDtm tipoDePermiso, ClasePermisoDtm claseDePermiso)
        {
            var registro = new PermisoDtm();
            registro.Nombre = nombrePermiso;
            registro.IdClase = claseDePermiso.Id;
            registro.IdTipo = tipoDePermiso.Id;
            PersistirRegistro(registro, new ParametrosDeNegocio(TipoOperacion.Insertar));
            return registro;
        }
        
        internal PermisoDtm Modificar(PermisoDtm permiso)
        {
            PersistirRegistro(permiso, new ParametrosDeNegocio(TipoOperacion.Modificar));
            return permiso;
        }

        internal PermisoDtm Eliminar(PermisoDtm permiso)
        {
            PersistirRegistro(permiso, new ParametrosDeNegocio(TipoOperacion.Eliminar));
            return permiso;
        }

        protected override void AntesDePersistir(PermisoDtm registro, ParametrosDeNegocio parametros)
        {
            base.AntesDePersistir(registro, parametros);

            //validamos que el permiso no esté en un rol
            if (parametros.Tipo == TipoOperacion.Eliminar)
            {
                var gestor = new GestorDeRolesDePermisos(Contexto, Mapeador);
                var filtro = new ClausulaDeFiltrado { Clausula = nameof(RolesDeUnPermisoDtm.IdPermiso), Criterio = CriteriosDeFiltrado.igual, Valor = registro.Id.ToString() };
                var filtros = new List<ClausulaDeFiltrado> { filtro };
                if (gestor.Contar(filtros) > 0)
                {
                    throw new Exception($"El permiso {registro.Nombre} esta incluido en algún rol, desasígnelo primero");
                }
            }
        }

    }

}
