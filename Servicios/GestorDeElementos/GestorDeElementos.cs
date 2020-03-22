﻿using AutoMapper;
using Gestor.Elementos.ModeloBd;
using Gestor.Elementos.ModeloIu;
using Gestor.Errores;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Utilidades;

namespace Gestor.Elementos
{

    public class ClausulaDeFiltrado
    {
        public string Propiedad { get; set; }
        public string Criterio { get; set; }
        public string Valor { get; set; }
    }


    public static class RegistroBaseFiltros
    {
        public const string FiltroPorId = "Id";

        public static IQueryable<TRegistro> AplicarFiltroId<TRegistro>(this IQueryable<TRegistro> registros, List<ClausulaDeFiltrado> filtros) where TRegistro : Registro
        {
            foreach (ClausulaDeFiltrado filtro in filtros)
                if (filtro.Propiedad.ToLower() == FiltroPorId.ToLower())
                    return registros.Where(x => x.Id == filtro.Valor.Entero());

            return registros;
        }
    }

    public enum Ordenacion { Ascendente, Descendente };


    public enum TipoOperacion { Insertar, Modificar, Leer };

    public static class Ordenaciones
    {
        public const string OrdenPorId = "PorId";

        public static IQueryable<TRegistro> Orden<TRegistro>(this IQueryable<TRegistro> set, Dictionary<string, Ordenacion> orden) where TRegistro : Registro
        {
            if (orden.ContainsKey(OrdenPorId))
            {
                if (orden[OrdenPorId] == Ordenacion.Ascendente)
                    return set.OrderBy(x => x.Id);

                if (orden[OrdenPorId] == Ordenacion.Descendente)
                    return set.OrderByDescending(x => x.Id);
            }

            return set;
        }
    }


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

        #region Métodos de inserción
        public async Task InsertarElementoAsync(TElemento elemento)
        {
            TRegistro elementoBD = MapearRegistro(elemento, TipoOperacion.Insertar);
            Contexto.Add(elementoBD);
            await Contexto.SaveChangesAsync();
        }

        public void InsertarElementos(List<TElemento> elementos)
        {
            foreach (var e in elementos)
                InsertarElemento(e);
        }

        public void InsertarElemento(TElemento elemento)
        {
            TRegistro registro = MapearRegistro(elemento, TipoOperacion.Insertar);
            InsertarRegistro(registro);
            elemento.Id = registro.Id;
        }
        protected void InsertarRegistros(List<TRegistro> registros)
        {
            using (var transaction = Contexto.Database.BeginTransaction())
            try
            {

                foreach (var registro in registros)
                    Contexto.Add(registro);

                Contexto.SaveChanges();
                transaction.Commit();
            }
            catch (Exception exc)
            {
                transaction.Rollback();
                throw exc;
            }
        }
        protected void InsertarRegistro(TRegistro registro)
        {
            using (var transaction = Contexto.Database.BeginTransaction())
            try
            {
                Contexto.Add(registro);
                Contexto.SaveChanges();
                transaction.Commit();
            }
            catch (Exception exc)
            {
                transaction.Rollback();
                throw exc;
            }
        }

        #endregion

        #region Métodos de modificación

        public async Task ModificarElementoAsync(TElemento elemento)
        {
            TRegistro registro = MapearRegistro(elemento, TipoOperacion.Modificar);
            await ModificarRegistroAsync(registro);
        }

        public void ModificarElemento(TElemento elemento)
        {
            TRegistro registro = MapearRegistro(elemento, TipoOperacion.Modificar);
            ModificarRegistro(registro);

        }

        protected void ModificarRegistro(TRegistro registro)
        {
            Contexto.Update(registro);
            Contexto.SaveChanges();
        }
        protected async Task ModificarRegistroAsync(TRegistro registro)
        {
            Contexto.Update(registro);
            await Contexto.SaveChangesAsync();
        }


        #endregion

        #region Métodos de lectura

        public IEnumerable<TElemento> Leer(int posicion, int cantidad, List<ClausulaDeFiltrado> filtros, Dictionary<string, Ordenacion> orden)
        {
            List<TRegistro> elementosDeBd;

            IQueryable<TRegistro> registros = AplicarFiltros(Contexto.Set<TRegistro>(), filtros);

            registros = AplicarOrden(registros, orden);

            registros = registros.Skip(posicion);

            if (cantidad > 0)
            {
                registros = registros.Take(cantidad);
            }

            elementosDeBd = registros.AsNoTracking().ToList();

            return MapearElementos(elementosDeBd);
        }

        protected virtual IQueryable<TRegistro> AplicarOrden(IQueryable<TRegistro> registros, Dictionary<string, Ordenacion> orden)
        {
            return registros.Orden(orden);
        }

        protected virtual IQueryable<TRegistro> AplicarFiltros(IQueryable<TRegistro> registros, List<ClausulaDeFiltrado> filtros)
        {
            return registros.AplicarFiltroId(filtros);
        }

        #endregion

        #region Métodos de acceso a BD
        public bool ExisteObjetoEnBd(int id)
        {
            return Contexto.Set<TRegistro>().Any(e => e.Id == id);
        }

        public int Contar(List<ClausulaDeFiltrado> filtros)
        {

            IQueryable<TRegistro> registros = AplicarFiltros(Contexto.Set<TRegistro>(), filtros);

            var total = registros.Count();

            return total;
        }

        #endregion

        #region Métodos de mapeo

        public List<TRegistro> MapearRegistros(List<TElemento> elementos, TipoOperacion tipoOperacion)
        {
            var registros = new List<TRegistro>();
            foreach (var elemento in elementos)
            {
                var registro = MapearRegistro(elemento, tipoOperacion);
                registros.Add(registro);
            }
            return registros;
        }

        protected TRegistro MapearRegistro(TElemento elemento, TipoOperacion tipoOperacion)
        {
            AntesMapearRegistro(elemento, tipoOperacion);
            var registro = (TRegistro)Mapeador.Map(elemento, typeof(TElemento), typeof(TRegistro));
            DespuesDeMapearRegistro(elemento, registro, tipoOperacion);
            return registro;
        }

        protected virtual void DespuesDeMapearRegistro(TElemento elemento, TRegistro registro, TipoOperacion tipo)
        {
            if (TipoOperacion.Insertar == tipo)
                registro.Id = 0;
        }

        protected virtual void AntesMapearRegistro(TElemento elemento, TipoOperacion tipo)
        {
        }

        private IEnumerable<TElemento> MapearElementos(List<TRegistro> registros)
        {
            var lista = new List<TElemento>();
            foreach (var registro in registros)
            {

                var elemento = MapearElemento(registro);
                lista.Add(elemento);
            }
            return lista.AsEnumerable();
        }

        public TElemento MapearElemento(TRegistro registro, List<string> excluirPropiedad = null)
        {
            var elemento = (TElemento)Mapeador.Map(registro, typeof(TRegistro), typeof(TElemento));
            return elemento;
        }

        #endregion


        #region codigo creo que obsoleto

        public TElemento LeerElementoPorId(int id)
        {
            var elementoDeBd = LeerRegistroPorId(id);
            return MapearElemento(elementoDeBd);
        }

        public TRegistro LeerRegistroPorId(int id)
        {
            return Contexto.Set<TRegistro>().AsNoTracking().FirstOrDefault(m => m.Id == id);
        }


        public TElemento LeerElementoConDetalle(int id)
        {
            var elementoLeido = LeerConDetalle(id);
            return MapearElemento(elementoLeido);
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
