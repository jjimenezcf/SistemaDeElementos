﻿using UtilidadesParaIu;
using MVCSistemaDeElementos.Controllers;
using ModeloDeDto.TrabajosSometidos;
using ServicioDeDatos.Seguridad;

namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorDeTrabajosDeUsuario : DescriptorDeCrud<TrabajoDeUsuarioDto>
    {
        public class AccionesDeTu : AccionDeMenuMnt
        {
            const string desbloquear = "desbloquear-trabajo";
            const string bloquear = "bloquear-trabajo";
            const string iniciar = "iniciar-trabajo";
            const string resometer = "resometer-trabajo";
            public AccionesDeTu(string accion, string ayuda, bool permiteMultiSeleccion)
            : base(accion, enumCssOpcionMenu.DeElemento, ayuda)
            {
                PermiteMultiSeleccion = permiteMultiSeleccion;
            }

            public static AccionesDeTu Desbloquear => new AccionesDeTu(desbloquear, "Desbloquear un trabajo", true);
            public static AccionesDeTu Bloquear => new AccionesDeTu(bloquear, "Bloquear un trabajo", true);
            public static AccionesDeTu Iniciar => new AccionesDeTu(iniciar, "Ejecutar un trabajo", false);
            public static AccionesDeTu Resometer => new AccionesDeTu(resometer, "Resometer un trabajo", false);

            public override string RenderAccion()
            {
                return $"javascript:TrabajosSometido.Eventos('{TipoDeAccion}','')";
            }
        }

        public DescriptorDeTrabajosDeUsuario(ModoDescriptor modo)
        : base(controlador: nameof(TrabajosDeUsuarioController)
               , vista: $"{nameof(TrabajosDeUsuarioController.CrudDeTrabajosDeUsuario)}"
               , modo: modo
               , rutaBase: "TrabajosSometido")
        {
            var opcion = new OpcionDeMenu<TrabajoDeUsuarioDto>(Mnt.ZonaMenu.Menu, AccionesDeTu.Iniciar, $"Ejecutar", enumModoDeAccesoDeDatos.Gestor);
            Mnt.ZonaMenu.Menu.Add(opcion);

            var opcionBloquear = new OpcionDeMenu<TrabajoDeUsuarioDto>(Mnt.ZonaMenu.Menu, AccionesDeTu.Bloquear, $"Bloquear", enumModoDeAccesoDeDatos.Gestor);
            Mnt.ZonaMenu.Menu.Add(opcionBloquear);

            var opcionDesbloquear = new OpcionDeMenu<TrabajoDeUsuarioDto>(Mnt.ZonaMenu.Menu, AccionesDeTu.Desbloquear, $"Desbloquear", enumModoDeAccesoDeDatos.Gestor);
            Mnt.ZonaMenu.Menu.Add(opcionDesbloquear);

            var opcionResometer = new OpcionDeMenu<TrabajoDeUsuarioDto>(Mnt.ZonaMenu.Menu, AccionesDeTu.Resometer, $"Resometer", enumModoDeAccesoDeDatos.Gestor);
            Mnt.ZonaMenu.Menu.Add(opcionResometer);

            AnadirOpcionDeDependencias(Mnt
                                 , controlador: nameof(TrazasDeUnTrabajoController)
                                 , vista: nameof(TrazasDeUnTrabajoController.CrudDeTrazasDeUnTrabajo)
                                 , datosDependientes: nameof(TrazaDeUnTrabajoDto)
                                 , navegarAlCrud: DescriptorDeMantenimiento<TrazaDeUnTrabajoDto>.NombreMnt
                                 , nombreOpcion: "Traza"
                                 , propiedadQueRestringe: nameof(TrabajoDeUsuarioDto.Id)
                                 , propiedadRestrictora: nameof(TrazaDeUnTrabajoDto.IdTrabajoDeUsuario)
                                 , "Consultar la traza del trabajo de usuario");

            AnadirOpcionDeDependencias(Mnt
                                 , controlador: nameof(ErroresDeUnTrabajoController)
                                 , vista: nameof(ErroresDeUnTrabajoController.CrudDeErroresDeUnTrabajo)
                                 , datosDependientes: nameof(ErrorDeUnTrabajoDto)
                                 , navegarAlCrud: DescriptorDeMantenimiento<ErrorDeUnTrabajoDto>.NombreMnt
                                 , nombreOpcion: "Errores"
                                 , propiedadQueRestringe: nameof(TrabajoDeUsuarioDto.Id)
                                 , propiedadRestrictora: nameof(ErrorDeUnTrabajoDto.IdTrabajoDeUsuario)
                                 , "Consultar errores del trabajo de usuario");

            var fltGeneral = Mnt.Filtro.ObtenerBloquePorEtiqueta("General");
            new FiltroEntreFechas<TrabajoDeUsuarioDto>(bloque: fltGeneral,
                                etiqueta: "Planificado",
                                propiedad: nameof(TrabajoDeUsuarioDto.Planificado),
                                ayuda: "trabajos planificados entre",
                                posicion: new Posicion() { fila = 1, columna = 0 });
            new FiltroEntreFechas<TrabajoDeUsuarioDto>(bloque: fltGeneral,
                                etiqueta: "Ejecutado entre",
                                propiedad: nameof(TrabajoDeUsuarioDto.Iniciado),
                                ayuda: "trabajos ejecutados entre",
                                posicion: new Posicion() { fila = 2, columna = 0 });

        }

        public override string RenderControl()
        {
            var render = base.RenderControl();

            render = render +
                   $@"<script src=¨../../js/{RutaBase}/TrabajosDeUsuario.js¨></script>
                      <script>
                         try {{                           
                            TrabajosSometido.CrearCrudDeTrabajosDeUsuario('{Mnt.IdHtml}','{Creador.IdHtml}','{Editor.IdHtml}', '{Borrado.IdHtml}') 
                         }}
                         catch(error) {{                           
                            MensajesSe.Error('Creando el crud', error.message);
                         }}
                      </script>
                    ";
            return render.Render();
        }

    }
}
