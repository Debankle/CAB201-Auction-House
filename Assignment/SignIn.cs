using System;
namespace Assignment
{
    public class SignIn
    {
        private readonly UserManager userManager;

        private static string GetEmail() {
            Console.Write("Please enter your email address\n> ");
            return Console.ReadLine() ?? "";
        }

        private static string GetPassword() {
            Console.Write("\nPlease enter your password\n> ");
            return Console.ReadLine() ?? "";
        }

        public string SignInMenu() {
            Console.WriteLine("\nSign In\n-------\n");

            string email = GetEmail();
            string password = GetPassword();

            UserManager.User user = userManager.GetUserByEmail(email);
            if (user.password != password) {
                Console.WriteLine("\tPassword does not match user email");
                return "";
            } else {
                return user.email;
            }
        }

        public SignIn(UserManager userManager)
        {
            this.userManager = userManager;
        }
    }
}

