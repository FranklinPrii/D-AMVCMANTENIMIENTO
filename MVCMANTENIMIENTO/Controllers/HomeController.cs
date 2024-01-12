using MVCMANTENIMIENTO.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCMANTENIMIENTO.Controllers
{
    public class HomeController : Controller
    {
        private SisEdutivaEntities1 db = new SisEdutivaEntities1();


        public ActionResult Index()
        {

            return View();
        }

        public ActionResult Listar()
        {
            var datos = db.APRUEBA.ToList();

            return Json(datos, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Insertar(APRUEBA insertar)
        {
            if (ModelState.IsValid)
            {
                db.APRUEBA.Add(insertar);
                db.SaveChanges();

                return RedirectToAction("Index", "Home");
                //return Json(new { success = true, message = "Empleado agregado exitosamente" });

            }

            return Json(new { success = false, message = "Error en la validación del modelo" });
        }
        
        [HttpGet]
        public ActionResult ObtenerDetalleEmpleado(int id)
        {
            var empleado = db.APRUEBA.Find(id);
            return Json(empleado, JsonRequestBehavior.AllowGet);
        }




        [HttpPost]
        public ActionResult ActualizarEmpleado(APRUEBA empleadoActualizado)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var empleadoExistente = db.APRUEBA.Find(empleadoActualizado.IDALUMNO);

                    if (empleadoExistente == null)
                    {
                        return Json(new { success = false, message = "No se encontró el empleado para actualizar" });
                    }

                    // Actualizar solo los campos necesarios
                    empleadoExistente.ALUMNO = empleadoActualizado.ALUMNO;
                    empleadoExistente.APELLIDO = empleadoActualizado.APELLIDO;
                    empleadoExistente.EDAD = empleadoActualizado.EDAD;
                    empleadoExistente.SUELDO = empleadoActualizado.SUELDO;

                    db.SaveChanges();

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    var errores = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
                    return Json(new { success = false, message = "Error de validación", errors = errores });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al actualizar el empleado", error = ex.Message });
            }
        }



        [HttpPost]
        public ActionResult EliminarEmpleado(int idEmpleado)
        {
            try
            {
                var empleado = db.APRUEBA.Find(idEmpleado);

                if (empleado != null)
                {
                    db.APRUEBA.Remove(empleado);
                    db.SaveChanges();

                    return Json(new { success = true, message = "Empleado eliminado exitosamente" });
                }
                else
                {
                    return Json(new { success = false, message = "Empleado no encontrado" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al eliminar empleado", error = ex.Message });
            }
        }



    }
}