using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipeBookMaker.Models
{
    public class DropDownListObject
    {
        public DropDownListObject(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}
