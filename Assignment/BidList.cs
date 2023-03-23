using static Assignment.ProductManager;

namespace Assignment
{
    public class BidList
    {

        private readonly ProductManager productManager;
        private readonly UserManager.User user;

        private void MenuMessage() {
            string s = $"\nList Product Bids for {user.name}({user.email})";
            Console.WriteLine(s);
            for (int i = 0; i < s.Length; i++) {
                Console.Write('-');
            }
            Console.WriteLine("\n");
        }

        // Show all the products the signed in user advertised that have
        // a bid on them
        private void ShowBids() {
            productManager.OrderProducts();
            List<Product> products = productManager.GetProductsWithBid(user.email);
            int j;
            if (products.Count == 0) {
                Console.WriteLine("No bids were found.");
                return;
            } else {
                Console.WriteLine("Item #\tProduct name\tDescription\tList price\tBidder name\tBidder email\tBid amt");
                j = 0;
                foreach (Product product in products) {
                    j++;
                    Console.WriteLine($"{j}\t{product.productName}\t{product.productDescription}\t{product.listPrice}\t{product.bidderName}\t{product.bidderEmail}\t{product.bidAmt}");
                }
            }

            PromptSell(j);
        }


        // determine if the user would like to sell a specific item
        public void PromptSell(int j) {
            bool validPrompt = false;
            string res;
            do {
                Console.Write("\nWould you like to sell a product (yes or no)?\n> ");
                res = Console.ReadLine() ?? "";
                if (res == "yes" || res == "no") {
                    validPrompt = true;
                } else {
                    Console.WriteLine("\tPlease respond with yes or no.\n");
                }
            } while (!validPrompt);

            bool validItemChoice = false;
            string choice;
            do {
                Console.Write($"\nPlease enter an integet between 1 and {j}\n> ");
                choice = Console.ReadLine() ?? "";
                try {
                    int num = int.Parse(choice);
                    if (num <= j && num >= 1) {
                        validItemChoice = true;
                    } else {
                        Console.WriteLine("\tInvalid response.\n");
                    }
                } catch (FormatException) {
                    Console.WriteLine("\tInvalid response.\n");
                }
            } while (!validItemChoice);

            int index = productManager.productIndex[j - 1];
            Sell(index);
        }

        // Get the item and update it to sold so it can't be sold or bid on
        // again, and print the delivery details.
        private void Sell(int index) {
            Product product = productManager.products[index];
            product.sold = "true";
            productManager.products[index] = product;

            Console.WriteLine($"\nYou have sold {product.productName} to {product.bidderName} for {product.bidAmt}.");
            productManager.SaveProducts();

            if (product.deliveryType == "Delivery") {
                Console.WriteLine($"The product will be delivered to {product.deliveryDetails}.");
            } else if (product.deliveryType == "Pickup") {
                string[] stuff = product.deliveryDetails.Split(' ');
                Console.WriteLine($"The package can be picked up between {stuff[1]} on {stuff[0]} and {stuff[3]} on {stuff[2]}");
            }
        }

        public BidList(ProductManager productManager, UserManager.User user)
        {
            this.productManager = productManager;
            this.user = user;

            MenuMessage();
            ShowBids();
        }
    }
}

