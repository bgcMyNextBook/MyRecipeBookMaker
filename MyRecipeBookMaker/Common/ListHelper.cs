using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipeBookMaker.Common
{
    static class ListHelper
    {
        public static async  Task<string> ReadBundledTextFile(string filePath)
        {
            using Stream fileStream = await FileSystem.Current.OpenAppPackageFileAsync(filePath);
            using StreamReader reader = new StreamReader(fileStream);

            return await reader.ReadToEndAsync();
        }
        public static async Task<string> ReadDataFileWithDefault(string filename, string defaultBundle)
        {
            string appDataFilePath = Path.Combine(FileSystem.AppDataDirectory, filename);


            string jsonData = null;

            if (File.Exists(appDataFilePath))
            {
                jsonData = await File.ReadAllTextAsync(appDataFilePath);
            }
            else
            {
                jsonData = await ListHelper.ReadBundledTextFile(defaultBundle);
                //jsonData = await File.ReadAllTextAsync(rawFilePath);
            }
            return jsonData;
        }
    }
}
