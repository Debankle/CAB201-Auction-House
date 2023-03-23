using static Assignment.ProductManager;
using System.Globalization;

namespace Assignment
{
    public class ProductSearch
    {

        private UserManager.User user;
        private readonly ProductManager productManager;
        private readonly AddressManager addressManager;
        private readonly Validator validator;

        private void MenuMessage() {
            string s = $"\nProduct search for {user.name}({user.email})";
            Console.WriteLine(s);
            for (int i = 0; i < s.Length; i++) {
                Console.Write('-');
            }
            Console.WriteLine();
        }

        private static string GetSearchKey() {
            Console.Write("\n\nPlease supply a search phrase (ALL to see all products)\n> ");
            string search = Console.ReadLine() ?? "";
            return search;
        }


        // Private function DisplayResults, which takes in a string key, used
        // To search the list of products for any that match the criteria
        private void DisplayResults(string key) {
            Console.WriteLine("\nSearch results\n--------------\n");

            // Order the products alphabetically and prepare for a list
            productManager.OrderProducts();
            List<Product> products;

            // Depending on the search, get the products that match the
            // criteria
            if (key == "ALL") {
                products = productManager.GetAllProducts(user.email);
            } else {
                products = productManager.GetProductsByKey(key, user.email);
            }

            // Display the products in tabulated format, with the specific
            // details sorted, or a message if none are found
            int j = 0;
            if (products.Count == 0) {
                Console.WriteLine("No products listed matched the search criteria.\n");
                return;
            } else {
                Console.WriteLine("Item #\tProduct name\tDescription\tList " +
                    "price\tBidder name\tBidder email\tBid amt");
                foreach (Product product1 in products) {
                    j++;
                    Console.WriteLine($"{j}\t{product1.productName}\t" +
                        $"{product1.productDescription}\t{product1.listPrice}" +
                        $"\t{product1.bidderName}\t{product1.bidderEmail}\t" +
                        $"{product1.bidAmt}");
                }
            }

            // Req/res loop whether the user would like to place a bid
            // If not, then the function returns and the state switches back
            // to the client menu. Otherwise, move to the BidItem function
            bool validateBidQuery = false;
            string response;
            do {
                Console.Write("\nWould you like to place a bid on any " +
                    "of these items (yes or no)?\n> ");
                response = Console.ReadLine() ?? "";
                if (response == "yes" || response == "no") {
                    validateBidQuery = true;
                } else {
                    Console.WriteLine("\tPlease answer yes or no\n");
                }
            } while (!validateBidQuery);
            if (response == "yes") {
                BidItem(j);
            } else {
                return;
            }
        }

        private void BidItem(int j) {
            bool validGetItem = false;
            string itemNum;
            int num = 1;
            do {
                Console.Write($"\nPlease enter a non-negative integer " +
                    $"between 1 and {j}:\n> ");
                itemNum = Console.ReadLine() ?? "";
                try {
                    num = int.Parse(itemNum);
                    if (num <= j && num >= 1) {
                        validGetItem = true;
                    } else {
                        Console.WriteLine("\tInvalid response.\n");
                    }
                } catch (FormatException) {
                    Console.WriteLine("\tInvalid response.\n");
                }
            } while (!validGetItem);

            int index = productManager.productIndex[num - 1];
            Bid(index);
        }

        // Private Bid(int index), bids on item j, using it as an index
        // to reference the specific product struct in the productManager
        // instance, and add the bidding information
        private void Bid(int index) {
            Product product = productManager.products[index];
            string highestBid;
            if (product.bidAmt == "-") {
                highestBid = "$0.00";
            } else {
                highestBid = product.bidAmt;
            }
            Console.WriteLine($"\nBidding for {product.productName} " +
                $"(regular price {product.listPrice}), current highest " +
                $"bid {highestBid}\n");

            bool validBid = false;
            string bidAmt;
            do {
                Console.Write("\nHow much do you bid?\n> ");
                bidAmt = Console.ReadLine() ?? "";
                if (!validator.ValidateCurrency(bidAmt)) {
                    Console.WriteLine("\tInvalid response. Please " +
                        "enter a price\n");
                } else if (highestBid == "$0.00" && bidAmt == "$0.00") {
                    validBid = true;
                } else if (!validator.ComparePrices(highestBid, bidAmt)) {
                    Console.WriteLine("\tBid must be higher than the " +
                        "current highest bid\n");
                } else {
                    validBid = true;
                }
            } while (!validBid);

            product.bidAmt = bidAmt;
            product.bidderName = user.name;
            product.bidderEmail = user.email;

            Console.WriteLine($"\nYour bid of {bidAmt} for " +
                $"{product.productName} is placed.");

            // Determine the delivery method and add the details to the product
            // instance in productManager
            product = DeliveryOptions(product);

            // Update the product that has been bid on and then save the
            // product information back into the txt file
            productManager.products[index] = product;
            productManager.SaveProducts();

            return;
        }

        private Product DeliveryOptions(Product product) {

            Console.WriteLine("\nDelivery Instructions\n-------------------" +
                "--\n(1) Click and collect\n(2) Home Delivery");

            bool validChoice = false;
            string choice;
            int n = 0;
            do {
                Console.Write("\nPlease select an option between 1 and 2\n> ");
                choice = Console.ReadLine() ?? "";
                try {
                    n = int.Parse(choice);
                    if (n != 1 && n != 2) {
                        Console.WriteLine("\tInvalid choice\n");
                    } else {
                        validChoice = true;
                    }
                } catch (FormatException) {
                    Console.WriteLine("\tInvalid choice\n");
                }
            } while (!validChoice);

            string details;
            if (n == 1) {
                product.deliveryType = "Pickup";
                details = ClickCollect();
                string[] times = details.Split(' ');
                Console.WriteLine($"\nThank you for your bid. If successful, " +
                    $"the item will be provided via collection between " +
                    $"{times[1]} on {times[0]} and {times[3]} on {times[2]}");
            } else if (n == 2) {
                product.deliveryType = "Delivery";
                details = HomeDelivery();
                Console.WriteLine($"\nThank you for your bid. If successful, " +
                    $"the item will be provided via delivery to {details}");
            } else {
                product.deliveryType = "";
                details = "";
            }

            product.deliveryDetails = details;
            return product;
        }

        private string ClickCollect() {
            bool validStartTime = false;
            string startTime;
            DateTime startTimeTemp = DateTime.Now;
            do {
                Console.Write("\nDeliver window start (dd/mm/yyyy hh:mm)\n> ");
                startTime = Console.ReadLine() ?? "";
                DateTime temp;
                try {
                    temp = DateTime.Parse(startTime, CultureInfo.GetCultureInfo("en-AU"));
                    if ((temp - DateTime.Now).TotalHours < 1) {
                        Console.WriteLine("\tDelivery window start must be at least one hour in the future.\n");
                    } else {
                        validStartTime = true;
                        startTimeTemp = temp;
                    }
                } catch (FormatException) {
                    Console.WriteLine("\tPlease enter a valid date and time.\n");
                }
            } while (!validStartTime);

            bool validEndTime = false;
            string endTime;
            do {
                Console.Write("\nDelivery window end (dd/mm/yyyy hh:mm)\n> ");
                endTime = Console.ReadLine() ?? "";
                DateTime temp;
                try {
                    temp = DateTime.Parse(endTime, CultureInfo.GetCultureInfo("en-AU"));
                    if ((temp - startTimeTemp).TotalHours < 1) {
                        Console.WriteLine("\tDelivery window end must be at least one hour later than the start.\n");
                    } else {
                        validEndTime = true;
                    }
                } catch (FormatException) {
                    Console.WriteLine("\tPlease enter a valid date and time.\n");
                }
            } while (!validEndTime);

            return $"{startTime} {endTime}";
        }


        // Using copied functions from the AddressManager, get the delivery
        // Details and combine it into one string
        // Not an optimal design for future changes but it works fine for this
        // situation
        private string HomeDelivery() {

            Console.WriteLine("\nPlease provide your delivery address.");

            string unitNo = GetUnitNumber();
            string streetNo = GetStreetNumber();
            string streetName = GetStreetName();
            string streetSuffix = GetStreetSuffix();
            string city = GetCityName();
            string state = GetState();
            string postcode = GetPostcode();

            string deliver = $"{unitNo}/{streetNo} {streetName} {streetSuffix}, {city} {state} {postcode}";

            return deliver;
        }

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

        public ProductSearch(UserManager.User user, ProductManager productManager, AddressManager addressManager)
        {
            this.user = user;
            this.productManager = productManager;
            this.addressManager = addressManager;
            validator = new Validator();

            MenuMessage();
            string key = GetSearchKey();
            DisplayResults(key);
        }
    }
}

