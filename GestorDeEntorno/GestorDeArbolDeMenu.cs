﻿using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Utilidades;

namespace Gestor.Elementos.Entorno
{
    public class GestorDeArbolDeMenu : GestorDeElementos<CtoEntorno, ArbolDeMenuDtm, ArbolDeMenuDto>
    {
        public class MapearMenus : Profile
        {
            public MapearMenus()
            {
                CreateMap<ArbolDeMenuDtm, ArbolDeMenuDto>();                  
            }
        }

        public GestorDeArbolDeMenu(CtoEntorno contexto, IMapper mapeador)
            : base(contexto, mapeador)
        {

        }

        protected override void DespuesDeMapearElemento(ArbolDeMenuDtm registro, ArbolDeMenuDto elemento, ParametrosDeMapeo parametros)
        {
            base.DespuesDeMapearElemento(registro, elemento, parametros);
            elemento.Submenus = new List<ArbolDeMenuDto>();
            elemento.VistaMvc = new VistaMvcDto
            {
                Id = registro.IdVistaMvc.GetValueOrDefault(),
                Nombre = registro.Vista,
                Controlador = registro.Controlador,
                Accion = registro.accion,
                Parametros = registro.parametros
            };
        }

    }
}