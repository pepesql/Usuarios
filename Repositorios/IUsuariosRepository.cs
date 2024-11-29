using Usuarios.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Usuarios.Repositorios
{
   
    public interface IUsuariosRepository
    {

        Task<string> Authenticate(UserLogin userlogin);


        Task<string> CreateUsuario(User usuario );

        Task<string> UpdateUsuario(UpdateUser usuario);

        Task<string> DeleteUsuario(int idUsuario);

        //Task<string> ConfirmEmail(ConfirmEmail confirmEmail);
        
        Task<string> ChangePassword(PasswordChange model);

        Task<string> ForgetPassword(string email);

        List<User> GetUsuarios();


    }
}
