namespace Usuarios.Models
{
    public class UpdateUser
    {
        public int Id { get; set; }

        public string Fullname { get; set; } = null!;

        public string Username { get; set; } = null!;
               
        public string Email { get; set; } = null!;
    }
}
