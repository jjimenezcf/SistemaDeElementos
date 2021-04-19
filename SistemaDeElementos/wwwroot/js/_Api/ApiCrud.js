var ApiControl;
(function (ApiControl) {
    function OcultarMostrarExpansor(idHtmlExpansor, idHtmlBloque) {
        let extensor = document.getElementById(`${idHtmlExpansor}`);
        if (NumeroMayorDeCero(extensor.value)) {
            extensor.value = "0";
            ApiCrud.OcultarPanel(document.getElementById(`${idHtmlBloque}`));
        }
        else {
            extensor.value = "1";
            ApiCrud.MostrarPanel(document.getElementById(`${idHtmlBloque}`));
        }
        //EntornoSe.AjustarModalesAbiertas();
    }
    ApiControl.OcultarMostrarExpansor = OcultarMostrarExpansor;
    function BloquearMenu(panel) {
        let opciones = panel.querySelectorAll(`input[${atControl.tipo}="${TipoControl.opcion}"]`);
        for (var i = 0; i < opciones.length; i++) {
            let opcion = opciones[i];
            let clase = opcion.getAttribute(atOpcionDeMenu.clase);
            if (clase === ClaseDeOpcioDeMenu.Basico)
                continue;
            bloquearOpcionDeMenu(opcion, true);
        }
    }
    ApiControl.BloquearMenu = BloquearMenu;
    function OcultarOpcionDeMenu(panel, nombreOpcion) {
        let opcion = buscarOpcionDeMenu(panel, nombreOpcion);
        if (opcion !== null) {
            ocultarOpcionDeMenu(opcion, true);
            return true;
        }
        return false;
    }
    ApiControl.OcultarOpcionDeMenu = OcultarOpcionDeMenu;
    function OcultarMostrarOpcionDeMenu(opcion, ocultar) {
        ocultarOpcionDeMenu(opcion, ocultar);
    }
    ApiControl.OcultarMostrarOpcionDeMenu = OcultarMostrarOpcionDeMenu;
    function BloquearDesbloquearOpcionDeMenu(opcion, bloquear) {
        bloquearOpcionDeMenu(opcion, bloquear);
    }
    ApiControl.BloquearDesbloquearOpcionDeMenu = BloquearDesbloquearOpcionDeMenu;
    function BloquearOpcionDeMenu(panel, nombreOpcion) {
        let opcion = buscarOpcionDeMenu(panel, nombreOpcion);
        if (opcion !== null) {
            bloquearOpcionDeMenu(opcion, true);
            return true;
        }
        return false;
    }
    ApiControl.BloquearOpcionDeMenu = BloquearOpcionDeMenu;
    function DesbloquearOpcionDeMenu(panel, nombreOpcion) {
        let opcion = buscarOpcionDeMenu(panel, nombreOpcion);
        if (opcion !== null) {
            bloquearOpcionDeMenu(opcion, false);
            return true;
        }
        return false;
    }
    ApiControl.DesbloquearOpcionDeMenu = DesbloquearOpcionDeMenu;
    function buscarOpcionDeMenu(panel, nombreOpcion) {
        let opciones = panel.querySelectorAll(`input[${atControl.tipo}="${TipoControl.opcion}"]`);
        for (var i = 0; i < opciones.length; i++) {
            let opcion = opciones[i];
            if (opcion.value === nombreOpcion)
                return opcion;
        }
        return null;
    }
    function bloquearOpcionDeMenu(opcion, bloquear) {
        opcion.disabled = bloquear;
        opcion.setAttribute(atOpcionDeMenu.bloqueada, bloquear ? "S" : "N");
    }
    function ocultarOpcionDeMenu(opcion, ocultar) {
        opcion.hidden = ocultar;
        opcion.setAttribute(atOpcionDeMenu.oculta, ocultar ? "S" : "N");
    }
    function EstaBloqueada(opcion) { return opcion.getAttribute(atOpcionDeMenu.bloqueada) === "S" || opcion.disabled; }
    ApiControl.EstaBloqueada = EstaBloqueada;
    function EstaOculta(opcion) { return opcion.getAttribute(atOpcionDeMenu.oculta) === "S" || opcion.hidden; }
    ApiControl.EstaOculta = EstaOculta;
    function BloquearListaDinamica(panel, propiedad) {
        let lista = BuscarLista(panel, propiedad);
        if (lista !== null) {
            lista.disabled = true;
            lista.readOnly = true;
            return true;
        }
        return false;
    }
    ApiControl.BloquearListaDinamica = BloquearListaDinamica;
    function DesbloquearListaDinamica(panel, propiedad) {
        let lista = BuscarLista(panel, propiedad);
        if (lista !== null) {
            lista.disabled = false;
            lista.readOnly = false;
            return true;
        }
        return false;
    }
    ApiControl.DesbloquearListaDinamica = DesbloquearListaDinamica;
    function BloquearEditorPorPropiedad(panel, propiedad) {
        let editor = BuscarEditor(panel, propiedad);
        if (editor !== null) {
            return BloquearEditor(editor);
        }
        return false;
    }
    ApiControl.BloquearEditorPorPropiedad = BloquearEditorPorPropiedad;
    function BloquearEditor(editor) {
        if (editor !== null) {
            editor.disabled = true;
            editor.readOnly = true;
            return true;
        }
        return false;
    }
    ApiControl.BloquearEditor = BloquearEditor;
    function DesbloquearEditorPorPropiedad(panel, propiedad) {
        let editor = BuscarEditor(panel, propiedad);
        if (editor !== null) {
            return DesbloquearEditor(editor);
        }
        return false;
    }
    ApiControl.DesbloquearEditorPorPropiedad = DesbloquearEditorPorPropiedad;
    function DesbloquearEditor(editor) {
        if (editor !== null) {
            editor.disabled = false;
            editor.readOnly = false;
            return true;
        }
        return false;
    }
    ApiControl.DesbloquearEditor = DesbloquearEditor;
    function BuscarEditor(panel, propiedad) {
        let editores = panel.querySelectorAll(`input[${atControl.tipo}="${TipoControl.Editor}"]`);
        for (let i = 0; i < editores.length; i++) {
            let lista = editores[i];
            if (lista.getAttribute(atControl.propiedad) == propiedad.toLocaleLowerCase()) {
                return lista;
            }
        }
        return null;
    }
    function BuscarLista(panel, propiedad) {
        let listas = panel.querySelectorAll(`input[${atControl.tipo}="${TipoControl.ListaDinamica}"]`);
        for (let i = 0; i < listas.length; i++) {
            let lista = listas[i];
            if (lista.getAttribute(atControl.propiedad) == propiedad.toLocaleLowerCase()) {
                return lista;
            }
        }
        return null;
    }
    function BlanquearFecha(fecha) {
        fecha.value = "";
        let tipo = fecha.getAttribute(atControl.tipo);
        if (tipo === TipoControl.SelectorDeFechaHora) {
            let idHora = fecha.getAttribute(atSelectorDeFecha.hora);
            if (!IsNullOrEmpty(idHora)) {
                let controlHora = document.getElementById(idHora);
                controlHora.value = '';
                controlHora.setAttribute(atSelectorDeFecha.milisegundos, '0');
            }
        }
    }
    ApiControl.BlanquearFecha = BlanquearFecha;
    function AsignarFecha(panel, propiedad, fecha) {
        let control = BuscarFecha(panel, propiedad);
        if (control !== null) {
            MapearAlControl.FechaDate(control, fecha);
            if (control.getAttribute(atControl.tipo) === TipoControl.SelectorDeFechaHora)
                return MapearAlControl.HoraDate(control, fecha);
            return true;
        }
        return false;
    }
    ApiControl.AsignarFecha = AsignarFecha;
    function BuscarFecha(panel, propiedad) {
        let fechas = panel.querySelectorAll(`input[tipo="${TipoControl.SelectorDeFecha}"]`);
        for (var i = 0; i < fechas.length; i++) {
            let fecha = fechas[i];
            if (fecha.getAttribute(atControl.propiedad) == propiedad.toLocaleLowerCase()) {
                return fecha;
            }
        }
        fechas = panel.querySelectorAll(`input[tipo="${TipoControl.SelectorDeFechaHora}"]`);
        for (var i = 0; i < fechas.length; i++) {
            let fecha = fechas[i];
            if (fecha.getAttribute(atControl.propiedad) == propiedad.toLocaleLowerCase()) {
                return fecha;
            }
        }
        return null;
    }
    function AjustarColumnaDelGrid(columanDeOrdenacion) {
        let columna = document.getElementById(columanDeOrdenacion.IdColumna);
        columna.setAttribute(atControl.modoOrdenacion, columanDeOrdenacion.Modo);
        let a = columna.getElementsByTagName('a')[0];
        a.setAttribute("class", columanDeOrdenacion.ccsClase);
    }
    ApiControl.AjustarColumnaDelGrid = AjustarColumnaDelGrid;
    function BlanquearEditor(editor) {
        AnularError(editor);
        editor.value = "";
    }
    ApiControl.BlanquearEditor = BlanquearEditor;
    function AnularError(control) {
        control.classList.remove(ClaseCss.crtlNoValido);
        control.classList.add(ClaseCss.crtlValido);
    }
    ApiControl.AnularError = AnularError;
    function MarcarError(control) {
        control.classList.add(ClaseCss.crtlNoValido);
        control.classList.remove(ClaseCss.crtlValido);
    }
    ApiControl.MarcarError = MarcarError;
    function BlanquearSelector(selector) {
        selector.classList.remove(ClaseCss.crtlNoValido);
        selector.classList.add(ClaseCss.crtlValido);
        selector.selectedIndex = 0;
    }
    ApiControl.BlanquearSelector = BlanquearSelector;
    function LeerEntreFechas(controlDeFechaDesde) {
        let idHora = controlDeFechaDesde.getAttribute(atEntreFechas.horaDesde);
        let entreFechas = LeerFechaHora(controlDeFechaDesde, idHora);
        let idFechaHasta = controlDeFechaDesde.getAttribute(atEntreFechas.fechaHasta);
        let fechaHasta = document.getElementById(idFechaHasta);
        idHora = controlDeFechaDesde.getAttribute(atEntreFechas.horaHasta);
        entreFechas = entreFechas + '-' + LeerFechaHora(fechaHasta, idHora);
        return entreFechas;
    }
    ApiControl.LeerEntreFechas = LeerEntreFechas;
    function LeerFechaHora(controlDeFecha, idHora) {
        let valorDeFecha = controlDeFecha.value;
        let resultado = "";
        if (!IsNullOrEmpty(valorDeFecha)) {
            let fecha = new Date(valorDeFecha);
            resultado = fecha.toLocaleDateString();
            let controlDeHora = document.getElementById(idHora);
            let valorDeHora = controlDeHora.value;
            if (!IsNullOrEmpty(valorDeHora)) {
                resultado = resultado + ' ' + valorDeHora;
            }
        }
        return resultado;
    }
})(ApiControl || (ApiControl = {}));
var ApiCrud;
(function (ApiCrud) {
    function MapearControlesDesdeLaIuAlJson(crud, panel, modoDeTrabajo) {
        let elementoJson = crud.AntesDeMapearDatosDeIU(crud, panel, modoDeTrabajo);
        MapearAlJson.ListasDeElementos(panel, elementoJson);
        MapearAlJson.ListaDinamicas(panel, elementoJson);
        MapearAlJson.Restrictores(panel, elementoJson);
        MapearAlJson.Editores(panel, elementoJson);
        MapearAlJson.Textos(panel, elementoJson);
        MapearAlJson.Archivos(panel, elementoJson);
        MapearAlJson.Urls(panel, elementoJson);
        MapearAlJson.Checks(panel, elementoJson);
        MapearAlJson.Fechas(panel, elementoJson);
        return crud.DespuesDeMapearDatosDeIU(crud, panel, elementoJson, modoDeTrabajo);
    }
    ApiCrud.MapearControlesDesdeLaIuAlJson = MapearControlesDesdeLaIuAlJson;
    function BlanquearControlesDeIU(panel) {
        BlanquearEditores(panel);
        BlanquearSelectores(panel);
        BlanquearArchivos(panel);
    }
    ApiCrud.BlanquearControlesDeIU = BlanquearControlesDeIU;
    function MostrarPanel(panel) {
        panel.classList.remove(ClaseCss.divNoVisible);
    }
    ApiCrud.MostrarPanel = MostrarPanel;
    function OcultarPanel(panel) {
        panel.classList.add(ClaseCss.divNoVisible);
        panel.classList.remove(ClaseCss.divVisible);
    }
    ApiCrud.OcultarPanel = OcultarPanel;
    function CerrarModal(modal) {
        modal.style.display = "none";
        //modal.setAttribute('altura-inicial', "0");
        //modal.setAttribute('ratio-inicial', "0");
        //var body = document.getElementsByTagName("body")[0];
        //body.style.position = "inherit";
        //body.style.height = "auto";
        //body.style.overflow = "visible";
    }
    ApiCrud.CerrarModal = CerrarModal;
    function QuitarClaseDeCtrlNoValido(panel) {
        let crtls = panel.getElementsByClassName(ClaseCss.crtlNoValido);
        for (let i = 0; i < crtls.length; i++) {
            crtls[i].classList.remove(ClaseCss.crtlNoValido);
        }
    }
    ApiCrud.QuitarClaseDeCtrlNoValido = QuitarClaseDeCtrlNoValido;
    function ActivarOpciones(opciones, activas, seleccionadas) {
        for (var i = 0; i < opciones.length; i++) {
            let opcion = opciones[i];
            if (ApiControl.EstaBloqueada(opcion))
                continue;
            let literal = opcion.value.toLowerCase();
            if (activas.indexOf(literal) >= 0) {
                let permiteMultiSeleccion = opcion.getAttribute(atOpcionDeMenu.permiteMultiSeleccion);
                if (!EsTrue(permiteMultiSeleccion))
                    opcion.disabled = !(seleccionadas === 1);
                else {
                    if (seleccionadas === 1)
                        opcion.disabled = false;
                }
            }
        }
    }
    ApiCrud.ActivarOpciones = ActivarOpciones;
    function DesactivarOpciones(opciones, desactivas) {
        for (var i = 0; i < opciones.length; i++) {
            let opcion = opciones[i];
            if (ApiControl.EstaBloqueada(opcion))
                continue;
            let literal = opcion.value.toLowerCase();
            if (desactivas.indexOf(literal) >= 0)
                opcion.disabled = true;
        }
    }
    ApiCrud.DesactivarOpciones = DesactivarOpciones;
    function DesactivarConMultiSeleccion(opciones, seleccionadas) {
        for (var i = 0; i < opciones.length; i++) {
            let opcion = opciones[i];
            if (ApiControl.EstaBloqueada(opcion))
                continue;
            let permiteMultiSeleccion = opcion.getAttribute(atOpcionDeMenu.permiteMultiSeleccion);
            if (!EsTrue(permiteMultiSeleccion) && !opcion.disabled)
                opcion.disabled = !(seleccionadas === 1);
        }
    }
    ApiCrud.DesactivarConMultiSeleccion = DesactivarConMultiSeleccion;
    function CambiarLiteralOpcion(opciones, antiguo, nuevo) {
        for (var i = 0; i < opciones.length; i++) {
            let opcion = opciones[i];
            if (ApiControl.EstaBloqueada(opcion))
                continue;
            let literal = opcion.value.toLowerCase();
            if (literal.toLowerCase() === antiguo)
                opcion.value = nuevo;
        }
    }
    ApiCrud.CambiarLiteralOpcion = CambiarLiteralOpcion;
    function BlanquearEditores(panel) {
        let editores = panel.querySelectorAll(`input[${atControl.tipo}="${TipoControl.Editor}"]`);
        for (let i = 0; i < editores.length; i++) {
            ApiControl.BlanquearEditor(editores[i]);
        }
    }
    function BlanquearSelectores(panel) {
        let selectores = panel.querySelectorAll(`select[${atControl.tipo}="${TipoControl.ListaDeElementos}"]`);
        for (let i = 0; i < selectores.length; i++) {
            ApiControl.BlanquearSelector(selectores[i]);
        }
    }
    function BlanquearArchivos(panel) {
        let archivos = panel.querySelectorAll(`${atControl.tipo}[tipo="${TipoControl.Archivo}"]`);
        for (let i = 0; i < archivos.length; i++) {
            ApiDeArchivos.BlanquearArchivo(archivos[i], true);
        }
    }
})(ApiCrud || (ApiCrud = {}));
var ApiRuote;
(function (ApiRuote) {
    function NavegarARelacionar(crud, idOpcionDeMenu, idSeleccionado, filtroRestrictor) {
        let filtroJson = ApiFiltro.DefinirRestrictorNumerico(filtroRestrictor.Propiedad, filtroRestrictor.Valor);
        let form = document.getElementById(idOpcionDeMenu);
        if (form === null) {
            throw new Error(`La opción de menú '${idOpcionDeMenu}' está mal definida, actualice el descriptor`);
        }
        let navegarAlCrud = form.getAttribute(atNavegar.navegarAlCrud);
        let idRestrictor = form.getAttribute(atNavegar.idRestrictor);
        let idOrden = form.getAttribute(atNavegar.orden);
        let restrictor = document.getElementById(idRestrictor);
        restrictor.value = filtroJson;
        let ordenInput = document.getElementById(idOrden);
        ordenInput.value = "";
        let valores = new Diccionario();
        valores.Agregar(Sesion.paginaDestino, navegarAlCrud);
        valores.Agregar(Sesion.restrictor, filtroRestrictor);
        valores.Agregar(Sesion.idSeleccionado, idSeleccionado);
        Navegar(crud, form, valores);
    }
    ApiRuote.NavegarARelacionar = NavegarARelacionar;
    function NavegarADependientes(crud, idOpcionDeMenu, idSeleccionado, filtroRestrictor) {
        let filtroJson = ApiFiltro.DefinirRestrictorNumerico(filtroRestrictor.Propiedad, filtroRestrictor.Valor);
        let form = document.getElementById(idOpcionDeMenu);
        if (form === null) {
            throw new Error(`La opción de menú '${idOpcionDeMenu}' está mal definida, actualice el descriptor`);
        }
        let navegarAlCrud = form.getAttribute(atNavegar.navegarAlCrud);
        let idRestrictor = form.getAttribute(atNavegar.idRestrictor);
        let idOrden = form.getAttribute(atNavegar.orden);
        let restrictor = document.getElementById(idRestrictor);
        restrictor.value = filtroJson;
        let ordenInput = document.getElementById(idOrden);
        ordenInput.value = "";
        let valores = new Diccionario();
        valores.Agregar(Sesion.paginaDestino, navegarAlCrud);
        valores.Agregar(Sesion.restrictor, filtroRestrictor);
        valores.Agregar(Sesion.idSeleccionado, idSeleccionado);
        Navegar(crud, form, valores);
    }
    ApiRuote.NavegarADependientes = NavegarADependientes;
    function Navegar(crud, form, valores) {
        crud.AntesDeNavegar(valores);
        EntornoSe.Historial.GuardarEstadoDePagina(crud.Estado);
        EntornoSe.Sumit(form);
    }
})(ApiRuote || (ApiRuote = {}));
;
var ApiFiltro;
(function (ApiFiltro) {
    function DefinirFiltroPorId(id) {
        return ApiFiltro.DefinirRestrictorNumerico(literal.filtro.clausulaId, id);
    }
    ApiFiltro.DefinirFiltroPorId = DefinirFiltroPorId;
    function DefinirRestrictorNumerico(propiedad, valor) {
        var clausulas = new Array();
        var clausula = new ClausulaDeFiltrado(propiedad, literal.filtro.criterio.igual, `${valor}`);
        clausulas.push(clausula);
        return JSON.stringify(clausulas);
    }
    ApiFiltro.DefinirRestrictorNumerico = DefinirRestrictorNumerico;
    function DefinirFiltroListaDinamica(input, criterio) {
        let buscarPor = input.getAttribute(atListasDinamicas.buscarPor);
        let longitud = Numero(input.getAttribute(atListasDinamicas.longitudNecesaria));
        let valor = input.value;
        if (longitud == 0)
            longitud = 3;
        if (valor.length < longitud)
            return null;
        let clausula = new ClausulaDeFiltrado(buscarPor, criterio, valor.toString());
        return clausula;
    }
    ApiFiltro.DefinirFiltroListaDinamica = DefinirFiltroListaDinamica;
})(ApiFiltro || (ApiFiltro = {}));
//# sourceMappingURL=ApiCrud.js.map