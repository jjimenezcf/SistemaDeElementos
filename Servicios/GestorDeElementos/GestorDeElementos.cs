﻿using AutoMapper;
using Gestor.Elementos.ModeloBd;
using Gestor.Elementos.ModeloIu;
using Gestor.Errores;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Utilidades;

namespace Gestor.Elementos
{
    public enum CriteriosDeFiltrado { igual, mayor, menor, esNulo, noEsNulo, contiene, comienza, termina, mayorIgual, menorIgual }
    public enum ModoDeOrdenancion { ascendente, descendente }
    public enum TipoOperacion { Insertar, Modificar, Leer };

    #region Extensiones para filtrar, hacer joins y ordenar
    public class ClausulaDeJoin
    {
        public Type Dtm { get; set; }
    }
    public class ClausulaDeFiltrado
    {
        public string Propiedad { get; set; }
        public CriteriosDeFiltrado Criterio { get; set; }
        public string Valor { get; set; }
    }

    public class ClausulaOrdenacion
    {

        public string Propiedad { get; set; }
        public ModoDeOrdenancion modo { get; set; }
    };

    public static partial class Joins
    {
        public static IQueryable<TRegistro> JoinBase<TRegistro>(this IQueryable<TRegistro> registros, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros = null) where TRegistro : Registro
        {
            return registros;
        }
    }

    public static partial class Filtros
    {
        public static IQueryable<TRegistro> FiltroBase<TRegistro>(this IQueryable<TRegistro> registros, List<ClausulaDeFiltrado> filtros, ParametrosDeNegocio parametros = null) where TRegistro : Registro
        {
            foreach (ClausulaDeFiltrado filtro in filtros)
                return registros.FiltrarPorId(filtro);

            return registros;
        }

        public static IQueryable<TRegistro> FiltrarPorId<TRegistro>(this IQueryable<TRegistro> registros, ClausulaDeFiltrado filtro) where TRegistro : Registro
        {
            if (filtro.Propiedad.ToLower() == nameof(Registro.Id).ToLower())
                return registros.Where(x => x.Id == filtro.Valor.Entero());

            return registros;
        }
    }

    public static partial class Ordenaciones
    {
        public static IQueryable<TRegistro> OrdenBase<TRegistro>(this IQueryable<TRegistro> registros, List<ClausulaOrdenacion> ordenacion) where TRegistro : Registro
        {
            foreach (var orden in ordenacion)
            {
                if (orden.Propiedad == nameof(Registro.Id))
                    return registros.OrdenPorId(orden);
            }

            return registros;
        }

        public static IQueryable<TRegistro> OrdenPorId<TRegistro>(this IQueryable<TRegistro> registros, ClausulaOrdenacion orden) where TRegistro : Registro
        {

            if (orden.Propiedad == nameof(Registro.Id))
                return orden.modo == ModoDeOrdenancion.ascendente
                    ? registros.OrderBy(x => x.Id)
                    : registros.OrderByDescending(x => x.Id);

            return registros;
        }
    }
    #endregion

    #region Extensiones a pasar a las operaciones a realizar

    public class ParametrosDeNegocio
    {
        public TipoOperacion Tipo { get; private set; }
        public IDbContextTransaction Transaccion { get; internal set; }

        public Dictionary<string, object> Parametros = new Dictionary<string, object>();

        public ParametrosDeNegocio(TipoOperacion tipo)
        {
            Tipo = tipo;
        }
    }

    #endregion

    public abstract class GestorDeElementos<TContexto, TRegistro, TElemento>
        where TRegistro : Registro
        where TElemento : Elemento
        where TContexto : ContextoDeElementos
    {
        protected ClaseDeElemetos<TRegistro, TElemento> Metadatos;
        public TContexto Contexto;
        private GestorDeErrores _gestorDeErrores;
        public IMapper Mapeador;

        protected abstract TRegistro LeerConDetalle(int Id);

        public GestorDeElementos(TContexto contexto, IMapper mapeador)
        {
            Mapeador = mapeador;

            IniciarClase(contexto);
        }

        public void AsignarGestores(GestorDeErrores gestorErrores)
        {
            _gestorDeErrores = gestorErrores;
        }


        protected virtual void IniciarClase(TContexto contexto)
        {
            Contexto = contexto;
            Metadatos = ClaseDeElemetos<TRegistro, TElemento>.ObtenerGestorDeLaClase();
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

        #region Métodos de inserción

        public void InsertarElementos(List<TElemento> elementos, ParametrosDeNegocio parametros = null)
        {
            foreach (var e in elementos)
                InsertarElemento(e, parametros);
        }

        public void InsertarElemento(TElemento elemento, ParametrosDeNegocio parametros = null)
        {
            if (parametros == null)
                parametros = new ParametrosDeNegocio(TipoOperacion.Insertar);

            TRegistro registro = MapearRegistro(elemento, parametros);
            InsertarRegistro(registro, parametros);
            elemento.Id = registro.Id;
        }

        protected void InsertarRegistro(TRegistro registro, ParametrosDeNegocio parametros = null) => InsertarRegistros(new List<TRegistro> { registro }, parametros);

        protected void InsertarRegistros(List<TRegistro> registros, ParametrosDeNegocio parametros = null)
        {
            if (parametros == null)
                parametros = new ParametrosDeNegocio(TipoOperacion.Insertar);

            var transaccionAbierta = IniciarTransaccion(parametros);
            try
            {
                foreach (var registro in registros)
                    Contexto.Add(registro);

                Contexto.SaveChanges();
                Commit(parametros, transaccionAbierta);
            }
            catch (Exception exc)
            {
                RollBack(parametros, transaccionAbierta);
                throw exc;
            }
        }



        #endregion

        #region Métodos de modificación

        public void ModificarElemento(TElemento elemento, ParametrosDeNegocio parametros = null)
        {
            if (parametros == null)
                parametros = new ParametrosDeNegocio(TipoOperacion.Modificar);

            TRegistro registro = MapearRegistro(elemento, parametros);
            ModificarRegistro(registro, parametros);

        }

        protected void ModificarRegistro(TRegistro registro, ParametrosDeNegocio parametros = null) => ModificarRegistros(new List<TRegistro> { registro }, parametros);

        protected void ModificarRegistros(List<TRegistro> registros, ParametrosDeNegocio parametros = null)
        {
            if (parametros == null)
                parametros = new ParametrosDeNegocio(TipoOperacion.Modificar);

            var transaccionAbierta = IniciarTransaccion(parametros);
            try
            {
                foreach (var registro in registros)
                    Contexto.Update(registro);

                Contexto.SaveChanges();
                Commit(parametros, transaccionAbierta);
            }
            catch (Exception exc)
            {
                RollBack(parametros, transaccionAbierta);
                throw exc;
            }
        }



        #endregion

        #region Métodos de lectura

        public IEnumerable<TElemento> LeerElementos(int posicion, int cantidad, List<ClausulaDeFiltrado> filtros, List<ClausulaOrdenacion> orden)
        {
            List<TRegistro> elementosDeBd = LeerRegistros(posicion, cantidad, filtros, orden);

            return (IEnumerable<TElemento>)Mapeador.Map(elementosDeBd, typeof(IEnumerable<TRegistro>), typeof(IEnumerable<TElemento>));
        }

        public List<TElemento> ProyectarElementos(int posicion, int cantidad, List<ClausulaDeFiltrado> filtros, List<ClausulaOrdenacion> orden, ParametrosDeNegocio parametros = null)
        {
            IQueryable<TRegistro> registros = DefinirConsulta(posicion, cantidad, filtros, orden, null, parametros);

            return Mapeador.ProjectTo<TElemento>(registros).AsNoTracking().ToList();
        }

        public List<TRegistro> LeerRegistros(int posicion, int cantidad, List<ClausulaDeFiltrado> filtros = null, List<ClausulaOrdenacion> orden = null, List<ClausulaDeJoin> joins =null, ParametrosDeNegocio parametros = null)
        {

            List<TRegistro> elementosDeBd;

            IQueryable<TRegistro> registros = DefinirConsulta(posicion, cantidad, filtros, orden, joins, parametros);

            elementosDeBd = registros.AsNoTracking().ToList();

            return elementosDeBd;
        }

        private IQueryable<TRegistro> DefinirConsulta(int posicion, int cantidad, List<ClausulaDeFiltrado> filtros, List<ClausulaOrdenacion> orden, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros)
        {
            if (parametros == null)
                parametros = new ParametrosDeNegocio(TipoOperacion.Leer);

            IQueryable<TRegistro> registros = null;
            
            if (joins != null && joins.Count > 0)
            {
                DefinirJoins(filtros, joins, parametros);
                registros = AplicarJoins(Contexto.Set<TRegistro>(), joins, parametros);
            }

            if (filtros != null && filtros.Count > 0)
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

        protected virtual void DefinirJoins(List<ClausulaDeFiltrado> filtros, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros)
        {

        }

        protected virtual IQueryable<TRegistro> AplicarOrden(IQueryable<TRegistro> registros, List<ClausulaOrdenacion> ordenacion)
        {
            return registros.OrdenBase(ordenacion);
        }

        protected virtual IQueryable<TRegistro> AplicarFiltros(IQueryable<TRegistro> registros, List<ClausulaDeFiltrado> filtros, ParametrosDeNegocio parametros)
        {
            return registros.FiltroBase(filtros, parametros);
        }

        protected virtual IQueryable<TRegistro> AplicarJoins(IQueryable<TRegistro> registros, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros)
        {
            return registros.JoinBase(joins, parametros);
        }

        #endregion

        #region Métodos de acceso a BD
        public bool ExisteObjetoEnBd(int id)
        {
            return Contexto.Set<TRegistro>().Any(e => e.Id == id);
        }

        public int Contar(List<ClausulaDeFiltrado> filtros, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros)
        {
            var registros = DefinirConsulta(0, -1, filtros, null, joins, parametros);
            var total = registros.Count();

            return total;
        }

        private bool IniciarTransaccion(ParametrosDeNegocio parametros)
        {
            if (Contexto.Database.CurrentTransaction == null)
            {
                parametros.Transaccion = Contexto.Database.BeginTransaction();
                return true;
            }
            return false;
        }

        private void Commit(ParametrosDeNegocio parametros, bool transaccionAbierta)
        {
            if (transaccionAbierta)
            {
                parametros.Transaccion.Commit();
                parametros.Transaccion.Dispose();
            }
        }

        private void RollBack(ParametrosDeNegocio parametros, bool transaccionAbierta)
        {
            if (transaccionAbierta)
            {
                parametros.Transaccion.Rollback();
                parametros.Transaccion.Dispose();
            }
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

        protected virtual void AntesMapearRegistro(TElemento elemento, ParametrosDeNegocio opciones)
        {
        }

        public IEnumerable<TElemento> MapearElementos(List<TRegistro> registros, Dictionary<string, object> parametros = null)
        {
            if (parametros == null)
                parametros = new Dictionary<string, object>();

            var lista = new List<TElemento>();
            foreach (var registro in registros)
            {
                var elemento = MapearElemento(registro, parametros);
                if (elemento != null)
                    lista.Add(elemento);
            }
            return lista.AsEnumerable();
        }

        protected virtual bool AntesDeMapearElemento(TRegistro registro, Dictionary<string, object> parametros)
        {
            return true;
        }

        protected TElemento MapearElemento(TRegistro registro, Dictionary<string, object> parametros)
        {
            TElemento elemento = null;
            if (AntesDeMapearElemento(registro, parametros))
            {
                elemento = (TElemento)Mapeador.Map(registro, typeof(TRegistro), typeof(TElemento));
                DespuesDeMapearElemento(registro, elemento, parametros);
            }
            return elemento;
        }

        protected virtual void DespuesDeMapearElemento(TRegistro registro, TElemento elemento, Dictionary<string, object> parametros)
        {
        }

        #endregion


        #region codigo creo que obsoleto

        public TElemento LeerElementoPorId(int id)
        {
            var elementoDeBd = LeerRegistroPorId(id);
            return MapearElemento(elementoDeBd, new Dictionary<string, object>());
        }

        public TRegistro LeerRegistroPorId(int? id)
        {
            if (id == null)
                return null;

            return Contexto.Set<TRegistro>().AsNoTracking().FirstOrDefault(m => m.Id == id);
        }


        public TElemento LeerElementoConDetalle(int id)
        {
            var elementoLeido = LeerConDetalle(id);
            return MapearElemento(elementoLeido, new Dictionary<string, object>());
        }

        public void BorrarPorId(int id)
        {
            var registro = LeerRegistroPorId(id);
            Contexto.Remove(registro);
            Contexto.SaveChangesAsync();
        }

        #endregion

    }
}
