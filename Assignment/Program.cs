using System.IO;

namespace Assignment {

    class Program {

        // Program entry point
        static void Main(string[] args) {

            // Create an auction house object and open it, start the loop,
            // Then exit smoothly when it's closed
            AuctionHouse auctionHouse = new();
            auctionHouse.OpenAuction();
            auctionHouse.AuctionLoop();
            auctionHouse.CloseAuction();
            Environment.Exit(0);
        }
    }

}