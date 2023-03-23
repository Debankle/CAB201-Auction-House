using System;
namespace Assignment
{
    public class Registration
    {

        private readonly UserManager userManager;
        private readonly Validator validator;

        private string GetName() {
            bool validName = false;
            string name;
            do {
                Console.Write("Please enter your name\n> ");
                name = Console.ReadLine() ?? "";
                if (!validator.ValidateName(name)) {
                    Console.WriteLine("\tThe supplied value is not a valid name.\n");
                } else {
                    validName = true;
                }
            } while (!validName);
            return name;
        }

        private string GetEmail() {
            Console.WriteLine();
            bool validEmail = false;
            string email;
            do {
                Console.Write("Please enter your email address\n> ");
                email = Console.ReadLine() ?? "";
                if (!validator.ValidateEmail(email)) {
                    Console.WriteLine("\tThe supplied value is not a valid email address.\n");
                } else {
                    if (userManager.DoesUserEmailExist(email)) {
                        Console.WriteLine("\tA user with that email already exists.\n");
                    } else {
                        validEmail = true;
                    }
                }
            } while (!validEmail);
            return email;
        }

        private string GetPassword() {
            Console.WriteLine();
            bool validPassword = false;
            string password;
            do {
                Console.WriteLine("Please choose a password");
                Console.WriteLine("* At least 8 characters");
                Console.WriteLine("* No white space characters");
                Console.WriteLine("* At least one upper-case letter");
                Console.WriteLine("* At lease one lower-case letter");
                Console.WriteLine("* At lease one digit");
                Console.WriteLine("* At least one special character");
                Console.Write("> ");
                password = Console.ReadLine() ?? "";
                if (!validator.ValidatePassword(password)) {
                    Console.WriteLine("\tThe supplied value is not a valid password\n");
                } else {
                    validPassword = true;
                }
            } while (!validPassword);
            return password;
        }


        // Simple registration menu, with three loops to validate input
        // before creating a new user object and saving registering them
        public void RegistrationMenu() {
            Console.WriteLine("\nRegistration\n------------\n");

            string name = GetName();
            string email = GetEmail();
            string password = GetPassword();

            UserManager.User newUser;
            newUser.name = name;
            newUser.email = email;
            newUser.password = password;
            userManager.RegisterUser(newUser);

            Console.WriteLine($"\nClient {name}({email}) has successfully registered at the Auction House.");
        }

        public Registration(UserManager userManager)
        {
            this.userManager = userManager;
            validator = new Validator();
        }
    }
}

