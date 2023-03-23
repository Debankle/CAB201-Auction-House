using System;
namespace Assignment
{
    public class ProductList
    {

        public ProductList(UserManager.User user, ProductManager productManager)
        {
            string s = $"\nProduct List for {user.name}({user.email})";
            Console.WriteLine(s);
            for (int i = 0; i < s.Length; i++) {
                Console.Write('-');
            }
            Console.WriteLine("\n");

            productManager.OrderProducts();
            List<ProductManager.Product> products = productManager.GetProductsByUser(user.email);

            if (products.Count == 0) {
                Console.WriteLine($"User {user.name}({user.email}) has no products listed");
            } else {
                Console.WriteLine("Item #\tProduct name\tDescription\tList price\tBidder name\tBidder email\tBid amt");
                int j = 0;
                foreach (ProductManager.Product product in products) {
                    j++;
                    Console.WriteLine($"{j}\t{product.productName}\t{product.productDescription}\t{product.listPrice}\t{product.bidderName}\t{product.bidderEmail}\t{product.bidAmt}");
                }
            }
        }
    }
}

