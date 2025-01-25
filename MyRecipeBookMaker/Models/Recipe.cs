using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipeBookMaker.Models
{

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Ingredient
    {
        public string name { get; set; }
        public double? quantity { get; set; }
        public string unit { get; set; }
    }

    public class IngredientGroup
    {
        public string name { get; set; }
        public List<Ingredient> ingredients { get; set; }
    }

    public class Instruction
    {
        public int step { get; set; }
        public string description { get; set; }
    }

    public class InstructionGroup
    {
        public string name { get; set; }
        public List<Instruction> instructions { get; set; }
    }

    public class Nutrition
    {
        public string combinedListOfNutriates { get; set; }
        public List<string> listOfNutriates { get; set; }

    }

    public class Recipe
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string servings { get; set; }
        public string cookTime { get; set; }
        public string prepTime { get; set; }
        public string totalTime { get; set; }
        public string author { get; set; }
        public string cuisine { get; set; }
        public string course { get; set; }
        public string imageBASE64 { get; set; }
        public string imageURL { get; set; }
        public object calories { get; set; }
        public Nutrition nutrition { get; set; }
        public List<IngredientGroup> ingredientGroups { get; set; }
        public List<InstructionGroup> instructionGroups { get; set; }
        public List<string> tags { get; set; }
        public string notes { get; set; }
    }
}
