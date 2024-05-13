using System;
using System.Collections.Generic;

namespace RecipeApplication
{
    // Delegate to notify the user when calories exceed 300
    public delegate void RecipeCaloriesExceeded(string recipeName, double totalCalories);

    // Ingredient class
    public class Ingredient
    {
        // getters and setters
        public string Name { get; set; }
        public double Quantity { get; set; }
        public string UnitOfMeasurement { get; set; }
        public double Calories { get; set; }
        public string FoodGroup { get; set; }

        // constructor
        public Ingredient(string name, double quantity, string unitOfMeasurement, double calories, string foodGroup)
        {
            Name = name;
            Quantity = quantity;
            UnitOfMeasurement = unitOfMeasurement;
            Calories = calories;
            FoodGroup = foodGroup;
        }
    }

    // Step class
    public class Step
    {
        // getter and setter
        public string Description { get; set; }

        public Step(string description)
        {
            Description = description;
        }
    }

    // Recipe class
    public class Recipe
    {
        public string Name { get; set; }
        public List<Ingredient> Ingredients { get; set; }
        public List<Step> Steps { get; set; }

        // Event to notify the user when calories exceed 300
        public event RecipeCaloriesExceeded CaloriesExceeded;

        public Recipe(string name)
        {
            Name = name;
            Ingredients = new List<Ingredient>();
            Steps = new List<Step>();
        }

        public void AddIngredient(string name, double quantity, string unit, double calories, string foodGroup)
        {
            Ingredients.Add(new Ingredient(name, quantity, unit, calories, foodGroup));
        }

        public void AddStep(string description)
        {
            Steps.Add(new Step(description));
        }

        // Calculates calories method
        public double CalculateTotalCalories()
        {
            double totalCalories = 0;
            foreach (var ingredient in Ingredients)
            {
                totalCalories += ingredient.Calories * ingredient.Quantity;
            }
            return totalCalories;
        }

        // Display the recipe method
        public void DisplayRecipe(bool recipeCaloriesExceeded = false)
        {
            Console.WriteLine($"Recipe: {Name}");
            Console.WriteLine("Ingredients:");
            foreach (var ingredient in Ingredients)
            {
                Console.WriteLine($"{ingredient.Quantity} {ingredient.UnitOfMeasurement} of {ingredient.Name}");
            }
            Console.WriteLine("\nSteps:");
            int stepNumber = 1;
            foreach (var step in Steps)
            {
                Console.WriteLine($"{stepNumber}. {step.Description}");
                stepNumber++;
            }
            Console.WriteLine($"Total Calories: {CalculateTotalCalories()}");

            // Checks the total calories exceed 300 and show warnings if necessary
            if (recipeCaloriesExceeded && CalculateTotalCalories() > 300)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Warning: Total calories for recipe '{Name}' exceed 300. Total calories: {CalculateTotalCalories()}");
                Console.ResetColor();
            }
        }
    }

    // Main class
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Welcome to the Recipe Application!");

            var recipes = new List<Recipe>();

            // While loop
            while (true)
            {
                Console.WriteLine("\nEnter the name of the recipe:");
                string recipeName = Console.ReadLine();

                var recipe = new Recipe(recipeName);

                Console.WriteLine("Enter the number of ingredients:");
                int numIngredients;
                while (!int.TryParse(Console.ReadLine(), out numIngredients) || numIngredients <= 0)
                {
                    Console.WriteLine("Invalid input. Please enter a valid number.");
                }

                // For loop
                for (int i = 0; i < numIngredients; i++)
                {
                    Console.WriteLine($"\nIngredient {i + 1}:");
                    Console.WriteLine("Name:");
                    string ingredientName = Console.ReadLine();
                    Console.WriteLine("Quantity:");
                    double quantity;
                    while (!double.TryParse(Console.ReadLine(), out quantity) || quantity <= 0)
                    {
                        Console.WriteLine("Invalid input. Please enter a valid quantity.");
                    }
                    Console.WriteLine("Unit of measurement:");
                    string unit = Console.ReadLine();
                    Console.WriteLine("Calories:");
                    double calories;
                    while (!double.TryParse(Console.ReadLine(), out calories) || calories <= 0)
                    {
                        Console.WriteLine("Invalid input. Please enter valid calories.");
                    }
                    Console.WriteLine("Food Group:");
                    string foodGroup = Console.ReadLine();

                    recipe.AddIngredient(ingredientName, quantity, unit, calories, foodGroup);
                }

                Console.WriteLine("\nEnter the number of steps:");
                int numSteps;
                while (!int.TryParse(Console.ReadLine(), out numSteps) || numSteps <= 0)
                {
                    Console.WriteLine("Invalid input. Please enter a valid number.");
                }

                for (int i = 0; i < numSteps; i++)
                {
                    Console.WriteLine($"\nStep {i + 1}:");
                    Console.WriteLine("Description:");
                    string description = Console.ReadLine();
                    recipe.AddStep(description);
                }

                // Subscribe to CaloriesExceeded event
                recipe.CaloriesExceeded += (name, totalCalories) =>
                {
                    Console.WriteLine($"Warning: Total calories for recipe '{name}' exceed 300. Total calories: {totalCalories}");
                };

                recipes.Add(recipe);

                Console.WriteLine("\nRecipe added successfully!");

                Console.WriteLine("\nDo you want to add another recipe? (yes/no)");
                string response = Console.ReadLine();
                if (response.ToLower() != "yes")
                {
                    break;
                }
            }

            // Display menu
            while (true)
            {
                Console.WriteLine("\nSelect an option:");
                Console.WriteLine("1. Display all recipes");
                Console.WriteLine("2. Display a specific recipe");
                Console.WriteLine("3. Exit");

                string option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        Console.WriteLine("\nRecipes:");
                        foreach (var recipe in recipes)
                        {
                            recipe.DisplayRecipe(recipeCaloriesExceeded: true);
                            Console.WriteLine();
                        }
                        break;
                    case "2":
                        Console.WriteLine("\nEnter the name of the recipe:");
                        string recipeName = Console.ReadLine();
                        Recipe selectedRecipe = recipes.Find(r => r.Name.Equals(recipeName, StringComparison.OrdinalIgnoreCase));
                        if (selectedRecipe != null)
                        {
                            selectedRecipe.DisplayRecipe();
                        }
                        else
                        {
                            Console.WriteLine("Recipe not found.");
                        }
                        break;
                    case "3":
                        Console.WriteLine("Thank you for using the Recipe Application!");
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }
    }
}

