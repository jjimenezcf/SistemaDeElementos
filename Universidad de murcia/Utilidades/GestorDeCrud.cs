﻿using Extensiones;
using System;
using System.Collections.Generic;
using System.Text;

namespace UniversidadDeMurcia.Utilidades
{

    public class ColumnaGrid
    {
        public string Nombre { get; set; }
        public bool Ordenar { get; set; }
        public string OrdenPor => $"ordenoPor{Nombre}";
        public string Sentido = "Asc";
    }

    public class BaseCrud<T>
    {
        protected string NombreDelObjeto => typeof(T).Name.Replace("Elemento","");
        private string _verbo;
        private string _accion;
        private string _formulario;

        public string Ruta { get; set; }
        public string Vista => $"{_verbo}{NombreDelObjeto}";
        public string Titulo { get; set; }
        public string Ir => $"Ira{_verbo}{NombreDelObjeto}";

        public string Accion => _accion ?? $"{_verbo}{NombreDelObjeto}";

        public string Formulario => _formulario ?? _verbo;

        public BaseCrud(string verbo)
        {
            _verbo = verbo;
        }

        public void AsignarTitulo(string titulo)
        {
            Titulo = titulo;
        }

        public void AsignarAccion(string accion)
        {
            _accion = accion;
        }

        public void AsignarFormulario(string formulario)
        {
            _formulario = formulario;
        }

    }

    public class MantenimientoCrud<T> : BaseCrud<T>
    {

        //public Func<IEnumerable<ColumnaGrid>> DefinirColumnasDelGrid { private get; set; }
        public IEnumerable<ColumnaGrid> ColumnasDelGrid { get;}

        public MantenimientoCrud(Func<IEnumerable<ColumnaGrid>> definirColumnasDelGrid)
        :base("Mantenimiento")
        {
            AsignarTitulo($"Mantenimiento de {NombreDelObjeto}s");
            ColumnasDelGrid = definirColumnasDelGrid == null ? renderVacio() : definirColumnasDelGrid();
        }

        private IEnumerable<ColumnaGrid> renderVacio()
        {
             return new List<ColumnaGrid>();
        }

        public string Render()
        {
            //IEnumerable<ColumnaGrid> columnasGrid = DefinirColumnasDelGrid();

            var htmlMantenimiento =
                RenderCabecera() +
                RenderFiltro() +
                RenderGrid(ColumnasDelGrid) +
                RenderPie();

            return htmlMantenimiento;
        }
        
        private string RenderCabecera()
        {
            var htmlCabecera = RenderOpcionesComunes();
            return htmlCabecera;
        }

        private string RenderOpcionesComunes()
        {
            var htmlOpcionesComunes = "";
            return htmlOpcionesComunes;
        }

        private string RenderFiltro()
        {
            var htmlFiltro = "";
            return htmlFiltro;
        }

        private string RenderGrid(IEnumerable<ColumnaGrid> columnasGrid)
        {
           var htmlGrid = 
                RenderCabeceraGrid(columnasGrid) +
                RenderDetalleGrid()+
                RenderNavegadorGrid() +
                RenderOpcionesGrid();

            return htmlGrid;
        }

        private string RenderCabeceraGrid(IEnumerable<ColumnaGrid> columnasGrid)
        {
            var htmlCabeceraGrid = @"
                                    <table class=¨table¨>
                                        <thead>
                                            <tr>
                                            renderColunasCabecera
                                            </tr>
                                        </thead>
                                    	renderizarCuerpo
                                    </table>                                    
                                   ";
            var htmlColumnaCabecera = @" <th>
                                           <a href=¨/ruta/accion?orden=ordenPor¨>Columna.Nombre</a>
                                         </th>
                                       ";
            var htmlColumnasCabecera = new StringBuilder();
            foreach (var columna in columnasGrid)
            {
                var html = htmlColumnaCabecera;
                if (columna.Ordenar)
                {
                    html = html.Replace("ruta", Ruta)
                        .Replace("accion", Ir)
                        .Replace("ordenPor", $"{columna.OrdenPor}{columna.Sentido}");
                }
                else
                {
                    html = html.Replace(" href=¨/ruta/accion?orden=ordenPor¨", "");
                }
                html=html.Replace("Columna.Nombre", columna.Nombre).Render();
                htmlColumnasCabecera.AppendLine(html);
            }
            
            return htmlCabeceraGrid.Replace("renderColunasCabecera",htmlColumnasCabecera.ToString()).Render();
        }

        private string RenderDetalleGrid()
        {
            var htmlDetalleGrid = "";
            return htmlDetalleGrid;
        }

        private string RenderNavegadorGrid()
        {
            var htmlNavegadorGrid = "";
            return htmlNavegadorGrid;
        }

        private string RenderOpcionesGrid()
        {
            var htmlOpcionesGrid = "";
            return htmlOpcionesGrid;
        }

        private string RenderPie()
        {
            var htmlPie = "";
            return htmlPie;
        }

    }

    public class CreacionCrud<T> : BaseCrud<T>
    {
        public CreacionCrud() :
        base("Crear")
        {
            AsignarTitulo($"Creación de {NombreDelObjeto}");
        }
    }


    public class EdicionCrud<T> : BaseCrud<T>
    {
        public EdicionCrud() :
        base("Editar")
        {
            AsignarTitulo($"Edición de {NombreDelObjeto}");
            AsignarAccion($"Modificar{NombreDelObjeto}");
            AsignarFormulario("Modificar");
        }
    }


    public class DetalleCrud<T> : BaseCrud<T>
    {
        public string TituloDetalle { get; private set; }
        public DetalleCrud() :
        base("Detalle")
        {
            AsignarTitulo($"Detalle de {NombreDelObjeto}");
        }

        public void AsignarTituloDetalle(string titulo)
        {
            TituloDetalle = titulo;
        }
    }

    public class BorradoCrud<T> : BaseCrud<T>
    {
        public BorradoCrud() :
        base("Borrar")
        {
            AsignarTitulo($"Borrado de {NombreDelObjeto}");
        }
    }

    public class GestorCrud<T>
    {

        private string _ruta;

        public string NombreDelObjeto => typeof(T).Name;
        public string Ruta
        {
            get { return _ruta ?? $"{NombreDelObjeto.Replace("Elemento","")}s"; }
            set { _ruta = value; }
        }
        public string Titulo { get; set; }
        public Dictionary<string, SelectorModal> Modales = new Dictionary<string, SelectorModal>();

        public MantenimientoCrud<T> Mantenimiento { get; }
        public CreacionCrud<T> Creador { get; }
        public EdicionCrud<T> Editor { get; }
        public DetalleCrud<T> Detalle { get; }
        public BorradoCrud<T> Supresor { get; }

        public GestorCrud(Func<IEnumerable<ColumnaGrid>> definirColumnasDelGrid)
        {
            Titulo = $"Gestor de {NombreDelObjeto}";
            Mantenimiento = new MantenimientoCrud<T>(definirColumnasDelGrid) { Ruta = Ruta };
            Creador = new CreacionCrud<T>();
            Editor = new EdicionCrud<T>();
            Detalle = new DetalleCrud<T>();
            Supresor = new BorradoCrud<T>();
        }
    }
}
