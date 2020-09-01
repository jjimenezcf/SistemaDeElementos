﻿using UtilidadesParaIu;
using MVCSistemaDeElementos.Controllers;
using ModeloDeDto.Entorno;
using ModeloDeDto.Seguridad;
using ModeloDeDto;

namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorDeUsuario : DescriptorDeCrud<UsuarioDto>
    {
        DescriptorDePuestosDeUnUsuario descriptorDePuestos = new DescriptorDePuestosDeUnUsuario(ModoDescriptor.Mantenimiento);

        public DescriptorDeUsuario(ModoDescriptor modo)
        : base(controlador: nameof(UsuariosController)
               , vista: $"{nameof(UsuariosController.CrudUsuario)}"
               , modo: modo)
        {
            if (modo == ModoDescriptor.Mantenimiento)
                new SelectorDeFiltro<UsuarioDto, PermisoDto>(
                       padre: new BloqueDeFitro<UsuarioDto>(filtro: Mnt.Filtro, titulo: "Específico", dimension: new Dimension(1, 2)),
                       etiqueta: "Permiso",
                       filtrarPor: UsuariosPor.Permisos,
                       ayuda: "Seleccionar Permiso",
                       posicion: new Posicion() { fila = 0, columna = 0 },
                       paraFiltrar: nameof(PermisoDto.Id),
                       paraMostrar: nameof(PermisoDto.Nombre),
                       crudModal: new DescriptorDePermiso(ModoDescriptor.Seleccion),
                       propiedadDondeMapear: FiltroPor.Nombre.ToString());
            
            BuscarControlEnFiltro(FiltroPor.Nombre).CambiarAtributos(UsuariosPor.NombreCompleto, "Buscar por 'apellido, nombre'");
            RutaVista = "Entorno";

            AnadirOpciondeRelacion(Mnt
                , controlador: nameof(PuestosDeUnUsuarioController)
                , vista: nameof(PuestosDeUnUsuarioController.CrudPuestosDeUnUsuario)
                , relacionarCon: nameof(PuestoDto)
                , navegarAlCrud: DescriptorDeMantenimiento<PuestosDeUnUsuarioDto>.NombreMnt
                , nombreOpcion: "Puestos"
                , propiedadQueRestringe: nameof(UsuarioDto.Id)
                , propiedadRestrictora: nameof(PuestosDeUnUsuarioDto.IdUsuario));

        }

        public override string RenderControl()
        {
            var render = base.RenderControl();

            render = render +
                   $@"<script src=¨../../ts/Entorno/Usuario.js¨></script>
                      <script>
                         try {{                           
                            Entorno.CrearCrudDeUsuarios('{Mnt.IdHtml}','{Creador.IdHtml}','{Editor.IdHtml}', '{Borrado.IdHtml}') 
                         }}
                         catch(error) {{                           
                            Mensaje(TipoMensaje.Error, error);
                         }}
                      </script>
                    ";
            return render.Render();
        }

    }
}
