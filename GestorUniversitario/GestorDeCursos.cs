﻿using Gestor.Elementos.Universitario.ModeloBd;
using Gestor.Elementos.Universitario.ContextosDeBd;
using Gestor.Elementos.Universitario.ModeloIu;
using System.Reflection;

namespace Gestor.Elementos.Universitario
{
    public class GestorDeCursos : GestorDeElementos<ContextoUniversitario, RegistroDeCurso, ElementoCurso>
    {

        public GestorDeCursos(ContextoUniversitario contexto)
            : base(contexto)
        {
        }
               
        protected override RegistroDeCurso LeerConDetalle(int Id)
        {
            return null;
        }

        protected override void MapearDetalleParaLaIu(RegistroDeCurso registro, ElementoCurso elemento)
        {

        }

        protected override void MapearElemento(RegistroDeCurso registro, ElementoCurso elemento, PropertyInfo propiedad)
        {

        }

    }
}
