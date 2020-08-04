﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Text.Json;
using MVCSistemaDeElementos.Descriptores;
using Gestor.Elementos.ModeloIu;
using Utilidades;

namespace UtilidadesParaIu
{
    public class Grid<TElemento> where TElemento : ElementoDto
    {
        public string Id { get; private set; }

        public ZonaDeDatos<TElemento> ZonaDeDatos { get; set; }

        public string IdHtml => Id.ToLower();

        public string IdHtmlCabecera => $"{IdHtml}_c_tr_0";

        public string IdHtmlTabla => $"{IdHtml}_table";
        public string IdHtmlNavegador => $"{IdHtml}_nav";
        public string IdHtmlNavegador_1 => $"{IdHtmlNavegador}_1";
        public string IdHtmlNavegador_2 => $"{IdHtmlNavegador}_2";
        public string IdHtmlNavegador_3 => $"{IdHtmlNavegador}_3";
        public string IdHtmlPorLeer => $"{IdHtmlNavegador_2}_reg";

        public string Controlador => ZonaDeDatos.Mnt.Crud.Controlador;
        public List<ColumnaDelGrid<TElemento>> columnas { get; private set; } = new List<ColumnaDelGrid<TElemento>>();
        public List<FilaDelGrid<TElemento>> filas { get; private set; } = new List<FilaDelGrid<TElemento>>();

        public int NumeroDeFilas => filas.Count;

        public int TotalEnBd => ZonaDeDatos.TotalEnBd;
        private int PosicionInicial => ZonaDeDatos.PosicionInicial;
        private int CantidadPorLeer => ZonaDeDatos.CantidadPorLeer;
        public int Seleccionables { get; set; }
        public int Ultimo_Leido => PosicionInicial + filas.Count;

        public bool ConNavegador { get; set; } = true;

        public Grid(ZonaDeDatos<TElemento> zonaDeDatos)
        {
            ZonaDeDatos = zonaDeDatos;
            Id = ZonaDeDatos.Id;
            Seleccionables = 2;
        }

        public FilaDelGrid<TElemento> ObtenerFila(int i)
        {
            return filas[i];
        }


        public string ToHtml()
        {
            ZonaDeDatos.CalcularAnchosColumnas();
            return RenderizarGrid(this).Render();
        }

        private static string RenderColumnaCabecera(ColumnaDelGrid<TElemento> columna)
        {
            var porcentaje = columna.ZonaDeDatos.Mnt.Crud.Modo == ModoDescriptor.Seleccion
                ? columna.PorAnchoSel
                : columna.PorAnchoMnt;
            var atributosDelEstilo = $"text-align: {columna.AlineacionCss}";
            if (columna.Visible)
                atributosDelEstilo = $" width: {porcentaje}%; {atributosDelEstilo}";
            string htmlRef = columna.Ordenar ? RenderAccionOrdenar(columna) : columna.Visible ? columna.Titulo : "";

            string claseCss = columna.Visible ? "columna-cabecera" : "columna-cabecera-oculta";

            var htmlTh = $@"{Environment.NewLine}
                          <th id = ¨{columna.IdHtml}¨ 
                              class=¨{claseCss}¨ 
                              propiedad = ¨{columna.Propiedad.ToLower()}¨
                              modo-ordenacion=¨sin-orden¨ 
                              style = ¨{atributosDelEstilo}¨
                              alineacion=¨{columna.AlineacionCss}¨
                              >
                              {htmlRef}
                          </th>";
            return htmlTh;
        }

        private static string RenderAccionOrdenar(ColumnaDelGrid<TElemento> columna)
        {
            string htmlRef = columna.ZonaDeDatos.IdHtmlModal.IsNullOrEmpty()
                ? $"href =¨javascript: Crud.EventosDelMantenimiento('ordenar-por', '{columna.IdHtml}')¨"
                : $"href=¨javascript:Crud.EventosModalDeSeleccion('ordenar-por','{columna.ZonaDeDatos.IdHtmlModal}#{columna.IdHtml}')¨";

            var estilo = columna.Visible ? "" : @"visibility: hidden; style=¨width: 0px; height: 0px; float: right;¨";

            return $@"<a {htmlRef} class=¨ordenada-sin-orden¨ {estilo}>{columna.Titulo}</a>";
        }

        private static string RenderEventoPuslsa(CeldaDelGrid<TElemento> celda, string idControlHtml)
        {
            return celda.Fila.Datos.Mnt.Crud.Modo == ModoDescriptor.Seleccion
               ? $"Crud.EventosModalDeSeleccion('fila-pulsada', '{celda.Fila.Datos.IdHtmlModal}#{celda.Fila.idHtmlCheckDeSeleccion}#{idControlHtml}');"
               : $"Crud.EventosDelMantenimiento('fila-pulsada', '{celda.Fila.idHtmlCheckDeSeleccion}#{idControlHtml}');";
        }


        private static string RenderTd(CeldaDelGrid<TElemento> celda)
        {

            var nombreTd = $"td.{celda.Propiedad}.{celda.Fila.Datos.IdHtml}".ToLower();
            string pulsarCheck = RenderEventoPuslsa(celda, celda.idHtmlTd);

            var onclickTd = $"onclick=¨{pulsarCheck}¨";
            var ocultar = celda.Visible ? "" : "hidden";

            var tdHtml = $@"<td id=¨{celda.idHtmlTd}¨ 
                                name=¨{nombreTd}¨ 
                                style=¨text-align: {celda.AlineacionCss()};¨ 
                                propiedad=¨{celda.Propiedad}¨ 
                                {onclickTd} 
                                {ocultar} >
                                {RenderCeldaDelTd(celda)}
                           </td>";
            return tdHtml;
        }

        private static string RenderCeldaDelTd(CeldaDelGrid<TElemento> celda)
        {
            var idDelInput = $"{celda.idHtml}";
            string pulsarCheck = RenderEventoPuslsa(celda, idDelInput);

            var tipoHtml = celda.Tipo == typeof(bool) ? "type =¨checkbox¨" : "type =¨text¨";
            var onclick = celda.Tipo == typeof(bool)
                  ? $"onclick=¨{pulsarCheck}¨"
                  : "";


            var editable = !celda.Editable ? "readonly" : "";

            var nombreInput = $"{celda.Propiedad}.{celda.Fila.Datos.IdHtml}".ToLower();

            var input = $" <input {tipoHtml} id=¨{idDelInput}¨ " +
            $"        name=¨{nombreInput}¨ " +
            $"        style=¨width:100%; border:0; text-align: {celda.AlineacionCss()};¨ " +
            $"        propiedad=¨{celda.Propiedad}¨ " +
            $"        style=¨width:100%; border:0¨ " +
            $"        {editable} " +
            $"        {onclick} " +
            $"        value=¨{celda.Valor}¨ />";

            return input;
        }


        private static string RenderFila(FilaDelGrid<TElemento> fila)
        {
            var filaHtml = new StringBuilder();
            var numCol = 0;
            for (var j = 0; j < fila.NumeroDeCeldas; j++)
            {
                var celda = fila.ObtenerCelda(j);
                filaHtml.AppendLine(RenderTd(celda));
                numCol++;
            }
            return $@"{filaHtml}";
        }

        private static string RenderFilaSeleccionable(FilaDelGrid<TElemento> fila)
        {
            string celdaDelCheck = ""; // RenderCeldaCheck(fila.Datos.IdHtml, fila.IdHtml, fila.NumeroDeCeldas);
            string filaHtml = RenderFila(fila);

            return $"<tr id='{fila.IdHtml}'>{Environment.NewLine}" +
                   $"   {celdaDelCheck}{filaHtml}{Environment.NewLine}" +
                   $"</tr>{Environment.NewLine}";
        }

        private static string RenderCabecera(Grid<TElemento> grid)
        {
            var cabeceraHtml = new StringBuilder();
            foreach (var columna in grid.columnas)
            {
                cabeceraHtml.Append(RenderColumnaCabecera(columna));
            }

            return $@"<thead id='{grid.IdHtml}_cabecera'>{Environment.NewLine}
                         <tr id=¨{grid.IdHtmlCabecera}¨>
                            {cabeceraHtml}{Environment.NewLine}
                         </tr>{Environment.NewLine}
                      </thead>";

        }

        private static string RenderDetalleGrid(Grid<TElemento> grid)
        {
            return "";

            var htmlDetalleGrid = new StringBuilder();
            for (var i = 0; i < grid.NumeroDeFilas; i++)
            {
                var fila = grid.ObtenerFila(i);
                htmlDetalleGrid.Append(RenderFilaSeleccionable(fila));
            }
            return $@"<tbody id='{grid.IdHtml}_detalle'>
                         {htmlDetalleGrid}
                      </tbody>";
        }

        private static string RenderNavegadorGrid(Grid<TElemento> grid)
        {
            var accionSiguiente = grid.ZonaDeDatos.Mnt.Crud.Modo == ModoDescriptor.Seleccion
                ? $"Crud.EventosModalDeSeleccion('obtener-siguientes','{grid.ZonaDeDatos.IdHtmlModal}')"
                : $"Crud.EventosDelMantenimiento('obtener-siguientes')";

            var accionBuscar = grid.ZonaDeDatos.Mnt.Crud.Modo == ModoDescriptor.Seleccion
                ? $"Crud.EventosModalDeSeleccion('buscar-elementos','{grid.ZonaDeDatos.IdHtmlModal}')"
                : $"Crud.EventosDelMantenimiento('buscar-elementos')";

            var accionAnterior = grid.ZonaDeDatos.Mnt.Crud.Modo == ModoDescriptor.Seleccion
                ? $"Crud.EventosModalDeSeleccion('obtener-anteriores','{grid.ZonaDeDatos.IdHtmlModal}')"
                : $"Crud.EventosDelMantenimiento('obtener-anteriores')";

            var accionUltimos = grid.ZonaDeDatos.Mnt.Crud.Modo == ModoDescriptor.Seleccion
                ? $"Crud.EventosModalDeSeleccion('obtener-ultimos','{grid.ZonaDeDatos.IdHtmlModal}')"
                : $"Crud.EventosDelMantenimiento('obtener-ultimos')";

            var htmlNavegadorGrid = $@"
            <div id= ¨{grid.IdHtml}_pie¨ class=¨pie-grid¨>
                <div id=¨{grid.IdHtmlNavegador}¨ class = ¨navegador-grid¨>
                    <div id=¨{grid.IdHtmlNavegador_1}¨ data-type=¨img¨>
                        <img src=¨/images/paginaInicial.png¨ alt=¨Primera página¨ title=¨Ir al primer registro¨ onclick=¨{accionBuscar}¨>
                    </div>
                    <div id=¨{grid.IdHtmlNavegador_2}¨>
                        <input type=¨number¨ 
                               id=¨{grid.IdHtmlPorLeer}¨ 
                               class = ¨cantidad-grid¨
                               value=¨{grid.CantidadPorLeer}¨ 
                               min=¨1¨ step=¨1¨ max=¨999¨ 
                               pagina=¨1¨  
                               posicion=¨{grid.Ultimo_Leido}¨  
                               controlador=¨{grid.Controlador}¨  
                               total-en-bd=¨{grid.TotalEnBd}¨ 
                               title=¨Pagina: 1 de un total de {Math.Ceiling((decimal)(grid.TotalEnBd/grid.CantidadPorLeer))}¨ />
                    </div>
                    <div id=¨id=¨{grid.IdHtmlNavegador_3}¨ data-type=¨img¨ >
                        <img src=¨/images/paginaAnterior.png¨ alt=¨Primera página¨ title=¨Página anterior¨ onclick=¨{accionAnterior}¨>
                        <img src=¨/images/paginaSiguiente.png¨ alt=¨Siguiente página¨ title=¨Página siguiente¨ onclick=¨{accionSiguiente}¨>
                        <img src=¨/images/paginaUltima.png¨ alt=¨Última página¨ title=¨Última página¨ onclick=¨{accionUltimos}¨>
                    </div>
                </div>
                <div id= ¨{grid.IdHtml}_info¨ class=¨info-grid¨>
                   ¨Pagina: 1 de un total de {Math.Ceiling((decimal)(grid.TotalEnBd / grid.CantidadPorLeer))}¨
                </div>
            </div>
            ";
            return htmlNavegadorGrid;
        }

        private static string RenderOpcionesGrid()
        {
            var htmlOpcionesGrid = "";
            return htmlOpcionesGrid;
        }

        private static string RenderizarGrid(Grid<TElemento> grid)
        {
            var htmlTabla = $@"<div class=¨div-grid¨> 
                                  <table id=¨{grid.IdHtmlTabla}¨ class=¨tabla-grid¨ >
                                    {RenderCabecera(grid)}
                                    {RenderDetalleGrid(grid)}
                                  </table>
                               </div> ";
            var htmlNavegador = grid.ConNavegador ? RenderNavegadorGrid(grid) : "";
            return (htmlTabla + htmlNavegador + RenderOpcionesGrid());
        }


    }
}