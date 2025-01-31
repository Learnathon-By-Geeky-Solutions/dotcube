namespace DeltaShare.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string IpAddress { get; set; }
        public bool IsAdmin { get; set; }

        private static int nextId = 1;

        private static void incrementId()
        {
            nextId++;
        }

        public User(string name, string email, string username, string ipAddress, bool isAdmin)
        {
            this.Id = nextId;
            incrementId();

            this.Name = name;
            this.Email = email;
            this.Username = username;
            this.IpAddress = ipAddress;
            this.IsAdmin = isAdmin;
        }

        public override string ToString()
        {
            return "Id: " + Id + ", Name: " + Name + ", Email: " + Email + ", Username: " + Username + ", IpAddress: " + IpAddress + ", IsAdmin: " + IsAdmin;
        }
    }
}
