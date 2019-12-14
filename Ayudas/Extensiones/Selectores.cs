﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Extensiones
{
    public class SelectorModal
    {
        const string _htmlModalSelector =
            @"
             <div class=¨modal fade¨ id=¨idModal¨ tabindex=¨-1¨ role=¨dialog¨ aria-labelledby=¨exampleModalLabel¨ aria-hidden=¨true¨>
               <div class=¨modal-dialog¨ role=¨document¨>
                 <div class=¨modal-content¨>
                   <div class=¨modal-header¨>
                     <h5 class=¨modal-title¨ id=¨exampleModalLabel¨>titulo</h5>
                   </div>
                   <div class=¨modal-body¨>
                     listaDeElementos
                   </div>
                   <div class=¨modal-footer¨>
                     <button type = ¨button¨ class=¨btn btn-secondary¨ data-dismiss=¨modal¨>Cerrar</button>
                     <button type = ¨button¨ class=¨btn btn-primary¨ data-dismiss=¨modal¨ onclick=¨Seleccionar()¨>Seleccionar</button>
                   </div>
                 </div>
               </div>
             </div>
             ";

        const string _htmlSelector =
              @"<div class=¨input-group mb-3¨>
                   <input id=¨idSelector¨ type = ¨text¨ class=¨form-control¨ placeholder=¨titulo¨ aria-label=¨Curso seleccionado¨ aria-describedby=¨basic-addon2¨>
                   <div class=¨input-group-append¨>
                        <button class=¨btn btn-outline-secondary¨ type=¨button¨ data-toggle=¨modal¨ data-target=¨#idModal¨ >Seleccionar</button>
                   </div>
                </div>
              ";

        const string _funcionDeSeleccion =
                        @"function Seleccionar() {
                             var checkboxes = document.getElementsByName(¨grupoChek_idModal¨);
                             var cont = 0; 

                             for (var x=0; x < checkboxes.length; x++) {
                              if (checkboxes[x].checked) {
                               document.getElementById(¨idSelector¨).value = x;
                               console.log(x);
                              }
                             }
                          };
                         ";

        //@"function Seleccionar() {
        //     document.getElementById(¨idSelector¨).value = 'trigo';
        //  };
        // ";

        private string _titulo;
        private string _idSelector;

        Func<string> _renderElementos;

        public string Id { get; }

        public string jsDeSeleccion { get; set; }
        
        public SelectorModal(string elemento, Func<string> RenderElementos)
        {
            Id = $"SelectorDe{elemento}";

            _titulo = $"Seleccionar {elemento}";
            _idSelector = $"id{elemento}Seleccionado";
            _renderElementos = RenderElementos;
        }

        public string RenderSelector()
        {
            return _htmlSelector
                    .Replace("idModal", Id)
                    .Replace("titulo", _titulo)
                    .Replace("idSelector", _idSelector)
                    .Render();
        }

        public string RenderModal()
        {
            return _htmlModalSelector
                    .Replace("idModal", Id)
                    .Replace("titulo", _titulo)
                    .Replace("listaDeElementos", RenderizarElementos())
                    .Render();
        }
        
        public string ScriptDeSeleccion()
        {
            return _funcionDeSeleccion
                   .Replace("idModal", Id)
                   .Replace("idSelector", _idSelector)
                   .Render();
        }

        private string RenderizarElementos()
        {
            return _renderElementos();
        }

    }
}
