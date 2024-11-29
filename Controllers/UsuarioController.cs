using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Usuarios.Models;
using Usuarios.Repositorios;
using Usuarios.Services;


namespace Usuarios.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuarioController : ControllerBase
    {

        private readonly INotificationManager _notificationManager;
        private readonly IUsuariosRepository _usuariosRepository;
        //private readonly HttpContext.WebSockets _httpContext;

        public UsuarioController(IUsuariosRepository usuariosRepository, INotificationManager notificationManager)
        {
            //_webSocketConnectionManager = webSocketConnectionManager;
            _usuariosRepository = usuariosRepository;
            _notificationManager = notificationManager;
            //_httpContext = httpContext;
        }



        /// <summary>
        /// Autenticacion del usuario en el sistema
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] UserLogin model)
        {
            try
            {
                var response = await _usuariosRepository.Authenticate(model);

                Notifications notification = new Notifications();
                notification.message = response.ToString();

                if (response.ToString().ToUpper().Contains(" AUTENTICADO "))
                {
                    //await NotificationManager.Instance.SendNotificationAsync(notification);
                    await _notificationManager.SendNotificationAsync(notification);
                    return Ok(response);
                }
                else 
                {
                    await _notificationManager.SendNotificationAsync(notification);
                    return BadRequest(response);

                }

            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }

        }



        /// <summary>
        /// Crea el usuario del sistema
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>

        [HttpPost("CreateUsuario")]
        //lo voy a permitir anonimo para poder hacer pruebas, esto no va asi
        //[AllowAnonymous]
        [Authorize]
        public async Task<IActionResult> CreateUsuario([FromBody] User model)
        {


            try
            {
                var response = await _usuariosRepository.CreateUsuario(model);

                Notifications notification = new Notifications();
                notification.message = response.ToString();

                if (response.ToString().ToUpper().Contains(" CREADO "))
                {
                    await _notificationManager.SendNotificationAsync(notification);
                    return Ok(response);
                }
                else
                {
                    await _notificationManager.SendNotificationAsync(notification);
                    return BadRequest(response);

                }
                
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }

        /// <summary>
        /// Cambia la contraseña del sistema para el usuario 
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>

        [HttpPost("ChangePassword")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] PasswordChange model)
        {
            if (model.ConfirmPassword.Contains(" ") || model.NewPassword.Contains(" "))
            {
                return BadRequest("La contraseña, no debe tener espacios en blanco.");

            }



            try
            {
                var response = await _usuariosRepository.ChangePassword(model);

                Notifications notification = new Notifications();
                notification.message = response.ToString();

                if (response.ToString().ToUpper().Contains(" CAMBIO "))
                {
                    await _notificationManager.SendNotificationAsync(notification);
                    return Ok(response);
                }
                else
                {
                    await _notificationManager.SendNotificationAsync(notification);
                    return BadRequest(response);

                }

            }
            catch (System.Exception ex)
            {

                return BadRequest(ex.Message.ToString());
            }

        }


        /// <summary>
        /// Elimina el usuario del sistema
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        [HttpDelete("DeleteUser")]
        [Authorize]
        public async Task<IActionResult> DeleteUser([FromBody] int idUsuario)
        {
  
            try
            {
                var response = await _usuariosRepository.DeleteUsuario(idUsuario);
                Notifications notification = new Notifications();
                notification.message = response.ToString();

                if (response.ToString().ToUpper().Contains(" ELIMINADO "))
                {
                    await _notificationManager.SendNotificationAsync(notification);
                    return Ok(response);
                }
                else
                {
                    await _notificationManager.SendNotificationAsync(notification);
                    return BadRequest(response);

                }
            
            }
            catch (System.Exception ex)
            {

                return BadRequest(ex.Message.ToString());
            }

        }




        /// <summary>
        /// Actualiza el usuario del sistema
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("UpdateUsuario")]
        [Authorize]
        public async Task<IActionResult> UpdateUsuario([FromBody] UpdateUser model)
        {
            //logger.LogInformation($"Actualizando el usuario");

            try
            {
                var response = await _usuariosRepository.UpdateUsuario(model);
                Notifications notification = new Notifications();
                    notification.message = response.ToString();

                if (response.ToString().ToUpper().Contains(" MODIFICADO "))
                {
                    await _notificationManager.SendNotificationAsync(notification);
                    return Ok(response);
                }
                else
                {
                    await _notificationManager.SendNotificationAsync(notification);
                    return BadRequest(response);

                }

            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }

        }



        /// <summary>
        /// Get Los usuarios del sistema
        /// </summary>
        /// <returns></returns>

        [HttpGet("GetUsuarios")]
        [Authorize]
        [ResponseCache(Duration =10)]
        public async Task<IActionResult> GetUsuarios()
        {
            try
            {
                List<User> usuarios = _usuariosRepository.GetUsuarios();
                return Ok(usuarios);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }

        }


    }
}
