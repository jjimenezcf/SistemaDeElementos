﻿using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ServicioDeDatos;
using GestorDeElementos;
using ServicioDeDatos.Callejero;
using ModeloDeDto.Callejero;
using Utilidades;
using GestoresDeNegocio.TrabajosSometidos;
using System;
using GestoresDeNegocio.Archivos;
using Microsoft.EntityFrameworkCore;
using ModeloDeDto;
using Gestor.Errores;
using ServicioDeDatos.TrabajosSometidos;

namespace GestoresDeNegocio.Callejero
{
    public class GestorDeProvincias : GestorDeElementos<ContextoSe, ProvinciaDtm, ProvinciaDto>
    {
        class archivoParaImportar
        {
            public string parametro { get; set; }
            public int valor { get; set; }
        }

        public const string ParametroProvincia = "csvProvincia";

        public class MapearVariables : Profile
        {
            public MapearVariables()
            {
                CreateMap<ProvinciaDtm, ProvinciaDto>()
                    .ForMember(dto => dto.Pais, dtm => dtm.MapFrom(dtm => $"({dtm.Pais.Codigo}) {dtm.Pais.Nombre}"));

                CreateMap<ProvinciaDto, ProvinciaDtm>()
                .ForMember(dtm => dtm.Pais, dto => dto.Ignore())
                .ForMember(dtm => dtm.FechaCreacion, dto => dto.Ignore())
                .ForMember(dtm => dtm.FechaModificacion, dto => dto.Ignore())
                .ForMember(dtm => dtm.IdUsuaCrea, dto => dto.Ignore())
                .ForMember(dtm => dtm.IdUsuaModi, dto => dto.Ignore());

            }
        }

        public GestorDeProvincias(ContextoSe contexto, IMapper mapeador)
        : base(contexto, mapeador)
        {

        }

        public static GestorDeProvincias Gestor(ContextoSe contexto, IMapper mapeador)
        {
            return new GestorDeProvincias(contexto, mapeador); ;
        }

        internal static ProvinciaDtm LeerProvinciaPorCodigo(ContextoSe contexto, string iso2Pais, string codigoProvincia, bool paraActualizar, bool errorSiNoHay = true, bool errorSiMasDeUno = true)
        {
            var gestor = Gestor(contexto, contexto.Mapeador);
            var filtros = new List<ClausulaDeFiltrado>();
            var filtro1 = new ClausulaDeFiltrado(nameof(ProvinciaDtm.Pais.ISO2), CriteriosDeFiltrado.igual, iso2Pais);
            var filtro2 = new ClausulaDeFiltrado(nameof(ProvinciaDtm.Codigo), CriteriosDeFiltrado.igual, codigoProvincia);
            filtros.Add(filtro1);
            filtros.Add(filtro2);
            List<ProvinciaDtm> provincias = gestor.LeerRegistros(0, -1, filtros, null, null, new ParametrosDeNegocio(paraActualizar ? enumTipoOperacion.LeerConBloqueo : enumTipoOperacion.LeerSinBloqueo));

            if (provincias.Count == 0 && errorSiNoHay)
                GestorDeErrores.Emitir($"No se ha localizado la provincia para el código del pais {iso2Pais} y codigo de provincia {codigoProvincia}");
            if (provincias.Count > 1 && errorSiMasDeUno)
                GestorDeErrores.Emitir($"Se han localizado más de un registro de provincia con el código del pais {iso2Pais} y codigo de provincia {codigoProvincia}");

            return provincias.Count == 1 ? provincias[0] : null;
        }

        public static void ImportarFicheroDeProvincias(EntornoDeTrabajo entorno, int idArchivo)
        {
            var gestorProceso = GestorDeProvincias.Gestor(entorno.contextoDelProceso, entorno.contextoDelProceso.Mapeador);
            var rutaFichero = GestorDocumental.DescargarArchivo(entorno.contextoDelProceso, idArchivo, entorno.ProcesoIniciadoPorLaCola);
            var fichero = new FicheroCsv(rutaFichero);
            var linea = 0;
            entorno.CrearTraza($"Inicio del proceso");
            var trazaPrcDtm = entorno.CrearTraza($"Procesando la fila {linea}");
            var trazaInfDtm = entorno.CrearTraza($"Traza informativa del proceso");
            foreach (var fila in fichero)
            {
                var tran = gestorProceso.IniciarTransaccion();
                try
                {
                    linea++;
                    if (fila.EnBlanco)
                        continue;

                    if (fila.Columnas != 5)
                        throw new Exception($"la fila {linea} solo debe tener 5 columnas");

                    if (fila["A"].IsNullOrEmpty() || fila["B"].IsNullOrEmpty() ||
                        fila["C"].IsNullOrEmpty() || fila["D"].IsNullOrEmpty() ||
                        fila["E"].IsNullOrEmpty())
                        throw new Exception($"El contenido de la fila {linea} debe ser: nombre de la provincia, nombre en ingles, iso de 2 iso de 3 y prefijo telefónico");

                    ProcesarProvinciaLeida(entorno, gestorProceso,
                        iso2Pais:fila["E"],
                        nombreProvincia: fila["C"],
                        sigla: fila["A"], 
                        codigo: fila["B"],
                        prefijoTelefono: fila["D"],
                        trazaInfDtm);
                    gestorProceso.Commit(tran);
                }
                catch (Exception e)
                {
                    gestorProceso.Rollback(tran);
                    entorno.AnotarError(e);
                }
                finally
                {
                    entorno.ActualizarTraza(trazaPrcDtm, $"Procesando la fila {linea}");
                }
            }

            entorno.CrearTraza($"Procesadas un total de {linea} filas");
        }

        private static ProvinciaDtm ProcesarProvinciaLeida(EntornoDeTrabajo entorno, GestorDeProvincias gestor, string iso2Pais, string nombreProvincia, string sigla, string codigo, string prefijoTelefono, TrazaDeUnTrabajoDtm trazaInfDtm)
        {
            ParametrosDeNegocio operacion;
            var p = LeerProvinciaPorCodigo(gestor.Contexto, iso2Pais, codigo, paraActualizar: true, errorSiNoHay: false);
            if (p == null)
            {
                var pais = GestorDePaises.LeerPaisPorCodigo(gestor.Contexto, iso2Pais, paraActualizar: false, errorSiNoHay: false);
                p = new ProvinciaDtm();
                p.Codigo = codigo;
                p.Nombre = nombreProvincia;
                p.Sigla = sigla;
                p.IdPais = pais.Id;
                p.Prefijo = prefijoTelefono;
                operacion = new ParametrosDeNegocio(enumTipoOperacion.Insertar);
                entorno.ActualizarTraza(trazaInfDtm, $"Creando la provincia {nombreProvincia}");
            }
            else
            {
                if (p.Nombre != nombreProvincia || p.Codigo != codigo || p.Sigla != sigla || p.Prefijo != prefijoTelefono)
                {
                    p.Nombre = nombreProvincia;
                    p.Sigla = sigla;
                    p.Codigo = codigo;
                    p.Prefijo = prefijoTelefono;
                    operacion = new ParametrosDeNegocio(enumTipoOperacion.Modificar);
                    entorno.ActualizarTraza(trazaInfDtm, $"Modificando la provincia {nombreProvincia}");
                }
                else
                {
                    entorno.ActualizarTraza(trazaInfDtm, $"La provincia {nombreProvincia} ya exite");
                    return p;
                }
            }

            return gestor.PersistirRegistro(p, operacion);
        }

        protected override IQueryable<ProvinciaDtm> AplicarJoins(IQueryable<ProvinciaDtm> registros, List<ClausulaDeFiltrado> filtros, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros)
        {
            registros = base.AplicarJoins(registros, filtros, joins, parametros);
            registros = registros.Include(p => p.Pais);
            return registros;
        }

        public List<ProvinciaDto> LeerProvincias(int posicion, int cantidad, List<ClausulaDeFiltrado> filtros)
        {
            var registros = LeerRegistrosPorNombre(posicion, cantidad, filtros);
            return MapearElementos(registros).ToList();
        }

    }
}
