﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Enumerados;
using ModeloDeDto;
using MVCSistemaDeElementos.Descriptores;
using ServicioDeDatos.Seguridad;

namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorDeExportacion<TElemento> : ControlHtml where TElemento : ElementoDto
    {
        public DescriptorDeCrud<TElemento> Crud => (DescriptorDeCrud<TElemento>)Padre;
        public DescriptorDeMantenimiento<TElemento> Mnt => Crud.Mnt;
        public DescriptorDeExportacion(DescriptorDeCrud<TElemento> crud)
        : base(
          padre: crud,
          id: $"{crud.Id}_{enumTipoControl.pnlExportacion.Render()}",
          etiqueta: "Selección de exportación",
          propiedad: null,
          ayuda: null,
          posicion: null
        )
        {
            Tipo = enumTipoControl.pnlExportacion;
        }

        public string RenderDeExportacion()
        {
            return RenderControl();
        }

        public override string RenderControl()
        {
                var htmlModal = RenderizarModal(
                    idHtml: IdHtml
                    , controlador: Crud.Controlador
                    , tituloH2: Etiqueta
                    , cuerpo: cuerpoDeExportacion()
                    , idOpcion: $"{IdHtml}-exportar"
                    , opcion: Crud.NegocioActivo ? "Exportar" : ""
                    , accion: Crud.NegocioActivo ? $"Crud.{GestorDeEventos.EventosModalDeExportacion}('{TipoDeAccionDeExportar.Exportar}','{IdHtml}')" : ""
                    , cerrar: $"Crud.{GestorDeEventos.EventosModalDeExportacion}('{TipoDeAccionDeExportar.Cerrar}','{IdHtml}')"
                    , navegador: ""
                    , claseBoton: enumCssOpcionMenu.DeElemento
                    , permisosNecesarios: enumModoDeAccesoDeDatos.Consultor);

            return htmlModal;
        }

        private string cuerpoDeExportacion()
        {
            var htmlCuerpo = $@"<div id=¨{IdHtml}_cuerpo_contenedor¨ class=¨{enumCssExportacion.Contenedor.Render()}¨>
                                     <div id=¨{IdHtml}_cuerpo_exportacion¨ class=¨{enumCssExportacion.lista.Render()}¨>
                                        {listaDeExportaciones()}
                                     </div>
                                     <div id=¨{IdHtml}_cuerpo_sometido¨ class=¨{enumCssExportacion.sometido.Render()}¨>
                                        {checkDeSometido()}
                                     </div>
                                     <div id=¨{IdHtml}_cuerpo_enviar¨ class=¨{enumCssExportacion.enviar.Render()}¨>
                                        {editorDeEMail()}
                                     </div>
                                </div>";
            return htmlCuerpo;
        }

        private string listaDeExportaciones()
        {
            return RenderListaConEtiquetaEncima(IdHtml, "ExportacionDto", "Nombre", "Plantilla").Replace("Seleccionar ...", "Estandard");
        }
        private string checkDeSometido()
        {
            var accion = $"onClick = ¨Crud.{GestorDeEventos.EventosModalDeExportacion}('{TipoDeAccionDeExportar.PulsarSometer}')¨";
            return RenderCheck(PlantillasHtml.checkDto, $"{IdHtml}_sometido", "", true, "Someter", accion) +
                   RenderCheck(PlantillasHtml.checkDto, $"{IdHtml}_mostradas", "", true, "Las mostradas", accion);
        }

        private string editorDeEMail()
        {
            var idHtmlCorreos = $"{IdHtml}_correos";
            var a = AtributosHtml.AtributosComunes($"div_{idHtmlCorreos}", idHtmlCorreos, "", enumTipoControl.Editor);
            a.Editable = false;
            a.Ayuda = "Indique los correos de e-mail receptores";
            a.Etiqueta = "Indicar los correos del destinatario separados por ;";
            a.AlPerderElFoco = $"onBlur = ¨Crud.{GestorDeEventos.EventosModalDeExportacion}('{TipoDeAccionDeExportar.SalirListaDeCorreos}')¨";

            return RenderEditorConEtiquetaEncima(PlantillasHtml.editorDto, a);
        }
    }
}