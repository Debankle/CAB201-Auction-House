using System;
namespace Assignment
{
    public class ProductManager
    {

        private const string productFileName = "products.txt";

        // Product struct, defining the structure of a product in the program
        // taken from a text file and represented like this so to be easier to
        // manage
        public struct Product {
            public string email;
            public string productName;
            public string productDescription;
            public string listPrice;
            public string bidderName;
            public string bidderEmail;
            public string bidAmt;
            public string sold;
            public string deliveryType;
            public string deliveryDetails;
        }

        // products should be private but this program has gotten so convoluted
        // it was impossible to preserve that and i just gave up on writing
        // another four complex accessor functions and left it to die
        public List<Product> products;
        public List<int> productIndex;

        public List<Product> GetPurchasedProducts(string email) {
            List<Product> purchasedProducts = new();
            foreach (Product product in products) {
                if (product.bidderEmail == email && product.sold == "true") {
                    purchasedProducts.Add(product);
                }
            }
            return purchasedProducts;
        }

        // Return all the products a user advertised, that havent been sold but
        // have been bid on
        public List<Product> GetProductsWithBid(string email) {
            List<Product> productBid = new();
            productIndex.Clear();
                int j = 0;
            foreach (Product product in products) {
                if (product.bidderEmail != email && product.bidAmt != "-" && product.sold == "false") {
                    productBid.Add(product);
                    productIndex.Add(j);
                }
                j++;
            }
            return productBid;
        }

        // Order the products alphabetically by name, description and then price
        public void OrderProducts() {
            products = products.OrderBy(x => x.productName).ThenBy(x => x.productDescription).ThenBy(x => x.listPrice).ToList();
        }

        // Get all the products in the case of search key ALL
        public List<Product> GetAllProducts(string email) {
            List<Product> allProducts = new();
            productIndex.Clear();
            int j = 0;
            foreach (Product product in products) {
                if (product.email != email && product.sold == "false") {
                    allProducts.Add(product);
                    productIndex.Add(j);
                }
                j++;
            }
            return allProducts;
        }

        // Alternative search function, which returns all products that match
        // the key and aren't posted by the logged in user
        public List<Product> GetProductsByKey(string key, string email) {
            List<Product> allProducts = new();
            productIndex.Clear();
            int j = 0;
            foreach (Product product in products) {
                if (product.productName.Contains(key) || product.productDescription.Contains(key)) {
                    if (product.email != email && product.sold == "false") {
                        productIndex.Add(j);
                        allProducts.Add(product);
                    }
                }
                j++;
            }
            return allProducts;
        }

        // Return a list of all the products the user advertised, for the product list
        public List<Product> GetProductsByUser(string email) {
            List<Product> userProducts = new();
            foreach (Product product in products) {
                if (product.email == email && product.sold == "false") {
                    userProducts.Add(product);
                }
            }
            return userProducts;
        } 

        // Write a product into the database txt file, and then add the product
        // to the list
        public void AddProduct(Product product) {
            using StreamWriter writer = new(File.Open(productFileName, FileMode.Append));
            writer.WriteLine("Product");
            writer.WriteLine(product.email);
            writer.WriteLine(product.productName);
            writer.WriteLine(product.productDescription);
            writer.WriteLine(product.listPrice);
            writer.WriteLine(product.bidderName);
            writer.WriteLine(product.bidderEmail);
            writer.WriteLine(product.bidAmt);
            writer.WriteLine(product.sold);
            writer.WriteLine(product.deliveryType);
            writer.WriteLine(product.deliveryDetails);
            products.Add(product);
        }

        // Load in every product in the database txt, creating a new struct
        // instance for each one
        private void LoadProducts() {
            using StreamReader reader = new(productFileName);
            while (!reader.EndOfStream) {
                string currentLine = reader.ReadLine() ?? "";
                if (currentLine == "Product") {
                    Product newProduct;
                    newProduct.email = reader.ReadLine() ?? "";
                    newProduct.productName = reader.ReadLine() ?? "";
                    newProduct.productDescription = reader.ReadLine() ?? "";
                    newProduct.listPrice = reader.ReadLine() ?? "";
                    newProduct.bidderName = reader.ReadLine() ?? "";
                    newProduct.bidderEmail = reader.ReadLine() ?? "";
                    newProduct.bidAmt = reader.ReadLine() ?? "";
                    newProduct.sold = reader.ReadLine() ?? "";
                    newProduct.deliveryType = reader.ReadLine() ?? "";
                    newProduct.deliveryDetails = reader.ReadLine() ?? "";
                    products.Add(newProduct);
                }
            }
        }

        // When updating a product, the database needs to be backed up, or it won't be saved.
        // Unfortunately this means that the file has to be rewritten every time
        // which is not at all optimal but it does work, just would get slow
        // for too many products
        public void SaveProducts() {
            using StreamWriter writer = new(productFileName, false);
            foreach (Product product in products) {
                writer.WriteLine("Product");
                writer.WriteLine(product.email);
                writer.WriteLine(product.productName);
                writer.WriteLine(product.productDescription);
                writer.WriteLine(product.listPrice);
                writer.WriteLine(product.bidderName);
                writer.WriteLine(product.bidderEmail);
                writer.WriteLine(product.bidAmt);
                writer.WriteLine(product.sold);
                writer.WriteLine(product.deliveryType);
                writer.WriteLine(product.deliveryDetails);
            }
        }

        public ProductManager()
        {
            using StreamWriter wr = File.AppendText(productFileName);
            wr.Close();

            products = new List<Product>();
            productIndex = new List<int>();
            LoadProducts();
        }
    }
}

