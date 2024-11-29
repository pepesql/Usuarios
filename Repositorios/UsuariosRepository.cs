using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Usuarios.Errors;
using Usuarios.Models;
using Usuarios.Services;
using static System.Reflection.Metadata.BlobBuilder;

namespace Usuarios.Repositorios
{
    public class UsuariosRepository : IUsuariosRepository
    {
        private User currentUser = null;
        private readonly ApplicationDBContext _db;
        private readonly ILoggerManager _loggerManager;
        private readonly IPasswordsManagers _passwordsManagers;
      
        public UsuariosRepository(ApplicationDBContext db, ILoggerManager loggerManager, IPasswordsManagers passwordsManagers)
        {

            _db = db;
            _loggerManager = loggerManager;
            _passwordsManagers = passwordsManagers;
            //_httpContext = httpContext;
            
        }



        public async Task<string> Authenticate(UserLogin userlogin)
        {
            //throw new NotImplementedException();

           
            if (string.IsNullOrEmpty(userlogin.UserName) || string.IsNullOrEmpty(userlogin.Password))
            {
                throw new Exception($"Error Username or password Vacios ");
               // return "NotEmpty";

            }


            if (await VerificaFormatoEmail(userlogin.UserName) == false)
                throw new Exception($"Error email incorrecto ");
            // return "NovalidEmail";

            currentUser = _db.Users.FirstOrDefault(bb => bb.Email == userlogin.UserName);


            if (currentUser == null)
            {
                //entonces es nulo y se acabo la historia
                //return "NoUser";
                throw new NotFoundException($"Usuario no encontrado ");
            }

            
            {
                string encriptpassword = _passwordsManagers.EncryptText(userlogin.Password);
                if (encriptpassword != currentUser.Password)
                    //return "WrongPassword";
                    throw new Exception($"Clave Incorrecta ");

            }

            _loggerManager.LogInfo(" Usuario autenticado "+ userlogin.UserName);
            return " Usuario autenticado " + userlogin.UserName+" token "+  _passwordsManagers.GenerateToken(userlogin);

        }

          

       

       

        public async Task<string> ChangePassword(PasswordChange model)
        {

            if (string.IsNullOrEmpty(model.Email))
            {
                //return "NovalidEmail";
                throw new Exception($"Email Incorrecto");
            }

            if (await VerificaFormatoEmail(model.Email) == false)
                //return "NovalidEmail";
                throw new Exception($"Email Incorrecto ");


            if (string.IsNullOrEmpty(model.OldPassword) || string.IsNullOrEmpty(model.NewPassword) || string.IsNullOrEmpty(model.ConfirmPassword))
            {
                //return "FieldNull";
                throw new Exception($"Error campos Vacios ");
            }

            var user = _db.Users.FirstOrDefault(bb => bb.Email == model.Email);
            if (user == null)
            {
                //return "NotExistUser";
                throw new Exception($"Error Username no existe ");
            }

            if (!model.NewPassword.Equals(model.ConfirmPassword))
            {
                //return "DiferentPassword";
                throw new Exception($"Contraseñas incorrectas, no coinciden ");
            }

            //comparacion de las claves viejas

            if (_passwordsManagers.EncryptText(model.OldPassword) != user.Password)
            {
                // return "ErrorChangePassword";
                throw new Exception($"Contraseña incorrecta ");
            }

            user.Password = _passwordsManagers.EncryptText(model.NewPassword);

            _db.Users.Update(user);
            _db.SaveChanges();

            _loggerManager.LogInfo(" Usuario cambio password " + model.Email);
            return " Usuario cambio password " + model.Email;









        }

        public async Task<string> CreateUsuario(User usuario)
        {

            if (await VerificaFormatoEmail(usuario.Email) == false)
                //return "NovalidEmail";
                throw new Exception($"Error email invalido ");




                var currentUser = _db.Users.FirstOrDefault(bb => bb.Email == usuario.Email);

            if (currentUser != null)
            {
                //entonces existe y se acabo la historia
                //return "UsuarioExiste";
                throw new Exception($"Error Usuario inexistente ");
            }


           
            var dbContextTransaction = _db.Database.BeginTransaction();

            //debo agregar el usuario a la tabla de usuarios no importa que sea del mismo dominio pues ahi siempre lo va a buscar

            usuario.Password = _passwordsManagers.EncryptText(usuario.Password);
            _db.Users.Add(usuario);
            _db.SaveChanges();
           
          
            dbContextTransaction.Commit();
            _loggerManager.LogInfo(" Usuario creado  " + usuario.Email);

            return " Usuario creado  " + usuario.Email;

        }

        public async Task<string> DeleteUsuario(int idUsuario)
        {
           
            var currentUser = _db.Users.FirstOrDefault(bb => bb.Id == idUsuario);

            

            if (currentUser == null)
            {
                //entonces es nulo y se acabo la historia
                // return "UsuarioNoExiste";
                throw new Exception($"Error Usuario inexistente ");
            }

            _loggerManager.LogInfo(" Usuario eliminado " + currentUser.Email );

            var dbContextTransaction = _db.Database.BeginTransaction();
            
            _db.Users.Remove(currentUser);
            _db.SaveChanges();
            dbContextTransaction.Commit();
            return "OKUsuarioEliminado";

        }

        public Task<string> ForgetPassword(string email)
        {
            throw new NotImplementedException();
        }

        public List<User> GetUsuarios()
        {
            return _db.Users.ToList();
        }

        public async Task<string> UpdateUsuario(UpdateUser usuario)
        {

            if (await VerificaFormatoEmail(usuario.Email) == false)
                //return "NovalidEmail";
                throw new Exception($"Error Username or email incorrecto ");


            var currentUser = _db.Users.FirstOrDefault(bb => bb.Id == usuario.Id);

            if (currentUser == null)
            {
                //entonces es nulo y se acabo la historia
                //return "UsuarioNoExiste";
                throw new Exception($"Error Usuario no existe ");
            }

            currentUser.Email = usuario.Email;
            currentUser.Fullname = usuario.Fullname;
            currentUser.Username = usuario.Username;
            var dbContextTransaction = _db.Database.BeginTransaction();

            

            _db.Users.Update(currentUser);
            _db.SaveChanges();

            dbContextTransaction.Commit();

            _loggerManager.LogInfo(" Usuario modificado " + usuario.Email);
            return "OKUsuarioModificado";







        }




        public async Task<bool> VerificaFormatoEmail(string email)
        {
            string pattern = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";

            if (!Regex.IsMatch(email, pattern))
            {
                //if email is valid
                return false;
            }
            else
                return true;


        }


    }
}
