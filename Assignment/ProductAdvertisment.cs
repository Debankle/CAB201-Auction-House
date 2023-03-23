namespace Assignment
{
    public class ProductAdvertisment
    {
        UserManager.User user;
        private readonly Validator validator;

        private void MenuMessage() {
            string s = $"\nProduct Advertisment for {user.name}({user.email})";
            Console.WriteLine(s);
            for (int i = 0; i < s.Length; i++) {
                Console.Write('-');
            }
            Console.WriteLine();
        }

        private string GetName() {
            bool validatedName = false;
            string name;
            do {
                Console.Write("\nProduct Name\n> ");
                name = Console.ReadLine() ?? "";
                if (!validator.ValidateProductName(name)) {
                    Console.WriteLine("\tPlease provide a valid product name\n");
                } else {
                    validatedName = true;
                }
            } while (!validatedName);
            return name;
        }

        private string GetDescription(string name) {
            bool validatedDescription = false;
            string description;
            do {
                Console.Write("\nProduct Description\n> ");
                description = Console.ReadLine() ?? "";
                if (!validator.ValidateDescription(name, description)) {
                    Console.WriteLine("\tPlease provide a valid product description\n");
                } else {
                    validatedDescription = true;
                }
            } while (!validatedDescription);
            return description;
        }

        private string GetPrice() {
            bool validatedPrice = false;
            string price;
            do {
                Console.Write("\nProduct price ($d.cc)\n> ");
                price = Console.ReadLine() ?? "";
                if (!validator.ValidateCurrency(price)) {
                    Console.WriteLine("\tA currency value is required, e.g. $54.95, $9.99, $2314.15.\n");
                } else {
                    validatedPrice = true;
                }
            } while (!validatedPrice);
            return price;
        }


        // Run-once initialization that creates a product with three loop prompts
        // and adds it to the productManager
        public ProductAdvertisment(UserManager.User user, ProductManager productManager)
        {
            this.user = user;
            validator = new Validator();

            MenuMessage();

            string productName = GetName();
            string description = GetDescription(productName);
            string price = GetPrice();

            ProductManager.Product product;
            product.email = user.email;
            product.productName = productName;
            product.productDescription = description;
            product.listPrice = price;
            product.bidderName = "-";
            product.bidderEmail = "-";
            product.bidAmt = "-";
            product.sold = "false";
            product.deliveryType = "not done";
            product.deliveryDetails = "no";
            productManager.AddProduct(product);

            Console.WriteLine($"\nSuccessfully added product {productName} - {description}, {price}");
        }
    }
}

