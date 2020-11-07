﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ServicioDeDatos;

namespace Migraciones.Migrations
{
    [DbContext(typeof(ContextoSe))]
    partial class ContextoUniversitarioModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ServicioDeDatos.Archivos.ArchivoDtm", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasColumnType("INT")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AlmacenadoEn")
                        .IsRequired()
                        .HasColumnName("RUTA")
                        .HasColumnType("VARCHAR(2000)");

                    b.Property<DateTime>("FechaCreacion")
                        .HasColumnName("FECCRE")
                        .HasColumnType("DATETIME");

                    b.Property<DateTime?>("FechaModificacion")
                        .HasColumnName("FECMOD")
                        .HasColumnType("DATETIME");

                    b.Property<int>("IdUsuaCrea")
                        .HasColumnName("IDUSUCREA")
                        .HasColumnType("INT");

                    b.Property<int?>("IdUsuaModi")
                        .HasColumnName("IDUSUMODI")
                        .HasColumnType("INT");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnName("NOMBRE")
                        .HasColumnType("VARCHAR(250)");

                    b.HasKey("Id");

                    b.HasIndex("IdUsuaCrea")
                        .HasName("I_ARCHIVO_IDUSUCREA");

                    b.HasIndex("IdUsuaModi")
                        .HasName("I_ARCHIVO_IDUSUMODI");

                    b.HasIndex("Nombre")
                        .HasName("I_ARCHIVO_NOMBRE");

                    b.ToTable("ARCHIVO","SISDOC");
                });

            modelBuilder.Entity("ServicioDeDatos.Entorno.ArbolDeMenuDtm", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasColumnType("INT")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Activo")
                        .HasColumnName("ACTIVO")
                        .HasColumnType("BIT");

                    b.Property<string>("Controlador")
                        .HasColumnName("CONTROLADOR")
                        .HasColumnType("VARCHAR(250)");

                    b.Property<string>("Descripcion")
                        .HasColumnName("DESCRIPCION")
                        .HasColumnType("VARCHAR(MAX)");

                    b.Property<string>("Icono")
                        .IsRequired()
                        .HasColumnName("ICONO")
                        .HasColumnType("VARCHAR(250)");

                    b.Property<int?>("IdPadre")
                        .HasColumnName("IDPADRE")
                        .HasColumnType("INT");

                    b.Property<int?>("IdVistaMvc")
                        .HasColumnName("IDVISTA_MVC")
                        .HasColumnType("INT");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnName("NOMBRE")
                        .HasColumnType("VARCHAR(250)");

                    b.Property<int>("Orden")
                        .HasColumnName("ORDEN")
                        .HasColumnType("INT");

                    b.Property<string>("Padre")
                        .HasColumnName("PADRE")
                        .HasColumnType("VARCHAR(250)");

                    b.Property<string>("Vista")
                        .HasColumnName("VISTA")
                        .HasColumnType("VARCHAR(250)");

                    b.Property<string>("accion")
                        .HasColumnName("ACCION")
                        .HasColumnType("VARCHAR(250)");

                    b.Property<string>("parametros")
                        .HasColumnName("PARAMETROS")
                        .HasColumnType("VARCHAR(250)");

                    b.HasKey("Id");

                    b.ToTable("MENU_SE","ENTORNO");
                });

            modelBuilder.Entity("ServicioDeDatos.Entorno.MenuDtm", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasColumnType("INT")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Activo")
                        .HasColumnName("ACTIVO")
                        .HasColumnType("BIT");

                    b.Property<string>("Descripcion")
                        .HasColumnName("DESCRIPCION")
                        .HasColumnType("VARCHAR(MAX)");

                    b.Property<string>("Icono")
                        .IsRequired()
                        .HasColumnName("ICONO")
                        .HasColumnType("VARCHAR(250)");

                    b.Property<int?>("IdPadre")
                        .HasColumnName("IDPADRE")
                        .HasColumnType("INT");

                    b.Property<int?>("IdVistaMvc")
                        .HasColumnName("IDVISTA_MVC")
                        .HasColumnType("INT");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnName("NOMBRE")
                        .HasColumnType("VARCHAR(250)");

                    b.Property<int>("Orden")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ORDEN")
                        .HasColumnType("INT")
                        .HasDefaultValue(0);

                    b.HasKey("Id");

                    b.HasIndex("IdPadre");

                    b.HasIndex("IdVistaMvc");

                    b.HasIndex("Nombre")
                        .IsUnique();

                    b.ToTable("MENU","ENTORNO");
                });

            modelBuilder.Entity("ServicioDeDatos.Entorno.PermisosDeUnUsuarioDtm", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasColumnType("INT")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("IdPermiso")
                        .HasColumnName("IDPERMISO")
                        .HasColumnType("INT");

                    b.Property<int>("IdUsuario")
                        .HasColumnName("IDUSUA")
                        .HasColumnType("INT");

                    b.Property<string>("Origen")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnName("ORIGEN")
                        .HasColumnType("VARCHAR(MAX)")
                        .HasComputedColumnSql("SEGURIDAD.OBTENER_ORIGEN(idusua,idpermiso)");

                    b.HasKey("Id");

                    b.HasIndex("IdPermiso");

                    b.HasIndex("IdUsuario");

                    b.ToTable("USU_PERMISO","ENTORNO");
                });

            modelBuilder.Entity("ServicioDeDatos.Entorno.UsuarioDtm", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasColumnType("INT")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Alta")
                        .HasColumnName("F_ALTA")
                        .HasColumnType("DATE");

                    b.Property<string>("Apellido")
                        .IsRequired()
                        .HasColumnName("APELLIDO")
                        .HasColumnType("VARCHAR(250)");

                    b.Property<bool>("EsAdministrador")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ADMINISTRADOR")
                        .HasColumnType("BIT")
                        .HasDefaultValue(false);

                    b.Property<int?>("IdArchivo")
                        .HasColumnName("IDARCHIVO")
                        .HasColumnType("INT");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnName("LOGIN")
                        .HasColumnType("VARCHAR(50)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnName("NOMBRE")
                        .HasColumnType("VARCHAR(50)");

                    b.Property<string>("eMail")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnName("EMAIL")
                        .HasColumnType("VARCHAR(50)")
                        .HasDefaultValue("pendiente@se.com");

                    b.Property<string>("password")
                        .IsRequired()
                        .HasColumnName("PASSWORD")
                        .HasColumnType("VARCHAR(250)");

                    b.HasKey("Id");

                    b.HasIndex("IdArchivo")
                        .HasName("I_USUARIO_IDARCHIVO");

                    b.HasIndex("Login")
                        .IsUnique()
                        .HasName("IX_USUARIO");

                    b.ToTable("USUARIO","ENTORNO");
                });

            modelBuilder.Entity("ServicioDeDatos.Entorno.VariableDtm", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasColumnType("INT")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Descripcion")
                        .HasColumnName("DESCRIPCION")
                        .HasColumnType("VARCHAR(MAX)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnName("NOMBRE")
                        .HasColumnType("VARCHAR(50)");

                    b.Property<string>("Valor")
                        .IsRequired()
                        .HasColumnName("VALOR")
                        .HasColumnType("VARCHAR(250)");

                    b.HasKey("Id");

                    b.ToTable("VARIABLE","ENTORNO");
                });

            modelBuilder.Entity("ServicioDeDatos.Entorno.VistaMvcDtm", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasColumnType("INT")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Accion")
                        .IsRequired()
                        .HasColumnName("ACCION")
                        .HasColumnType("VARCHAR(250)");

                    b.Property<string>("Controlador")
                        .IsRequired()
                        .HasColumnName("CONTROLADOR")
                        .HasColumnType("VARCHAR(250)");

                    b.Property<int>("IdPermiso")
                        .HasColumnName("IDPERMISO")
                        .HasColumnType("INT");

                    b.Property<bool>("MostrarEnModal")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("MODAL")
                        .HasColumnType("BIT")
                        .HasDefaultValue(false);

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnName("NOMBRE")
                        .HasColumnType("VARCHAR(250)");

                    b.Property<string>("Parametros")
                        .HasColumnName("PARAMETROS")
                        .HasColumnType("VARCHAR(250)");

                    b.HasKey("Id");

                    b.HasIndex("IdPermiso")
                        .IsUnique()
                        .HasName("IX_VISTA_MVC_IDPERMISO");

                    b.HasIndex("Nombre")
                        .IsUnique()
                        .HasName("IX_VARIABLE");

                    b.HasIndex("Controlador", "Accion", "Parametros")
                        .IsUnique()
                        .HasName("IX_VISTA_MVC")
                        .HasFilter("[PARAMETROS] IS NOT NULL");

                    b.ToTable("VISTA_MVC","ENTORNO");
                });

            modelBuilder.Entity("ServicioDeDatos.Seguridad.ClasePermisoDtm", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasColumnType("INT")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnName("NOMBRE")
                        .HasColumnType("VARCHAR(30)");

                    b.HasKey("Id");

                    b.HasIndex("Nombre")
                        .IsUnique()
                        .HasName("I_CLASE_PERMISO_NOMBRE");

                    b.ToTable("CLASE_PERMISO","SEGURIDAD");
                });

            modelBuilder.Entity("ServicioDeDatos.Seguridad.PermisoDtm", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasColumnType("INT")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("IdClase")
                        .HasColumnName("IDCLASE")
                        .HasColumnType("INT");

                    b.Property<int>("IdTipo")
                        .HasColumnName("IDTIPO")
                        .HasColumnType("INT");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnName("NOMBRE")
                        .HasColumnType("VARCHAR(250)");

                    b.HasKey("Id");

                    b.HasIndex("IdTipo");

                    b.HasIndex("Nombre")
                        .IsUnique()
                        .HasName("I_PERMISO_NOMBRE");

                    b.HasIndex("IdClase", "IdTipo")
                        .HasName("I_PERMISO_IDCLASE_IDTIPO");

                    b.ToTable("PERMISO","SEGURIDAD");
                });

            modelBuilder.Entity("ServicioDeDatos.Seguridad.PermisosDeUnRolDtm", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasColumnType("INT")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("IdPermiso")
                        .HasColumnName("IDPERMISO")
                        .HasColumnType("INT");

                    b.Property<int>("IdRol")
                        .HasColumnName("IDROL")
                        .HasColumnType("INT");

                    b.HasKey("Id");

                    b.HasAlternateKey("IdRol", "IdPermiso")
                        .HasName("AK_ROL_PERMISO");

                    b.HasIndex("IdPermiso");

                    b.ToTable("ROL_PERMISO","SEGURIDAD");
                });

            modelBuilder.Entity("ServicioDeDatos.Seguridad.PuestoDtm", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasColumnType("INT")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Descripcion")
                        .HasColumnName("DESCRIPCION")
                        .HasColumnType("VARCHAR(MAX)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnName("NOMBRE")
                        .HasColumnType("VARCHAR(250)");

                    b.HasKey("Id");

                    b.HasIndex("Nombre")
                        .IsUnique()
                        .HasName("I_PUESTO_NOMBRE");

                    b.ToTable("PUESTO","SEGURIDAD");
                });

            modelBuilder.Entity("ServicioDeDatos.Seguridad.PuestosDeUnUsuarioDtm", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasColumnType("INT")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("IdPuesto")
                        .HasColumnName("IDPUESTO")
                        .HasColumnType("INT");

                    b.Property<int>("IdUsuario")
                        .HasColumnName("IDUSUA")
                        .HasColumnType("INT");

                    b.HasKey("Id");

                    b.HasIndex("IdPuesto")
                        .HasName("I_USU_PUESTO_IDPUESTO");

                    b.HasIndex("IdUsuario")
                        .HasName("I_USU_PUESTO_IDUSUA");

                    b.HasIndex("IdPuesto", "IdUsuario")
                        .IsUnique()
                        .HasName("I_USU_PUESTO_IDPUESTO_IDUSUA");

                    b.ToTable("USU_PUESTO","SEGURIDAD");
                });

            modelBuilder.Entity("ServicioDeDatos.Seguridad.RolDtm", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasColumnType("INT")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Descripcion")
                        .HasColumnName("DESCRIPCION")
                        .HasColumnType("VARCHAR(MAX)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnName("NOMBRE")
                        .HasColumnType("VARCHAR(250)");

                    b.HasKey("Id");

                    b.HasIndex("Nombre")
                        .IsUnique()
                        .HasName("I_ROL_NOMBRE");

                    b.ToTable("ROL","SEGURIDAD");
                });

            modelBuilder.Entity("ServicioDeDatos.Seguridad.RolesDeUnPuestoDtm", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasColumnType("INT")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("IdRol")
                        .HasColumnName("IDROL")
                        .HasColumnType("INT");

                    b.Property<int>("idPuesto")
                        .HasColumnName("IDPUESTO")
                        .HasColumnType("INT");

                    b.HasKey("Id");

                    b.HasAlternateKey("IdRol", "idPuesto")
                        .HasName("AK_ROL_PUESTO");

                    b.HasIndex("idPuesto");

                    b.ToTable("ROL_PUESTO","SEGURIDAD");
                });

            modelBuilder.Entity("ServicioDeDatos.Seguridad.TipoPermisoDtm", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasColumnType("INT")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnName("NOMBRE")
                        .HasColumnType("VARCHAR(30)");

                    b.HasKey("Id");

                    b.HasIndex("Nombre")
                        .IsUnique()
                        .HasName("I_TIPO_PERMISO_NOMBRE");

                    b.ToTable("TIPO_PERMISO","SEGURIDAD");
                });

            modelBuilder.Entity("ServicioDeDatos.Archivos.ArchivoDtm", b =>
                {
                    b.HasOne("ServicioDeDatos.Entorno.UsuarioDtm", "UsuarioCreador")
                        .WithMany()
                        .HasForeignKey("IdUsuaCrea")
                        .HasConstraintName("FK_ARCHIVO_IDUSUCREA")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("ServicioDeDatos.Entorno.UsuarioDtm", "UsuarioModificador")
                        .WithMany()
                        .HasForeignKey("IdUsuaModi")
                        .HasConstraintName("FK_ARCHIVO_IDUSUMODI")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("ServicioDeDatos.Entorno.MenuDtm", b =>
                {
                    b.HasOne("ServicioDeDatos.Entorno.MenuDtm", "Padre")
                        .WithMany("Submenus")
                        .HasForeignKey("IdPadre")
                        .HasConstraintName("FK_MENU_IDPADRE");

                    b.HasOne("ServicioDeDatos.Entorno.VistaMvcDtm", "VistaMvc")
                        .WithMany("Menus")
                        .HasForeignKey("IdVistaMvc")
                        .HasConstraintName("FK_MENU_IDVISTA_MVC");
                });

            modelBuilder.Entity("ServicioDeDatos.Entorno.PermisosDeUnUsuarioDtm", b =>
                {
                    b.HasOne("ServicioDeDatos.Seguridad.PermisoDtm", "Permiso")
                        .WithMany("Usuarios")
                        .HasForeignKey("IdPermiso")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ServicioDeDatos.Entorno.UsuarioDtm", "Usuario")
                        .WithMany("Permisos")
                        .HasForeignKey("IdUsuario")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ServicioDeDatos.Entorno.UsuarioDtm", b =>
                {
                    b.HasOne("ServicioDeDatos.Archivos.ArchivoDtm", "Archivo")
                        .WithMany()
                        .HasForeignKey("IdArchivo")
                        .HasConstraintName("FK_USUARIO_IDARCHIVO")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("ServicioDeDatos.Entorno.VistaMvcDtm", b =>
                {
                    b.HasOne("ServicioDeDatos.Seguridad.PermisoDtm", "Permiso")
                        .WithMany()
                        .HasForeignKey("IdPermiso")
                        .HasConstraintName("FK_VISTA_MVC_IDPERMISO")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("ServicioDeDatos.Seguridad.PermisoDtm", b =>
                {
                    b.HasOne("ServicioDeDatos.Seguridad.ClasePermisoDtm", "Clase")
                        .WithMany("Permisos")
                        .HasForeignKey("IdClase")
                        .HasConstraintName("FK_PERMISO_IDCLASE")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("ServicioDeDatos.Seguridad.TipoPermisoDtm", "Tipo")
                        .WithMany("Permisos")
                        .HasForeignKey("IdTipo")
                        .HasConstraintName("FK_PERMISO_IDTIPO")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("ServicioDeDatos.Seguridad.PermisosDeUnRolDtm", b =>
                {
                    b.HasOne("ServicioDeDatos.Seguridad.PermisoDtm", "Permiso")
                        .WithMany("Roles")
                        .HasForeignKey("IdPermiso")
                        .HasConstraintName("FK_ROL_PERMISO_IDPERMISO")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("ServicioDeDatos.Seguridad.RolDtm", "Rol")
                        .WithMany("Permisos")
                        .HasForeignKey("IdRol")
                        .HasConstraintName("FK_ROL_PERMISO_IDROL")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("ServicioDeDatos.Seguridad.PuestosDeUnUsuarioDtm", b =>
                {
                    b.HasOne("ServicioDeDatos.Seguridad.PuestoDtm", "Puesto")
                        .WithMany("Usuarios")
                        .HasForeignKey("IdPuesto")
                        .HasConstraintName("FK_USU_PUESTO_IDPUESTO")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("ServicioDeDatos.Entorno.UsuarioDtm", "Usuario")
                        .WithMany("Puestos")
                        .HasForeignKey("IdUsuario")
                        .HasConstraintName("FK_USU_PUESTO_IDUSUA")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("ServicioDeDatos.Seguridad.RolesDeUnPuestoDtm", b =>
                {
                    b.HasOne("ServicioDeDatos.Seguridad.RolDtm", "Rol")
                        .WithMany("Puestos")
                        .HasForeignKey("IdRol")
                        .HasConstraintName("FK_ROL_PUESTO_IDROL")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("ServicioDeDatos.Seguridad.PuestoDtm", "Puesto")
                        .WithMany("Roles")
                        .HasForeignKey("idPuesto")
                        .HasConstraintName("FK_ROL_PUESTO_IDPUESTO")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
