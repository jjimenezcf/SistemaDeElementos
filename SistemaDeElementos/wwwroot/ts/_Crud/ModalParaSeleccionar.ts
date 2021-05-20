﻿namespace Crud {

    export class ModalParaSeleccionar extends ModalConGrid {

        private _crud: CrudMnt;
        protected get Crud(): CrudMnt {
            return this._crud;
        }

        protected get PropiedadRestrictora(): string {
            return this.Modal.getAttribute(atControl.propiedadRestrictora);
        }

        protected get Restrictor(): HTMLInputElement {
            let propiedadRestrictora: string = this.PropiedadRestrictora;
            if (IsNullOrEmpty(propiedadRestrictora))
                throw new Error(`la modal ${this.IdModal} no tiene definida la ${propiedadRestrictora}`);

            let input: HTMLInputElement = this.ZonaDeFiltro.querySelector(`input[${atControl.propiedad}="${propiedadRestrictora}"]`);
            if (input === null)
                throw new Error(`No se ha definido el control input asociado a la ${propiedadRestrictora}`);

            return input;
        }

        private _selector: HTMLDivElement;

        private get EditorDeFiltro(): HTMLInputElement {
            var idEditorDeFiltro: string = this._selector.getAttribute(atSelectorDeElementos.IdEditorDeFiltro);
            let editorDeFiltro: HTMLInputElement = document.getElementById(idEditorDeFiltro) as HTMLInputElement;
            if (NoDefinida(editorDeFiltro))
                throw new Error(`el editor ${idEditorDeFiltro} no está definido en la zona de filtro de la modal asociada al selector ${this._selector.id}`);
            return editorDeFiltro;
        }

        private get EditorAsociado(): HTMLInputElement {
            return ApiCrud.ObtenerEditorAsociadoAlSelector(this._selector);
        }


        constructor(crudPadre: CrudMnt, idModal: string) {
            super(idModal, document.getElementById(idModal).getAttribute(atControl.crudModal));
            this._crud = crudPadre;
        }

        protected InicializarModalParaSeleccionar(selector: HTMLDivElement) {
            this.InicializarModalConGrid();
            this._selector = selector;
            this.EditorDeFiltro.value = this.EditorAsociado.value;
        }

        public AbrirModalParaSeleccionar(selector: HTMLDivElement) {
            this.InicializarModalParaSeleccionar(selector);
            this.RecargarGrid()
                .then((valor) => {
                    if (!valor) {
                        ApiCrud.CerrarModal(this.Modal);
                        let idModal: string = selector.getAttribute(atSelectorDeElementos.ModalPadre);
                        if (!NoDefinida(idModal)) ApiCrud.AbrirModalPorId(idModal);
                    }
                })
                .catch((valor) => {
                    ApiCrud.CerrarModal(this.Modal);
                    let idModal: string = selector.getAttribute(atSelectorDeElementos.ModalPadre);
                    if (!NoDefinida(idModal)) ApiCrud.AbrirModalPorId(idModal);
                }
                );
        };

        public CerrarModalParaSeleccionar() {
            this.CerrarModalConGrid();
            this._crud.ModalEnviarCorreo_Abrir();
        }


    }
}
