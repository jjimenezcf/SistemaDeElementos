﻿namespace Crud {

    export class HTMLSelector extends HTMLInputElement {
    }

    export class ListaDeElemento {
        private lista: HTMLSelectElement;

        get Lista(): HTMLSelectElement {
            return this.lista;
        }

        constructor(idLista: string) {
            this.lista = document.getElementById(idLista) as HTMLSelectElement;
        }

        public AgregarOpcion(valor: number, texto: string): void {

            var opcion = document.createElement("option");
            opcion.setAttribute("value", valor.toString());
            opcion.setAttribute("label", texto);
            this.Lista.appendChild(opcion);
        }
    }

    export class DatosPeticionLista {
        ClaseDeElemento: string;
        IdLista: string;

        get Selector(): ListaDeElemento {
            return new ListaDeElemento(this.IdLista);
        }
    }

    export class ListaDinamica {
        private _IdLista: string;

        get Input(): HTMLInputElement {
            return document.querySelector(`input[list="${this._IdLista}"]`);
        }

        get Lista(): HTMLDataListElement {
            return document.getElementById(this._IdLista) as HTMLDataListElement;
        }

        constructor(input: HTMLInputElement) {
            this._IdLista = input.getAttribute(atListas.idDeLaLista);
        }

        public AgregarOpcion(valor: number, texto: string): void {

            for (var i = 0; i < this.Lista.children.length; i++)
                if (this.Lista.children[i].getAttribute(atListas.identificador).Numero() === valor)
                    return;

            let opcion: HTMLOptionElement = document.createElement("option");
            opcion.setAttribute(atListas.identificador, valor.toString());
            opcion.value = texto;

            this.Lista.appendChild(opcion);
        }

        public BuscarSeleccionado(valor: string): number {
            for (var i = 0; i < this.Lista.children.length; i++) {
                if (this.Lista.children[i] instanceof HTMLOptionElement) {
                    let opcion: HTMLOptionElement = this.Lista.children[i] as HTMLOptionElement;
                    if (opcion.value === valor)
                        return opcion.getAttribute(atListas.identificador).Numero();
                }
            }
            return 0;
        }

        public Borrar(): void {
            this.Input.value = "";
            this.Lista.innerHTML = "";
        }

    }

    export class DatosPeticionDinamica {
        public ClaseDeElemento: string;
        public IdInput: string;
    }

    export class DatosRestrictor {
        public Propiedad: string;
        public Valor: number;
        public Texto: string;

        constructor(propiedad: string, valor: number, texto: string) {
            this.Propiedad = propiedad;
            this.Valor = valor;
            this.Texto = texto;
        }
    }

    export class DatosParaRelacionar {
        public idOpcionDeMenu: string;
        public RelacionarCon: string;
        public FiltroRestrictor: DatosRestrictor;

        constructor() {
        }
    }


    export class CrudBase {

        private modoTrabajo: string;
        protected get ModoTrabajo(): string {
            return this.modoTrabajo;
        }

        protected set ModoTrabajo(modo: string) {
            this.modoTrabajo = modo;
        }



        private _idPagina: string;

        protected get Pagina(): string {
            return this._idPagina;
        }
        protected set Pagina(idPagina: string) {
            EntornoSe.InicializarHistorial();
            this._idPagina = idPagina;
        }

        private _estado: EstadoPagina = undefined;

        public get Estado(): EstadoPagina {
            if (this._estado === undefined)
                this._estado = EntornoSe.Historial.ObtenerEstadoDePagina(this.Pagina);
            return this._estado;
        }

        public get HayHistorial(): boolean {
            return EntornoSe.Historial.HayHistorial(this.Pagina);
        }


        constructor() {
        }

        //funciones de ayuda para la herencia
        protected InicializarListasDeElementos(panel: HTMLDivElement, controlador: string) {
            let listas: NodeListOf<HTMLSelectElement> = panel.querySelectorAll(`select[${atControl.tipo}="${TipoControl.ListaDeElementos}"]`) as NodeListOf<HTMLSelectElement>;
            for (let i = 0; i < listas.length; i++) {
                if (listas[i].getAttribute(atListas.yaCargado) === "S")
                    continue;

                let claseElemento: string = listas[i].getAttribute(atListas.claseElemento);
                this.CargarListaDeElementos(controlador, claseElemento, listas[i].getAttribute(atControl.id));
            }
        }


        protected InicializarListasDinamicas(panel: HTMLDivElement) {
            let listas: NodeListOf<HTMLInputElement> = panel.querySelectorAll(`input[${atControl.tipo}="${TipoControl.ListaDinamica}"]`) as NodeListOf<HTMLInputElement>;
            for (let i = 0; i < listas.length; i++) {
                let lista: ListaDinamica = new ListaDinamica(listas[i]);
                lista.Borrar();
            }
        }

        protected InicializarCanvases(panel: HTMLDivElement) {
            let canvases: NodeListOf<HTMLCanvasElement> = panel.querySelectorAll("canvas") as NodeListOf<HTMLCanvasElement>;
            canvases.forEach((canvas) => { canvas.width = canvas.width; });
        }

        protected Cerrar(panelMostrar: HTMLDivElement, panelCerrar: HTMLDivElement) {
            this.OcultarPanel(panelCerrar);
            this.MostrarPanel(panelMostrar);

            BlanquearMensaje();
        }

        protected BlanquearControlesDeIU(panel: HTMLDivElement) {
            this.BlanquearEditores(panel);
            this.BlanquearSelectores(panel);
            this.BlanquearArchivos(panel);
        }

        private BlanquearEditores(panel: HTMLDivElement) {
            let editores: NodeListOf<HTMLInputElement> = panel.querySelectorAll(`input[${atControl.tipo}="${TipoControl.Editor}"]`) as NodeListOf<HTMLInputElement>;
            for (let i = 0; i < editores.length; i++) {
                this.BlanquearEditor(editores[i]);
            }
        }

        private BlanquearEditor(editor: HTMLInputElement) {
            editor.classList.remove(ClaseCss.crtlNoValido);
            editor.classList.add(ClaseCss.crtlValido);
            editor.value = "";
        }


        private BlanquearSelectores(panel: HTMLDivElement) {
            let selectores: NodeListOf<HTMLSelectElement> = panel.querySelectorAll(`select[${atControl.tipo}="${TipoControl.ListaDeElementos}"]`) as NodeListOf<HTMLSelectElement>;
            for (let i = 0; i < selectores.length; i++) {
                this.BlanquearSelector(selectores[i]);
            }
        }

        private BlanquearSelector(selector: HTMLSelectElement) {
            selector.classList.remove(ClaseCss.crtlNoValido);
            selector.classList.add(ClaseCss.crtlValido);
            selector.selectedIndex = 0;
        }


        private BlanquearArchivos(panel: HTMLDivElement) {
            let archivos: NodeListOf<HTMLInputElement> = panel.querySelectorAll(`${atControl.tipo}[tipo="${TipoControl.Archivo}"]`) as NodeListOf<HTMLInputElement>;
            for (let i = 0; i < archivos.length; i++) {
                ApiDeArchivos.BlanquearArchivo(archivos[i]);
            }
        }

        protected MostrarPanel(panel: HTMLDivElement) {
            panel.classList.add(ClaseCss.divVisible);
            panel.classList.remove(ClaseCss.divNoVisible);
        }

        protected OcultarPanel(panel: HTMLDivElement) {
            panel.classList.add(ClaseCss.divNoVisible);
            panel.classList.remove(ClaseCss.divVisible);
        }

        protected CerrarModal(idModal: string) {
            let modalBorrar: HTMLDivElement = document.getElementById(idModal) as HTMLDivElement;
            modalBorrar.style.display = "none";
            var body = document.getElementsByTagName("body")[0];
            body.style.position = "inherit";
            body.style.height = "auto";
            body.style.overflow = "visible";

        }

        public NavegarARelacionar(idOpcionDeMenu: string, filtroRestrictor: DatosRestrictor) {

            let filtro: string = this.DefinirFiltroPorRestrictor(filtroRestrictor.Propiedad, filtroRestrictor.Valor);

            let form: HTMLFormElement = document.getElementById(idOpcionDeMenu) as HTMLFormElement;

            if (form === null) {
                throw new Error(`La opción de menú '${idOpcionDeMenu}' está mal definida, actualice el descriptor`);
            }

            let navegarA: string = form.getAttribute(atRelacion.navegarA);
            let idRestrictor: string = form.getAttribute(atRelacion.restrictor) as string;
            let idOrden: string = form.getAttribute(atRelacion.orden) as string;

            let restrictor: HTMLInputElement = document.getElementById(idRestrictor) as HTMLInputElement;
            restrictor.value = filtro;
            let ordenInput: HTMLInputElement = document.getElementById(idOrden) as HTMLInputElement;
            ordenInput.value = "";

            let estadoDeLaPagina: EstadoPagina = EntornoSe.Historial.ObtenerEstadoDePagina(navegarA);
            estadoDeLaPagina.Agregar(atRelacion.restrictor, filtroRestrictor);
            EntornoSe.Historial.GuardarEstadoDePagina(estadoDeLaPagina);
            this.Navegar(form);
        }

        private Navegar(form: HTMLFormElement) {
            this.AntesDeNavegar();
            EntornoSe.Historial.GuardarEstadoDePagina(this.Estado);
            EntornoSe.Historial.Persistir();
            PonerCapa();
            form.submit();
        }

        public AntesDeNavegar() {
        }

        protected SiHayErrorTrasPeticionAjax(peticion: ApiDeAjax.DescriptorAjax) {
            Mensaje(TipoMensaje.Error, peticion.resultado.mensaje);
        }

        // funciones para mapear un elemento Json a los controles de un panel

        protected MapearElementoLeido(panel: HTMLDivElement, elementoJson: JSON) {
            this.MapearPropiedadesDelElemento(panel, "elementoJson", elementoJson);
            this.MaperaPropiedadesDeListasDeElementos(panel, elementoJson);
            this.MaperaOpcionesListasDinamicas(panel, elementoJson);
            this.MapearSelectoresDeArchivo(panel, elementoJson);
        }

        private MaperaPropiedadesDeListasDeElementos(panel: HTMLDivElement, elementoJson: JSON) {
            let select: HTMLCollectionOf<HTMLSelectElement> = panel.getElementsByTagName('select') as HTMLCollectionOf<HTMLSelectElement>;
            for (var i = 0; i < select.length; i++) {
                var selector = select[i] as HTMLSelectElement;
                var guardarEn = selector.getAttribute(atListas.guardarEn);
                let id: number = this.BuscarValorEnJson(guardarEn, elementoJson) as number;
                if (id === null || id == 0)
                    selector.selectedIndex = 0;
                else
                    for (var j = 0; j < selector.options.length; j++) {
                        if (selector.options[j].value.Numero() == id) {
                            selector.selectedIndex = j;
                            break;
                        }
                    }
            }
        }

        private MaperaOpcionesListasDinamicas(panel: HTMLDivElement, elementoJson: JSON) {

            let listas: NodeListOf<HTMLInputElement> = panel.querySelectorAll(`input[${atControl.tipo}="${TipoControl.ListaDinamica}"]`) as NodeListOf<HTMLInputElement>;

            for (var i = 0; i < listas.length; i++) {
                let input: HTMLInputElement = listas[i] as HTMLInputElement;
                let propiedad: string = input.getAttribute(atControl.propiedad);
                let guardarEn: string = input.getAttribute(atListas.guardarEn);
                let id: number = this.BuscarValorEnJson(guardarEn, elementoJson) as number;
                let valor: string = this.BuscarValorEnJson(propiedad, elementoJson) as string;

                if (id === null || id == 0)
                    input.value = "";
                else {
                    let listaDinamica = new ListaDinamica(input);
                    listaDinamica.AgregarOpcion(id, valor);
                    input.value = valor;
                }

            }
        }

        private MapearPropiedadesDelElemento(panel: HTMLDivElement, propiedad: string, valorPropiedadJson: any) {

            if (valorPropiedadJson === undefined || valorPropiedadJson === null) {
                this.MapearPropiedad(panel, propiedad, "");
                return;
            }

            var tipoDeObjeto = typeof valorPropiedadJson;
            if (tipoDeObjeto === "object") {
                for (var propiedad in valorPropiedadJson) {
                    this.MapearPropiedadesDelElemento(panel, propiedad.toLowerCase(), valorPropiedadJson[propiedad]);
                }
            } else {
                this.MapearPropiedad(panel, propiedad, valorPropiedadJson);
            }
        }

        private BuscarValorEnJson(propiedad: string, valorPropiedadJson: any) {
            var tipoDeObjeto = typeof valorPropiedadJson;
            if (tipoDeObjeto === "object") {
                for (var p in valorPropiedadJson) {
                    if (propiedad.toLowerCase() === p.toLowerCase())
                        return valorPropiedadJson[p];
                }
            }
            return null;
        }


        private MapearPropiedad(panel: HTMLDivElement, propiedad: string, valor: any) {

            if (this.MapearPropiedaAlEditor(panel, propiedad, valor))
                return;

            //if (this.MapearPropiedadAlSelectorDeArchivo(panel, propiedad, valor))
            //    return;

            if (this.MapearPropiedadAlSelectorDeUrlDelArchivo(panel, propiedad, valor))
                return;

            if (this.MapearPropiedadAlCheck(panel, propiedad, valor))
                return;
        }

        private MapearPropiedaAlEditor(panel: HTMLDivElement, propiedad: string, valor: any): boolean {
            let editor: HTMLInputElement = this.BuscarEditor(panel, propiedad);

            if (editor === null)
                return false;

            editor.classList.remove(ClaseCss.crtlNoValido);
            editor.classList.add(ClaseCss.crtlValido);
            editor.value = valor;
            return true;
        }

        private MapearPropiedadAlCheck(panel: HTMLDivElement, propiedad: string, valor: any): boolean {
            let check: HTMLInputElement = this.BuscarCheck(panel, propiedad);

            if (check === null)
                return false;

            check.classList.remove(ClaseCss.crtlNoValido);
            check.classList.add(ClaseCss.crtlValido);

            if (IsBool(valor))
                check.checked = valor === true;
            else
                if (IsString(valor))
                    check.checked = valor.toLowerCase() === 'true';

            return true;
        }


        private MapearSelectoresDeArchivo(panel: HTMLDivElement, elementoJson: JSON) {

            let selectores: NodeListOf<HTMLInputElement> = panel.querySelectorAll(`input[tipo="${TipoControl.Archivo}"]`) as NodeListOf<HTMLInputElement>;

            for (var i = 0; i < selectores.length; i++) {
                let selector: HTMLInputElement = selectores[i] as HTMLInputElement;
                let propiedad: string = selector.getAttribute(atControl.propiedad);
                let valor: number = this.BuscarValorEnJson(propiedad, elementoJson) as number;
                if (valor !== null) {
                    let visorVinculado: string = selector.getAttribute(atArchivo.imagen);
                    selector.setAttribute(atArchivo.id, valor.toString());
                    this.MapearImagenes(panel, elementoJson, visorVinculado);
                }
            }

        }

        private MapearImagenes(panel: HTMLDivElement, elementoJson: JSON, visorVinculado: string) {
            let visor: HTMLImageElement = document.getElementById(visorVinculado) as HTMLImageElement;
            let propiedadDelVisor: string = visor.getAttribute(atControl.propiedad);
            let url: string = this.BuscarValorEnJson(propiedadDelVisor, elementoJson) as string;
            this.MostrarImagenUrl(visor, url);
        }


        private MapearPropiedadAlSelectorDeUrlDelArchivo(panel: HTMLDivElement, propiedad: string, valor: any): boolean {
            let selector: HTMLInputElement = this.BuscarUrlDelArchivo(panel, propiedad);

            if (selector === null)
                return false;
            let ruta: string = selector.getAttribute(atArchivo.rutaDestino);

            selector.classList.remove(ClaseCss.crtlNoValido);
            selector.classList.add(ClaseCss.crtlValido);
            selector.setAttribute(atArchivo.nombre, valor);
            this.MapearPropiedadAlVisorDeImagen(panel, propiedad, `${ruta}/${valor}`);

            return true;
        }

        private MapearPropiedadAlVisorDeImagen(panel: HTMLDivElement, propiedad: string, valor: any) {
            let visor: HTMLImageElement = this.BuscarVisorDeImagen(panel, propiedad);

            if (visor === null)
                return;

            this.MostrarImagenUrl(visor, valor);
        }

        protected MapearRestrictor(restrictores: NodeListOf<HTMLInputElement>, porpiedadRestrictora: string, valorMostrar: string, valorRestrictor: number) {
            for (let i = 0; i < restrictores.length; i++) {
                if (restrictores[i].getAttribute(atControl.propiedad) === porpiedadRestrictora) {
                    restrictores[i].setAttribute(atControl.valor, valorMostrar);
                    restrictores[i].setAttribute(atControl.restrictor, valorRestrictor.toString());
                }
            }
        }

        private MostrarImagenUrl(visor: HTMLImageElement, url: any) {
            visor.setAttribute('src', url);
            let idCanva: string = visor.getAttribute(atControl.id).replace('img', 'canvas');
            let htmlCanvas: HTMLCanvasElement = document.getElementById(idCanva) as HTMLCanvasElement;
            htmlCanvas.width = 100;
            htmlCanvas.height = 100;
            var canvas = htmlCanvas.getContext('2d');
            var img = new Image();
            img.src = url;
            img.onload = function () {
                canvas.drawImage(img, 0, 0, 100, 100);
            };
        }

        // funciones para la gestión de los mapeos de controles a un json  ****************************************************************************

        protected BuscarEditor(controlPadre: HTMLDivElement, propiedadDto: string): HTMLInputElement {
            let editores: NodeListOf<HTMLInputElement> = controlPadre.querySelectorAll(`input[${atControl.tipo}='${TipoControl.Editor}']`) as NodeListOf<HTMLInputElement>;
            for (var i = 0; i < editores.length; i++) {
                var control = editores[i] as HTMLInputElement;
                var dto = control.getAttribute(atControl.propiedad);
                if (dto === propiedadDto.toLowerCase())
                    return control;
            }
            return null;
        }

        protected BuscarCheck(controlPadre: HTMLDivElement, propiedadDto: string): HTMLInputElement {
            let checkes: NodeListOf<HTMLInputElement> = controlPadre.querySelectorAll(`input[${atControl.tipo}='${TipoControl.Check}']`) as NodeListOf<HTMLInputElement>;
            for (var i = 0; i < checkes.length; i++) {
                var control = checkes[i] as HTMLInputElement;
                var dto = control.getAttribute(atControl.propiedad);
                if (dto === propiedadDto.toLowerCase())
                    return control;
            }
            return null;
        }

        protected BuscarSelectorDeArchivo(controlPadre: HTMLDivElement, propiedadDto: string): HTMLInputElement {
            let selectores: NodeListOf<HTMLInputElement> = controlPadre.querySelectorAll(`input[tipo="${TipoControl.Archivo}"]`) as NodeListOf<HTMLInputElement>;
            for (var i = 0; i < selectores.length; i++) {
                var control = selectores[i] as HTMLInputElement;
                var dto = control.getAttribute(atControl.propiedad);
                if (dto === propiedadDto.toLowerCase())
                    return control;
            }
            return null;
        }

        protected BuscarVisorDeImagen(controlPadre: HTMLDivElement, propiedadDto: string): HTMLImageElement {
            let visor: NodeListOf<HTMLImageElement> = controlPadre.querySelectorAll(`img[${atControl.tipo}='${TipoControl.VisorDeArchivo}']`) as NodeListOf<HTMLImageElement>;
            for (var i = 0; i < visor.length; i++) {
                var control = visor[i] as HTMLImageElement;
                var dto = control.getAttribute(atControl.propiedad);
                if (dto === propiedadDto.toLowerCase())
                    return control;
            }
            return null;
        }

        protected BuscarUrlDelArchivo(controlPadre: HTMLDivElement, propiedadDto: string): HTMLInputElement {
            let selectores: NodeListOf<HTMLInputElement> = controlPadre.querySelectorAll(`input[tipo="${TipoControl.UrlDeArchivo}"]`) as NodeListOf<HTMLInputElement>;
            for (var i = 0; i < selectores.length; i++) {
                var control = selectores[i] as HTMLInputElement;
                var dto = control.getAttribute(atControl.propiedad);
                if (dto === propiedadDto.toLowerCase())
                    return control;
            }
            return null;
        }

        protected BuscarSelect(controlPadre: HTMLDivElement, propiedadDto: string): HTMLSelectElement {
            let select: HTMLCollectionOf<HTMLSelectElement> = controlPadre.getElementsByTagName('select') as HTMLCollectionOf<HTMLSelectElement>;
            for (var i = 0; i < select.length; i++) {
                var control = select[i] as HTMLSelectElement;
                var dto = control.getAttribute(atControl.propiedad);
                if (dto === propiedadDto)
                    return control;
            }
            return null;
        }

        protected BuscarListaDinamica(controlPadre: HTMLDivElement, propiedadDto: string): HTMLInputElement {

            let inputs: NodeListOf<HTMLInputElement> = controlPadre.querySelectorAll(`input[${atControl.tipo}="${TipoControl.ListaDinamica}"]`) as NodeListOf<HTMLInputElement>;

            for (var i = 0; i < inputs.length; i++) {
                var control = inputs[i] as HTMLInputElement;
                var dto = control.getAttribute(atControl.propiedad);
                if (dto === propiedadDto)
                    return control;
            }
            return null;
        }

        protected AntesDeMapearDatosDeIU(panel: HTMLDivElement): JSON {
            if (this.ModoTrabajo === ModoTrabajo.creando)
                return JSON.parse(`{"${Literal.id}":"0"}`);

            if (this.ModoTrabajo === ModoTrabajo.editando) {
                let input: HTMLInputElement = this.BuscarEditor(panel, Literal.id);
                if (Number(input.value) <= 0)
                    throw new Error(`El valor del id ${Number(input.value)} debe ser mayor a 0`);
                return JSON.parse(`{"${Literal.id}":"${Number(input.value)}"}`);
            }
        }

        protected MapearControlesDeIU(panel: HTMLDivElement): JSON {
            let elementoJson: JSON = this.AntesDeMapearDatosDeIU(panel);

            this.MapearSelectoresDeElementosAlJson(panel, elementoJson);
            this.MapearSelectoresDinamicosAlJson(panel, elementoJson);
            this.MapearRestrictoresAlJson(panel, elementoJson);
            this.MapearEditoresAlJson(panel, elementoJson);
            this.MapearArchivosAlJson(panel, elementoJson);
            this.MapearUrlArchivosAlJson(panel, elementoJson);
            this.MapearCheckesAlJson(panel, elementoJson);

            return this.DespuesDeMapearDatosDeIU(panel, elementoJson);
        }

        protected MapearEditoresAlJson(panel: HTMLDivElement, elementoJson: JSON): void {
            let editores: NodeListOf<HTMLInputElement> = panel.querySelectorAll(`input[tipo="${TipoControl.Editor}"]`) as NodeListOf<HTMLInputElement>;
            for (let i = 0; i < editores.length; i++) {
                this.MapearEditorAlJson(editores[i], elementoJson);
            }
        }

        private MapearEditorAlJson(input: HTMLInputElement, elementoJson: JSON): void {
            var propiedadDto = input.getAttribute(atControl.propiedad);
            let valor: string = (input as HTMLInputElement).value;
            let obligatorio: string = input.getAttribute(atControl.obligatorio);

            if (obligatorio === "S" && valor.NoDefinida()) {
                input.classList.remove(ClaseCss.crtlValido);
                input.classList.add(ClaseCss.crtlNoValido);
                throw new Error(`El campo ${propiedadDto} es obligatorio`);
            }

            input.classList.remove(ClaseCss.crtlNoValido);
            input.classList.add(ClaseCss.crtlValido);
            elementoJson[propiedadDto] = valor;
        }


        protected MapearRestrictoresAlJson(panel: HTMLDivElement, elementoJson: JSON): void {
            let restrictores: NodeListOf<HTMLInputElement> = panel.querySelectorAll(`input[tipo="${TipoControl.restrictorDeEdicion}"]`) as NodeListOf<HTMLInputElement>;
            for (let i = 0; i < restrictores.length; i++) {
                this.MapearRestrictorAlJson(restrictores[i], elementoJson);
            }
        }

        private MapearRestrictorAlJson(input: HTMLInputElement, elementoJson: JSON): void {
            let propiedadDto: string = input.getAttribute(atControl.propiedad);
            let idRestrictor: string = input.getAttribute(atControl.restrictor);

            if (!NumeroMayorDeCero(idRestrictor)) {
                input.classList.remove(ClaseCss.crtlValido);
                input.classList.add(ClaseCss.crtlNoValido);
                throw new Error(`El campo ${propiedadDto} es obligatorio`);
            }

            input.classList.remove(ClaseCss.crtlNoValido);
            input.classList.add(ClaseCss.crtlValido);
            elementoJson[propiedadDto] = idRestrictor;
        }

        protected MapearCheckesAlJson(panel: HTMLDivElement, elementoJson: JSON): void {
            let checkes: NodeListOf<HTMLInputElement> = panel.querySelectorAll(`input[tipo="${TipoControl.Check}"]`) as NodeListOf<HTMLInputElement>;
            for (let i = 0; i < checkes.length; i++) {
                this.MapearCheckAlJson(checkes[i], elementoJson);
            }
        }


        private MapearCheckAlJson(check: HTMLInputElement, elementoJson: JSON): void {
            var propiedadDto = check.getAttribute(atControl.propiedad);
            elementoJson[propiedadDto] = check.checked;
        }
        protected MapearSelectoresDinamicosAlJson(panel: HTMLDivElement, elementoJson: JSON): void {
            let selectores: NodeListOf<HTMLInputElement> = panel.querySelectorAll(`input[tipo="${TipoControl.ListaDinamica}"]`) as NodeListOf<HTMLInputElement>;
            for (let i = 0; i < selectores.length; i++) {
                this.MapearSelectorDinamico(selectores[i], elementoJson);
            }
        }

        private MapearSelectorDinamico(input: HTMLInputElement, elementoJson: JSON) {
            let propiedadDto = input.getAttribute(atControl.propiedad);
            let guardarEn: string = input.getAttribute(atListas.guardarEn);
            let obligatorio: string = input.getAttribute(atControl.obligatorio);
            let lista: ListaDinamica = new ListaDinamica(input);
            let valor: number = lista.BuscarSeleccionado(input.value);

            if (obligatorio === "S" && (IsNullOrEmpty(input.value) || Number(valor) === 0)) {
                input.classList.remove(ClaseCss.crtlValido);
                input.classList.add(ClaseCss.crtlNoValido);
                throw new Error(`Debe seleccionar un elemento de la lista ${propiedadDto}`);
            }

            input.classList.remove(ClaseCss.crtlNoValido);
            input.classList.add(ClaseCss.crtlValido);
            elementoJson[guardarEn] = valor.toString();
        }

        protected MapearSelectoresDeElementosAlJson(panel: HTMLDivElement, elementoJson: JSON): void {
            let selectores: NodeListOf<HTMLSelectElement> = panel.querySelectorAll(`select[tipo="${TipoControl.ListaDeElementos}"]`) as NodeListOf<HTMLSelectElement>;
            for (let i = 0; i < selectores.length; i++) {
                this.MapearSelectorDeElementosAlJson(selectores[i], elementoJson);
            }
        }

        private MapearSelectorDeElementosAlJson(selector: HTMLSelectElement, elementoJson: JSON) {
            let propiedadDto = selector.getAttribute(atControl.propiedad);
            let guardarEn: string = selector.getAttribute(atListas.guardarEn);
            let obligatorio: string = selector.getAttribute(atControl.obligatorio);

            if (obligatorio === "S" && Number(selector.value) === 0) {
                selector.classList.remove(ClaseCss.crtlValido);
                selector.classList.add(ClaseCss.crtlNoValido);
                throw new Error(`Debe seleccionar un elemento de la lista ${propiedadDto}`);
            }

            selector.classList.remove(ClaseCss.crtlNoValido);
            selector.classList.add(ClaseCss.crtlValido);
            elementoJson[guardarEn] = selector.value;
        }

        protected MapearArchivosAlJson(panel: HTMLDivElement, elementoJson: JSON): void {
            let archivos: NodeListOf<HTMLInputElement> = panel.querySelectorAll(`input[tipo="${TipoControl.Archivo}"]`) as NodeListOf<HTMLInputElement>;
            for (let i = 0; i < archivos.length; i++) {
                this.MapearArchivoAlJson(archivos[i], elementoJson);
            }
        }


        private MapearArchivoAlJson(archivo: HTMLInputElement, elementoJson: JSON): void {
            var propiedadDto = archivo.getAttribute(atControl.propiedad);
            let valor: string = archivo.getAttribute(atArchivo.id);
            let obligatorio: string = archivo.getAttribute(atControl.obligatorio);

            if (obligatorio === "S" && IsNullOrEmpty(valor)) {
                archivo.classList.remove(ClaseCss.crtlValido);
                archivo.classList.add(ClaseCss.crtlNoValido);
                throw new Error(`El campo ${propiedadDto} es obligatorio`);
            }

            archivo.classList.remove(ClaseCss.crtlNoValido);
            archivo.classList.add(ClaseCss.crtlValido);
            elementoJson[propiedadDto] = valor;
        }

        private MapearUrlArchivosAlJson(panel: HTMLDivElement, elementoJson: JSON): void {
            let urlsDeArchivos: NodeListOf<HTMLInputElement> = panel.querySelectorAll(`input[tipo="${TipoControl.UrlDeArchivo}"]`) as NodeListOf<HTMLInputElement>;
            for (let i = 0; i < urlsDeArchivos.length; i++) {
                this.MapearUrlArchivoAlJson(urlsDeArchivos[i], elementoJson);
            }
        }

        private MapearUrlArchivoAlJson(urlDeArchivo: HTMLInputElement, elementoJson: JSON): void {
            var propiedadDto = urlDeArchivo.getAttribute(atControl.propiedad);
            let valor: string = urlDeArchivo.getAttribute(atArchivo.nombre);
            let obligatorio: string = urlDeArchivo.getAttribute(atControl.obligatorio);

            if (obligatorio === "S" && IsNullOrEmpty(valor)) {
                urlDeArchivo.classList.remove(ClaseCss.crtlValido);
                urlDeArchivo.classList.add(ClaseCss.crtlNoValido);
                throw new Error(`El campo ${propiedadDto} es obligatorio`);
            }

            urlDeArchivo.classList.remove(ClaseCss.crtlNoValido);
            urlDeArchivo.classList.add(ClaseCss.crtlValido);
            elementoJson[propiedadDto] = valor;
        }

        protected DespuesDeMapearDatosDeIU(panel: HTMLDivElement, elementoJson: JSON): JSON {
            return elementoJson;
        }


        // funciones de carga de elementos para los selectores   ************************************************************************************

        protected CargarListaDinamica(input: HTMLInputElement, controlador: string) {
            let clase: string = input.getAttribute(atListas.claseElemento);
            let idInput: string = input.getAttribute('id');
            let url: string = this.DefinirPeticionDeCargarDinamica(controlador, clase, input.value);
            let datosDeEntrada = `{"ClaseDeElemento":"${clase}", "IdInput":"${idInput}"}`;
            let a = new ApiDeAjax.DescriptorAjax(this
                , Ajax.EndPoint.CargaDinamica
                , datosDeEntrada
                , url
                , ApiDeAjax.TipoPeticion.Asincrona
                , ApiDeAjax.ModoPeticion.Get
                , this.AnadirOpciones
                , this.SiHayErrorTrasPeticionAjax
            );

            a.Ejecutar();
        }

        private AnadirOpciones(peticion: ApiDeAjax.DescriptorAjax) {
            let datos: DatosPeticionDinamica = JSON.parse(peticion.DatosDeEntrada);

            let listaDinamica: ListaDinamica = new ListaDinamica(document.getElementById(datos.IdInput) as HTMLInputElement);
            for (var i = 0; i < peticion.resultado.datos.length; i++) {
                listaDinamica.AgregarOpcion(peticion.resultado.datos[i].id, peticion.resultado.datos[i].nombre);
            }

            listaDinamica.Lista.click();
        }

        protected CargarListaDeElementos(controlador: string, claseDeElementoDto: string, idLista: string) {

            let url: string = this.DefinirPeticionDeCargarElementos(controlador, claseDeElementoDto);
            let datosDeEntrada = `{"ClaseDeElemento":"${claseDeElementoDto}", "IdLista":"${idLista}"}`;
            let a = new ApiDeAjax.DescriptorAjax(this
                , Ajax.EndPoint.CargarLista
                , datosDeEntrada
                , url
                , ApiDeAjax.TipoPeticion.Asincrona
                , ApiDeAjax.ModoPeticion.Get
                , this.MapearElementosEnLista
                , this.SiHayErrorTrasPeticionAjax
            );

            a.Ejecutar();
        }

        private MapearElementosEnLista(peticion: ApiDeAjax.DescriptorAjax) {
            let datos: DatosPeticionLista = JSON.parse(peticion.DatosDeEntrada);
            let idLista = datos.IdLista;
            let lista = new ListaDeElemento(idLista);
            for (var i = 0; i < peticion.resultado.datos.length; i++) {
                lista.AgregarOpcion(peticion.resultado.datos[i].id, peticion.resultado.datos[i].nombre);
            }

            lista.Lista.setAttribute(atListas.yaCargado, "S");
        }

        private DefinirPeticionDeCargarElementos(controlador: string, claseElemento: string): string {
            let url: string = `/${controlador}/${Ajax.EndPoint.CargarLista}?${Ajax.Param.claseElemento}=${claseElemento}`;
            return url;
        }

        protected DefinirFiltroPorId(id: number): string {
            return this.DefinirFiltroPorRestrictor(Literal.filtro.clausulaId, id);
        }

        protected DefinirFiltroPorRestrictor(propiedad: string, valor: number): string {
            var clausulas = new Array<ClausulaDeFiltrado>();
            var clausula: ClausulaDeFiltrado = new ClausulaDeFiltrado(propiedad, Literal.filtro.criterio.igual, `${valor}`);
            clausulas.push(clausula);
            return JSON.stringify(clausulas);
        }


        private DefinirPeticionDeCargarDinamica(controlador: string, claseElemento: string, filtro: string): string {
            let url: string = `/${controlador}/${Ajax.EndPoint.CargaDinamica}?${Ajax.Param.claseElemento}=${claseElemento}&posicion=0&cantidad=-1&filtro=${filtro}`;
            return url;
        }

    }

}