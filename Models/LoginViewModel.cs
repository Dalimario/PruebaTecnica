
using System.ComponentModel.DataAnnotations;


namespace Prueba_Tecnica.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El nombre de usuario es requerido")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida")]
        public string UserPassword { get; set; }
    }
}
