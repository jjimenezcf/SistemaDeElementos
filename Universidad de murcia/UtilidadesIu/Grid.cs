﻿using System;
using System.Text;
using System.Collections.Generic;

namespace UtilidadesParaIu
{
    public class Grid
    {
        public string Id { get; private set; }
        public string Ruta { get; set; }
        public List<ColumnaDelGrid> columnas { get; private set; }
        public List<FilaDelGrid> filas { get; private set; }

        public bool ConSeleccion { get; set; } = true;
        public bool ConNavegador { get; set; } = true;

        public Grid(string idGrid, Func<List<ColumnaDelGrid>> definirColumnasGrid, Func<List<ColumnaDelGrid>, List<FilaDelGrid>> obtenerFilasDelGrid)
        {
            columnas = definirColumnasGrid();
            filas = obtenerFilasDelGrid(columnas);
            IniciarClase(idGrid, columnas, filas);
        }

        public Grid(string idGrid, List<ColumnaDelGrid> columnasGrid, List<FilaDelGrid> filasDelGrid)
        {
            IniciarClase(idGrid, columnasGrid, filasDelGrid);
        }

        private void IniciarClase(string idGrid, List<ColumnaDelGrid> columnasGrid, List<FilaDelGrid> filasDelGrid)
        {
            Id = idGrid;
            columnas = columnasGrid;
            filas = filasDelGrid;
        }

        public string ToHtml()
        {
            return RenderizarGrid(this).Render();
        }

        private static string RenderCeldaCheck(string idGrid, string idCelda)
        {
            var check = $"<input type=¨checkbox¨ id=¨{idGrid}_{idCelda}¨ name=¨chx_{idGrid}¨ class=¨text-center¨ aria-label=¨Marcar para seleccionar¨>";
            var celdaDelCheck = $@"<td>{Environment.NewLine}{check}{Environment.NewLine}</td>";
            return celdaDelCheck;
        }

        private static string RenderColumnaCabecera(ColumnaDelGrid columna)
        {
            var visible = columna.Visible ? "" : "hidden";
            var ancho = columna.Ancho == 0 ? "" : $"width: {columna.Ancho}%;";
            var estilo = visible + ancho == "" ? "" : $"{ancho} {visible}";
            return $"{Environment.NewLine}<th scope=¨col¨ id= ¨{columna.Id}¨ class=¨{columna.AlineacionCss()}¨ {estilo}>{columna.Titulo}</th>";
            //<a href=¨/ruta/accion?orden=ordenPor¨>
            //if (columna.Ordenar)
            //{
            //    html = html.Replace("ruta", columna.Ruta)
            //        .Replace("accion", columna.Accion)
            //        .Replace("ordenPor", $"{columna.OrdenPor}{columna.Sentido}");
            //}
            //else
            //{
            //    html = html.Replace(" href=¨/ruta/accion?orden=ordenPor¨", "");
            //}

            //{ title: ¨Name¨, field: ¨name¨, sorter: ¨string¨, width: 200, editor: true },
        }


        private static string RenderCelda(CeldaDelGrid celda)
        {
            var ocultar = celda.Visible ? "" : "hidden";
            return $"<td id=¨{celda.Id}¨ name=¨{celda.IdCabecera}¨ class=¨{celda.AlineacionCss()}¨ {ocultar}>{celda.Valor}</td>";
        }

        private static string RenderFila(int numFil, FilaDelGrid fila)
        {
            var filaHtml = new StringBuilder();
            foreach (var celda in fila.Celdas)
            {
                celda.Id = $"{celda.IdCabecera}_{numFil}";
                filaHtml.AppendLine(RenderCelda(celda));
            }
            return $@"{filaHtml.ToString()}";
        }

        private static string RenderFilaSeleccionable(string idGrid, int numFil, FilaDelGrid filaDelGrid)
        {
            string filaHtml = RenderFila(numFil, filaDelGrid);
            string celdaDelCheck = RenderCeldaCheck($"{idGrid}", $"chk_{numFil}");
            return $"<tr>{Environment.NewLine}{filaHtml}{celdaDelCheck}{Environment.NewLine}</tr>{Environment.NewLine}";
        }

        private static string RenderCabecera(string idGrid, IEnumerable<ColumnaDelGrid> columnasGrid)
        {
            var cabeceraHtml = new StringBuilder();
            foreach (var columna in columnasGrid)
            {
                columna.Id = $"{idGrid}_{columna.Nombre}";
                cabeceraHtml.Append(RenderColumnaCabecera(columna));
            }
            string celdaDelCheck = RenderCeldaCheck($"{idGrid}", $"chx");
            cabeceraHtml.Append(celdaDelCheck);
            return $@"<thead>{Environment.NewLine}<tr>{cabeceraHtml.ToString()}{Environment.NewLine}</tr>{Environment.NewLine}</thead>";
        }


        private static string RenderDetalleGrid(string idGrid, IEnumerable<FilaDelGrid> filas)
        {
            var htmlDetalleGrid = new StringBuilder();
            int i = 0;
            foreach (var fila in filas)
            {
                htmlDetalleGrid.Append(RenderFilaSeleccionable(idGrid, i, fila));
                i = i + 1;
            }
            return htmlDetalleGrid.ToString();
        }

        private static string RenderNavegadorGrid(Grid grid)
        {
            var htmlNavegadorGrid = $@"
            <div id=¨Nav-{grid.Id}¨>
                <div id=¨Nav-{grid.Id}-1¨ data-type=¨img¨ style=¨display:inline-block¨>
                    <img src=¨/images/paginaInicial.png¨ alt=¨Primera página¨ title=¨Ir al primer registro¨ width=¨22¨ height=¨22¨ onclick=¨Leer('{grid.Id}','{grid.Ruta}')¨>
                </div>
                <div id=¨Nav-{grid.Id}-2¨ class=¨mx-sm-3¨ style=¨display:inline-block¨>
                    <input type=¨number¨ id=¨Nav-{grid.Id}-Reg¨ value=¨10¨ min=¨5¨ step=¨5¨ max=¨999¨ style=¨width: 50px;margin-top: 5px;align-content:center; border-radius: 10px¨>
                </div>
                <div id=¨Nav-{grid.Id}-3¨ data-type=¨img¨ style=¨display:inline-block¨>
                    <img src=¨/images/paginaAnterior.png¨ alt=¨Primera página¨ title=¨Ir al primer registro¨ width=¨22¨ height=¨22¨ onclick=¨LeerAnteriores('{grid.Id}','{grid.Ruta}')¨>
                    <img src=¨/images/paginaSiguiente.png¨ alt=¨Siguiente página¨ title=¨Ir al primer registro¨ width=¨22¨ height=¨22¨ onclick=¨LeerSiguientes('{grid.Id}','{grid.Ruta}')¨>
                    <img src=¨/images/paginaUltima.png¨ alt=¨Última página¨ title=¨Ir al primer registro¨ width=¨22¨ height=¨22¨ onclick=¨LeerUltimos('{grid.Id}','{grid.Ruta}')¨>
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

        private static string RenderizarGrid(Grid grid)
        {
            var htmlTabla = $"<table id=¨{grid.Id}¨ class=¨table table-striped table-hover¨ width=¨100%¨>{Environment.NewLine}{RenderCabecera(grid.Id, grid.columnas)}{Environment.NewLine}{RenderDetalleGrid(grid.Id, grid.filas)}</table>";
            var htmlNavegador = grid.ConNavegador ? RenderNavegadorGrid(grid) : "";
            return (htmlTabla + htmlNavegador + RenderOpcionesGrid());
        }

    }
}