using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Prueba_Tecnica.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Prueba_Tecnica.Controller
{
    public class LoginController : Helpers.HelpController
    {
        public LoginController(IConfiguration configuration) : base(configuration)
        {
        }

        //Inicio de pantalla Login, al cerrar sesion se envia el llamado de este metodo y se corrobora si la sesion dejo de exisir
        public IActionResult Login()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
                return RedirectToAction("Menu", "Menu");
            return View();
        }

        [HttpPost]//Metodo para validar sesion y en caso de ser exitoso, crea una cookie de sesion para navegar entre pantallas mediante HTTPContext
        public IActionResult Login(LoginViewModel Accesos)
        {
            if (!ModelState.IsValid || Accesos.UserName == null || Accesos.UserPassword == null)
                return View();

            var user = Authenticate(Accesos);
            if (user == null || !user.Granted)
            {
                TempData["IsAtuhtenticated"] = false;
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name, ClaimValueTypes.String, ""),
                new Claim(ClaimTypes.Surname, user.Apellido, ClaimValueTypes.String, ""),
            };

            var userIdentity = new ClaimsIdentity(claims, "Cookie");
            var userPrincipal = new ClaimsPrincipal(userIdentity);

            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal,
                new AuthenticationProperties
                { IsPersistent = false });


            return RedirectToAction("Menu", "Menu");
        }

        //Llamada a la api para corroborar las credenciales
        private UserModel Authenticate(LoginViewModel Accesos)
        {
            PruebaTecnicaBackEndService.ServiceClient WebService = new PruebaTecnicaBackEndService.ServiceClient();
            PruebaTecnicaBackEndService.LoginViewModel ViewModel = new PruebaTecnicaBackEndService.LoginViewModel();
            PruebaTecnicaBackEndService.UserModel UM = new PruebaTecnicaBackEndService.UserModel();
            UserModel Response = new UserModel();

            ViewModel.UserName = Accesos.UserName;
            ViewModel.UserPassword = Accesos.UserPassword;
            try
            {
                UM = WebService.AuthenticateUserAsync(ViewModel).Result;

                Response.Name = UM.Name;
                Response.Apellido = UM.Apellido;
                Response.Granted = UM.Granted;
            }
            catch {
                Response.Granted = false;
            }

            return Response;
            
        }

        //Metodo que cierra sesion eliminando la variable HttpContext
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction(nameof(LoginController.Login), "Login");
        }

    }
}
