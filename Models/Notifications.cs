namespace Usuarios.Models
{
    public class Notifications
    {

        public Guid? notificationId { get; set; }
        public int id { get; set; }
        public DateTime timestamp { get; set; }
        public string message { get; set; }
        public string type { get; set; }

        public Notifications()
        {
            // add a new guid as a unique identifier for the notification in the db
            notificationId = Guid.NewGuid();
        }

    }
}
