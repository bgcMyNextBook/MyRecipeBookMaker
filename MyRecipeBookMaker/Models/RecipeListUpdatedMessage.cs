

namespace MyRecipeBookMaker.Models
{
    public class RecipeListUpdatedMessage
    {
        public bool Updated;
            public RecipeListUpdatedMessage(bool updated)
        {
            Updated = updated;
        }
    }
}
