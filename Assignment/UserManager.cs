using System;
namespace Assignment
{
    public class UserManager
    {

        private const string userFileName = "users.txt";

        public struct User {
            public string name;
            public string email;
            public string password;
        }

        private User emptyUser;
#pragma warning disable IDE0044 // Add readonly modifier
        private List<User> users;
#pragma warning restore IDE0044 // Add readonly modifier


        // Write user data to file and add to the list
        public void RegisterUser(User user) {
            users.Add(user);
            using StreamWriter writer = new(File.Open(userFileName, FileMode.Append));
            writer.WriteLine("Client");
            writer.WriteLine(user.name);
            writer.WriteLine(user.email);
            writer.WriteLine(user.password);
            writer.Close();
        }

        public User GetUserByEmail(string email) {
            foreach (User user in users) {
                if (user.email == email) {
                    return user;
                }
            }
            return emptyUser;
        }

        public bool DoesUserEmailExist(string email) {
            return GetUserByEmail(email).email != "";
        }

        // load every user into memory as a User object struct
        public void LoadUsers() {
            using StreamReader reader = new(userFileName);
            while (!reader.EndOfStream) {
                string currentLine = reader.ReadLine() ?? "";
                if (currentLine == "Client") {
                    string name = reader.ReadLine() ?? "";
                    string email = reader.ReadLine() ?? "";
                    string password = reader.ReadLine() ?? "";
                    User newUser;
                    newUser.name = name;
                    newUser.email = email;
                    newUser.password = password;
                    users.Add(newUser);
                }
            }
        }

        public UserManager()
        {
            using StreamWriter w = File.AppendText(userFileName);
            w.Close();

            users = new List<User>();
            LoadUsers();
            emptyUser.name = "";
            emptyUser.email = "";
            emptyUser.password = "";
        }
    }
}

