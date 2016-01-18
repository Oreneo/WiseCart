namespace WiseCartLogic.Entities
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public User()
        {
            
        }

        public User(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
