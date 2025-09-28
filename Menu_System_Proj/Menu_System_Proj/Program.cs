using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using static System.Net.Mime.MediaTypeNames;



//                                                                        TESTING PLAN
//------------------------------------------------------------------------------------------------------------------------------------------------------------------------
//Index                        Test Case                                               Expected Result                     Actual Result
//------------------------------------------------------------------------------------------------------------------------------------------------------------------------
//Main Menu Tests
//------------------------------------------------------------------------------------------------------------------------------------------------------------------------
//1                            Select option "1" (Energy Calculator)                 Energy Calculator opens successfully   Energy Calculator opens successfully
//2                            Select option "2" (Product Listing)                   Product Listing menu opens             Product Listing menu opens
//3                            Select option "3" (Text Encoder)                      Text Encoder opens                     Text Encoder opens
//4                            Enter "q" or "Q"                                      Application exits                      Application exits
//5                            Enter invalid option (e.g., "x")                      Error message, retry prompt            Error message, retry prompt

//------------------------------------------------------------------------------------------------------------------------------------------------------------------------
//Energy Calculator Tests
//------------------------------------------------------------------------------------------------------------------------------------------------------------------------
//6                            Enter valid appliance name                            Proceeds to power rating input          Proceeds to power rating input
//7                            Enter invalid appliance name (empty string)           Error message, retry prompt             Error message, retry prompt
//8                            Enter "b" (appliance input)                           Returns to main menu                    Returns to main menu
//9                            Enter valid power rating (e.g., 1.5)                  Proceeds to hours input                 Proceeds to hours input
//10                           Enter invalid power rating (e.g., -1 or "abc")        Error message, retry prompt             Error message, retry prompt
//11                           Enter valid hours per day (e.g. 5)                    Proceeds to results display             Proceeds to results display
//12                           Enter invalid hours per day (e.g. 25)                 Error message, retry prompt             Error message, retry prompt
//13                           Enter "b" (hours per day input)                       Returns to power rating input           Returns to power rating input
//14                           Complete valid input                                  Displays correct energy calculations    Displays correct energy calculations
//15                           Enter "b" (power rating input)                        Returns to appliance name input         Returns to appliance name input

//------------------------------------------------------------------------------------------------------------------------------------------------------------------------
//Product Listing Tests
//------------------------------------------------------------------------------------------------------------------------------------------------------------------------
//16                           Select option "1"                                     Displays category list                  Displays category list
//17                           Add product with valid details                        Product is added successfully           Product is added successfully
//18                           Enter invalid product name                            Error message, retry prompt             Error message, retry prompt
//19                           Enter invalid product cost (e.g., -1, "abc")          Error message, retry prompt             Error message, retry prompt
//20                           Enter invalid quantity (e.g., -5, "xyz")              Error message, retry prompt             Error message, retry prompt
//21                           Enter "b" at any step                                 Returns to previous menu                Returns to previous menu
//22                           Select option "2" to list products                    Displays correct product details        Displays correct product details
//23                           Enter valid product cost (e.g., 5.99)                 Proceeds to quantity input              Proceeds to quantity input
//24                           Enter valid quantity (e.g., 10)                       Product added successfully              Product added successfully
//------------------------------------------------------------------------------------------------------------------------------------------------------------------------
//Text Encoder Tests
//------------------------------------------------------------------------------------------------------------------------------------------------------------------------
//25                           Enter valid input (e.g., "hello")                     Displays binary encoding                Displays binary encoding
//26                           Enter input with spaces (e.g., "hi there")            Displays encoded result with spaces     Displays encoded result with spaces
//27                           Enter unrecognized characters (e.g., "#123")          Warning for unrecognized characters     Warning for unrecognized characters
//28                           Enter "b"                                             Returns to main menu                    Returns to main menu

//------------------------------------------------------------------------------------------------------------------------------------------------------------------------
//Boundary and Stress Tests
//------------------------------------------------------------------------------------------------------------------------------------------------------------------------
//29                           Enter power rating of 0                               Proceeds with calculation               Proceeds with calculation
//30                           Enter hours per day of 0                              Displays energy as 0 kWh                Displays energy as 0 kWh
//31                           Add product with cost of 0                            Product added successfully              Product added successfully
//32                           Add product with very high cost (e.g., 999999)        Product added successfully              Product added successfully
//33                           List products with empty category                     Displays "0 items" message              Displays "0 items" message
//34                           Add 1000 products                                     Application remains responsive          Application remains responsive
//35                           Perform rapid inputs                                  No crashes, menus function correctly    No crashes, menus function correctly
//------------------------------------------------------------------------------------------------------------------------------------------------------------------------

namespace MenuSystem
{
    class Program
    {
        static List<Category> categories = new List<Category>();
        public static bool currentlyTesting { get; set; } = false;

        public static void Main(string[] args)
        {
            // Main Menu Loop - each subprogram reads from its own class 
            while (true)
            {
                DisplayMenu();
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        EnergyCalculator.RunCalculator();
                        break;
                    case "2":
                        ProductList.RunProductListing();
                        break;
                    case "3":
                        TextEncoder.RunEncoder();
                        break;
                    case "q":
                    case "Q":
                        Console.WriteLine("Exiting application...");
                        return;
                    default:
                        Console.WriteLine("\nInvalid option, please try again.\n");
                        Console.ReadKey();
                        break;
                }
            }
        }

        public static void ConsClear()
        {
            if (!currentlyTesting)
            {
                Console.Clear();
            }
        }
        static void DisplayMenu()
        {
            ConsClear();
            Console.WriteLine("===== Main Menu =====");
            Console.WriteLine("1. Run Energy Calculator");
            Console.WriteLine("2. Run Product List");
            Console.WriteLine("3. Run Character Encoder");
            Console.WriteLine("q. Quit");
            Console.Write("Please choose an option: ");
        }
    }

    // Energy Calculator Class
    public static class EnergyCalculator
    {
        public static void RunCalculator()
        {
            Program.ConsClear();
            Console.WriteLine("===== Energy Calculator =====");

            Console.WriteLine("Running Energy Calculator... (Enter 'b' to go back to the previous step)");

            string applianceName = null;
            double powerRating = -1;
            double hoursPerDay = -1;

            int stage = 1;  // Controls the current prompt stage

            while (true)
            {
                switch (stage)
                {
                    case 1: // Prompt for name of the appliance 
                        Program.ConsClear();
                        Console.WriteLine("===== Energy Calculator =====");
                        Console.Write("Enter appliance name (or 'b' to go back): ");
                        applianceName = Console.ReadLine();
                        if (applianceName.ToLower() == "b")
                        {
                            Program.ConsClear();
                            return;
                        }
                        else if (!string.IsNullOrEmpty(applianceName))
                        {
                            stage++;
                        }
                        else
                        {
                            Console.WriteLine("Invalid input. Please enter a valid name.");
                        }
                        break;

                    case 2: // Prompt for Power Rating
                        Program.ConsClear();
                        Console.WriteLine("===== Energy Calculator =====");
                        Console.Write("Enter the power rating of the appliance in kilowatts (e.g., 1.5) or 'b' to go back: ");
                        powerRating = DoubleInputHandling();
                        if (powerRating == -1)
                        {
                            stage--; // Go back to previous prompt
                        }
                        else
                        {
                            stage++; // Continue to next prompt stage 
                        }
                        break;

                    case 3: // Prompt for Hours Per Day
                        Program.ConsClear();
                        Console.WriteLine("===== Energy Calculator =====");
                        Console.Write("Enter the number of hours the appliance is used per day (max 24) or 'b' to go back: ");
                        hoursPerDay = DoubleInputHandling(24);
                        if (hoursPerDay == -1)
                        {
                            stage--; // Go back to previous prompt
                        }
                        else
                        {
                            stage++; // Continue to next prompt stage 
                        }
                        break;

                    case 4: // Perform Calculations and Display Results
                        double energyPerDay = powerRating * hoursPerDay;
                        double energyPerMonth = energyPerDay * 30;
                        double energyPerYear = energyPerDay * 365;

                        Console.WriteLine();
                        Console.WriteLine($"Energy Usage for {applianceName}:");
                        Console.WriteLine($"Daily Energy Usage: {energyPerDay:F2} kWh");
                        Console.WriteLine($"Monthly Energy Usage: {energyPerMonth:F2} kWh");
                        Console.WriteLine($"Yearly Energy Usage: {energyPerYear:F2} kWh");

                        Console.WriteLine("\nPress any key to return to the main menu..."); // takes input from user before returning 
                        Console.ReadLine();
                        Program.ConsClear();
                        return; // Exit to main menu after displaying results
                }
            }
        }

        private static double DoubleInputHandling(double maxValue = double.MaxValue)
        {
            while (true)
            {
                string input = Console.ReadLine();
                if (input.ToLower() == "b") return -1;

                if (double.TryParse(input, out double result) && result >= 0 && result <= maxValue) // checks to see if input is between 0 and max value - stored as a double 
                    return result;

                Console.Write($"Invalid input. Try Again, or 'b' to go back: ");
            }
        }
    }


    // Product Management Class
    public static class ProductList
    {
        private static List<Category> categories = new List<Category> // creates lists for each category 
    {
        new Category("Fruit & Vegetables"),
        new Category("Bakery"),
        new Category("Dairy")
    };

        public static void RunProductListing()
        {
            Program.ConsClear();
            Console.WriteLine("Running Product Listing...\n");

            bool exitProductListing = false;

            while (!exitProductListing) // runs program while exit variable is false 
            {
                DisplayProductManagementMenu();
                string choice = Console.ReadLine(); // takes input from user

                switch (choice.ToLower())
                {
                    case "1":
                        AddProductToCategory();
                        break;
                    case "2":
                        ListProductsByCategory();
                        break;
                    case "b":
                        exitProductListing = true; // Return to the main menu
                        Program.ConsClear();
                        break;
                    default:
                        Console.WriteLine("\nInvalid option, please try again.\n");
                        break;
                }
            }
        }

        private static void DisplayProductManagementMenu()
        {
            // displays main product management menu options to user
            Console.WriteLine("===== Product Management Menu =====");
            Console.WriteLine("1. Add product to Category");
            Console.WriteLine("2. List Products by Category");
            Console.WriteLine("b. Back to Main Menu");
            Console.Write("Please choose an option: ");
        }

        private static void AddProductToCategory()
        {
            Program.ConsClear();
            Console.WriteLine("Select Product Category (or 'b' to go back):");

            int categoryIndex; // stores users chosen category index
            bool addToAllCategories = false; // tracks whether user wants to add product to all categories

            while (true)
            {
                if (DisplayCategoriesAndGetIndex(out categoryIndex, true)) return; // If 'b' is entered, exit to previous menu
                if (categoryIndex >= 0 && categoryIndex < categories.Count)
                    break; // carrys on if category valid

                if (categoryIndex == categories.Count) // "All" category selected
                {
                    addToAllCategories = true;
                    break; // adds products to all categories
                }

                Console.WriteLine("Invalid category selection. Please choose a valid category.");
            }

            // variables store the details of the products, will be retained if the user chooses to go back on any step
            string productName = "";
            double cost = 0;
            int quantity = 0;

            int step = 1; // tracks the current step the program is on - 1 = product name, 2 = cost, 3 = quantity

            while (true)
            {
                switch (step)
                {
                    case 1:
                        Program.ConsClear();
                        Console.WriteLine("===== Product Management Menu =====");
                        Console.Write("Enter the name of the product (or 'b' to go back): ");
                        productName = Console.ReadLine();
                        if (productName.ToLower() == "b")
                        {
                            Program.ConsClear();
                            return;
                        }
                        step++; // goes onto next step
                        break;

                    case 2:
                        Program.ConsClear();
                        Console.WriteLine("===== Product Management Menu =====");
                        Console.Write("Enter the cost of one unit (or 'b' to go back): £");
                        if (GetCorrectDoubleInput(out cost))
                        {
                            step--; // goes back to previous step if requested by user
                        }
                        else
                        {
                            step++;
                        }
                        break;

                    case 3:
                        Program.ConsClear();
                        Console.WriteLine("===== Product Management Menu =====");
                        Console.Write("Enter the quantity of items (or 'b' to go back): ");
                        if (GetValidIntInput(out quantity))
                        {
                            step--;
                        }
                        else
                        {
                            step++;
                        }
                        break;

                    case 4:
                        // adds products to category selected by user
                        Product newProduct = new Product(productName, cost, quantity);

                        if (addToAllCategories)
                        {
                            foreach (var category in categories)
                            {
                                category.AddProduct(newProduct);
                            }
                            Console.WriteLine($"{productName} added to all categories.");
                        }
                        else if (categoryIndex >= 0 && categoryIndex < categories.Count)
                        {
                            categories[categoryIndex].AddProduct(newProduct);
                            categories[categories.Count - 1].AddProduct(newProduct); // Add to "All" category
                            Console.WriteLine($"{productName} added to {categories[categoryIndex].CategoryName}.");
                        }
                        else
                        {
                            Console.WriteLine("Error: Invalid category selection. Returning to Product Listing Menu...");
                        }

                        // wait for user input then returns to main menu
                        Console.WriteLine("\nPress any key to return to Product Listing Menu...");
                        Console.ReadLine();
                        Program.ConsClear();
                        return;
                }
            }
        }

        private static void ListProductsByCategory()
        {
            Program.ConsClear();
            Console.WriteLine("Select Category to List (or 'b' to go back):");

            if (DisplayCategoriesAndGetIndex(out int categoryIndex, true)) return; //gets the category selectd by the user

            if (categoryIndex == categories.Count)
            {
                DisplayCategoryDetails(categories[categories.Count - 1]); // Display "All" category separately
            }
            else
            {
                DisplayCategoryDetails(categories[categoryIndex]);
            }
        }

        private static bool DisplayCategoriesAndGetIndex(out int categoryIndex, bool includeAllOption = false)
        {
            for (int i = 0; i < categories.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {categories[i].CategoryName}");
            }
            if (includeAllOption)
            {
                Console.WriteLine($"{categories.Count + 1}. All");
            }
            Console.Write("Please select a category: ");

            string input = Console.ReadLine();
            if (input.ToLower() == "b")
            {
                categoryIndex = -1;
                Program.ConsClear();
                return true;
            }

            if (int.TryParse(input, out categoryIndex) &&
                categoryIndex > 0 &&
                categoryIndex <= (includeAllOption ? categories.Count + 1 : categories.Count))
            {
                categoryIndex--; // Convert to zero-based index
                return false;
            }

            Console.WriteLine("\nInvalid category. Please try again.\n");
            return DisplayCategoriesAndGetIndex(out categoryIndex, includeAllOption); // Retry until valid input
        }


        private static bool GetCorrectDoubleInput(out double value)
        {
            while (true)
            {
                string input = Console.ReadLine();
                if (input.ToLower() == "b")
                {
                    value = -1;
                    return true; // Go back
                }

                if (double.TryParse(input, out value) && value >= 0)
                {
                    return false;
                }

                Console.Write("Invalid input. Please enter a valid positive number (or 'b' to go back): ");
            }
        }

        private static bool GetValidIntInput(out int value)
        {
            while (true)
            {
                string input = Console.ReadLine();
                if (input.ToLower() == "b")
                {
                    value = -1;
                    return true; // Go back
                }

                if (int.TryParse(input, out value) && value >= 0)
                {
                    return false;
                }

                Console.Write("Invalid input. Please enter a valid positive integer (or 'b' to go back): ");
            }
        }

        private static void DisplayCategoryDetails(Category category)
        {
            Program.ConsClear();
            Console.WriteLine($"Total number of items: {category.GetTotalItems()}");
            Console.WriteLine($"Item names: {string.Join(", ", category.GetProductNames())}");
            Console.WriteLine($"Total cost of items: £{category.GetTotalCost():F2}");
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadLine();
            Program.ConsClear();
        }
    }

    // Text Encoder Class
    public static class TextEncoder
    {
        public static void RunEncoder()
        {
            Program.ConsClear();
            Console.WriteLine("Running Character Encoder...");

            // Define dictionary for encoding each character
            Dictionary<char, string> charToBinary = new Dictionary<char, string>()
        {
            {'a', "00001"}, {'b', "00010"}, {'c', "00011"}, {'d', "00100"}, {'e', "00101"},
            {'f', "00110"}, {'g', "00111"}, {'h', "01000"}, {'i', "01001"}, {'j', "01010"},
            {'k', "01011"}, {'l', "01100"}, {'m', "01101"}, {'n', "01110"}, {'o', "01111"},
            {'p', "10000"}, {'q', "10001"}, {'r', "10010"}, {'s', "10011"}, {'t', "10100"},
            {'u', "10101"}, {'v', "10110"}, {'w', "10111"}, {'x', "11000"}, {'y', "11001"},
            {'z', "11010"}
        };

            Console.WriteLine("Enter characters you wish to encode (or 'b' to return to the main menu): ");
            string inputFromUser = Console.ReadLine().ToLower();

            if (inputFromUser == "b")
            {
                Program.ConsClear();
                return;  // Return to the main menu if user enters 'b'
            }
            string encodedResult = "";
            bool hasUnrecognizedCharacters = false;

            foreach (char c in inputFromUser)
            {
                if (charToBinary.ContainsKey(c)) // Check character in dictionary
                {
                    encodedResult += charToBinary[c] + " ";
                }
                else if (c == ' ') // Handles spaces
                {
                    encodedResult += "// ";
                }
                else // Handles unrecognized characters
                {
                    Console.WriteLine($"Character '{c}' not recognized - cannot be encoded");
                    hasUnrecognizedCharacters = true;
                }
            }

            if (!hasUnrecognizedCharacters)
            {
                Console.WriteLine("\nEncoded Text Result:");
                Console.WriteLine(encodedResult.Trim()); // Displays encoded result without trailing spaces
            }
            else
            {
                Console.WriteLine("Some characters were not recognized and could not be encoded.");
            }

            Console.WriteLine("\nPress any key to return to the main menu...");
            Console.ReadLine();  // Wait for user input before returning to the main menu
            Program.ConsClear();
        }
    }


    // Product and Category Classes
    public class Product
    {
        public string Name { get; set; }
        public double Cost { get; set; }
        public int Quantity { get; set; }

        public Product(string name, double cost, int quantity)
        {
            Name = name;
            Cost = cost;
            Quantity = quantity;
        }

        public double TotalCost()
        {
            return Cost * Quantity;
        }
    }

    public class Category
    {
        public string CategoryName { get; set; }
        public List<Product> Products { get; set; }

        public Category(string categoryName)
        {
            CategoryName = categoryName;
            Products = new List<Product>();
        }

        public void AddProduct(Product product)
        {
            Products.Add(product);
        }

        public int GetTotalItems()
        {
            int total = 0;
            foreach (var product in Products)
            {
                total += product.Quantity;
            }
            return total;
        }

        public double GetTotalCost()
        {
            double totalCost = 0;
            foreach (var product in Products)
            {
                totalCost += product.TotalCost();
            }
            return totalCost;
        }

        public List<string> GetProductNames()
        {
            List<string> productNames = new List<string>();
            foreach (var product in Products)
            {
                productNames.Add(product.Name);
            }
            return productNames;
        }
    }
}
