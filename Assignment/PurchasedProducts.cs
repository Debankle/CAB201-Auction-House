using System;
namespace Assignment
{
    public class PurchasedProducts
    {

        private readonly ProductManager productManager;
        private readonly UserManager.User user;


        // Dense function that does everything once and then returns to
        // the client menu state
        // It fetches the ordered products that have been sold for the specified
        // email and lists them in a tabular view
        private void ShowPurchasedProducts() {
            string s = $"\nPurchased Items for {user.name}({user.email})";
            Console.WriteLine(s);
            for (int i = 0; i < s.Length; i++) {
                Console.Write('-');
            }
            Console.WriteLine("\n");

            productManager.OrderProducts();
            List<ProductManager.Product> products = productManager.GetPurchasedProducts(user.email);

            if (products.Count == 0) {
                Console.WriteLine("You have no purchased products at the moment.");
            } else {
                Console.WriteLine("Item #\tSeller email\tProduct name" +
                    "\tDescription\tList price\tPurchased Price\tDelivery");

                int j = 0;
                foreach (ProductManager.Product product in products) {
                    j++;
                    if (product.deliveryType == "Delivery") {
                        Console.WriteLine($"{j}\t{product.email}\t" +
                            $"{product.productName}\t{product.productDescription}" +
                            $"\t{product.listPrice}\t{product.bidAmt}\tDelivered to {product.deliveryDetails}");
                    } else {
                        string[] stuff = product.deliveryDetails.Split(" ");
                        Console.WriteLine($"{j}\t{product.email}\t{product.productName}" +
                            $"\t{product.productDescription}\t{product.listPrice}" +
                            $"\t{product.bidAmt}\tPickup between {stuff[1]} on {stuff[0]} and {stuff[3]} on {stuff[2]}");
                    }
                }
            }
        }

        public PurchasedProducts(ProductManager productManager, UserManager.User user)
        {
            this.productManager = productManager;
            this.user = user;

            ShowPurchasedProducts();
        }
    }
}

