﻿using Gestor.Elementos;
using Gestor.Elementos.Universitario.ModeloBd;
using Gestor.Elementos.Universitario.ContextosDeBd;
using Gestor.Elementos.Universitario.ModeloIu;
using System.Reflection;

namespace Gestor.Elementos.Universitario
{
    public class GestorDeInscripciones : GestorDeElementos<ContextoUniversitario, RegistroDeInscripcion, ElementoInscripcionesDeUnEstudiante>
    {

        public GestorDeInscripciones(ContextoUniversitario contexto)
            : base(contexto)
        {
        }



        protected override RegistroDeInscripcion LeerConDetalle(int Id)
        {
            return null;
        }



        protected override void MapearDetalleParaLaIu(RegistroDeInscripcion registroDeInscripcion, ElementoInscripcionesDeUnEstudiante elementoInscripcion )
        {

        }


        protected override void MapearElemento(RegistroDeInscripcion registro, ElementoInscripcionesDeUnEstudiante elemento, PropertyInfo propiedad)
        {
            if (propiedad.Name == elemento.PropiedadCurso)
            {
                var gestor = new GestorDeCursos(_Contexto);
                elemento.Curso = gestor.MapearElemento(registro.Curso);
            }

            //if (propiedad.Name == "Estudiante")
            //{
            //    var gestor = new GestorDeEstudiantes(_Contexto);
            //    elemento.Estudiante = gestor.MapearElemento(registro.Estudiante);
            //}

        }

    }
}
