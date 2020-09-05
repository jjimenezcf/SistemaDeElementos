﻿using AutoMapper;
using Gestor.Errores;
using Microsoft.EntityFrameworkCore;
using ModeloDeDto;
using ServicioDeDatos;
using ServicioDeDatos.Elemento;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilidades;

namespace GestorDeElementos
{
    public enum CriteriosDeFiltrado { igual, mayor, menor, esNulo, noEsNulo, contiene, comienza, termina, mayorIgual, menorIgual, diferente }
    public enum ModoDeOrdenancion { ascendente, descendente }
    public enum TipoOperacion { Insertar, Modificar, Leer, NoDefinida, Eliminar };

    #region Extensiones para filtrar, hacer joins y ordenar
    public class ClausulaDeJoin
    {
        public Type Dtm { get; set; }
    }
    public class ClausulaDeFiltrado
    {
        public string Clausula { get; set; }
        public CriteriosDeFiltrado Criterio { get; set; }

        private string _valor = "";
        public string Valor { get { return _valor.Trim(); } set { _valor = value; } }
    }

    public class ClausulaDeOrdenacion
    {
        public string Propiedad { get; set; }
        public ModoDeOrdenancion Modo { get; set; }
    }

    public static partial class Joins
    {
        public static IQueryable<TRegistro> JoinBase<TRegistro>(this IQueryable<TRegistro> registros, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros = null) where TRegistro : Registro
        {
            return registros;
        }
    }

    public static partial class Filtros
    {
        public static IQueryable<TRegistro> FiltrarPorId<TRegistro>(this IQueryable<TRegistro> registros, List<ClausulaDeFiltrado> filtros) where TRegistro : Registro
        {
            foreach (ClausulaDeFiltrado filtro in filtros)
            {
                if (filtro.Clausula.ToLower() == nameof(Registro.Id).ToLower() && filtro.Valor.Entero() > 0)
                    return registros.Where(x => x.Id == filtro.Valor.Entero());
            }
            return registros;
        }

        public static IQueryable<TRegistro> FiltrarPorNombre<TRegistro>(this IQueryable<TRegistro> registros, List<ClausulaDeFiltrado> filtros) where TRegistro : Registro
        {
            foreach (ClausulaDeFiltrado filtro in filtros)
            {
                if (filtro.Clausula.ToLower() == nameof(Registro.Nombre).ToLower() && !filtro.Valor.IsNullOrEmpty())
                {
                    if (filtro.Criterio == CriteriosDeFiltrado.contiene)
                        return registros.Where(x => x.Nombre.Contains(filtro.Valor));

                    if (filtro.Criterio == CriteriosDeFiltrado.igual)
                        return registros.Where(x => x.Nombre == filtro.Valor);
                }
            }
            return registros;
        }

    }

    public static partial class Ordenaciones
    {
        public static IQueryable<TRegistro> OrdenBase<TRegistro>(this IQueryable<TRegistro> registros, List<ClausulaDeOrdenacion> ordenacion) where TRegistro : Registro
        {
            foreach (var orden in ordenacion)
            {
                if (orden.Propiedad.ToLower() == nameof(Registro.Id).ToLower())
                    return registros.OrdenPorId(orden);

                if (orden.Propiedad.ToLower() == nameof(Registro.Nombre).ToLower())
                    return registros.OrdenPorNombre(orden);

            }

            return registros;
        }

        public static IQueryable<TRegistro> OrdenPorId<TRegistro>(this IQueryable<TRegistro> registros, ClausulaDeOrdenacion orden) where TRegistro : Registro
        {
            return orden.Modo == ModoDeOrdenancion.ascendente
                ? registros.OrderBy(x => x.Id)
                : registros.OrderByDescending(x => x.Id);
        }

        public static IQueryable<TRegistro> OrdenPorNombre<TRegistro>(this IQueryable<TRegistro> registros, ClausulaDeOrdenacion orden) where TRegistro : Registro
        {
            return orden.Modo == ModoDeOrdenancion.ascendente
                ? registros.OrderBy(x => x.Nombre)
                : registros.OrderByDescending(x => x.Nombre);
        }
    }
    #endregion

    #region Extensiones a pasar a las operaciones a realizar

    public enum enumParametro { Creado }


    public class ParametrosDeNegocio
    {
        public TipoOperacion Tipo { get; private set; }
        public Dictionary<enumParametro, object> Parametros = new Dictionary<enumParametro, object>();

        public ParametrosDeNegocio(TipoOperacion tipo)
        {
            Tipo = tipo;
        }
    }

    public class ParametrosDeMapeo
    {
        public bool AnularMapeo = false;
        public Dictionary<string, object> parametros = new Dictionary<string, object>();
    }

    #endregion

    public abstract class GestorDeElementos<TContexto, TRegistro, TElemento>
        where TRegistro : Registro
        where TElemento : ElementoDto
        where TContexto : ContextoSe
    {
        public TContexto Contexto;
        public IMapper Mapeador;

        private static readonly ConcurrentDictionary<string, TRegistro> _CacheDeRegistros = new ConcurrentDictionary<string, TRegistro>();
        private static readonly ConcurrentDictionary<string, bool> _CacheDeRecuentos = new ConcurrentDictionary<string, bool>();
        private static readonly ConcurrentDictionary<Type, object> _CacheDeGestores = new ConcurrentDictionary<Type, object>();

        public TRegistro RegistroEnBD { get; private set; }

        public GestorDeElementos(TContexto contexto, IMapper mapeador)
        {
            Mapeador = mapeador;

            IniciarClase(contexto);
        }


        public GestorDeElementos(Func<TContexto> generadorDeContexto, IMapper mapeador)
        : this(generadorDeContexto(), mapeador)
        {
        }

        protected T CrearGestor<T>(Func<T> constructor)
        {
            if (!_CacheDeGestores.ContainsKey(typeof(T)))
            {
                _CacheDeGestores.TryAdd(typeof(T), constructor());
            }

            return (T)_CacheDeGestores[typeof(T)];
        }


        protected virtual void IniciarClase(TContexto contexto)
        {
            Contexto = contexto;
        }

        #region ASYNC

        #region Métodos de inserción ASYN

        public async Task InsertarElementoAsync(TElemento elemento, ParametrosDeNegocio parametros = null)
        {
            if (parametros == null)
                parametros = new ParametrosDeNegocio(TipoOperacion.Insertar);

            TRegistro elementoBD = MapearRegistro(elemento, parametros);
            Contexto.Add(elementoBD);
            await Contexto.SaveChangesAsync();
        }

        #endregion

        #region Métodos de modificación

        public async Task ModificarElementoAsync(TElemento elemento, ParametrosDeNegocio parametros = null)
        {
            if (parametros == null)
                parametros = new ParametrosDeNegocio(TipoOperacion.Modificar);

            TRegistro registro = MapearRegistro(elemento, parametros);
            await ModificarRegistroAsync(registro);
        }

        protected async Task ModificarRegistroAsync(TRegistro registro, ParametrosDeNegocio parametros = null)
        {
            if (parametros == null)
                parametros = new ParametrosDeNegocio(TipoOperacion.Modificar);

            Contexto.Update(registro);
            await Contexto.SaveChangesAsync();
        }

        #endregion

        #endregion

        #region Métodos de persistencia

        public bool IniciarTransaccion()
        {
            return Contexto.IniciarTransaccion();
        }

        public void Rollback(bool transaccion)
        {
            Contexto.Rollback(transaccion);
        }
        public void Commit(bool transaccion)
        {
            Contexto.Commit(transaccion);
        }

        public void PersistirElementosDto(List<TElemento> elementosDto, ParametrosDeNegocio parametros)
        {
            foreach (var elementoDto in elementosDto)
                PersistirElementoDto(elementoDto, parametros);
        }

        public void PersistirElementoDto(TElemento elementoDto, ParametrosDeNegocio parametros)
        {
            TRegistro registro = MapearRegistro(elementoDto, parametros);
            PersistirRegistro(registro, parametros);
            elementoDto.Id = registro.Id;
        }

        public string CrearRelacion(int idElemento1, int idElemento2)
        {     
            var registro = Registro.RegistroVacio<TRegistro>();
            if (!registro.RegistroDeRelacion)
                throw new Exception($"El registro {typeof(TRegistro)} no es de relación.");

            MapearDatosDeRelacion(registro, idElemento1, idElemento2);
            var filtros = new List<ClausulaDeFiltrado>();
            filtros.Add(new ClausulaDeFiltrado() { Clausula = nameof(idElemento1), Criterio = CriteriosDeFiltrado.igual, Valor = idElemento1.ToString() });
            filtros.Add(new ClausulaDeFiltrado() { Clausula = nameof(idElemento2), Criterio = CriteriosDeFiltrado.igual, Valor = idElemento2.ToString() });
            var registros = LeerRegistros(filtros).ToList();
            if (registros.Count != 0)
                return $"El registro {registro} ya existe";

            PersistirRegistro(registro, new ParametrosDeNegocio(TipoOperacion.Insertar));

            return "";
        }

        protected virtual void MapearDatosDeRelacion(TRegistro registro, int idElemento1, int idElemento2)
        {
            throw new Exception($"El registro {typeof(TRegistro)} no tiene definida la función de {nameof(MapearDatosDeRelacion)}.");
        }

        protected void PersistirRegistro(TRegistro registro, ParametrosDeNegocio parametros)
        {
            var registros = new List<TRegistro> { registro };
            PersistirRegistros(registros, parametros);
        }

        protected void PersistirRegistros(List<TRegistro> registros, ParametrosDeNegocio parametros)
        {
            var transaccion = Contexto.IniciarTransaccion();
            try
            {
                foreach (var registro in registros)
                {
                    AntesDePersistir(registro, parametros);

                    if (parametros.Tipo == TipoOperacion.Insertar)
                        Contexto.Add(registro);
                    else
                    if (parametros.Tipo == TipoOperacion.Modificar)
                        Contexto.Update(registro);
                    else
                    if (parametros.Tipo == TipoOperacion.Eliminar)
                        Contexto.Remove(registro);
                    else
                        throw new Exception($"Solo se pueden persistir operaciones del tipo {TipoOperacion.Insertar} o  {TipoOperacion.Modificar} o {TipoOperacion.Eliminar}");
                    Contexto.SaveChanges();

                    DespuesDePersistir(registro, parametros);
                }

                Contexto.Commit(transaccion);
            }
            catch (Exception exc)
            {
                Contexto.Rollback(transaccion);
                throw exc;
            }
        }

        protected virtual void DespuesDePersistir(TRegistro registro, ParametrosDeNegocio parametros)
        {
            _CacheDeRecuentos[typeof(TRegistro).FullName] = true;

            foreach (var clave in _CacheDeRegistros.Keys)
            {
                if (clave.StartsWith(typeof(TRegistro).FullName))
                {
                    _CacheDeRegistros.TryRemove(clave, out _);
                }
            }
        }

        protected virtual void AntesDePersistir(TRegistro registro, ParametrosDeNegocio parametros)
        {
            RegistroEnBD = LeerRegistroPorId(registro.Id);
        }

        protected void PersistirElementoDtm(ElementoDtm elementoDtm, ParametrosDeNegocio parametros) => PersistirElementosDtm(new List<ElementoDtm> { elementoDtm }, parametros);

        protected void PersistirElementosDtm(List<ElementoDtm> elementosDtm, ParametrosDeNegocio parametros)
        {

            foreach (var elementoDtm in elementosDtm)
            {
                if (parametros.Tipo == TipoOperacion.Insertar)
                {
                    elementoDtm.IdUsuaCrea = Contexto.DatosDeConexion.IdUsuario;
                    elementoDtm.FechaCreacion = DateTime.Now;
                }
                else
                if (parametros.Tipo == TipoOperacion.Modificar)
                {
                    elementoDtm.IdUsuaModi = Contexto.DatosDeConexion.IdUsuario;
                    elementoDtm.FechaModificacion = DateTime.Now;
                }

                PersistirRegistro(elementoDtm as TRegistro, parametros);
            }
        }


        #endregion


        #region Métodos de lectura

        public IEnumerable<TElemento> LeerElementos(int posicion, int cantidad, List<ClausulaDeFiltrado> filtros, List<ClausulaDeOrdenacion> orden)
        {
            List<TRegistro> elementosDeBd = LeerRegistros(posicion, cantidad, filtros, orden);

            // (IEnumerable<TElemento>)Mapeador.Map(elementosDeBd, typeof(IEnumerable<TRegistro>), typeof(IEnumerable<TElemento>));

            return MapearElementos(elementosDeBd);
        }

        public List<TElemento> ProyectarElementos(int posicion, int cantidad, List<ClausulaDeFiltrado> filtros, List<ClausulaDeOrdenacion> orden, ParametrosDeNegocio parametros = null)
        {
            IQueryable<TRegistro> registros = DefinirConsulta(posicion, cantidad, filtros, orden, null, parametros);

            return Mapeador.ProjectTo<TElemento>(registros).AsNoTracking().ToList();
        }

        public TRegistro LeerRegistroCacheado(string propiedad, string valor, bool errorSiNoHay = true, bool errorSiHayMasDeUno = true)
        {
            var indice = $"{typeof(TRegistro).FullName}-{propiedad}-{valor}";
            if (!_CacheDeRegistros.ContainsKey(indice))
            {
                var a = LeerRegistro(propiedad, valor, errorSiNoHay, errorSiHayMasDeUno);
                if (a == null)
                    return null;

                _CacheDeRegistros[indice] = a;
            }
            return _CacheDeRegistros[indice];
        }

        public TRegistro LeerRegistro(string propiedad, string valor, bool errorSiNoHay, bool errorSiHayMasDeUno)
        {
            List<TRegistro> registros = LeerRegistroInterno(propiedad, valor);

            if (errorSiNoHay && registros.Count == 0)
                GestorDeErrores.Emitir($"No se ha localizado el registro solicitada para el valor {valor} en la clase {typeof(TRegistro).Name}");

            if (errorSiHayMasDeUno && registros.Count > 1)
                GestorDeErrores.Emitir($"Hay más de un registro para el valor {valor} en la clase {typeof(TRegistro).Name}");

            return registros.Count == 1 ? registros[0] : null;
        }

        public (int resultado, TRegistro registro) ExisteRegistro(string propiedad, string valor)
        {
            List<TRegistro> registros = LeerRegistroInterno(propiedad, valor);

            if (registros.Count == 0)
                return (0, null);

            return (registros.Count, registros[0]);
        }

        private List<TRegistro> LeerRegistroInterno(string propiedad, string valor)
        {
            var filtro = new ClausulaDeFiltrado()
            {
                Criterio = CriteriosDeFiltrado.igual,
                Clausula = propiedad,
                Valor = valor
            };
            var filtros = new List<ClausulaDeFiltrado>() { filtro };
            IQueryable<TRegistro> registros = DefinirConsulta(0, -1, filtros, null, null, null);
            return registros.AsNoTracking().ToList();
        }

        public List<TRegistro> LeerRegistros(List<ClausulaDeFiltrado> filtros)
        {
            return LeerRegistros(0,0,filtros);
        }

        public List<TRegistro> LeerRegistros(int posicion, int cantidad, List<ClausulaDeFiltrado> filtros = null, List<ClausulaDeOrdenacion> orden = null, List<ClausulaDeJoin> joins = null, ParametrosDeNegocio parametros = null)
        {
            List<TRegistro> elementosDeBd;

            IQueryable<TRegistro> registros = DefinirConsulta(posicion, cantidad, filtros, orden, joins, parametros);

            elementosDeBd = registros.AsNoTracking().ToList();

            return elementosDeBd;
        }

        private IQueryable<TRegistro> DefinirConsulta(int posicion, int cantidad, List<ClausulaDeFiltrado> filtros, List<ClausulaDeOrdenacion> orden, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros)
        {
            if (parametros == null)
                parametros = new ParametrosDeNegocio(TipoOperacion.Leer);

            if (joins == null)
                joins = new List<ClausulaDeJoin>();

            if (filtros == null)
                filtros = new List<ClausulaDeFiltrado>();

            DefinirJoins(filtros, joins, parametros);

            IQueryable<TRegistro> registros = Contexto.Set<TRegistro>();

            if (joins.Count > 0)
                registros = AplicarJoins(registros, joins, parametros);

            if (filtros.Count > 0)
                registros = AplicarFiltros(registros, filtros, parametros);

            if (orden != null && orden.Count > 0)
                registros = AplicarOrden(registros, orden);

            registros = registros.Skip(posicion);

            if (cantidad > 0)
            {
                registros = registros.Take(cantidad);
            }

            return registros;
        }

        /// <summary>
        /// se indican que joins se han de montar cuando se defina la consulta en función de los filtros y los parámetros de negocio
        /// </summary>
        /// <param name="filtros">filtros que se van a aplicar</param>
        /// <param name="joins">join a incluir</param>
        /// <param name="parametros">parámetros de negocio que modifican el comportamiento</param>
        protected virtual void DefinirJoins(List<ClausulaDeFiltrado> filtros, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros)
        {

        }

        protected virtual IQueryable<TRegistro> AplicarOrden(IQueryable<TRegistro> registros, List<ClausulaDeOrdenacion> ordenacion)
        {
            return registros.OrdenBase(ordenacion);
        }

        protected virtual IQueryable<TRegistro> AplicarFiltros(IQueryable<TRegistro> registros, List<ClausulaDeFiltrado> filtros, ParametrosDeNegocio parametros)
        {
            registros = registros.FiltrarPorId(filtros);

            if (HayFiltroPorId(registros))
                return registros;

            return registros.FiltrarPorNombre(filtros);
        }

        protected virtual IQueryable<TRegistro> AplicarJoins(IQueryable<TRegistro> registros, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros)
        {
            return registros.JoinBase(joins, parametros);
        }

        protected bool HayFiltroPorId(IQueryable<TRegistro> registros)
        {
            return registros.Expression.ToString().Contains(".Where(x => x.Id");
        }

        #endregion

        #region Métodos de acceso a BD
        public bool ExisteObjetoEnBd(int id)
        {
            return Contexto.Set<TRegistro>().Any(e => e.Id == id);
        }

        public int Contar(List<ClausulaDeFiltrado> filtros, List<ClausulaDeJoin> joins = null, ParametrosDeNegocio parametros = null)
        {
            var registros = DefinirConsulta(0, -1, filtros, null, joins, parametros);
            var total = registros.Count();

            _CacheDeRecuentos[typeof(TRegistro).FullName] = false;

            return total;
        }

        public int Recontar(List<ClausulaDeFiltrado> filtros, List<ClausulaDeJoin> joins = null, ParametrosDeNegocio parametros = null)
        {
            if (!_CacheDeRecuentos.ContainsKey(typeof(TRegistro).FullName) || _CacheDeRecuentos[typeof(TRegistro).FullName])
            {
                return Contar(filtros, joins, parametros);
            }

            return 0;
        }

        #endregion

        #region Métodos de mapeo

        public List<TRegistro> MapearRegistros(List<TElemento> elementos, ParametrosDeNegocio opciones)
        {
            var registros = new List<TRegistro>();
            foreach (var elemento in elementos)
            {
                var registro = MapearRegistro(elemento, opciones);
                registros.Add(registro);
            }
            return registros;
        }

        protected TRegistro MapearRegistro(TElemento elemento, ParametrosDeNegocio opciones)
        {
            var registro = Mapeador.Map<TElemento, TRegistro>(elemento,
                   opt =>
                   {
                       opt.BeforeMap((src, dest) => AntesMapearRegistro(elemento, opciones));
                       opt.AfterMap((src, dest) => DespuesDeMapearRegistro(elemento, dest, opciones));
                   }
                );

            return registro;
        }

        protected virtual void DespuesDeMapearRegistro(TElemento elemento, TRegistro registro, ParametrosDeNegocio opciones)
        {
            if (TipoOperacion.Insertar == opciones.Tipo)
                registro.Id = 0;
        }

        private void AntesMapearRegistro(TElemento elemento, ParametrosDeNegocio opciones)
        {

            if (opciones.Tipo == TipoOperacion.Insertar)
                AntesMapearRegistroParaInsertar(elemento, opciones);
            else
            if (opciones.Tipo == TipoOperacion.Modificar)
                AntesMapearRegistroParaModificar(elemento, opciones);
            else
            if (opciones.Tipo == TipoOperacion.Eliminar)
                AntesMapearRegistroParaEliminar(elemento, opciones);
        }

        protected virtual void AntesMapearRegistroParaEliminar(TElemento elemento, ParametrosDeNegocio opciones)
        {
            if (elemento.Id == 0)
                GestorDeErrores.Emitir($"No puede eliminar un elemento {typeof(TElemento).Name} con id 0");
        }

        protected virtual void AntesMapearRegistroParaModificar(TElemento elemento, ParametrosDeNegocio opciones)
        {
            if (elemento.Id == 0)
                GestorDeErrores.Emitir($"No puede modificar un elemento {typeof(TElemento).Name} con id 0");
        }

        protected virtual void AntesMapearRegistroParaInsertar(TElemento elemento, ParametrosDeNegocio opciones)
        {
            if (elemento.Id > 0)
                GestorDeErrores.Emitir($"No puede crear un elemento {typeof(TElemento).Name} con id {elemento.Id}");
        }

        public IEnumerable<TElemento> MapearElementos(List<TRegistro> registros, ParametrosDeMapeo parametros = null)
        {
            if (parametros == null)
                parametros = new ParametrosDeMapeo();

            var lista = new List<TElemento>();
            foreach (var registro in registros)
            {
                var elemento = MapearElemento(registro, parametros);
                if (elemento != null)
                    lista.Add(elemento);
            }
            return lista.AsEnumerable();
        }

        protected virtual void AntesDeMapearElemento(TRegistro registro, ParametrosDeMapeo parametros)
        {

        }

        public TElemento MapearElemento(TRegistro registro, ParametrosDeMapeo parametros = null)
        {
            if (parametros == null)
                parametros = new ParametrosDeMapeo();

            TElemento elemento = null;
            elemento = Mapeador.Map<TRegistro, TElemento>(registro,
                opt =>
                {
                    opt.BeforeMap((registro, elemento) => AntesDeMapearElemento(registro, parametros));
                    opt.AfterMap((registro, elemento) => DespuesDeMapearElemento(registro, elemento, parametros));
                }
                );

            if (parametros.AnularMapeo)
                elemento = null;

            return elemento;
        }

        protected virtual void DespuesDeMapearElemento(TRegistro registro, TElemento elemento, ParametrosDeMapeo parametros)
        {

        }

        #endregion


        public string ObtenerTabla()
        {
            var entityType = Contexto.Model.FindEntityType(typeof(TRegistro));
            var schema = entityType.GetSchema();
            var tableName = entityType.GetTableName();
            return $"{schema}.{tableName}";
        }

        #region codigo creo que obsoleto

        public TElemento LeerElementoPorId(int id)
        {
            var elementoDeBd = LeerRegistroPorId(id);
            if (elementoDeBd == null)
                throw new Exception($"No existe en la base de datos un registro de {typeof(TRegistro).Name} con Id {id}");
            return MapearElemento(elementoDeBd);
        }

        public TRegistro LeerRegistroPorId(int? id)
        {
            if (id == null)
                return null;

            return Contexto.Set<TRegistro>().AsNoTracking().FirstOrDefault(m => m.Id == id);
        }


        #endregion


    }

}

