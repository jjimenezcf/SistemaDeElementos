﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using Gestor.Errores;
using GestorDeElementos;
using GestorDeElementos.BdModelo;
using GestorDeElementos.IuModelo;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UniversidadDeMurcia.Controllers
{
        
    public class EntidadController<Tctx, Tbd, Tiu> : BaseController where Tctx: DbContext  where Tbd : BdElemento where Tiu : IuElemento
    {

        protected GestorDeElementos<Tctx, Tbd,Tiu> entorno { get; }


        protected GestorCrud<Tiu> GestorDelCrud { get; }


        public EntidadController(GestorDeElementos<Tctx, Tbd,Tiu> gestorDeElementos, Errores gestorErrores) :
        base(gestorErrores)
        {
            entorno = gestorDeElementos;
            GestorDelCrud = new GestorCrud<Tiu>("Gestor de estudiantes", "Inscripciones");
        }

        public IActionResult Index()
        {
            return RedirectToAction(GestorDelCrud.Mantenimiento.Ir);
        }

        public override ViewResult View(string viewName, object model)
        {
            ViewBag.Crud = GestorDelCrud;
            return base.View(viewName, model);
        }

        protected async Task<IActionResult> CrearObjeto(Tiu iuElemento)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await entorno.InsertarElementoAsync(iuElemento);
                    return RedirectToAction(GestorDelCrud.Mantenimiento.Ir);
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", $"No es posible crear el registro.");
                GestorDeErrores.Enviar("Error al crear un estudiante", e);
            }
            return View((GestorDelCrud.Creador.Vista, iuElemento));
        }



        protected async Task<IActionResult> ModificarObjeto(int id, Tiu elemento)
        {
            if (id != elemento.Id)
            {
                ModelState.AddModelError("", $"El registro pedido no se ha localizado."); ;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await entorno.ModificarElementoAsync(elemento);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExisteObjetoEnBd(elemento.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(GestorDelCrud.Mantenimiento.Ir);
            }

            return View(GestorDelCrud.Editor.Vista, elemento);
        }


        private bool ExisteObjetoEnBd(int id)
        {
            return entorno.ExisteObjetoEnBd(id);
        }

    }

}

