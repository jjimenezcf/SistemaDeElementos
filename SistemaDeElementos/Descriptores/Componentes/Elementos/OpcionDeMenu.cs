﻿using System;
using System.Collections.Generic;
using ModeloDeDto;
using ServicioDeDatos.Utilidades;

namespace MVCSistemaDeElementos.Descriptores
{
    public enum TipoDeLlamada { Post, Get }

    public static class TipoAccionMnt
    {
        public const string CrearElemento = "crear-elemento";
        public const string EditarElemento = "editar-elemento";
        public const string EliminarElemento = "eliminar-elemento";
        public const string RelacionarElementos = "relacionar-elementos";
        public const string CrearRelaciones = "crear-relaciones";
    }

    public static class TipoAccionCreacion
    {
        public const string NuevoElemento = "nuevo-elemento";
        public const string CerrarCreacion = "cerrar-creacion";
    }
    public static class TipoAccionEdicion
    {
        public const string ModificarElemento = "modificar-elemento";
        public const string CancelarEdicion = "cancelar-edicion";
    }
    public static class TipoAccionCrearRelaciones
    {
        public const string RelacionarElementos = "relacionar-elementos";
        public const string CancelarSeleccion = "cancelar-seleccion";
    }

    public class AccionDeMenu
    {

        public string TipoDeAccion { get; private set; }

        public AccionDeMenu(string tipoDeAccion)
        {
            TipoDeAccion = tipoDeAccion;
        }

        public virtual string RenderAccion()
        {
            return "";
        }
    }

    /**********************************************************/
    // Acciones de menú de para navegar
    // renderiza llamada Crud.EventosDelMantenimiento(...)
    /**********************************************************/
    public class AccionDeNavegarParaRelacionar : AccionDeMenu
    {
        public string TipoAccion { get; private set; }
        public string UrlDelCrudDeRelacion { get; private set; }
        public string RelacionarCon { get; private set; }
        public string PropiedadRestrictora { get; private set; }
        public string PropiedadQueRestringe { get; private set; }
        public string NavegarAlCrud { get; private set; }

        public AccionDeNavegarParaRelacionar(string urlDelCrud, string relacionarCon, string nombreDelMnt,string propiedadQueRestringe, string propiedadRestrictora)
        : base(TipoAccionMnt.RelacionarElementos)
        {
            TipoAccion = TipoAccionMnt.RelacionarElementos;
            RelacionarCon = relacionarCon.ToLower();
            PropiedadRestrictora = propiedadRestrictora.ToLower();
            PropiedadQueRestringe = propiedadQueRestringe.ToLower();
            UrlDelCrudDeRelacion = urlDelCrud;
            NavegarAlCrud = nombreDelMnt;
        }

        public override string RenderAccion()
        {
            return $"Crud.EventosDelMantenimiento('{TipoAccion}','IdOpcionDeMenu==idDeOpcMenu#{nameof(RelacionarCon)}=={RelacionarCon}#{nameof(PropiedadQueRestringe)}=={PropiedadQueRestringe}#{nameof(PropiedadRestrictora)}=={PropiedadRestrictora}')";
        }
    }

    /**********************************************************/
    // Acciones de menú de un mantenimiento
    // renderiza llamada Crud.EventosDelMantenimiento(...)
    /**********************************************************/
    public class AccionDeMenuMnt : AccionDeMenu
    {
        protected List<string> Parametros = new List<string>();

        public AccionDeMenuMnt(string tipoAccion)
        : base(tipoAccion)
        {
        }

        public override string RenderAccion()
        {
            var parametros = ""; 
            for(var i=0; i< Parametros.Count; i++)
                parametros = $"{parametros}{(i ==0 ? "": "#")}{Parametros[i]}";

            return $"Crud.EventosDelMantenimiento('{TipoDeAccion}'{(Parametros.Count == 0 ? "": $",'{parametros}'")})";
        }
    }

    public class CrearElemento : AccionDeMenuMnt
    {
        public CrearElemento()
        : base(TipoAccionMnt.CrearElemento)
        {
        }
    }

    public class BorrarElemento : AccionDeMenuMnt
    {
        public BorrarElemento()
        : base(TipoAccionMnt.EliminarElemento)
        {
        }
    }

    public class EditarElemento : AccionDeMenuMnt
    {
        public EditarElemento()
        : base(TipoAccionMnt.EditarElemento)
        {
        }
    }

    public class RelacionarElementos: AccionDeMenuMnt
    {
        public string IdHtmlDeLaModalAsociada {get; private set;}
        public Func<string> RenderDeLaModal { get; private set; }
        public RelacionarElementos(string idHtmlDeLaModalAsociada, Func<string> renderDeLaModal)
        : base(TipoAccionMnt.CrearRelaciones)
        {
            IdHtmlDeLaModalAsociada = idHtmlDeLaModalAsociada;
            Parametros.Add(idHtmlDeLaModalAsociada);
            RenderDeLaModal = renderDeLaModal;
        }

        public override string RenderAccion()
        {
            return base.RenderAccion();
        }
    }

    /**********************************************************/
    // Acciones de menú de la modal o vista de creación
    // renderiza llamada Crud.EjecutarMenuCrt(...)
    /**********************************************************/
    public class AccionDeMenuCreacion : AccionDeMenu
    {
        public AccionDeMenuCreacion(string tipoDeAccionDeCreacion)
            : base(tipoDeAccionDeCreacion)
        {
        }

        public override string RenderAccion()
        {
            return $"Crud.EjecutarMenuCrt('{TipoDeAccion}')";
        }
    }

    public class NuevoElemento : AccionDeMenuCreacion
    {
        public NuevoElemento()
        : base(TipoAccionCreacion.NuevoElemento)
        {
        }
    }

    public class CerrarCreacion : AccionDeMenuCreacion
    {
        public CerrarCreacion()
        : base(TipoAccionCreacion.CerrarCreacion)
        {
        }
    }


    /**********************************************************/
    // Acciones de menú de la modal o vista de creación
    // renderiza llamada Crud.EjecutarMenuEdt(...)
    /**********************************************************/
    public class AccionDeMenuEdicion : AccionDeMenu
    {
        public AccionDeMenuEdicion(string tipoDeAccionDeEdicion)
        : base(tipoDeAccionDeEdicion)
        {
        }

        public override string RenderAccion()
        {
            return $"Crud.EjecutarMenuEdt('{TipoDeAccion}')";
        }
    }

    public class ModificarElemento : AccionDeMenuEdicion
    {
        public ModificarElemento()
        : base(TipoAccionEdicion.ModificarElemento)
        {
        }
    }
    public class CancelarEdicion : AccionDeMenuEdicion
    {
        public CancelarEdicion()
        : base(TipoAccionEdicion.CancelarEdicion)
        {
        }
    }


    /**********************************************************/
    // Definir una opción dentro de un menú. 
    // - la opción define que acción que ha de realizar
    // - renderiza un boton que al pulsarlo ejecuta la opción
    /**********************************************************/
    public class OpcionDeMenu<TElemento> : ControlHtml where TElemento : ElementoDto
    {
        public Menu<TElemento> Menu => (Menu<TElemento>)Padre;
        public AccionDeMenu Accion { get; private set; }
        public TipoDeLlamada TipoDeLLamada { get; private set; } = TipoDeLlamada.Get;

        public OpcionDeMenu(Menu<TElemento> menu, AccionDeMenu accion, string titulo)
        : this(menu, accion, TipoDeLlamada.Get, titulo)
        {
        }

        public OpcionDeMenu(Menu<TElemento> menu, AccionDeMenu accion, TipoDeLlamada tipoAccion, string titulo)
        : base(
          padre: menu,
          id: $"{menu.Id}_{TipoControl.Opcion}_{menu.OpcioneDeMenu.Count}",
          etiqueta: titulo,
          propiedad: null,
          ayuda: null,
          posicion: null
        )
        {
            Tipo = TipoControl.Opcion;
            TipoDeLLamada = tipoAccion;
            Accion = accion;
        }

        public override string RenderControl()
        {
            if (TipoDeLLamada == TipoDeLlamada.Post)
            {
                var htmlFormPost = $@"
                    <form id=¨{IdHtml}¨ action=¨{((AccionDeNavegarParaRelacionar)Accion).UrlDelCrudDeRelacion}¨ method=¨post¨ navegar-al-crud=¨{((AccionDeNavegarParaRelacionar)Accion).NavegarAlCrud}¨ restrictor=¨{IdHtml}-restrictor¨ orden=¨{IdHtml}-orden¨ style=¨display: inline-block;¨ >
                        <input id=¨{IdHtml}-restrictor¨ type=¨hidden¨ name =¨restrictor¨ >
                        <input id=¨{IdHtml}-orden¨ type=¨hidden¨ name = ¨orden¨ >
                        <input type=¨button¨ value=¨{Etiqueta}¨ onClick=¨{Accion.RenderAccion().Replace("idDeOpcMenu", IdHtml)}¨ />
                    </form>
                ";
                return htmlFormPost;
            }

            var htmlOpcionMenu = $"<input id=¨{IdHtml}¨ type=¨button¨ value=¨{Etiqueta}¨ onClick=¨{Accion.RenderAccion()}¨ />";
            return htmlOpcionMenu;
        }
    }
}
