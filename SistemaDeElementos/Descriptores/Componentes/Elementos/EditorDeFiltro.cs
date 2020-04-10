﻿namespace MVCSistemaDeElementos.Descriptores
{
    public class EditorFiltro<TElemento> : ControlFiltroHtml
    {
        public EditorFiltro(BloqueDeFitro<TElemento> bloque, string etiqueta, string propiedad, string ayuda, Posicion posicion)
        : base(padre: bloque
              , id: $"{bloque.Id}_{TipoControl.Editor}_{propiedad}"
              , etiqueta
              , propiedad
              , ayuda
              , posicion
              )
        {
            Tipo = TipoControl.Editor;
            Criterio = TipoCriterio.contiene.ToString();
            bloque.AnadirControl(this);
        }

        public override string RenderControl()
        {
            return RenderEditor();
        }

        public string RenderEditor()
        {
            return $@"<div class=¨input-group mb-3¨>
                         <input id=¨{IdHtml}¨ type = ¨text¨ class=¨form-control¨ {base.RenderAtributos()}  placeholder=¨{Ayuda}¨>
                      </div>
                  ";
        }
    }

}