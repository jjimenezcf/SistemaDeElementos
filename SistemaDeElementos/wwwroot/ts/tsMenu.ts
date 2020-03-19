﻿module Menu {
    export function MostrarMenu() {
        let idProductoHtml: HTMLElement = document.getElementById('id_menu');
        let idModalMenu: string = idProductoHtml.getAttribute('modal-menu');
        let idModalHtml: HTMLElement = document.getElementById(idModalMenu);

        if (idModalHtml === undefined) {
            console.log(`No se ha definido el contenedor del menú ${idModalMenu}`);
        }
        else {
            var menuAbierto = idProductoHtml.getAttribute("menu-abierto");
            if (menuAbierto === undefined || menuAbierto === "false") {
                idProductoHtml.setAttribute("menu-abierto", "true");
                idModalHtml.style.display = "block";
                idModalHtml.style.height = `${document.documentElement.clientHeight - 60}px`;
            }
            else {
                idProductoHtml.setAttribute("menu-abierto", "false");
                idModalHtml.style.display = "none";
            }
        }
    }

    export function OpcionSeleccionada(opcion: string) {
        let urlBase: string = window.location.origin;
        window.location.href = `${urlBase}/${opcion}`;
    }

    export function MenuPulsado(id_menu_pulsado: string) {
        let elementosHtml: NodeListOf<HTMLElement> = document.getElementsByName("menu");
        let menuHtmlPulsado: HTMLMenuElement = document.getElementById(id_menu_pulsado) as HTMLMenuElement;


        if (menuHtmlPulsado.getAttribute("menu-plegado") == "false") {
            plegarMenu(menuHtmlPulsado);
            return;
        }

        for (let i = 0; i < elementosHtml.length; i++)
            plegarMenu(elementosHtml[i] as HTMLMenuElement);

        desplegarMenu(menuHtmlPulsado);

        let padreHtml: HTMLElement = menuHtmlPulsado.parentElement;
        while (padreHtml !== null) {
            if (padreHtml.constructor.toString().indexOf("HTMLUListElement") > 0)
                desplegarMenu(padreHtml as HTMLMenuElement);
            padreHtml = padreHtml.parentElement;
        }

    }

    function desplegarMenu(menuHtml: HTMLMenuElement) {
        menuHtml.style.display = "block";
        menuHtml.compact = false;
        menuHtml.setAttribute("menu-plegado", "false");
    }

    function plegarMenu(menuHtml: HTMLMenuElement) {
        menuHtml.style.display = "none";
        menuHtml.compact = true;
        menuHtml.setAttribute("menu-plegado", "true");
    }
}






