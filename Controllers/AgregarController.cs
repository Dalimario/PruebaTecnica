using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Prueba_Tecnica.Models;
using PruebaTecnicaBackEndService;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace Prueba_Tecnica.Controller
{
    public class AgregarController : Helpers.HelpController
    {
        public AgregarController(Microsoft.Extensions.Configuration.IConfiguration configuration) : base(configuration)
        {
        }

        //Se validara siempre que la cookie sea valida antes de trabajar en la pantalla
        public IActionResult Agregar()
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Login");


            var GradosArray = ConsultaGrados();
            var SexosArray = ConsultaSexos();



            Models.UserModel Model = new Models.UserModel();

            List<GradosViewModel> gradosViews = new List<GradosViewModel>(GradosArray);
            List<SexoViewModel> sexosViews = new List<SexoViewModel>(SexosArray);

            ViewBag.Grados = gradosViews;
            ViewBag.Sexo = sexosViews;

            return View();
        }


        [HttpPost]
        public JsonResult Agregar(string Nombre, string Paterno, string Materno, string Fecha, int SexoID, int GradoID,string Mail, int Telefono)
        {


            int RetCode = 1;
            if (new EmailAddressAttribute().IsValid(Mail)) 
            {
                CrearAlumnoViewModel crearAlumnoViewModel = new CrearAlumnoViewModel();
                crearAlumnoViewModel.Nombre = Nombre;
                crearAlumnoViewModel.ApellidoPaterno = Paterno;
                crearAlumnoViewModel.ApellidoMaterno = Materno;
                crearAlumnoViewModel.FechaNacimiento = Convert.ToDateTime(Fecha);
                crearAlumnoViewModel.Sexo = SexoID;
                crearAlumnoViewModel.Grado = GradoID;
                crearAlumnoViewModel.Email = Mail;
                crearAlumnoViewModel.Telefono = Telefono;
                PruebaTecnicaBackEndService.ServiceClient WebService = new PruebaTecnicaBackEndService.ServiceClient();
                try
                {
                  var Response = WebService.InsertarAlummnosAsync(crearAlumnoViewModel).Result;
                  RetCode = Response.ReturnCode;
                }
                catch
                {
                    RetCode = -1;
                }
            }

            else 
            {
                RetCode = -2;
            }
                
            

            return Json(RetCode);
        }


        private GradosViewModel[] ConsultaGrados()
        {
            PruebaTecnicaBackEndService.ServiceClient WebService = new PruebaTecnicaBackEndService.ServiceClient();

            
            try
            {
                var Response = WebService.ConsultaGradosAsync().Result;
                return Response;
            }
            catch
            {
              return null;
            }

        }
        private SexoViewModel[] ConsultaSexos()
        {
            PruebaTecnicaBackEndService.ServiceClient WebService = new PruebaTecnicaBackEndService.ServiceClient();


            try
            {
                var Response = WebService.ConsultaSexosAsync().Result;
                return Response;
            }
            catch
            {
                return null;
            }

        }


    }
}
