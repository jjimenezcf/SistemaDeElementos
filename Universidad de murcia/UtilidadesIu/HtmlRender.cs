﻿
namespace UtilidadesParaIu
{
    public enum Aliniacion { no_definida, izquierda, centrada, derecha, justificada };
    
    public static class HtmlRender
    {
        public static string Render(this string cadena)
        {
            return cadena.Replace("¨", "\"");
        }
    }
}