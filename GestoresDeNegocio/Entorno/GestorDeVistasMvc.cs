﻿using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Utilidades;
using ModeloDeDto.Entorno;
using ServicioDeDatos;
using ServicioDeDatos.Entorno;
using System;
using Gestor.Errores;
using ServicioDeDatos.Seguridad;
using GestorDeElementos;
using GestoresDeNegocio.Seguridad;
using Microsoft.EntityFrameworkCore;

namespace GestoresDeNegocio.Entorno
{
    public static partial class Joins
    {
        public static IQueryable<T> JoinConPermisos<T>(this IQueryable<T> registros, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros) where T : VistaMvcDtm
        {
            foreach (ClausulaDeJoin join in joins)
            {
                if (join.Dtm == typeof(PermisoDtm))
                    registros = registros.Include(p => p.Permiso);                   
            }

            return registros;
        }
    }

    public static partial class Filtros
    {
        public static IQueryable<T> FiltraPorControlador<T>(this IQueryable<T> registros, List<ClausulaDeFiltrado> filtros, ParametrosDeNegocio parametros) where T : VistaMvcDtm
        {
            foreach (ClausulaDeFiltrado filtro in filtros)
                if (filtro.Clausula.ToLower() == nameof(VistaMvcDtm.Controlador).ToLower())
                {
                    if (filtro.Criterio == CriteriosDeFiltrado.igual)
                        registros = registros.Where(x => x.Controlador == filtro.Valor);

                    if (filtro.Criterio == CriteriosDeFiltrado.contiene)
                        registros = registros.Where(x => x.Controlador.Contains(filtro.Valor));
                }

            return registros;
        }
        public static IQueryable<T> FiltraPorAccion<T>(this IQueryable<T> registros, List<ClausulaDeFiltrado> filtros, ParametrosDeNegocio parametros) where T : VistaMvcDtm
        {
            foreach (ClausulaDeFiltrado filtro in filtros)
                if (filtro.Clausula.ToLower() == nameof(VistaMvcDtm.Accion).ToLower())
                {
                    if (filtro.Criterio == CriteriosDeFiltrado.igual)
                        registros = registros.Where(x => x.Accion == filtro.Valor);
                }

            return registros;
        }
    }


    public class GestorDeVistaMvc : GestorDeElementos<ContextoSe, VistaMvcDtm, VistaMvcDto>
    {

        public class MapearVistaMvc : Profile
        {
            public MapearVistaMvc()
            {
                CreateMap<VistaMvcDtm, VistaMvcDto>()
                .ForMember(dto => dto.Menus, dtm => dtm.MapFrom(x => x.Menus))
                .ForMember(dto => dto.Permiso , dtm => dtm.MapFrom(x => x.Permiso.Nombre));

                CreateMap<VistaMvcDto, VistaMvcDtm>()
                .ForMember(dtm => dtm.Permiso, dto => dto.Ignore())
                ;
            }
        }

        public GestorDeVistaMvc(ContextoSe contexto, IMapper mapeador)
        : base(contexto, mapeador)
        {

        }


        public static GestorDeVistaMvc Gestor(ContextoSe contexto, IMapper mapeador)
        {
            return new GestorDeVistaMvc(contexto, mapeador);
        }

        protected override void DefinirJoins(List<ClausulaDeFiltrado> filtros, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros)
        {
            base.DefinirJoins(filtros, joins, parametros);
            joins.Add(new ClausulaDeJoin { Dtm = typeof(PermisoDtm) });
        }

        protected override IQueryable<VistaMvcDtm> AplicarJoins(IQueryable<VistaMvcDtm> registros, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros)
        {
            registros = base.AplicarJoins(registros, joins, parametros);
            return registros.JoinConPermisos(joins, parametros);
        }

        protected override IQueryable<VistaMvcDtm> AplicarFiltros(IQueryable<VistaMvcDtm> registros, List<ClausulaDeFiltrado> filtros, ParametrosDeNegocio parametros)
        {
            registros = base.AplicarFiltros(registros, filtros, parametros);

            if (HayFiltroPorId(registros))
                return registros;

            return registros.FiltraPorControlador(filtros, parametros).FiltraPorAccion(filtros, parametros);
        }

        public VistaMvcDtm LeerVistaMvc(string vistaMvc)
        {
            if (vistaMvc.IsNullOrEmpty())
                return null;

            var partes = vistaMvc.Split(".");

            if (partes.Length != 2)
                GestorDeErrores.Emitir($"El valor proporcionado {vistaMvc} no es válido, ha de seguir el patrón Controlador.Vista");


            var filtros = new List<ClausulaDeFiltrado>
                {
                    new ClausulaDeFiltrado { Clausula = nameof(VistaMvcDtm.Controlador), Criterio = CriteriosDeFiltrado.igual, Valor = partes[0] },
                    new ClausulaDeFiltrado { Clausula = nameof(VistaMvcDtm.Accion), Criterio = CriteriosDeFiltrado.igual, Valor = partes[1] }
                };

            var vistas = LeerRegistros(0, -1, filtros);
            if (vistas.Count != 1)
            {
                //if (vistas.Count == 0)
                //    GestorDeErrores.Emitir($"No se ha localizado la vistaMvc {partes[0]}.{partes[1]}");
                //else
                //    GestorDeErrores.Emitir($"Se han localizado {vistas.Count} vistasMvc para {partes[0]}.{partes[1]}");
                return null;
            }

            return vistas[0];
        }

        protected override void AntesDePersistir(VistaMvcDtm registro, ParametrosDeNegocio parametros)
        {
            base.AntesDePersistir(registro, parametros);
            if (parametros.Tipo == TipoOperacion.Insertar)
            {
                var permiso = GestorDePermisos.CrearObtener(Contexto, Mapeador, registro.Nombre, enumClaseDePermiso.Vista,enumTipoDePermiso.Acceso);
                registro.IdPermiso = permiso.Id;
            }
            if (parametros.Tipo == TipoOperacion.Modificar && registro.IdPermiso == null)
            {
               if (RegistroEnBD.IdPermiso != null)
                    registro.IdPermiso = RegistroEnBD.IdPermiso;
                else
                {
                    var permiso = GestorDePermisos.CrearObtener(Contexto, Mapeador, registro.Nombre, enumClaseDePermiso.Vista, enumTipoDePermiso.Acceso);
                    registro.IdPermiso = permiso.Id;
                    parametros.Parametros["permisoCreado"] = true;
                }
            }
            if (parametros.Tipo == TipoOperacion.Eliminar && RegistroEnBD.IdPermiso != null)
                    GestorDePermisos.Eliminar(Contexto, Mapeador, (int) RegistroEnBD.IdPermiso);
        }

        protected override void DespuesDePersistir(VistaMvcDtm registro, ParametrosDeNegocio parametros)
        {
            base.DespuesDePersistir(registro, parametros);

            if (parametros.Tipo == TipoOperacion.Modificar && !parametros.Parametros.ContainsKey("permisoCreado") && RegistroEnBD.Nombre != registro.Nombre)
                GestorDePermisos.Modificar(Contexto, Mapeador, (int)registro.IdPermiso, registro.Nombre, enumClaseDePermiso.Vista, enumTipoDePermiso.Acceso);
        }



    }

}

