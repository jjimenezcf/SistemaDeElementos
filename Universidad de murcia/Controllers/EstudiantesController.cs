﻿using System.Linq;
using System.Threading.Tasks;
using Extensiones;
using Microsoft.AspNetCore.Mvc;
using Gestor.Elementos.Universitario.ModeloIu;
using UniversidadDeMurcia.Objetos;
using Gestor.Elementos.Universitario.ModeloBd;
using System.Collections.Generic;
using Gestor.Elementos.Universitario.ContextosDeBd;
using Gestor.Errores;
using Gestor.Elementos.Universitario;
using AutoMapper;

namespace UniversidadDeMurcia.Controllers
{

    public class EstudiantesController : EntidadController<ContextoUniversitario, RegistroDeEstudiante, ElementoEstudiante>
    {

        public EstudiantesController(GestorDeEstudiantes gestorDeEstudiantes,IMapper gestorDeMapeo, GestorDeErrores gestorDeErrores):
            base(gestorDeEstudiantes, gestorDeMapeo,  gestorDeErrores)
        {
            GestorDelCrud.Creador.AsignarTitulo("Crear un nuevo estudiante");
        }


        public IActionResult IraMantenimientoEstudiante(string orden)
        {
            ViewData[EstudianteEnlace.Parametro.Nombre] = orden.IsNullOrEmpty() || orden == EstudianteEnlace.OrdenadoPor.NombreAsc
                                                        ? EstudianteEnlace.OrdenadoPor.NombreDes
                                                        : EstudianteEnlace.OrdenadoPor.NombreAsc;

            ViewData[EstudianteEnlace.Parametro.InscritoEl] = orden == EstudianteEnlace.OrdenadoPor.InscritoElAsc
                                                        ? EstudianteEnlace.OrdenadoPor.InscritoElDes
                                                        : EstudianteEnlace.OrdenadoPor.InscritoElAsc;

            var estudiantes =  (IEnumerable<ElementoEstudiante>)GestorDeElementos.LeerTodos();
            estudiantes = orden switch
            {
                EstudianteEnlace.OrdenadoPor.NombreAsc => estudiantes.OrderBy(s => s.Apellido),
                EstudianteEnlace.OrdenadoPor.NombreDes => estudiantes.OrderByDescending(s => s.Apellido),
                EstudianteEnlace.OrdenadoPor.InscritoElDes => estudiantes.OrderByDescending(s => s.InscritoEl),
                EstudianteEnlace.OrdenadoPor.InscritoElAsc => estudiantes.OrderBy(s => s.InscritoEl),
                _ => estudiantes.OrderBy(s => s.Apellido),
            };
            return View(GestorDelCrud.Mantenimiento.Vista, estudiantes.ToList());
        }

        public IActionResult IraCrearEstudiante()
        {
            return View(GestorDelCrud.Creador.Vista, new ElementoEstudiante());
        }

        public IActionResult IraDetalleEstudiante(int? id)
        {
            return View(GestorDelCrud.Detalle.Vista, LeerDetalle(id));
        }

        public IActionResult IraBorrarEstudiante(int? id)
        {
            return View(GestorDelCrud.Supresor.Vista, LeerEstudiante(id));
        }

        public IActionResult IraEditarEstudiante(int? id)
        {
            return View(GestorDelCrud.Editor.Vista, LeerEstudiante(id));
        }


        [HttpPost, ActionName(nameof(CrearEstudiante))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearEstudiante([Bind("ID,Apellido,Nombre,InscritoEl")] ElementoEstudiante estudiante)
        {
            return await CrearObjeto(estudiante);
        }



        [HttpPost, ActionName(nameof(ModificarEstudiante))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ModificarEstudiante(int id, [Bind("Id,Apellido,Nombre,InscritoEl")] ElementoEstudiante estudiante)
        {
            return await ModificarObjeto(id, estudiante);
        }



        [HttpPost, ActionName(nameof(BorrarEstudiante))]
        [ValidateAntiForgeryToken]
        public IActionResult BorrarEstudiante(int id)
        {

            GestorDeElementos.BorrarPorId(id);
            return RedirectToAction(GestorDelCrud.Mantenimiento.Ir);
        }

        private ElementoEstudiante LeerEstudiante(int? id)
        {
            if (id == null)
            {
                GestorDeErrores.LanzarExcepcion("El id del estudiante no puede ser nulo");
            }

            var estudiante = (ElementoEstudiante)GestorDeElementos.LeerElementoPorId((int) id);
            if (estudiante == null)
            {
                GestorDeErrores.LanzarExcepcion($"El id {id} del estudiante no se pudo localizar");
            }

            return estudiante;
        }

        private ElementoEstudiante LeerDetalle(int? id)
        {
            if (id == null)
            {
                GestorDeErrores.LanzarExcepcion("El id del estudiante no puede ser nulo");
            }

            var estudiante = (ElementoEstudiante)GestorDeElementos.LeerElementoConDetalle((int)id);
            if (estudiante == null)
            {
                GestorDeErrores.LanzarExcepcion($"El id {id} del estudiante no se pudo localizar");
            }

            return estudiante;
        }

    }
}
