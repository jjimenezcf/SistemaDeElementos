﻿using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ServicioDeDatos;
using GestorDeElementos;
using Microsoft.EntityFrameworkCore;
using ServicioDeDatos.TrabajosSometidos;
using ModeloDeDto.TrabajosSometidos;
using System;

namespace GestoresDeNegocio.TrabajosSometidos
{

    public class GestorDeTrazasDeUnTrabajo : GestorDeElementos<ContextoSe, TrazaDeUnTrabajoDtm, TrazaDeUnTrabajoDto>
    {

        public class MapeadorTrazasDeUnTrabajo : Profile
        {
            public MapeadorTrazasDeUnTrabajo()
            {
                CreateMap<TrazaDeUnTrabajoDtm, TrazaDeUnTrabajoDto>()
                .ForMember(dto => dto.TrabajoDeUsuario, dtm => dtm.MapFrom(x => $"({x.TrabajoDeUsuario.Sometedor.Login})- {x.TrabajoDeUsuario.Trabajo.Nombre}"));


                CreateMap<TrazaDeUnTrabajoDto, TrazaDeUnTrabajoDtm>()
                .ForMember(dtm => dtm.TrabajoDeUsuario, dto => dto.Ignore());
            }
        }

        public GestorDeTrazasDeUnTrabajo(ContextoSe contexto, IMapper mapeador)
        : base(contexto, mapeador)
        {

        }

        public static GestorDeTrazasDeUnTrabajo Gestor(ContextoSe contexto, IMapper mapeador)
        {
            return new GestorDeTrazasDeUnTrabajo(contexto, mapeador);
        }


        public TrazaDeUnTrabajoDtm CrearTraza(TrabajoDeUsuarioDtm tu, string Traza)
        {
            var t = new TrazaDeUnTrabajoDtm();
            t.IdTrabajoDeUsuario = tu.Id;
            t.Traza = Traza;
            t.Fecha = DateTime.Now;
            return PersistirRegistro(t, new ParametrosDeNegocio(TipoOperacion.Insertar));
        }


        protected override IQueryable<TrazaDeUnTrabajoDtm> AplicarJoins(IQueryable<TrazaDeUnTrabajoDtm> registros, List<ClausulaDeFiltrado> filtros, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros)
        {
            registros = base.AplicarJoins(registros, filtros, joins, parametros);
            registros = registros.Include(p => p.TrabajoDeUsuario);
            registros = registros.Include(p => p.TrabajoDeUsuario.Sometedor);
            registros = registros.Include(p => p.TrabajoDeUsuario.Trabajo);
            return registros;
        }
        internal static void AnotarTraza(ContextoSe contextoTu, TrabajoDeUsuarioDtm tu, string mensaje)
        {
            var gestorTraza = Gestor(contextoTu, contextoTu.Mapeador);
            gestorTraza.CrearTraza(tu, mensaje);
        }
    }
}
