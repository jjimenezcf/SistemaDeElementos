﻿using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Utilidades;
using Gestor.Elementos.ModeloIu;

namespace Gestor.Elementos.Entorno
{

    public class GestorDeVistasMvc : GestorDeElementos<CtoEntorno, VistaDtm, VistaMvcDto>
    {

        public class MapearVistasMvc : Profile
        {
            public MapearVistasMvc()
            {
                CreateMap<VistaDtm, VistaMvcDto>();
                CreateMap<VistaMvcDto, VistaDtm>();
            }
        }

        public GestorDeVistasMvc(CtoEntorno contexto, IMapper mapeador)
            : base(contexto, mapeador)
        {

        }

        protected override VistaDtm LeerConDetalle(int Id)
        {
            throw new System.NotImplementedException();
        }


        public static List<VistaMvcDto> VistasMvc()
        {
            var vistasMvc = new List<VistaMvcDto>();

            vistasMvc.Add(new VistaMvcDto { Id = 0, Nombre = "Usuarios", Controlador = "Usuarios", Accion = "Index", Parametros = "" });
            vistasMvc.Add(new VistaMvcDto { Id = 0, Nombre = "Menus", Controlador = "Menus", Accion = "Index", Parametros = "" });

            return vistasMvc;
        }

        public void InicializarVistasMvc()
        {
            var e_vistasMvc = VistasMvc();
            var r_vistasMvc = MapearRegistros(e_vistasMvc, TipoOperacion.Insertar);
            InsertarRegistros(r_vistasMvc);
        }


    }

}
