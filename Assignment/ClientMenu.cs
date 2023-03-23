using System;
namespace Assignment
{
    public class ClientMenu
    {

        private static void MenuMessage() {
            Console.WriteLine("\nClient Menu\n-----------");
            Console.WriteLine("(1) Advertise Product");
            Console.WriteLine("(2) View My Product List");
            Console.WriteLine("(3) Search For Advertised Products");
            Console.WriteLine("(4) View Bids On My Products");
            Console.WriteLine("(5) View My Purchased Items");
            Console.WriteLine("(6) Log off\n");
        }

        private static int GetUserChoice() {
            int response;
            Console.WriteLine("Please select an option between 1 and 6");
            Console.Write("> ");
            string input = Console.ReadLine() ?? "0";
            try {
                response = Int32.Parse(input);
            } catch (FormatException) {
                response = -1;
            }
            return response;
        }

        public int StartMenu() {
            bool madeValidChoice;
            int choice;
            do {
                MenuMessage();
                choice = GetUserChoice();
                madeValidChoice = true;
                if (choice < 0 || choice > 6) {
                    Console.WriteLine("Invalid input\n");
                    madeValidChoice = false;
                }
            } while (!madeValidChoice);
            return choice;
        }

        public ClientMenu()
        {
        }
    }
}

