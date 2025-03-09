using System.Text;

namespace DeltaShare.Model
{
    public class User(string name, string email, string username, string ipAddress, bool isAdmin)
    {
        public string Name { get; set; } = name;
        public string Email { get; set; } = email;
        public string Username { get; set; } = username;
        public string IpAddress { get; set; } = ipAddress; // compute
        public bool IsAdmin { get; set; } = isAdmin; // compute

        public string ViewableUsername
        {
            get
            {
                if (Username.Length > 10)
                {
                    StringBuilder tempUsername = new();
                    // append new line after every 10th character
                    for (int i = 0; i < Username.Length; i++)
                    {
                        tempUsername.Append(Username[i]);
                        if ((i + 1) % 10 == 0)
                        {
                            tempUsername.Append("\n");
                        }
                    }
                    return tempUsername.ToString();
                }
                else
                {
                    return Username;
                }
            }
        }

        public override string ToString()
        {
            return "Name: " + Name + ", Email: " + Email + ", Username: " + Username + ", IpAddress: " + IpAddress + ", IsAdmin: " + IsAdmin;
        }
    }
}
