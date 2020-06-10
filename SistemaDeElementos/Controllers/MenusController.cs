﻿using System;
using System.Collections.Generic;
using ServicioDeDatos;
using Gestor.Errores;
using Microsoft.AspNetCore.Mvc;
using MVCSistemaDeElementos.Descriptores;
using ServicioDeDatos.Entorno;
using Utilidades;
using Gestor.Elementos.Entorno;

namespace MVCSistemaDeElementos.Controllers
{
    public class MenusController : EntidadController<ContextoSe, MenuDtm, MenuDto>
    {
        public GestorDeMenus GestorDeMenus { get; set; }

        public MenusController(GestorDeMenus gestorDeMenus, GestorDeErrores gestorDeErrores)
        : base
        (
          gestorDeMenus,
          gestorDeErrores,
          new DescriptorDeMenu(ModoDescriptor.Mantenimiento)
        )
        {
            GestorDeMenus = gestorDeMenus;
        }

        public IActionResult CrudMenu(string orden)
        {
            GestorDelCrud.Descriptor.MapearElementosAlGrid(LeerOrdenados(orden), cantidadPorLeer: 5, posicionInicial: 0);
            GestorDelCrud.Descriptor.TotalEnBd(Contar());
            return ViewCrud();
        }


        protected override dynamic CargarLista(string claseElemento)
        {
            if (claseElemento == nameof(MenuDto))
                return ((GestorDeMenus)GestorDeElementos).LeerPadres();

            return null;
        }


        protected override dynamic CargaDinamica(string claseElemento, int posicion, int cantidad, string filtro)
        {


            if (claseElemento == nameof(VistaMvcDto))
                return ((GestorDeMenus)GestorDeElementos).LeerVistas(posicion, cantidad, filtro);

            return null;

    }

        //END-POINT: Desde Menu.ts
        public JsonResult epSolicitarMenuHtml(string usuario)
        {
            var r = new ResultadoHtml();
            try
            {
                var procesadas = new List<int>();
                List<ArbolDeMenuDto> menu = GestorDeMenus.LeerArbolDeMenu(GestorDeElementos.Mapeador);
                var menuHtml = @$"<ul id='id_menuraiz' class=¨menu-contenido¨>{Environment.NewLine}" +
                               @$"   {RenderOpcionesMenu(menu, procesadas, 0)}{Environment.NewLine}" +
                               @$"</ul>{Environment.NewLine}";
                r.Html = menuHtml.Replace("¨", "\"");
                r.Estado = EstadoPeticion.Ok;
            }
            catch (Exception e)
            {
                r.Estado = EstadoPeticion.Error;
                r.consola = GestorDeErrores.Concatenar(e);
                r.Mensaje = "No se ha podido leer el menú";
            }
            return new JsonResult(r);
        }

        private static string RenderOpcionesMenu(List<ArbolDeMenuDto> opcionesMenu, List<int> procesadas, int idMenuPadre)
        {
            var menuHtml = "";
            foreach (ArbolDeMenuDto fDto in opcionesMenu)
            {
                if (procesadas.Contains(fDto.Id))
                    continue;

                menuHtml = menuHtml + RenderMenu(funcion: fDto, procesadas, idMenuPadre);
                procesadas.Add(fDto.Id);
            }
            return menuHtml;
        }

        private static string RenderMenu(ArbolDeMenuDto funcion, List<int> procesadas, int idMenuPadre)
        {
            if (funcion.IdVistaMvc != null)
            {
                var opcionHtml = RenderAccionMenu(funcion.VistaMvc);
                return opcionHtml;
            }

            var subMenuHtml = funcion.Submenus != null ? RenderOpcionesMenu(funcion.Submenus, procesadas, funcion.Id) : "";

            var idMenuHtml = $"id_menu_{funcion.Id}";
            var idMenuPadreHtml = $"id_menu_{idMenuPadre}";
            var liHtml =
                $@"<li>{Environment.NewLine}" +
                $@"  <a>{Environment.NewLine}" +
                $@"     {ComponerMenu(literalOpcion: funcion.Nombre, icono: funcion.Icono, idMenu: idMenuHtml)}" +
                $@"  </a>{Environment.NewLine}" +
                $@"  <ul id=¨{idMenuHtml}¨ name=¨menu¨ menu-padre=¨{idMenuPadreHtml}¨ menu-plegado=¨true¨>{Environment.NewLine}" +
                      subMenuHtml +
                $@"  </ul>{Environment.NewLine}" +
                $@"</li>{Environment.NewLine}";

            return liHtml;
        }

        private static string RenderAccionMenu(VistaMvcDto accion)
        {
            var idHtml = $"{accion.Id}";
            var opcionHtml =
            $@"<li>{Environment.NewLine}" +
            $@"  <input id='{idHtml}' type='button' class='menu-opcion' value='{accion.Nombre}' onclick=¨ArbolDeMenu.OpcionSeleccionada('{idHtml}','{accion.Controlador}','{accion.Accion}')¨ />{Environment.NewLine}" +
            $@"</li>{Environment.NewLine}";

            return opcionHtml;
        }

        private static object ComponerMenu(string literalOpcion, string icono, string idMenu)
        {
            var opcionHtml = "";

            if (!icono.IsNullOrEmpty()) //&& File.Exists(@$"wwwroot\images\menu\{icono}"))
                opcionHtml = @$"<img src=¨/images/menu/{icono}¨ class=¨icono izquierdo¨ />{Environment.NewLine}";

            opcionHtml = $@"{opcionHtml}{literalOpcion}{Environment.NewLine}";

            if (!idMenu.IsNullOrEmpty())
                opcionHtml = $"{opcionHtml}<img src=¨/images/menu/angle-down-solid.svg¨ class=¨icono derecho¨ onclick=¨ArbolDeMenu.MenuPulsado('{idMenu}')¨/>{Environment.NewLine}";

            return opcionHtml;
        }
    }
}