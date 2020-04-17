function Mensaje(tipo, mensaje) {
    var control = document.getElementById("Mensaje");
    control.value = `(${tipo}) ${mensaje}`;
    console.log(control.value);
}
function BlanquearMensaje() {
    var control = document.getElementById("Mensaje");
    control.value = "";
}
function StringBuilder(value) {
    this.strings = new Array();
    this.append(value);
}
StringBuilder.prototype.append = function (value) {
    if (value) {
        this.strings.push(value);
    }
};
StringBuilder.prototype.appendLine = function (value) {
    if (value) {
        this.strings.push(value + newLine);
    }
};
StringBuilder.prototype.clear = function () {
    this.strings.length = 0;
};
StringBuilder.prototype.toString = function () {
    return this.strings.join("");
};
function ObtenerIdDeLaFilaChequeada(idCheck) {
    return obtenerValorDeLaColumnaChequeada(idCheck, "id");
}
function obtenerValorDeLaColumnaChequeada(idCheck, columna) {
    var inputId = document.getElementById(idCheck.replace(".chksel", `.${columna}`));
    return inputId.value;
}
function EsNula(valor) {
    if (valor == null || valor == undefined)
        return true;
    return valor.NoDefinida();
}
String.prototype.NoDefinida = function () {
    var str = this;
    if (str !== undefined)
        return str.length === 0 || str.trim() === '';
    return true;
};
String.prototype.Numero = function () {
    if (this === undefined || this === null)
        return 0;
    if (this.NoDefinida())
        return 0;
    if (isNaN(this))
        return 0;
    return Number(this);
};
class ClausulaDeFiltrado {
    constructor(propiedad, criterio, valor) {
        this.Propiedad = propiedad;
        this.Criterio = criterio;
        this.Valor = valor;
    }
    EsVacia() {
        return this.Propiedad.NoDefinida() || this.Valor.NoDefinida() || this.Criterio.NoDefinida();
    }
}
function ParsearRespuesta(req) {
    var resultado;
    try {
        resultado = JSON.parse(req.response);
    }
    catch {
        Mensaje(TipoMensaje.Error, `Error al procesar la respuesta de ${Ajax.EndPoint.SolicitarMenuEnHtml}`);
        undefined;
    }
    return resultado;
}
//# sourceMappingURL=tsUtilidades.js.map