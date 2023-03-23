namespace Assignment
{
    public class AddressManager
    {

        private const string addressFileName = "addresses.txt";

        public struct Address {
            public string email;
            public string unitNo;
            public string streetNo;
            public string streetName;
            public string streetSuffix;
            public string city;
            public string state;
            public string postcode;
        }

        //private Address emptyAddress;
#pragma warning disable IDE0044 // Add readonly modifier
        private List<Address> addresses;
#pragma warning restore IDE0044 // Add readonly modifier
        private readonly UserManager userManager;
        private readonly Validator validator;
        private readonly Address empty_addr;

        private string GetUnitNumber() {
            bool validateUnitNo = false;
            string unitNo;
            do {
                Console.Write("\nUnit number (0 = none)\n> ");
                unitNo = Console.ReadLine() ?? "";
                if (!validator.ValidateUnitNo(unitNo)) {
                    Console.WriteLine("\tPlease provide a unit number, or zero for no unit\n");
                } else {
                    validateUnitNo = true;
                }
            } while (!validateUnitNo);
            return unitNo;
        }

        private string GetStreetNumber() {
            bool validateStreetNo = false;
            string streetNo;
            do {
                Console.Write("\nStreet number:\n> ");
                streetNo = Console.ReadLine() ?? "";
                if (!validator.ValidateStreetNo(streetNo)) {
                    Console.WriteLine("\tPlease provide a valid street number\n");
                } else {
                    validateStreetNo = true;
                }
            } while (!validateStreetNo);
            return streetNo;
        }

        private string GetStreetName() {
            bool validateStreetName = false;
            string streetName;
            do {
                Console.Write("\nStreet name:\n> ");
                streetName = Console.ReadLine() ?? "";
                if (!validator.ValidateStreetName(streetName)) {
                    Console.WriteLine("\tPlease provide a valid street name\n");
                } else {
                    validateStreetName = true;
                }
            } while (!validateStreetName);
            return streetName;
        }

        private string GetStreetSuffix() {
            bool validateStreetSuffix = false;
            string streetSuffix;
            do {
                Console.Write("\nStreet suffix:\n> ");
                streetSuffix = Console.ReadLine() ?? "";
                if (!validator.ValidateStreetSuffix(streetSuffix)) {
                    Console.WriteLine("\tPlease provide a valid street suffix\n");
                } else {
                    validateStreetSuffix = true;
                }
            } while (!validateStreetSuffix);
            return streetSuffix;
        }

        private string GetCityName() {
            bool validateCityName = false;
            string cityname;
            do {
                Console.Write("\nCity:\n> ");
                cityname = Console.ReadLine() ?? "";
                if (!validator.ValidateCityName(cityname)) {
                    Console.WriteLine("\tPlease provide a valid city name\n");
                } else {
                    validateCityName = true;
                }
            } while (!validateCityName);
            return cityname;
        }

        private string GetState() {
            bool validateState = false;
            string state;
            do {
                Console.Write("\nState (ACT, NSW, NT, QLD, SA, TAS, VIC, WA):\n> ");
                state = Console.ReadLine() ?? "";
                if (!validator.ValidateState(state)) {
                    Console.WriteLine("\tPlease provide a valid state\n");
                } else {
                    validateState = true;
                }
            } while (!validateState);
            return state;
        }

        private string GetPostcode() {
            bool validatePostcode = false;
            string postcode;
            do {
                Console.Write("\nPostcode (1000 .. 9999):\n> ");
                postcode = Console.ReadLine() ?? "";
                if (!validator.ValidatePostCode(postcode)) {
                    Console.WriteLine("\tPlease provide a valid postcode\n");
                } else {
                    validatePostcode = true;
                }
            } while (!validatePostcode);
            return postcode;
        }

        // Only public function, used to determine if the program
        // should ask for address on sign in like in story 5
        public void UserHasAddress(string email) {
            foreach (Address a in addresses) {
                if (a.email == email) {
                    return;
                }
            }
            GetUserAddress(email);
        }

        public Address GetAddressByEmail(string email) {
            foreach (Address addr in addresses) {
                if (addr.email == email) {
                    return addr;
                }
            }
            return empty_addr;
        }


        // Get the user address, only runs if the user hasn't added an
        // address yet, as determined in UserHasAddress. Gets address details
        // and saves them in the address list and address file
        private void GetUserAddress(string email) {
            UserManager.User user = userManager.GetUserByEmail(email);
            string s = $"\nPersonal Details for {user.name}({user.email})";
            Console.WriteLine(s);
            for (int i = 0; i < s.Length-1; i++) {
                Console.Write('-');
            }

            Console.WriteLine("\n\nPlease provide your home address.");

            string unitNo = GetUnitNumber();
            string streetNo = GetStreetNumber();
            string streetName = GetStreetName();
            string streetSuffix = GetStreetSuffix();
            string city = GetCityName();
            string state = GetState();
            string postcode = GetPostcode();

            Address newAddress;
            newAddress.email = email;
            newAddress.unitNo = unitNo;
            newAddress.streetNo = streetNo;
            newAddress.streetName = streetName;
            newAddress.streetSuffix = streetSuffix;
            newAddress.city = city;
            newAddress.state = state;
            newAddress.postcode = postcode;
            addresses.Add(newAddress);

            using StreamWriter writer = new(File.Open(addressFileName, FileMode.Append));
            writer.WriteLine("Address");
            writer.WriteLine(email);
            writer.WriteLine(unitNo);
            writer.WriteLine(streetNo);
            writer.WriteLine(streetName);
            writer.WriteLine(streetSuffix);
            writer.WriteLine(city);
            writer.WriteLine(state);
            writer.WriteLine(postcode);
            writer.Close();

            Console.WriteLine($"\nAddress has been updated to {unitNo}/{streetNo} {streetName} {streetSuffix}, {city} {state} {postcode}");
        }

        private void LoadAddresses() {
            using StreamReader reader = new(addressFileName);
            while (!reader.EndOfStream) {
                string currentLine = reader.ReadLine() ?? "";
                if (currentLine == "Address") {
                    Address newAddress;
                    newAddress.email = reader.ReadLine() ?? "";
                    newAddress.unitNo = reader.ReadLine() ?? "";
                    newAddress.streetNo = reader.ReadLine() ?? "";
                    newAddress.streetName = reader.ReadLine() ?? "";
                    newAddress.streetSuffix = reader.ReadLine() ?? "";
                    newAddress.city = reader.ReadLine() ?? "";
                    newAddress.state = reader.ReadLine() ?? "";
                    newAddress.postcode = reader.ReadLine() ?? "";
                    addresses.Add(newAddress);
                }
            }
        }

        public AddressManager(UserManager userManager)
        {
            using StreamWriter w = File.AppendText(addressFileName);
            w.Close();

            this.userManager = userManager;
            addresses = new List<Address>();
            validator = new Validator();
            LoadAddresses();

            empty_addr = new Address();
            empty_addr.email = "";
            empty_addr.unitNo = "";
            empty_addr.streetNo = "";
            empty_addr.streetName = "";
            empty_addr.streetSuffix = "";
            empty_addr.city = "";
            empty_addr.postcode = "";
            empty_addr.state = "";
        }
    }
}

