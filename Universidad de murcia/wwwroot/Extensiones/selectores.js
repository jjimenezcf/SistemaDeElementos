﻿class SelectorInfo {
    constructor(nombre, maximo) {
        this._nombre = nombre;
        this._maximo = maximo;
        this._seleccionados = new Array();
    }

    get Nombre() { return this._nombre; }
    get Cantidad() { return this._seleccionados.length; }

    Insertar(idSeleccionado) {
        if (this._seleccionados.length < this._maximo)
            this._seleccionados.push(idSeleccionado);
        else
            Mensaje(`Solo se pueden seleccionar ${this._maximo} elementos`);
    }

    Quitar(idSeleccionado) {
        var pos = this._seleccionados.indexOf(idSeleccionado);
        if (pos >= 0) 
            this._seleccionados.splice(pos, 1);
        else
            Mensaje(TipoMensaje.Info,`No se ha localizado el elemento con id  ${idSeleccionado}`);        
    }
}

var Selectores = new Array();


function AlSeleccionar(idSelector, referenciaChecks, columnaId, columnaMostrar) {

    var selector = document.getElementById(idSelector);

    blanquearSelector(selector);
    var checkboxes = $(`input[name='${referenciaChecks}']:checked`);
    for (var x = 0; x < checkboxes.length; x++) {
        var elemento = obtenerElementoSeleccionado(checkboxes[x].id, columnaId, columnaMostrar);
        mapearValoresAlSelector(selector, elemento);
    }
    cerrar(referenciaChecks);
}

function obtenerElementoSeleccionado(idCheck, columnaId, columnaMostrar) {
    var e = {
        id: parseInt(document.getElementById(idCheck.replace("chk", columnaId)).innerHTML.trim()),
        valor: document.getElementById(idCheck.replace("chk", columnaMostrar)).innerHTML.trim()
    };

    return e;
}

function AlAbrir(idTabla, columnaId, elementosMarcados) {
    var selectorInfo = new SelectorInfo(idTabla,1);
    marcarElementos(selectorInfo, idTabla, columnaId, elementosMarcados);
}


function AlCerrar(idModal, referenciaChecks) {
    console.log(`se ha cerrado la modal ${idModal}, hay que desmarcar los checks de la modal`);
    cerrar(referenciaChecks);
}

function ElementosMarcados(idSelector) {

    var seleccionados = "";
    var selector = document.getElementById(idSelector);
    if (selector.hasAttribute("idsSeleccionados")) {
        seleccionados = selector.getAttribute("idsSeleccionados");
    }
    
    return seleccionados;
}


function mapearValoresAlSelector(selector, elemento) {

    var valorDelSelector = selector.value;
    if (valorDelSelector !== undefined && valorDelSelector.trim() !== "")
        valorDelSelector = valorDelSelector + " | ";
    selector.value = valorDelSelector + elemento.valor;

    mapearIdsAlSelector(selector, elemento.id);

}

function mapearIdsAlSelector(selector, id) {
    var listaDeIds = selector.getAttribute("idsSeleccionados");
    if (listaDeIds === null) {
        atributo = document.createAttribute("idsSeleccionados");
        selector.setAttributeNode(atributo);
        listaDeIds = "";
    }

    if (listaDeIds.trim() !== "")
        listaDeIds = listaDeIds + ';';
    listaDeIds = listaDeIds + id;
    selector.setAttribute("idsSeleccionados", listaDeIds);
}


function blanquearSelector(selector) {
    selector.value = "";
    if (selector.hasAttribute("idsSeleccionados")) {
        var listaDeIds = selector.getAttribute("idsSeleccionados");
        listaDeIds = "";
        selector.setAttribute("idsSeleccionados", listaDeIds);
    }
}

function cerrar(referenciaChecks) {
    blanquearCheck(referenciaChecks);
    
}

function marcarElementos(selectorInfo, idTabla, columnaId, seleccionados) {
    var array = seleccionados.split(';');
    if (array.length === 1 && array[0] === "")
        return;

    Mensaje(TipoMensaje.Info, `Creado el selector ${selectorInfo.Nombre}`);

    var celdasId = document.getElementsByName(`${idTabla}_${columnaId}`);
    var len = celdasId.length;
    for (var i = 0; i < array.length; i++) {
        selectorInfo.Insertar(array[i]);
        for (var j = 0; j < len; j++) {
            if (celdasId[j].innerHTML === array[i]) {
                var check = document.getElementById(celdasId[j].id.replace("_id_", "_chk_"));
                check.checked = true;
                break;
            }
        }
    }
}

function blanquearCheck(referenciaChecks) {
    var checkboxes = $(`input[name='${referenciaChecks}']:checked`);

    for (var x = 0; x < checkboxes.length; x++) {
        checkboxes[x].checked = false;
    }

}

