﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using ServicioDeDatos.Elemento;
using ServicioDeDatos.Entorno;

namespace ServicioDeDatos.Seguridad
{


    [Table("USU_PUESTO", Schema = "SEGURIDAD")]
    public class PuestoDeUnUsuarioDtm : Registro
    {
        [Column("IDUSUA", TypeName = "INT")]
        public int IdUsuario { get; set; }
        public virtual UsuarioDtm Usuario { get; set; }

        [Column("IDPUESTO", TypeName = "INT")]
        public int idPuesto { get; set; }

        public virtual PuestoDtm Puesto { get; set; }
    }

    public static class TablaUsuPuesto
    {
        public static void Definir(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<PuestoDeUnUsuarioDtm>().Property(p => p.IdUsuario).IsRequired();
            modelBuilder.Entity<PuestoDeUnUsuarioDtm>().Property(p => p.idPuesto).IsRequired();


            modelBuilder.Entity<PuestoDeUnUsuarioDtm>().Property(p => p.IdUsuario).HasColumnName("IDUSUA");
            modelBuilder.Entity<PuestoDeUnUsuarioDtm>().Property(p => p.idPuesto).HasColumnName("IDPUESTO");


            modelBuilder.Entity<PuestoDeUnUsuarioDtm>()
                        .HasIndex(p =>new { p.idPuesto, p.IdUsuario })
                        .HasName("I_USU_PUESTO_IDPUESTO_IDUSUA")
                        .IsUnique();

            modelBuilder.Entity<PuestoDeUnUsuarioDtm>()
                .HasIndex(p => p.IdUsuario)
                .IsUnique(false)
                .HasName("I_USU_PUESTO_IDUSUA");

            modelBuilder.Entity<PuestoDeUnUsuarioDtm>()
                .HasIndex(p => p.idPuesto)
                .IsUnique(false)
                .HasName("I_USU_PUESTO_IDPUESTO");

            modelBuilder.Entity<PuestoDeUnUsuarioDtm>()
                .HasOne(x => x.Puesto)
                .WithMany(p => p.Usuarios)
                .HasForeignKey(x => x.idPuesto)
                .HasConstraintName("FK_USU_PUESTO_IDPUESTO")
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PuestoDeUnUsuarioDtm>()
                .HasOne(x => x.Usuario)
                .WithMany(p => p.Puestos)
                .HasForeignKey(x => x.IdUsuario)
                .HasConstraintName("FK_USU_PUESTO_IDUSUA")
                .OnDelete(DeleteBehavior.Restrict);

        }
    }

}