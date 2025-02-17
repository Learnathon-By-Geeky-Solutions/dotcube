namespace DeltaShare.Model
{
    public class User
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string IpAddress { get; set; }
        public bool IsAdmin { get; set; }

        public string ViewableUsername
        {
            get
            {
                if (Username.Length > 10)
                {
                    return Username.Substring(0, 10) + "...";
                }
                else
                {
                    return Username;
                }
            }
        }

        public User(string name, string email, string username, string ipAddress, bool isAdmin)
        {
            this.Name = name;
            this.Email = email;
            this.Username = username;
            this.IpAddress = ipAddress;
            this.IsAdmin = isAdmin;
        }

        public override string ToString()
        {
            return "Name: " + Name + ", Email: " + Email + ", Username: " + Username + ", IpAddress: " + IpAddress + ", IsAdmin: " + IsAdmin;
        }
    }
}
