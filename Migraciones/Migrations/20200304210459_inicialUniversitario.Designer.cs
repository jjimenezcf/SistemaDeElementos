﻿// <auto-generated />
using System;
using Gestor.Elementos.Seguridad;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Migraciones.Migrations
{
    [DbContext(typeof(CtoPermisos))]
    [Migration("20200304210459_inicialUniversitario")]
    partial class inicialUniversitario
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Gestor.Elementos.Permiso.RegistroDeCurso", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasColumnType("INT")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Creditos")
                        .HasColumnName("CREDITOS")
                        .HasColumnType("INT");

                    b.Property<string>("Titulo")
                        .IsRequired()
                        .HasColumnName("TITULO")
                        .HasColumnType("VARCHAR(250)");

                    b.HasKey("Id");

                    b.ToTable("CUR_ELEMENTO","UNIVERSIDAD");
                });

            modelBuilder.Entity("Gestor.Elementos.Permiso.RegistroDeEstudiante", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasColumnType("INT")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Apellido")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("InscritoEl")
                        .HasColumnType("datetime2");

                    b.Property<string>("Nombre")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("EST_ELEMENTO","UNIVERSIDAD");
                });

            modelBuilder.Entity("Gestor.Elementos.Permiso.RegistroDeInscripcion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasColumnType("INT")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CursoId")
                        .HasColumnType("INT");

                    b.Property<int>("EstudianteId")
                        .HasColumnType("INT");

                    b.Property<int?>("Grado")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CursoId");

                    b.HasIndex("EstudianteId");

                    b.ToTable("EST_CURSO","UNIVERSIDAD");
                });

            modelBuilder.Entity("Gestor.Elementos.Permiso.RegistroDeInscripcion", b =>
                {
                    b.HasOne("Gestor.Elementos.Permiso.RegistroDeCurso", "Curso")
                        .WithMany("Inscripciones")
                        .HasForeignKey("CursoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Gestor.Elementos.Permiso.RegistroDeEstudiante", "Estudiante")
                        .WithMany("Inscripciones")
                        .HasForeignKey("EstudianteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
