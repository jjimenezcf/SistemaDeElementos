﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gestor.Elementos.ModeloIu;
using UniversidadDeMurcia.Descriptores;
using Utilidades;
using UtilidadesParaIu;

namespace UniversidadDeMurcia.Descriptores
{
    public enum TipoControl { Selector, Editor, Label, Referencia, Desplegable, Lista, Fecha, GridModal, TablaBloque, Bloque }


    public class Posicion
    {
        public int fila { get; set; }
        public int columna { get; set; }
    }

    public class Dimension
    {
        public int Filas { get; private set; }
        public int Columnas { get; private set; }

        public Dimension(int filas, int columnas)
        {
            Filas = filas;
            Columnas = columnas;
        }
    }

    public class ControlHtml
    {
        public string Id { get; private set; }
        public string IdHtml => Id.ToLower();
        public string Etiqueta { get; private set; }
        public string Propiedad { get; private set; }
        public string Ayuda { get; private set; }
        public Posicion Posicion { get; private set; }

        public TipoControl Tipo { get; protected set; }

        public ControlHtml Padre { get; set; }

        public ControlHtml(ControlHtml padre, string id, string etiqueta, string propiedad, string ayuda, Posicion posicion)
        {
            Padre = padre;
            Id = id;
            Etiqueta = etiqueta;
            Propiedad = propiedad;
            Ayuda = ayuda;
            Posicion = posicion;
        }

        public string RenderLabel()
        {
            return $@"<div class=¨input-group mb-3¨>
                         {Etiqueta}
                      </div>
                  ";
        }

        public virtual string RenderControl()
        {
            if (Tipo != TipoControl.Selector && Tipo != TipoControl.Editor && Tipo != TipoControl.GridModal)
                throw new Exception($"El tipo {this.Tipo} de control no está definido");
            return "htmlControl";
        }

    }

    public class Selector<Tseleccionado> : ControlHtml
    {
        public string propiedadParaFiltrar { get; private set; }
        public string propiedadParaMostrar { get; private set; }
        public GridModal<Tseleccionado> GridModal { get; set; }

        public Selector(Bloque padre, string etiqueta, string propiedad, string ayuda, Posicion posicion, string paraFiltrar, string paraMostrar)
        : base(
              padre: padre
              , id: $"{typeof(Tseleccionado).Name.Replace("Elemento", "")}_{TipoControl.Selector}"
              , etiqueta
              , propiedad
              , ayuda
              , posicion
              )
        {
            Tipo = TipoControl.Selector;
            propiedadParaFiltrar = paraFiltrar.ToLower();
            propiedadParaMostrar = paraMostrar.ToLower();
            GridModal = new GridModal<Tseleccionado>(padre, this);
            padre.AnadirSelector(this);
        }

        public override string RenderControl()
        {
            return base.RenderControl().Replace("htmlControl", RenderSelector());
        }

        public string RenderSelector()
        {
            return $@"<div class=¨input-group mb-3¨>
                       <input id=¨{IdHtml}¨ type = ¨text¨ class=¨form-control¨ placeholder=¨{Ayuda}¨>
                       <div class=¨input-group-append¨>
                            <button class=¨btn btn-outline-secondary¨ type=¨button¨ data-toggle=¨modal¨ data-target=¨#{GridModal.IdHtml}¨ >Seleccionar</button>
                       </div>
                    </div>
                  ";
        }
    }

    public class Editor : ControlHtml
    {
        public Editor(Bloque padre, string id, string etiqueta, string propiedad, string ayuda, Posicion posicion)
        : base(padre: padre
              , id: $"edt_{id}"
              , etiqueta
              , propiedad
              , ayuda
              , posicion
              )
        {
            Tipo = TipoControl.Editor;
            padre.AnadirControl(this);
        }

        public override string RenderControl()
        {
            return base.RenderControl().Replace("htmlControl", RenderInput());
        }

        public string RenderInput()
        {
            return $@"<div class=¨input-group mb-3¨>
                         <input id=¨{IdHtml}¨ type = ¨text¨ class=¨form-control¨ placeholder=¨{Ayuda}¨>
                      </div>
                  ";
        }
    }

    public class Desplegable : ControlHtml
    {
        public ICollection<Valor> valores { get; set; }

        public Desplegable(ControlHtml padre, string id, string etiqueta, string propiedad, string ayuda, Posicion posicion)
        : base(padre: padre
              , id: $"ddl_{id}"
              , etiqueta
              , propiedad
              , ayuda
              , posicion
              )
        {
            Tipo = TipoControl.Desplegable;
        }
    }

    public class GridModal<Tseleccionado> : ControlHtml
    {
        public Selector<Tseleccionado> Selector { get; set; }
        public string gestorDeElementos { get; set; }
        public string claseDeElemento { get; set; }
        public List<ColumnaDelGrid> Columnas { get; set; }
        public string Registros { get; set; }

        public GridModal(ControlHtml padre, Selector<Tseleccionado> selectorAsociado)
        : base(
          padre: padre,
          id: selectorAsociado.Id.Replace(TipoControl.Selector.ToString(), TipoControl.GridModal.ToString()),
          etiqueta: $"Seleccionar {selectorAsociado.propiedadParaMostrar}",
          propiedad: selectorAsociado.propiedadParaMostrar,
          ayuda: selectorAsociado.Ayuda,
          posicion: null
        )
        {
            Tipo = TipoControl.GridModal;
            Selector = selectorAsociado;
            Selector.GridModal = this;
        }

        public override string RenderControl()
        {
            return base.RenderControl().Replace("htmlControl", RenderGridModal());
        }

        private string RenderGridModal()
        {
            var s = Selector;

            const string _htmlModalSelector =
            @"
             <div class=¨modal fade¨ id=¨idModal¨ tabindex=¨-1¨ role=¨dialog¨ aria-labelledby=¨exampleModalLabel¨ aria-hidden=¨true¨>
               <div class=¨modal-dialog¨ role=¨document¨>
                 <div class=¨modal-content¨>
                   <div class=¨modal-header¨>
                     <h5 class=¨modal-title¨ id=¨exampleModalLabel¨>titulo</h5>
                   </div>
                   <div id=¨{idContenedor}¨ class=¨modal-body¨>
                     {gridDeElementos}
                   </div>
                   <div class=¨modal-footer¨>
                     <button type = ¨button¨ class=¨btn btn-secondary¨ data-dismiss=¨modal¨>Cerrar</button>
                     <button type = ¨button¨ class=¨btn btn-primary¨ data-dismiss=¨modal¨ onclick=¨AlSeleccionar('{idSelector}', '{idGrid}', '{referenciaChecks}')¨>Seleccionar</button>
                   </div>
                 </div>
               </div>
             </div>
             <script>
               AlAbrirLaModal
               AlCerrarLaModal
             </script>
             ";


            var nombreCheckDeSeleccion = $"chksel.{s.IdHtml}";

            return _htmlModalSelector
                    .Replace("idModal", s.GridModal.IdHtml)
                    .Replace("titulo", s.GridModal.Ayuda)
                    .Replace("{idSelector}", s.IdHtml)
                    //.Replace("{idGrid}", IdGrid)
                    .Replace("{referenciaChecks}", $"{nombreCheckDeSeleccion}")
                    .Replace("{columnaId}", s.propiedadParaFiltrar)
                    .Replace("{columnaMostrar}", s.propiedadParaMostrar)
                    .Replace("{idContenedor}", $"contenedor.{s.GridModal.IdHtml}")
                    .Replace("{gridDeElementos}", "")
                    .Replace("AlAbrirLaModal", "")
                    .Replace("AlCerrarLaModal", "")
                    .Render();
        }
    }

    public class TablaBloque : ControlHtml
    {
        public Dimension Dimension { get; private set; }
        public ICollection<ControlHtml> Controles { get; set; }

        public TablaBloque(ControlHtml padre, string identificador, Dimension dimension, ICollection<ControlHtml> controles)
        : base(
          padre: padre,
          id: $"tbl_{identificador}",
          etiqueta: null,
          propiedad: null,
          ayuda: null,
          posicion: null
        )
        {
            Tipo = TipoControl.TablaBloque;
            Dimension = dimension;
            Controles = controles;
        }

        public string RenderTabla()
        {

            var htmlTabla = $@"<table id=¨{IdHtml}¨ width=¨100%¨>
                                  filas
                               </table>";
            var htmlFilas = "";
            for (var i = 0; i < Dimension.Filas; i++)
                htmlFilas = $"{htmlFilas}{(htmlFilas.IsNullOrEmpty() ? "" : Environment.NewLine)}{RenderFila(i)}";

            return htmlTabla.Replace("filas", htmlFilas);
        }

        private string RenderFila(int i)
        {
            var idFila = $"{IdHtml}_{i}";
            var htmlFila = $@"<tr id=¨{idFila}¨>
                                 columnas
                              </tr>";
            var htmlColumnas = "";
            for (var j = 0; j < Dimension.Columnas; j++)
                htmlColumnas = $"{htmlColumnas}{(htmlColumnas.IsNullOrEmpty() ? "" : Environment.NewLine)}{RenderColumnasControl(idFila, i, j)}";


            return htmlFila.Replace("columnas", htmlColumnas);
        }

        private string RenderColumnasControl(string idFila, int i, int j)
        {
            var idColumna = $"{idFila}_{j}";
            var htmlColumnaEtiqueta = $@"<td id=¨{idColumna}_e¨ style=¨width:15%¨>
                                            etiqueta
                                         </td>";
            var htmlColumnaControl = $@"<td id=¨{idColumna}_c¨ style=¨width:35%¨>
                                           control
                                        </td>";
            var htmlControl = "";
            var htmlEtiqueta = "";
            foreach (ControlHtml c in Controles)
            {
                if (c.Posicion == null)
                    continue;

                if (c.Posicion.fila == i && c.Posicion.columna == j)
                    htmlEtiqueta = $"{c.RenderLabel()}";

                if (c.Posicion.fila == i && c.Posicion.columna == j)
                    htmlControl = $"{c.RenderControl()}";
            }


            return htmlColumnaEtiqueta.Replace("etiqueta", htmlEtiqueta) +
                   Environment.NewLine +
                   htmlColumnaControl.Replace("control", htmlControl);
        }

    }

    public class Bloque : ControlHtml
    {
        public TablaBloque Tabla { get; set; }

        public ICollection<ControlHtml> Controles => Tabla.Controles;


        public Bloque(ZonaDeFiltro padre, string titulo, Dimension dimension)
        : base(
          padre: padre,
          id: $"blo_{padre.Id}_{padre.Bloques.Count}",
          etiqueta: titulo,
          propiedad: null,
          ayuda: null,
          posicion: null
        )
        {
            Tipo = TipoControl.Bloque;
            Tabla = new TablaBloque(this, $"{Id}", dimension, new List<ControlHtml>());
            padre.Bloques.Add(this);
        }


        public void AnadirControl(ControlHtml c)
        {
            Controles.Add(c);
        }

        public void AnadirSelector<T>(Selector<T> s)
        {
            Controles.Add(s);
            Controles.Add(s.GridModal);
        }
        public ControlHtml ObtenerControl(string id)
        {

            foreach (ControlHtml c in Controles)
            {
                if (c.Id == id)
                    return c;
            }

            throw new Exception($"El control {id} no está en la zona de filtrado");
        }

        public string RenderBloque()
        {
            string htmlBloque = $@"<div id = ¨{IdHtml}¨>     
                                     tabla 
                                    </div>";
            string htmlTabla = Tabla.RenderTabla();

            return htmlBloque.Replace("tabla", htmlTabla);
        }

        public string RenderModalesBloque()
        {
            var htmlModalesEnBloque = "";
            foreach (ControlHtml c in Controles)
            {
                if (c.Tipo == TipoControl.GridModal)
                    htmlModalesEnBloque =
                        $"{htmlModalesEnBloque}{(htmlModalesEnBloque.IsNullOrEmpty() ? "" : Environment.NewLine)}" +
                        $"{c.RenderControl()}";

            }
            return htmlModalesEnBloque;
        }
    }

    public class ZonaDeOpciones<Telemento> : ControlHtml
    {
        public ICollection<Opcion<Telemento>> Opciones { get; private set; } = new List<Opcion<Telemento>>();
        
        public ZonaDeOpciones(DescriptorDeCrud<Telemento> padre, VistaCrud vista)
        : base(
          padre: padre,
          id: $"Men_{padre.Id}",
          etiqueta: null,
          propiedad: null,
          ayuda: null,
          posicion: null
        )
        {
           new Opcion<Telemento>(this, vista.Ruta, vista.Accion, vista.Etiqueta);
        }

        public string RenderOpcionesMenu()
        {
            var htmlRef = "<div id=¨{idOpc}¨>{newLine}<a href =¨/{ruta}/{accion}¨>{titulo}</a>{newLine}</div>";
            var htmlMenu = "<div id=¨{idMenu}¨>{hmlOpciones}</div>";
            var htmlOpciones = "";
            foreach (Opcion<Telemento> o in Opciones)
            {
                htmlOpciones = htmlOpciones + htmlRef
                                             .Replace("{idOpc}", o.Id)
                                             .Replace("{ruta}", o.Ruta)
                                             .Replace("{accion}", o.Accion)
                                             .Replace("{titulo}", o.Etiqueta)
                                             .Replace("{newLine}", Environment.NewLine) + 
                                             Environment.NewLine;
            }

            return htmlMenu.Replace("{idMenu}", Id).Replace("{hmlOpciones}",$"{Environment.NewLine}{htmlOpciones}");
        }

    }

    public class ZonaDeGrid<TElemento> : ControlHtml
    {
        public List<ColumnaDelGrid> Columnas { get; private set; } = new List<ColumnaDelGrid>();

        public List<FilaDelGrid> Filas { get; private set; } = new List<FilaDelGrid>();

        public int CantidadPorLeer { get; set; } = 5;
        public int PosicionInicial { get; set; }

        public int TotalEnBd { get; set; }
        public ZonaDeGrid(DescriptorDeCrud<TElemento> padre)
        : base(
          padre: padre,
          id: $"grid_{padre.Id}",
          etiqueta: null,
          propiedad: null,
          ayuda: null,
          posicion: null
        )
        {
        }
        
        public string RenderGrid()
        {
            const string htmlDiv = @"<div id = ¨idContenedor¨>     
                                     contenido 
                                    </div>";
            var htmlContenedor = htmlDiv.Replace("idContenedor", $"contenedor.{IdHtml}").Replace("contenido", RenderFilasDelGrid());
            return htmlContenedor;
        }

        public string RenderFilasDelGrid()
        {
            var grid = new Grid(IdHtml, Columnas, Filas, PosicionInicial, CantidadPorLeer) 
            { 
              Controlador = ((DescriptorDeCrud<TElemento>)Padre).Ruta, 
              TotalEnBd = TotalEnBd 
            };
            var htmlGrid = grid.ToHtml();
            return htmlGrid.Render();
        }

    }

    public class ZonaDeFiltro : ControlHtml
    {
        public ICollection<Bloque> Bloques { get; private set; } = new List<Bloque>();

        public ZonaDeFiltro(ControlHtml padre)
        : base(
          padre: padre,
          id: $"flt_{padre.Id}",
          etiqueta: null,
          propiedad: null,
          ayuda: null,
          posicion: null
        )
        {
            var b1 = new Bloque(this, "General", new Dimension(1, 2));
                     new Bloque(this, "Común", new Dimension(1, 2));

            new Editor(padre: b1, id: $"{Id}_b1_filtro", etiqueta: "Nombre", propiedad: "Nombre", ayuda: "buscar por nombre", new Posicion { fila = 0, columna = 0 });
        }

        public void AnadirBloque(Bloque bloque)
        {
            Bloques.Add(bloque);
        }

        public Bloque ObtenerBloque(string identificador)
        {
            foreach (Bloque b in Bloques)
            {
                if (b.Id == identificador)
                    return b;
            }

            throw new Exception($"El bloque {identificador} no está en la zona de filtrado");
        }

        public string RenderFiltro()
        {
            var htmlFiltro = $@"<div id = ¨{IdHtml}¨ style=¨width:100%¨>     
                                     bloques 
                                </div>";

            var htmlBloques = "";
            foreach (Bloque b in Bloques)
                htmlBloques = $"{htmlBloques}{(htmlBloques.IsNullOrEmpty() ? "" : Environment.NewLine)}{b.RenderBloque()}";

            return htmlFiltro.Replace("bloques", htmlBloques);
        }


        public string RenderModalesFiltro()
        {
            var htmlModalesEnFiltro = "";
            foreach (Bloque b in Bloques)
                htmlModalesEnFiltro = $"{htmlModalesEnFiltro}{(htmlModalesEnFiltro.IsNullOrEmpty() ? "" : Environment.NewLine)}{b.RenderModalesBloque()}";

            return htmlModalesEnFiltro;
        }

    }

    public class VistaCrud : ControlHtml
    {
        public string Ruta { get; private set; }
        public string Accion { get; private set; }

        public VistaCrud(ControlHtml padre, string ruta, string vista, string texto)
        :base(
          padre: padre,
          id: $"opc_{padre.Id}",
          etiqueta: texto,
          propiedad: null,
          ayuda: null,
          posicion: null
        )
        {
            Ruta = ruta;
            Accion = vista;
        }
    }

    public class DescriptorDeCrud<TElemento> : ControlHtml
    {
        public VistaCrud VistaMnt { get; private set; }
        public VistaCrud VistaCreacion { get; private set; }
        
        public ZonaDeOpciones<TElemento> Menu { get; set; }
        public ZonaDeFiltro Filtro { get; private set; }
        public ZonaDeGrid<TElemento> Grid { get; set; }
        public string Ruta { get; private set; }

        public DescriptorDeCrud(string ruta, string vista, string titulo)
        : base(
          padre: null,
          id: typeof(TElemento).Name.Replace("Elemento", ""),
          etiqueta: titulo,
          propiedad: null,
          ayuda: null,
          posicion: null
        )
        {
            VistaMnt = new VistaCrud(this, ruta, vista, titulo);
            Filtro = new ZonaDeFiltro(this);
            Grid = new ZonaDeGrid<TElemento>(this);
            Ruta = ruta;
        }


        protected void DefinirVistaDeCreacion(string accion, string textoMenu)
        {
            VistaCreacion = new VistaCrud(this, Ruta, accion, textoMenu);
            Menu = new ZonaDeOpciones<TElemento>(this, VistaCreacion);
        }

        public string Render()
        {
            var htmlCrud =
                   RenderTitulo() + Environment.NewLine +
                   Menu.RenderOpcionesMenu() + Environment.NewLine +
                   Filtro.RenderFiltro() + Environment.NewLine +
                   Filtro.RenderModalesFiltro() + Environment.NewLine +
                   Grid.RenderGrid() + Environment.NewLine;
            //RenderPie();

            return htmlCrud.Render();
        }

        private string RenderTitulo()
        {
            var htmlCabecera = $"<h2>{this.Etiqueta}</h2>";
            return htmlCabecera;
        }

        protected virtual void DefinirColumnasDelGrid()
        {
        }

        public virtual void MapearElementosAlGrid((IEnumerable<TElemento> elementos, int totalEnBd) leidos)
        {
            Grid.TotalEnBd = leidos.totalEnBd;
        }
    }

    public class Valor
    {
        public string Nombreestudiante { get; set; }
        public string Fechadeinscripción { get; set; }
    }


    public class Opcion<Telemento>: ControlHtml
    {
        public string Ruta { get; private set; }
        public string Accion { get; private set; }

        public Opcion(ZonaDeOpciones<Telemento> padre, string ruta, string accion, string titulo)
        :base(
          padre: padre,
          id: $"opc_{padre.Id}_{padre.Opciones.Count}",
          etiqueta: titulo,
          propiedad: null,
          ayuda: null,
          posicion: null
        )
        {
            Ruta = ruta;
            Accion = accion;
            ((ZonaDeOpciones<Telemento>)Padre).Opciones.Add(this);
        }
    }


}



