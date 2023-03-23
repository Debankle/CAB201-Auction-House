using System;
namespace Assignment
{
    public class MainMenu
    {

        private void MenuMessage() {
            Console.WriteLine("\nMain Menu\n---------");
            Console.WriteLine("(1) Register");
            Console.WriteLine("(2) Sign In");
            Console.WriteLine("(3) Exit\n");
        }


        // Return a choice of 1,2 or 3 based on user input to control the
        // state machine
        private int GetUserChoice() {
            int response;
            Console.WriteLine("Please select an option between 1 and 3");
            Console.Write("> ");
            string input = Console.ReadLine() ?? "0";
            try {
                response = int.Parse(input);
            } catch(FormatException) {
                response = -1;
            }
            return response;
        }

        // Start the main menu loop to get user input
        public int StartMenu() {
            bool madeValidChoice;
            int choice;
            do {
                MenuMessage();
                choice = GetUserChoice();
                madeValidChoice = true;
                if (choice < 0 || choice > 3) {
                    Console.WriteLine("\tInvalid input\n");
                    madeValidChoice = false;
                }
            } while (!madeValidChoice);
            return choice;
        }

        public MainMenu() {
        }
    }
}

