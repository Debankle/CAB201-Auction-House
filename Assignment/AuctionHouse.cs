using System;
namespace Assignment
{
    public class AuctionHouse
    {

        // Enum for state machine, use a switch statement to control
        // program flow
        private enum AuctionState {
            MAIN_MENU,
            REGISTER,
            SIGN_IN,
            ADDRESS_CHECK,
            CLIENT_MENU,
            PRODUCT_AD,
            PRODUCT_LIST,
            PRODUCT_SEARCH,
            BID_LIST,
            PURCHASED_LIST,
            LOG_OFF
        };

        // Private readonly objects that are required to not close during the
        // state machine
        private readonly MainMenu menu;
        private readonly Registration registration;
        private readonly SignIn signIn;
        private readonly ClientMenu clientMenu;

        // global variables used for the program
        private AuctionState state;
        private readonly UserManager userManager;
        private readonly AddressManager addressManager;
        private readonly ProductManager productManager;
        private string emailOfSignedInUser;

        // Gracefully exit the program
        public void CloseAuction() {
            Console.WriteLine("+--------------------------------------------------+");
            Console.WriteLine("| Good bye, thank you for using the Auction House! |");
            Console.WriteLine("+--------------------------------------------------+");
            Environment.Exit(0);
        }

        // Welcome message
        public void OpenAuction() {
            Console.WriteLine("\n+------------------------------+");
            Console.WriteLine("| Welcome to the Auction House |");
            Console.WriteLine("+------------------------------+");
        }

        public void AuctionLoop() {
            while (true) {
                // Loop through the switch statement of the state machine
                // forever, each state being reachable
                switch (state) {
                    case AuctionState.MAIN_MENU:
                        int choice = menu.StartMenu();
                        if (choice == 1) {
                            state = AuctionState.REGISTER;
                        } else if (choice == 2) {
                            state = AuctionState.SIGN_IN;
                        } else if (choice == 3) {
                            CloseAuction();
                        }
                        break;

                    case AuctionState.REGISTER:
                        registration.RegistrationMenu();
                        state = AuctionState.MAIN_MENU;
                        break;

                    case AuctionState.SIGN_IN:
                        string email = signIn.SignInMenu();
                        if (email == "") {
                            state = AuctionState.MAIN_MENU;
                        } else {
                            state = AuctionState.ADDRESS_CHECK;
                            emailOfSignedInUser = email;
                        }
                        break;

                    case AuctionState.ADDRESS_CHECK:
                        addressManager.UserHasAddress(emailOfSignedInUser);
                        state = AuctionState.CLIENT_MENU;
                        break;

                    case AuctionState.CLIENT_MENU:
                        int choice2 = clientMenu.StartMenu();
                        if (choice2 == 1) {
                            state = AuctionState.PRODUCT_AD;
                        } else if (choice2 == 2) {
                            state = AuctionState.PRODUCT_LIST;
                        } else if (choice2 == 3) {
                            state = AuctionState.PRODUCT_SEARCH;
                        } else if (choice2 == 4) {
                            state = AuctionState.BID_LIST;
                        } else if (choice2 == 5) {
                            state = AuctionState.PURCHASED_LIST;
                        } else if (choice2 == 6) {
                            state = AuctionState.LOG_OFF;
                        }
                        break;

                    case AuctionState.PRODUCT_AD:
                        _ = new ProductAdvertisment(userManager.GetUserByEmail(emailOfSignedInUser), productManager);
                        state = AuctionState.CLIENT_MENU;
                        break;

                    case AuctionState.PRODUCT_LIST:
                        _ = new ProductList(userManager.GetUserByEmail(emailOfSignedInUser), productManager);
                        state = AuctionState.CLIENT_MENU;
                        break;

                    case AuctionState.PRODUCT_SEARCH:
                        _ = new ProductSearch(userManager.GetUserByEmail(emailOfSignedInUser), productManager, addressManager);
                        state = AuctionState.CLIENT_MENU;
                        break;

                    case AuctionState.BID_LIST:
                        _ = new BidList(productManager, userManager.GetUserByEmail(emailOfSignedInUser));
                        state = AuctionState.CLIENT_MENU;
                        break;

                    case AuctionState.PURCHASED_LIST:
                        _ = new PurchasedProducts(productManager, userManager.GetUserByEmail(emailOfSignedInUser));
                        state = AuctionState.CLIENT_MENU;
                        break;

                    case AuctionState.LOG_OFF:
                        emailOfSignedInUser = "";
                        state = AuctionState.MAIN_MENU;
                        break;

                    default:
                        break;
                }
            }
        }

        public AuctionHouse()
        {
            state = AuctionState.MAIN_MENU;
            userManager = new UserManager();
            menu = new MainMenu();
            registration = new Registration(userManager);
            signIn = new SignIn(userManager);
            clientMenu = new ClientMenu();
            addressManager = new AddressManager(userManager);
            productManager = new ProductManager();
            emailOfSignedInUser = "";
        }
    }
}

