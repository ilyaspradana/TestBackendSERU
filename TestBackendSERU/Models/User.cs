
namespace TestBackendSERU.Models
{
    public class User
    {
        public class Login
        {
            public string Username { get; set; }
        }

        public class Registration
        {
            public string Username { get; set; }
        }

        public class EditUser
        {
            public int ID { get; set; }
            public bool Is_Admin { get; set; }
        }
    }
}