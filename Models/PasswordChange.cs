using System.ComponentModel.DataAnnotations;

namespace Usuarios.Models
{
    public class PasswordChange
    {
        //[Required(ErrorMessage = "Por favor teclee el campo Correo")]
        //[StringLength(255)]
       // [EmailAddress(ErrorMessage = "Por favor teclee el correcto formato para el campo Correo Ej: xxxxxx@zzzzz.dfd")]
        //[Display(Name = "Correo")]
        public string Email { get; set; }

        //[Required(ErrorMessage = "Por favor teclee el campo Contraseña Actual")]
        //[StringLength(255)]
        //[DataType(DataType.Password)]
        //[Display(Name = "Contraseña Actual")]
        public string OldPassword { get; set; }



       // [Required(ErrorMessage = "Por favor teclee el campo Contraseña Nueva")]
       // [StringLength(255)]
       // [DataType(DataType.Password)]
        //[MinLength(8, ErrorMessage = " La contraseña tiene un minimo de 8 caracteres")]

       // [Display(Name = "Contraseña Nueva")]
        public string NewPassword { get; set; }

        //[Required(ErrorMessage = "Por favor teclee el campo Confirmar Contraseña")]
        //[StringLength(255)]
        //[Compare("NewPassword", ErrorMessage = "La Contraseña Nueva y la Confirmada no Coinciden")]
       // [DataType(DataType.Password)]
        //[MinLength(8, ErrorMessage = " La contraseña tiene un minimo de 8 caracteres")]

        //[Display(Name = "Confirmar Contraseña")]
        public string ConfirmPassword { get; set; }
    }
}
