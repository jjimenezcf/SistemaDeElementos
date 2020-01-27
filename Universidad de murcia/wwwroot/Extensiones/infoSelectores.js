﻿
//************************************************************************************************************************************************************************************/

/// Gestión de los info selectores

//************************************************************************************************************************************************************************************/

//TODO:
// Añadir tres propiedades más 
//  Si es para un htmlSelector
//  El id del htmSelector
//  La columna a mostrar en el htmlSelector vinculado

class InfoSelector {


    get Id() { return this._idGrid; }
    get Cantidad() { return this._seleccionados.length; }
    get Seleccionados() { return this._seleccionados; }
    
    iniciarClase(idGrid) {
        this._idGrid = idGrid;
        this._grid = document.getElementById(idGrid);
        this._seleccionables = this._grid.getAttribute("seleccionables");
        this._seleccionados = new Array();
        this._MostrarEnSelector = new Array();
        this._EsModal = false;
    }

    constructor(idGrid) {
        this.iniciarClase(idGrid);
    }

    deshabilitarCheck(deshabilitar) {

        var ejecutar = false;
        //si has desmarcado checks y los seleccionados son menos que los seleccionables --> ok a habilitar
        if (deshabilitar === false && this._seleccionados.length < this._seleccionables)
            ejecutar = true;

        //Si has marcado y los seleccionados son más o igual que los seleccionables --> ok a deshabilitar
        if (deshabilitar === true && this.Seleccionados.length >= this._seleccionables)
            ejecutar = true;

        if (ejecutar) {
            var checkboxes = document.getElementsByName(`chksel.${this._idGrid}`);
            for (var x = 0; x < checkboxes.length; x++) {
                if (!checkboxes[x].checked) {
                    checkboxes[x].disabled = deshabilitar;
                }
            }
        }
    }

    Modal(valor) { this._EsModal = valor; }

    InsertarId(id) {
        if (!id || isNaN(parseInt(id))) {
            console.log(`Ha intentado insertar en la lista un id no válido ${id}`);
            return -1;
        }

        if (this._seleccionados.length < this._seleccionables) {
            this._seleccionados.push(id);
        }

        this.deshabilitarCheck(true);
        return this.Cantidad;
    }

    InsertarElemento(id, textoMostrar) {

        if (this._EsModal) {
            var pos = this.InsertarId(id);
            if (pos === this._seleccionados.length) {
                this._MostrarEnSelector.push(textoMostrar);
            }
        }
        else {
            console.log(`Ha intentado insertar un elemento en un infoSelector no válido por no estar declarado como Modal`);
            return -1;
        }

        return pos;
    }

    InsertarIds(ids) {

        if (!ids || ids.length === 1 && isNaN(parseInt(ids))) {
            console.log(`Ha intentado insertar en la lista un array de ids no válidos ${ids}`);
            return -2;
        }

        for (var i = 0; i < ids.length; i++) {
            var idSeleccionado = ids[i];
            this.InsertarId(idSeleccionado);
        }

        return this.Cantidad;
    }

    Quitar(idSeleccionado) {
        var pos = this._seleccionados.indexOf(idSeleccionado);
        if (pos >= 0) {
            this._seleccionados.splice(pos, 1);
            this.deshabilitarCheck(false);
        }
        else
            Mensaje(TipoMensaje.Info, `No se ha localizado el elemento con id  ${idSeleccionado}`);
    }

    ToString() {
        var ids = "";
        for (var i = 0; i < this._seleccionados.length; i++) {
            ids = ids + this._seleccionados[i];
            if (i < this._seleccionados.length - 1)
                ids = ids + ';';
        }
        return ids;
    }

    SincronizarCheck() {
        this.deshabilitarCheck(true);
    }

}

class InfoSelectores {

    _infoSelectores = new Array();

    get Cantidad() {
        if (!this._infoSelectores)
            return 0;
        else
            return this._infoSelectores.length;
    }

    Obtener(id) {
        if (!this._infoSelectores || this._infoSelectores.length === 0)
            return;

        for (var i = 0; i < this.Cantidad; i++) {
            if (this._infoSelectores[i].Id === id)
                return this._infoSelectores[i];
        }
        return undefined;
    }


    Insertar(infoSelector) {
        var infSel = this.Obtener(infoSelector.Id);
        if (!infSel)
            this._infoSelectores.push(infoSelector);

        return this._infoSelectores.length;
    }

    Borrar(id) {
        var infSel = this.Obtener(id);
        if (infSel)
            this._infoSelectores.splice(infSel, 1);

        return this._infoSelectores.length;
    }
}


var infoSelectores = new InfoSelectores();



