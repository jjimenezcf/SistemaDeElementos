﻿using System.Collections.Generic;
using AutoMapper;
using ServicioDeDatos;
using GestorDeElementos;
using ServicioDeDatos.TrabajosSometidos;
using ModeloDeDto.TrabajosSometidos;
using System;
using GestoresDeNegocio.Entorno;
using Enumerados;
using ServicioDeCorreos;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ServicioDeDatos.Elemento;
using ModeloDeDto;
using Gestor.Errores;
using Utilidades;
using ServicioDeDatos.Seguridad;
using ServicioDeDatos.Entorno;
using GestoresDeNegocio.Negocio;

namespace GestoresDeNegocio.TrabajosSometidos
{
    public class GestorDeCorreos : GestorDeElementos<ContextoSe, CorreoDtm, CorreoDto>
    {

        public class ltrParam
        {
            internal static readonly string usuarios = nameof(usuarios);
            internal static readonly string puestos = nameof(puestos);
            internal static readonly string receptores = nameof(receptores);
            internal static readonly string asunto = nameof(asunto);
            internal static readonly string cuerpo = nameof(cuerpo);
            internal static readonly string adjuntos = nameof(adjuntos);
            internal static readonly string archivos = nameof(archivos);
            internal static readonly string elementosDeNegocio = nameof(elementosDeNegocio);
        }


        public class MapearArchivos : Profile
        {
            public MapearArchivos()
            {
                CreateMap<CorreoDtm, CorreoDto>()
                .ForMember(dto => dto.Creador, dtm => dtm.MapFrom(x => $"({x.Usuario.Login})- {x.Usuario.Nombre} {x.Usuario.Apellido}"));
                CreateMap<CorreoDto, CorreoDtm>();
            }
        }

        public static bool PermiteElEnvioDeCorreo<T>() where T : ElementoDto
        {
            try
            {
                ExtensionesDto.UrlParaMostrarUnDto(typeof(T));
            }
            catch
            {
               return false;
            }
            return true;
        }


        public GestorDeCorreos(ContextoSe contexto, IMapper mapeador)
        : base(contexto, mapeador)
        {
        }

        private static GestorDeCorreos Gestor(ContextoSe contexto, IMapper mapeador)
        {
            return new GestorDeCorreos(contexto, mapeador);
        }


        public static CorreoDtm CrearCorreoPara(ContextoSe contexto, List<string> receptores, string asunto, string cuerpo, List<TipoDtoElmento> elementos, List<string> archivos)
        {
            var correo = new CorreoDtm();
            correo.IdUsuario = contexto.DatosDeConexion.IdUsuario;
            correo.Emisor = new ServicioDeCorreo(CacheDeVariable.Cfg_ServidorDeCorreo).Emisor;
            correo.Receptores = receptores.ToJson();
            correo.Asunto = asunto;
            correo.Cuerpo = cuerpo;
            correo.Elementos = elementos.ToJson();
            correo.Archivos = archivos.ToJson();
            var gestor = Gestor(contexto, contexto.Mapeador);
            return gestor.PersistirRegistro(correo, new ParametrosDeNegocio(enumTipoOperacion.Insertar));
        }

        public static CorreoDtm CrearCorreoDe(ContextoSe contexto, string emisor, List<string> receptores, string asunto, string cuerpo, List<TipoDtoElmento> elementos, List<string> archivos)
        {
            var correo = new CorreoDtm();
            correo.IdUsuario = contexto.DatosDeConexion.IdUsuario;
            correo.Emisor = emisor;
            correo.Receptores = receptores.ToJson();
            correo.Asunto = asunto;
            correo.Cuerpo = cuerpo;
            correo.Elementos = elementos.ToJson();
            correo.Archivos = archivos.ToJson();
            var gestor = Gestor(contexto, contexto.Mapeador);
            return gestor.PersistirRegistro(correo, new ParametrosDeNegocio(enumTipoOperacion.Insertar));
        }

        public static CorreoDtm CrearCorreoDe(ContextoSe contexto, string parametrosJson)
        {
            Dictionary<string, object> parametros = parametrosJson.ToDiccionarioDeParametros();

            if (!parametros.ContainsKey(ltrParam.archivos))
                parametros[ltrParam.archivos] = new List<string>();

            ValidarParametrosDeCorreo(contexto, parametros);

            return CrearCorreoDe(contexto
                , GestorDeUsuarios.LeerUsuario(contexto, contexto.DatosDeConexion.IdUsuario).eMail
                , (List<string>)parametros[ltrParam.receptores]
                , (string)parametros[ltrParam.asunto]
                , (string)parametros[ltrParam.cuerpo]
                , (List<TipoDtoElmento>)parametros[ltrParam.adjuntos]
                , (List<string>)parametros[ltrParam.archivos]);
        }


        private static void ValidarParametrosDeCorreo(ContextoSe contexto, Dictionary<string, object> parametros)
        {
            if (!parametros.ContainsKey(ltrParam.usuarios) && !parametros.ContainsKey(ltrParam.puestos))
                GestorDeErrores.Emitir("Debe indicar algún receptor");

            var usuarios = parametros.ContainsKey(ltrParam.usuarios) ? parametros[ltrParam.usuarios].ToString().JsonToLista<int>() : new List<int>();
            var puestos = parametros.ContainsKey(ltrParam.puestos) ? parametros[ltrParam.puestos].ToString().JsonToLista<int>() : new List<int>();

            if (usuarios.Count == 0 && puestos.Count == 0)
                GestorDeErrores.Emitir("Debe indicar algún receptor");

            var receptores = "";
            foreach (var idUsuario in usuarios)
                receptores = $"{receptores};{GestorDeUsuarios.LeerUsuario(contexto, idUsuario).eMail}";
            foreach (var idPuesto in puestos)
            {
                List<UsuarioDtm> usuariosDeUnPuesto = GestorDePuestosDeTrabajo.LeerUsuarios(contexto, idPuesto);
                foreach (var usuario in usuariosDeUnPuesto)
                    receptores = $"{receptores};{GestorDeUsuarios.LeerUsuario(contexto, usuario.Id).eMail}";
            }

            parametros[ltrParam.receptores] = receptores.Substring(1).ToLista<string>();

            if (((string)parametros[ltrParam.asunto]).IsNullOrEmpty()) GestorDeErrores.Emitir("Debe indicar el asunto");
            if (((string)parametros[ltrParam.cuerpo]).IsNullOrEmpty()) GestorDeErrores.Emitir("Debe indicar el cuerpo");

            if (parametros.ContainsKey(ltrParam.adjuntos))
            {
                var lista = new List<TipoDtoElmento>();
                var elementosDto = parametros[ltrParam.adjuntos].ToString().JsonToLista<string>();
                foreach (var elementoDto in elementosDto)
                {
                    var partes = elementoDto.Split(":");
                    
                    if (partes.Length != 3 || partes[0].IsNullOrEmpty() || partes[2].IsNullOrEmpty() || partes[1].Entero() == 0)
                        GestorDeErrores.Emitir("Intenta enviar un correo adjuntando un elemento mal definido.");

                    var elemento = new TipoDtoElmento { TipoDto = partes[0], IdElemento = partes[1].Entero(), Referencia = partes[2]};
                    lista.Add(GestorDeNegocios.ValidarElementoDto(elemento));
                 }

                parametros[ltrParam.adjuntos] = lista;
            }

            //TODO: Validar que las rutas de los archivos o los Ids de los archivos existen

        }

        private static string AdjuntarElementos(CorreoDtm correoDtm)
        {
            var elementos = correoDtm.Elementos.JsonToLista<TipoDtoElmento>();
            var cuerpo = correoDtm.Cuerpo;
            foreach (TipoDtoElmento elemento in elementos)
            {
                cuerpo = $"{cuerpo}{Environment.NewLine}{GestorDeNegocios.ComponerUrl(elemento)}";
            }

            return cuerpo;
        }

        internal static void EnviarCorreoPendientes(ContextoSe contexto)
        {
            var gestor = Gestor(contexto, contexto.Mapeador);
            var filtro = new ClausulaDeFiltrado(nameof(CorreoDtm.Enviado), CriteriosDeFiltrado.esNulo);
            var pendientes = gestor.LeerRegistros(0, -1, new List<ClausulaDeFiltrado> { filtro });
            foreach (var pendiente in pendientes)
                try
                {
                    gestor.EnviarCorreoDe(pendiente);
                }
                catch (Exception e)
                {
                    try
                    {
                        ServicioDeCorreo.EnviarCorreoPara(CacheDeVariable.Cfg_ServidorDeCorreo
                            , new List<string> { "juan.jimenez@gmail.com" }
                            , "Fallo al enviar cooreos"
                            , $"Error al enviar el correo con id  {pendiente.Id}{Environment.NewLine}{GestorDeErrores.Mensaje(e)}"
                            );
                        pendiente.Enviado = DateTime.Now;
                        gestor.PersistirRegistro(pendiente, new ParametrosDeNegocio(enumTipoOperacion.Modificar));
                    }
                    catch (Exception ei)
                    {
                        gestor.Contexto.AnotarExcepcion(ei);
                    }
                }
        }

        private void EnviarCorreoDe(CorreoDtm correoDtm)
        {
            var archivos = correoDtm.Archivos.JsonToLista<string>();
            var receptores = correoDtm.Receptores.JsonToLista<string>();
            string cuerpo = AdjuntarElementos(correoDtm);

            ServicioDeCorreo.EnviarCorreoDe(CacheDeVariable.Cfg_ServidorDeCorreo, correoDtm.Emisor, receptores, correoDtm.Asunto, cuerpo, true, archivos);
            correoDtm.Enviado = DateTime.Now;
            PersistirRegistro(correoDtm, new ParametrosDeNegocio(enumTipoOperacion.Modificar));
        }

        protected override void AntesDePersistir(CorreoDtm registro, ParametrosDeNegocio parametros)
        {
            base.AntesDePersistir(registro, parametros);
            if (parametros.Operacion == enumTipoOperacion.Insertar)
            {
                registro.Creado = DateTime.Now;
            }
        }

        protected override IQueryable<CorreoDtm> AplicarJoins(IQueryable<CorreoDtm> registros, List<ClausulaDeFiltrado> filtros, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros)
        {
            registros = base.AplicarJoins(registros, filtros, joins, parametros);
            registros = registros.Include(p => p.Usuario);
            return registros;
        }

        protected override IQueryable<CorreoDtm> AplicarFiltros(IQueryable<CorreoDtm> registros, List<ClausulaDeFiltrado> filtros, ParametrosDeNegocio parametros)
        {
            registros = base.AplicarFiltros(registros, filtros, parametros);

            foreach (ClausulaDeFiltrado filtro in filtros)
            {
                if (filtro.Clausula.ToLower() == nameof(ElementoDtm.Nombre).ToLower())
                    registros = Filtrar.AplicarFiltroDeCadena(registros, filtro, nameof(CorreoDtm.Asunto));

                if (filtro.Clausula.ToLower() == ltrCorreos.seHaEnviado.ToLower())
                    registros = Filtrar.AplicarFiltroPorFechaNoNula(registros, nameof(CorreoDtm.Enviado));

                if (filtro.Clausula.ToLower() == ltrCorreos.NoSeHaEnviado.ToLower())
                    registros = Filtrar.AplicarFiltroPorFechaNula(registros, nameof(CorreoDtm.Enviado));

            }

            return registros;
        }
    }
}
