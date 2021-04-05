﻿const newLine = "\n";

const TipoMensaje = { Info: "informativo", Error: "Error", Warning: "Revision" };


const literal = {
    controlador: "controlador",
    idNegocio: "id-negocio",
    negocio: "negocio",
    id: "id",
    true: "true",
    false: "false",
    filtro: {
        clausulaId: 'id',
        criterio: {
            igual: 'igual',
            diferente: 'diferente'
        }
    }
};

const atMenu = {
    abierto: "menu-abierto",
    plegado: "menu-plegado",
};

const atControl = {
    propiedad: "propiedad",
    ordenarPor: "ordenar-por",
    nombre: "name",
    criterio: "criterio-de-filtro",
    filtro: "control-de-filtro",
    tablaDeDatos: "tabla-de-datos",
    id: literal.id,
    crudModal: 'crud-modal',
    propiedadRestrictora: 'propiedad-restrictora',
    propiedadMostrar: 'propiedad-mostrar',
    mostrarExpresion: 'mostrar-expresion',
    controlador: literal.controlador,
    obligatorio: "obligatorio",
    modoOrdenacion: "modo-ordenacion",
    expresionElemento: "expresion-elemento",
    tipo: "tipo",
    imagenVinculada: "imagen-vinculada",
    valorInput: "value",
    valorTr: "idDelElemento",
    restrictor: "restrictor",
    nombreModal: "idModal",
    editable: "editable",
    eventoJs: {
        onclick: 'onclick'
    }
};

const atCheck = {
    filtrarPorFalse: "filtrar-por-false"
};

const atMantenimniento = {
    zonaDeFiltro: "zona-de-filtro",
    controlador: literal.controlador,
    negocio: literal.negocio,
    idNegocio: literal.idNegocio,
    zonaMenu: "zona-de-menu",
    gridDelMnt: "grid-del-mnt"
};

const ModoOrdenacion = {
    ascedente: "ascendente",
    descendente: "descendente",
    sinOrden: "sin-orden"
};


const atGrid = {
    id: atMantenimniento.gridDelMnt,
    cargando: 'cargando',
    zonaNavegador: 'zona-de-navegador',
    cabeceraTabla: 'cabecera-de-tabla',
    idSeleccionado: 'id-seleccionado',
    nombreSeleccionado: 'nombre-Seleccionado',
    navegador: {
        cantidad: "cantidad-a-mostrar",
        pagina: "pagina",
        leidos: "leidos",
        posicion: "posicion",
        total: "total-en-bd",
        titulo: "title"
    },
    paginaDeDatos: "pagina-de-datos",
    accion: {
        buscar: "buscar",
        siguiente: "sigiente",
        anterior: "anterior",
        restaurar: "restaurar",
        ultima: "ultima",
        ordenar: "ordenar"
    },
    selector: "selector",
    idCtrlCantidad: "nav_2_reg",
    idInfo: "info",
    idMensaje: "mensaje"
};

const atArchivo = {
    idArchivo: "id-archivo",
    controlador: literal.controlador,
    nombre: "nombre-archivo",
    canvas: "canvas-vinculado",
    imagen: "imagen-vinculada",
    barra: "barra-vinculada",
    contenedorBarra: "contenedor-barra",
    infoArchivo: "info-archivo",
    estado: "estado-subida",
    situacion: {
        subiendo: 'subiendo',
        subido: 'subido',
        error: 'error',
        pendiente: 'pendiente',
        sinArchivo: 'sin-archivo'
    },
    rutaDestino: "ruta-destino",
    extensionesValidas: "accept",
    limiteEnByte: "limite-en-byte",
    url: "src"
};

const atOpcionDeMenu = {
    permisosNecesarios: "permisos-necesarios",
    permiteMultiSeleccion: "permite-multi-seleccion",
    clase: "clase",
    bloqueada: "bloqueada"
};

const atSelector = {
    popiedadBuscar: "propiedadBuscar",
    criterioBuscar: "criterioBuscar",
    idEditorMostrar: "ideditormostrar",
    idGridModal: "id-grid-modal",
    propiedadmostrar: atControl.propiedadMostrar,
    refCheckDeSeleccion: "refCheckDeSeleccion",
    idModal: "id-modal",
    idBtnSelector: "idBtnSelector",
    ListaDeSeleccionados: 'ids-seleccionados',
    selector: "selector",
    propiedadParaFiltrar: "propiedadFiltrar"
};

const atSelectorDeFecha = {
    hora: "idDeLaHora",
    milisegundos: "milisegundos"
};

const atRestrictor = {
    mostrarExpresion: atControl.mostrarExpresion
};

const atListas = {
    claseElemento: 'clase-elemento',
    mostrarExpresion: atControl.mostrarExpresion,
    yaCargado: 'ya-cargada',
    idDeLaLista: 'list',
    identificador: 'identificador',
    expresionPorDefecto: 'nombre',
};

const atListasDeElemento = {
    claseElemento: atListas.claseElemento,
    mostrarExpresion: atListas.mostrarExpresion,
    yaCargado: atListas.yaCargado,
    expresionPorDefecto: atListas.expresionPorDefecto
};

const atListasDinamicas = {
    claseElemento: atListas.claseElemento,
    buscarPor: 'como-buscar',
    criterio: atControl.criterio,
    mostrarExpresion: atListas.mostrarExpresion,
    longitudNecesaria: 'longitud',
    idSeleccionado: 'idseleccionado',
    cargando: 'cargando',
    expresionPorDefecto: atListas.expresionPorDefecto,
    ultimaCadenaBuscada: 'ultima-busqueda',
    cantidad: 'cantidad-a-leer'
};


const atListasDinamicasDto = {
    guardarEn: 'guardar-en'
};

const atCriterio = {
    contiene: 'contiene',
    comienza: 'comienza',
    igual: 'igual'
};

const atNavegar = {
    navegarAlCrud: 'navegar-al-crud',
    idRestrictor: atControl.restrictor,
    orden: 'orden'
};


const TagName = {
    input: "input",
    select: "select"
};

const ClaseCss = {
    crtlValido: "propiedad-valida",
    crtlNoValido: "propiedad-no-valida",
    propiedad: "propiedad",
    propiedadId: "propiedad-id",
    divVisible: "div-visible",
    divNoVisible: "div-no-visible",
    obligatorio: "obligatorio",
    ordenAscendente: "ordenada-ascendente",
    ordenDescendente: "ordenada-descendente",
    sinOrden: "ordenada-sin-orden",
    selectorElemento: "selector-de-elemento",
    barraVerde: "barra-verde",
    barraRoja: "barra-roja",
    barraAzul: "barra-azul",
    contenedorModal: "contenedor-modal",
    contenidoModal: 'contenido-modal',
    cabeceraModal: 'contenido-cabecera',
    cuerpoModal: 'contenido-cuerpo',
    pieModal: 'contenido-pie',
    soloLectura: "solo-lectura",
    columnaOculta: "columna-oculta",
    filaDelGrid: "cuerpo-datos-tbody-tr",
    cuerpoDeLaTabla: 'cuerpo-datos-tbody',
    sinCapaDeBloqueo: "sin-capa-de-bloqueo",
    conCapaDeBloqueo: "con-capa-de-bloqueo"
};

const Ajax = {
    EndPoint: {
        Crear: "epCrearElemento",
        LeerGridEnHtml: "epLeerGridHtml",
        LeerDatosParaElGrid: "epLeerDatosParaElGrid",
        SolicitarMenuEnHtml: "epSolicitarMenuHtml",
        LeerPorId: "epLeerPorId",
        Modificar: "epModificarPorId",
        Borrar: "epBorrarPorId",
        RecargarModalEnHtml: "epRecargarModalEnHtml",
        Leer: "epLeerParaSelector",
        CargarLista: "epCargarLista",
        CargaDinamica: "epCargaDinamica",
        SubirArchivo: "epSubirArchivo",
        CrearRelaciones: "epCrearRelaciones",
        LeerModoDeAccesoAlNegocio: "epLeerModoDeAccesoAlNegocio",
        LeerModoDeAccesoAlElemento: "epLeerModoDeAccesoAlElemento"
    },
    EpDeAcceso: {
        ReferenciarFoto: "epReferenciarFoto",
        ValidarAcceso: "epValidarAcceso"
    },
    Param: {
        elementoJson: "elementoJson",
        idModal: "idModal",
        idGrid: "idGrid",
        modo: "modo",
        accion: "accion",
        posicion: "posicion",
        cantidad: "cantidad",
        filtro: "filtro",
        orden: "orden",
        usuario: "usuario",
        id: "id",
        parametros: "parametrosJson",
        idsJson: "idsJson",
        claseElemento: "claseElemento",
        fichero: "fichero",
        rutaDestino: "rutaDestino",
        extensiones: "extensionesValidas",
        login: "login",
        password: "password",
        negocio: "negocio"
    },
    Callejero: {
        Importacion: 'importarCallejero',
        accion: {
            importar: 'epImportarCallejero'
        }
    },
    TrabajosSometidos: {
        rutaTu: 'TrabajosDeUsuario',
        accion: {
            iniciar: 'epIniciarTrabajoDeUsuario',
            bloquear: 'epBloquearTrabajoDeUsuario',
            desbloquear: 'epDesbloquearTrabajoDeUsuario',
            resometer: 'epResometerTrabajoDeUsuario'
        }
    },
    Usuarios: {
        ruta: 'usuarios',
        accion: {
            LeerUsuarioDeConexion: 'epLeerUsuarioDeConexion'
        }
    },
    jsonResultError: 1,
    jsonResultOk: 0,
    jsonUndefined: undefined,
    eventoLoad: "load",
    eventoError: "error",
    eventoProgreso: "progress"
};

const LiteralEdt = {
    idCtrlCantidad: "nav_2_reg"
};

const LiteralMnt = {
    posicion: "posicion",
    postfijoDeCheckDeSeleccion: ".chksel",
    idCuerpoDePagina: "cuerpo-de-pagina"
};

const Evento = {
    ModalSeleccion: {
        Abrir: "abrir-modal-seleccion",
        Seleccionar: "seleccionar-elementos",
        Cerrar: "cerrar-modal-seleccion",
        Buscar: "buscar-elementos",
        FilaPulsada: "fila-pulsada",
        ObtenerAnteriores: "obtener-anteriores",
        ObtenerSiguientes: "obtener-siguientes",
        ObtenerUltimos: "obtener-ultimos",
        OrdenarPor: "ordenar-por",
        MostrarSoloSeleccionadas: "mostrar-solo-seleccionadas",
        TeclaPulsada: "tecla-pulsada"
    },
    ModalParaRelacionar: {
        Relacionar: "nuevas-relaciones",
        Cerrar: "cerrar-relacionar",
        Buscar: "buscar-elementos",
        FilaPulsada: "fila-pulsada",
        ObtenerAnteriores: "obtener-anteriores",
        ObtenerSiguientes: "obtener-siguientes",
        ObtenerUltimos: "obtener-ultimos",
        OrdenarPor: "ordenar-por",
        MostrarSoloSeleccionadas: "mostrar-solo-seleccionadas",
        TeclaPulsada: "tecla-pulsada"
    },
    ModalParaConsultaDeRelaciones: {
        Cerrar: "cerrar-consulta",
        Buscar: "buscar-elementos",
        FilaPulsada: "fila-pulsada",
        ObtenerAnteriores: "obtener-anteriores",
        ObtenerSiguientes: "obtener-siguientes",
        ObtenerUltimos: "obtener-ultimos",
        OrdenarPor: "ordenar-por",
        MostrarSoloSeleccionadas: "mostrar-solo-seleccionadas",
        TeclaPulsada: "tecla-pulsada"
    },
    ModalCreacion: {
        Crear: "crear-elemento",
        Cerrar: "cerrar-modal",
    },
    ModalEdicion: {
        Modificar: "modificar-elemento",
        Cerrar: "cerrar-modal",
    },
    ModalBorrar: {
        Borrar: "borrar-elemento",
        Cerrar: "cerrar-modal-de-borrado",
    },
    ListaDinamica: {
        Cargar: 'cargar-lista-dinamica',
        Seleccionar: 'seleccionar--lista-dinamica'
    },
    Mnt: {
        Crear: "crear-elemento",
        Editar: "editar-elemento",
        Borrar: "eliminar-elemento",
        Relacionar: "relacionar-elementos",
        Dependencias: "gestionar-dependencias",
        AbrirModalParaRelacionar: "abrir-modal-para-relacionar",
        AbrirModalParaConsultarRelaciones: "abrir-modal-para-consultar-relaciones",
        Buscar: "buscar-elementos",
        ObtenerAnteriores: "obtener-anteriores",
        ObtenerSiguientes: "obtener-siguientes",
        ObtenerUltimos: "obtener-ultimos",
        OrdenarPor: "ordenar-por",
        FilaPulsada: "fila-pulsada",
        CambiarSelector: "cambiar-selector",
        OcultarMostrarFiltro: "ocultar-mostrar-filtro",
        OcultarMostrarBloque: "ocultar-mostrar-bloque",
        MostrarSoloSeleccionadas: "mostrar-solo-seleccionadas",
        TeclaPulsada: "tecla-pulsada"
    },
    Creacion: {
        Crear: "nuevo-elemento",
        Cerrar: "cerrar-creacion",
    },
    Edicion: {
        Modificar: "modificar-elemento",
        Cerrar: "cancelar-edicion",
        MostrarPrimero: "mostrar-primero",
        MostrarAnterior: "mostrar-anterior",
        MostrarSiguiente: "mostrar-siguiente",
        MostrarUltimo: "mostrar-ultimo",
    },
    Formulario: {
        Aceptar: "aceptar",
        Cerrar: "cerrar",
        OcultarMostrarBloque: "ocultar-mostrar-bloque",
    },
    Expansores: {
        OcultarMostrarBloque: "ocultar-mostrar-bloque",
        NavegarDesdeEdicion: "navegar-desde-edicion"
    },
    TrabajoDeUsuario: {
        iniciar: "iniciar-trabajo",
        bloquear: "bloquear-trabajo",
        desbloquear: "desbloquear-trabajo",
        resometer: "resometer-trabajo",
        traza: "traza-trabajo",
        errores: "errores-trabajo"
    }
};

const TipoControl = {
    Tipo: atControl.tipo,
    Editor: "editor",
    Check: "check",
    Selector: "selector",
    ListaDeElementos: "lista-de-elemento",
    SelectorDeFecha: "selector-de-fecha",
    SelectorDeFechaHora: "selector-de-fecha-hora",
    AreaDeTexto: "area-de-texto",
    ListaDinamica: "lista-dinamica",
    Archivo: "archivo",
    VisorDeArchivo: "visor-archivo",
    UrlDeArchivo: "url-archivo",
    restrictorDeFiltro: "restrictor-filtro",
    restrictorDeEdicion: "restrictor-edicion",
    opcion: 'opcion'
};

const ClaseDeOpcioDeMenu = {
    DeElemento: "opcion-menu-de-elemento",
    DeVista: "opcion-menu-de-vista",
    Basico: "opcion-menu-basica",
};


const ModoTrabajo = {
    creando: "creando",
    editando: "editando",
    consultando: "consultando",
    copiando: "copiando",
    borrando: "borrando",
    mantenimiento: "mantenimiento"
};


const Relaciones = {
    roles: 'RolDto',
    puestos: 'PuestoDto'
};

const Variables = {
    Usuario: {
        restrictor: "idusuario",
        Usuario: "nombre-usuario",
    },
    Puesto: {
        restrictor: "idpuesto",
        puesto: "nombre-puesto",
    }
};

const Sesion = {
    historial: "historial",
    restrictor: "restrictor",
    restrictores: "restrictores",
    idSeleccionado: "idSeleccionado",
    paginaDestino: "pagina-destino",
    paginaActual: "pagina-actual",
    urlActual: "url-actual"
};

const GestorDeEventos = {
    deSeleccion: "Crud.EventosModalDeSeleccion",
    deCrearRelaciones: "Crud.EventosModalDeCrearRelaciones",
    deConsultaDeRelaciones: "Crud.EventosModalDeConsultaDeRelaciones",
    delMantenimiento: "Crud.EventosDelMantenimiento",
    deListaDinamica: "Crud.EventosDeListaDinamica"
};

