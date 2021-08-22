using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Prueba_Tecnica.Models;
using PruebaTecnicaBackEndService;
using System.Collections.Generic;
using System.Net.Http;

namespace Prueba_Tecnica.Controller
{
    public class ConsultarController : Helpers.HelpController
    {
        public ConsultarController(Microsoft.Extensions.Configuration.IConfiguration configuration) : base(configuration)
        {
        }

        //Se validara siempre que la cookie sea valida antes de trabajar en la pantalla
        public IActionResult Consultar()
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Login");


            var ResponseArray = ConsultaAlumnos();



            Models.UserModel Model = new Models.UserModel();

            List<ConsultaAlumnosViewModel> ResponseView = new List<ConsultaAlumnosViewModel>(ResponseArray);
            Model.Name = HttpContext.User.Identity.Name;
            return View(ResponseView);
        }


        private ConsultaAlumnosViewModel[] ConsultaAlumnos()
        {
            PruebaTecnicaBackEndService.ServiceClient WebService = new PruebaTecnicaBackEndService.ServiceClient();

            
            try
            {
                var Response = WebService.ConsultaAlumnosAsync().Result;
                return Response;
            }
            catch
            {
              return null;
            }

            

        }


    }
}
