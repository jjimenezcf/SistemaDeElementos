﻿namespace ApiDeAjax {

    export enum TipoPeticion {
        Sincrona,
        Asincrona
    }

    function EsAsincrona(valor: TipoPeticion): boolean {
        if (valor === TipoPeticion.Sincrona)
            return false;
        return true;
    }

    export enum ModoPeticion {
        Get,
        Post
    }

    function ParsearModo(modo: ModoPeticion) {
        if (modo === ModoPeticion.Get)
            return 'get';
        return 'post';
    }

    export class DescriptorAjax {
        private _tipoPeticion: TipoPeticion;
        private _modoPeticion: ModoPeticion;
        private _req: XMLHttpRequest;
        private _url: string;


        public nombre: string;
        public datos: any;
        public resultado: ResultadoJson;
        public Error: boolean = false;

        public get Tipo(): TipoPeticion { return this._tipoPeticion; }
        public get Request(): XMLHttpRequest { return this._req; }
        public get Url(): string { return this._url; }
        public get Modo(): ModoPeticion { return this._modoPeticion; }

        public TrasLaPeticion: Function;
        public SiHayError: Function;
        constructor(peticion: string, datos: any, url: string, tipo: TipoPeticion, modo: ModoPeticion, trasLaPeticion: Function, siHayError: Function) {
            this.nombre = peticion;
            this.datos = datos;
            this.resultado = undefined;
            this._tipoPeticion = tipo;
            this._modoPeticion = modo;
            this.Inicializar(url, trasLaPeticion, siHayError);
        }

        private ParsearRespuesta() {
            try {
                this.resultado = JSON.parse(this.Request.response);
            }
            catch
            {
                Mensaje(TipoMensaje.Error, `Error al procesar la respuesta de ${this.nombre}`);
            }
        }

        public Inicializar(url: string, trasLaPeticion: Function, siHayError: Function) {
            this._req = new XMLHttpRequest();
            this._url = url;
            this.TrasLaPeticion = trasLaPeticion;
            this.SiHayError = siHayError;
        }

        public Ejecutar() {
            BlanquearMensaje();
            this.PeticionAjax();  
            if (this.Error) throw `${this.resultado.mensaje}`;
        }

        private PeticionAjax() {

            function RespuestaCorrecta(descriptor: DescriptorAjax) {

                if (EsNula(descriptor.Request.response))
                    descriptor.ErrorEnPeticion();
                else {
                    descriptor.ParsearRespuesta();

                    if (descriptor.resultado === undefined || descriptor.resultado.estado === Ajax.jsonResultError)
                        descriptor.ErrorEnPeticion();
                    else
                        descriptor.DespuesDeLaPeticion();
                }
            }

            function RespuestaErronea() {
                this.ErrorEnPeticion();
            }

            this.Request.addEventListener(Ajax.eventoLoad, () => RespuestaCorrecta(this));
            this.Request.addEventListener(Ajax.eventoError, () => RespuestaErronea());

            this.Request.open(ParsearModo(this.Modo), this.Url, EsAsincrona(this.Tipo));
            this.Request.send();
        }

        private ErrorEnPeticion() {
            this.Error = true;

            if (EsNula(this.Request.response)) 
                return `La peticion ${this.nombre} no se ha podido realizar`;

            let resultado: ResultadoJson = JSON.parse(this.Request.response);
            console.error(resultado.consola);
            if (!EsNula(resultado.mensaje))
                resultado.mensaje = `Error al ejecutar la peticion '${this.nombre}'. ${resultado.mensaje}`;

            if (this.SiHayError)
                this.SiHayError(this, resultado.mensaje);

            

        }

        private DespuesDeLaPeticion() {
            this.resultado = JSON.parse(this.Request.response);

            if (!EsNula(this.resultado.mensaje))
                Mensaje(TipoMensaje.Info, this.resultado.mensaje);

            if (this.TrasLaPeticion)
                this.TrasLaPeticion(this);
        }
    }




}