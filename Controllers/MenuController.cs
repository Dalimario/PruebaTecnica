using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Prueba_Tecnica.Models;
using System.Collections.Generic;
using System.Net.Http;

namespace Prueba_Tecnica.Controller
{
    public class MenuController : Helpers.HelpController
    {
        public MenuController(Microsoft.Extensions.Configuration.IConfiguration configuration) : base(configuration)
        {
        }

        //Se validara siempre que la cookie sea valida antes de trabajar en la pantalla
        public IActionResult Menu()
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Login");


            UserModel Model = new UserModel();

            Model.Name = HttpContext.User.Identity.Name;
            return View(Model);
        }

        [HttpPost]
        public IActionResult Menu(LoginViewModel Accesos)
        {
            return View();
        }

       
    }
}
