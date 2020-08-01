﻿namespace Crud {

    export function EventosDelMantenimiento(accion: string, parametros: any): void {
        try {
            switch (accion) {
                case Evento.Mnt.Crear: {
                    crudMnt.IraCrear();
                    break;
                }
                case Evento.Mnt.Editar: {
                    crudMnt.IraEditar();
                    break;
                }
                case Evento.Mnt.Borrar: {
                    crudMnt.AbrirModalBorrarElemento();
                    break;
                }
                case Evento.Mnt.Relacionar: {
                    let crudDeRelacion:string = parametros;
                    crudMnt.RelacionarCon(crudDeRelacion);
                    break;
                }
                case Evento.Mnt.Buscar: {
                    crudMnt.Buscar(atGrid.accion.buscar,0);
                    break;
                }
                case Evento.Mnt.ObtenerSiguientes: {
                    crudMnt.ObtenerSiguientes();
                    break;
                }
                case Evento.Mnt.ObtenerAnteriores: {
                    crudMnt.ObtenerAnteriores();
                    break;
                }
                case Evento.Mnt.ObtenerUltimos: {
                    crudMnt.ObtenerUltimos();
                    break;
                }
                case Evento.Mnt.FilaPulsada: {
                    let parIn: Array<string> = parametros.split("#");
                    crudMnt.FilaPulsada(parIn[0], parIn[1]);
                    break;
                }
                case Evento.Mnt.OrdenarPor: {
                    crudMnt.OrdenarPor(parametros);
                    break;
                }
                case Evento.Mnt.CambiarSelector: {
                    crudMnt.CambiarSelector(parametros);
                    break;
                }
                default: {
                    Mensaje(TipoMensaje.Error, `la opción ${accion} no está definida`);
                    break;
                }
            }
        }
        catch (error) {
            Mensaje(TipoMensaje.Error, error);
        }
    }

    export function EventosModalDeSeleccion(accion: string, parametros: string): void {

        let parIn: Array<string> = parametros.split("#");
        let modal: ModalSeleccion = crudMnt.ObtenerModal(parIn[0]);
        try {
            switch (accion) {
                case Evento.ModalSeleccion.Abrir: {
                    modal.AbrirModalDeSeleccion();
                    break;
                }
                case Evento.ModalSeleccion.Cerrar: {
                    modal.CerrarModalDeSeleccion();
                    break;
                }
                case Evento.ModalSeleccion.Seleccionar: {
                    modal.SeleccionarElementos();
                    break;
                }
                case Evento.ModalSeleccion.FilaPulsada: {
                    let idCheck: string = parIn[1];
                    let idOrigen: string = parIn[2]; // si se ha pulsado en el check o en la fila
                    modal.FilaPulsada(idCheck, idOrigen);
                    break;
                }
                case Evento.ModalSeleccion.Buscar: {
                    modal.RecargarGrid();
                    break;
                }
                case Evento.ModalSeleccion.ObtenerSiguientes: {
                    modal.ObtenerSiguientes();
                    break;
                }
                case Evento.ModalSeleccion.ObtenerAnteriores: {
                    modal.ObtenerAnteriores();
                    break;
                }
                case Evento.ModalSeleccion.ObtenerUltimos: {
                    modal.ObtenerUltimos();
                    break;
                }
                case Evento.ModalSeleccion.OrdenarPor: {
                    let columna: string = parIn[1];
                    modal.OrdenarPor(columna);
                    break;
                }
                default: {
                    Mensaje(TipoMensaje.Error, `la opción ${accion} no está definida`);
                    break;
                }
            }
        }
        catch (error) {
            Mensaje(TipoMensaje.Error, error);
        }
    }

    export function EventosModalDeBorrar(accion: string): void {
        try {
            switch (accion) {
                case Evento.ModalBorrar.Cerrar: {
                    crudMnt.CerrarModalDeBorrado();
                    break;
                }
                case Evento.ModalBorrar.Borrar: {
                    crudMnt.BorrarElemento();
                    break;
                }
                default: {
                    Mensaje(TipoMensaje.Error, `la opción ${accion} no está definida`);
                    break;
                }
            }
        }
        catch (error) {
            Mensaje(TipoMensaje.Error, error);
        }
    }

    export function EventosModalDeCreacion(accion: string): void {
        try {
            switch (accion) {
                case Evento.ModalCreacion.Cerrar: {
                    crudMnt.CerrarModalDeCreacion();
                    break;
                }
                case Evento.ModalCreacion.Crear: {
                    crudMnt.CrearElemento();
                    break;
                }
                default: {
                    Mensaje(TipoMensaje.Error, `la opción ${accion} no está definida`);
                    break;
                }
            }
        }
        catch (error) {
            Mensaje(TipoMensaje.Error, error);
        }
    }

    export function EventosModalDeEdicion(accion: string): void {
        try {
            switch (accion) {
                case Evento.ModalEdicion.Cerrar: {
                    crudMnt.CerrarModalDeEdicion();
                    break;
                }
                case Evento.ModalEdicion.Modificar: {
                    crudMnt.ModificarElemento();
                    break;
                }
                default: {
                    Mensaje(TipoMensaje.Error, `la opción ${accion} no está definida`);
                    break;
                }
            }
        }
        catch (error) {
            Mensaje(TipoMensaje.Error, error);
        }
    }

    export function EjecutarMenuEdt(accion: string): void {
        try {
            crudMnt.crudDeEdicion.EjecutarAcciones(accion);
        }
        catch (error) {
            Mensaje(TipoMensaje.Error, error);
        }
    }

    export function EventosDeEdicion(accion: string): void {
        try {
            crudMnt.crudDeEdicion.EjecutarAcciones(accion);
        }
        catch (error) {
            Mensaje(TipoMensaje.Error, error);
        }
    }

    export function EjecutarMenuCrt(accion: string): void {
        try {
            crudMnt.crudDeCreacion.EjecutarAcciones(accion);
        }
        catch (error) {
            Mensaje(TipoMensaje.Error, error);
        }
    }

    export function ListaDeElementos(accion: string, selector: HTMLInputElement) {

        try {
            switch (accion) {
                case Evento.ListaDinamica.Cargar: {
                    crudMnt.CargarListaDinamica(selector);
                    break;
                }
                default: {
                    Mensaje(TipoMensaje.Error, `la opción ${accion} no está definida`);
                    break;
                }
            }
        }
        catch (error) {
            Mensaje(TipoMensaje.Error, error);
        }



    }


}