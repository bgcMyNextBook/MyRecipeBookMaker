using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using Newtonsoft.Json.Linq;


namespace MyRecipeBookMaker.Common
{
    class RecipeOutputToWordTemplate
    {
        static bool CreateWordDocumentFromTemplate(string jsonForRecipe) 
        {
            // Deserialize JSON data
            JObject recipe = JObject.Parse(jsonForRecipe);

            using (FileStream fileStream = new FileStream("Template.docx", FileMode.Open, FileAccess.ReadWrite))
            {
                using (WordDocument document = new WordDocument(fileStream, FormatType.Docx))
                {
                    // Create MailMergeDataTable for the recipe
                    MailMergeDataTable recipeTable = new MailMergeDataTable("Recipe", new List<ExpandoObject> { ToExpandoObject(recipe) });

                    // Create MailMergeDataTable for the ingredients
                    JArray ingredientGroupsArray = (JArray)recipe["ingredientGroups"];
                    List<ExpandoObject> ingredientsList = new List<ExpandoObject>();
                    foreach (JObject ingredientGroup in ingredientGroupsArray)
                    {
                        JArray ingredientsArray = (JArray)ingredientGroup["ingredients"];
                        foreach (JObject ingredient in ingredientsArray)
                        {
                            ingredientsList.Add(ToExpandoObject(ingredient));
                        }
                    }
                    MailMergeDataTable ingredientsTable = new MailMergeDataTable("Ingredients", ingredientsList);

                    // Execute mail merge
                    document.MailMerge.ExecuteGroup(recipeTable);
                    document.MailMerge.ExecuteGroup(ingredientsTable);

                    // Save the result
                    using (FileStream outputStream = new FileStream("RecipeOutput.docx", FileMode.Create, FileAccess.ReadWrite))
                    {
                        document.Save(outputStream, FormatType.Docx);
                    }
                }
            }
            return true;
        }
        static ExpandoObject ToExpandoObject(JObject jObject)
        {
            IDictionary<string, object> expando = new ExpandoObject();
            foreach (var property in jObject.Properties())
            {
                expando.Add(property.Name, property.Value.ToString());
            }
            return (ExpandoObject)expando;
        }
    }
}



