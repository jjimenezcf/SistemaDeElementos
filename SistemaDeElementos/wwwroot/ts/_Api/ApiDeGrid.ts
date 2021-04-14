﻿namespace ApiGrid {
    export class PropiedadesDeLaFila {
        id: string;
        propiedad: string;
        visible: boolean;
        estilo: CSSStyleDeclaration;
        claseCss: string;
        editable: boolean;
        tipo: string;
        anchoEnPixel: number;
        constructor() {

        }
    }

    export function obtenerDescriptorDeLaCabecera(tabla: HTMLTableElement): Array<PropiedadesDeLaFila> {
        let filaCabecera: Array<PropiedadesDeLaFila> = new Array<PropiedadesDeLaFila>();
        var cabecera = tabla.rows[0];
        var ths = cabecera.querySelectorAll('th');
        for (let i = 0; i < ths.length; i++) {
            let p: PropiedadesDeLaFila = new PropiedadesDeLaFila();
            p.id = ths[i].id;
            p.visible = !ths[i].hidden;
            p.claseCss = ths[i].className;
            p.estilo = ths[i].style;
            p.anchoEnPixel = ths[i].getBoundingClientRect().width;
            p.editable = false;
            p.propiedad = ths[i].getAttribute('propiedad');
            filaCabecera.push(p);
        }
        return filaCabecera;
    }

    export function ColumnaVisible(tabla: HTMLTableElement, idColumna: string) {
        let columna: HTMLTableHeaderCellElement = document.getElementById(idColumna) as HTMLTableHeaderCellElement;
        columna.classList.remove('columna-oculta');
        columna.classList.add('columna-cabecera');
        let cuerpoDeLaTabla: HTMLTableSectionElement = tabla.tBodies[0];
        var tds = cuerpoDeLaTabla.querySelectorAll('td');
        for (let i = 0; i < tds.length; i++) {
            if (tds[i].headers === idColumna) {
                tds[i].classList.remove('columna-oculta');
                tds[i].classList.add('columna-cabecera');
            }
        }
    }

    export function ColumnaInvisible(tabla: HTMLTableElement, idColumna: string) {
        let columna: HTMLTableHeaderCellElement = document.getElementById(idColumna) as HTMLTableHeaderCellElement;
        columna.classList.add('columna-oculta');
        columna.classList.remove('columna-cabecera');
        let cuerpoDeLaTabla: HTMLTableSectionElement = tabla.tBodies[0];
        var tds = cuerpoDeLaTabla.querySelectorAll('td');
        for (let i = 0; i < tds.length; i++) {
            if (tds[i].headers === idColumna) {
                tds[i].classList.add('columna-oculta');
                tds[i].classList.remove('columna-cabecera');
            }
        }
    }

    export function RecalcularAnchoColumnas(tabla: HTMLTableElement) {
        recalcularPorcentajes(tabla);
        let cuerpo: HTMLTableSectionElement = tabla.tBodies[0];
        aplicarPorcentajes(cuerpo);
    }

    function aplicarPorcentajes(cuerpoDeLaTabla: HTMLTableSectionElement) {
        var tds = cuerpoDeLaTabla.querySelectorAll('td');
        for (let i = 0; i < tds.length; i++) {
            if (!tds[i].classList.contains('columna-oculta')) {
                let idCabecera: string = tds[i].headers;
                let cabecera: HTMLTableHeaderCellElement = document.getElementById(idCabecera) as HTMLTableHeaderCellElement;
                tds[i].style.width = cabecera.style.width;
            }
        }
    }

    function recalcularPorcentajes(tabla: HTMLTableElement) {
        var cabecera = tabla.rows[0];
        var ths = cabecera.querySelectorAll('th');
        let sumaDeLoQueHay: number = 0;
        for (let i = 0; i < ths.length; i++) {
            if (!ths[i].classList.contains('columna-oculta')) {
                sumaDeLoQueHay = sumaDeLoQueHay + Numero(ths[i].style.width.replace('%', ''));
            }
        }

        let loQueHay: number = 0;
        let loQueDebeSer: number = 0;
        for (let i = 0; i < ths.length; i++) {
            if (!ths[i].hidden) {
                loQueHay = Numero(ths[i].style.width.replace('%', ''));
                loQueDebeSer = (loQueHay * 100) / sumaDeLoQueHay;
                ths[i].style.width = `${loQueDebeSer}%`;
            }
        }
    }
}