using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipeBookMaker.Common
{
    static class ListHelper
    {
        public static async Task<string> CopyBundledModelFilesToAppDataDirectory(string filePath)
        {
            try
            {
                Stream fileStream;
                StreamReader reader;
                string content;
                string targetDirectory = Path.Combine(FileSystem.AppDataDirectory, Path.GetFileName(filePath));
                try
                {
                    fileStream = await FileSystem.Current.OpenAppPackageFileAsync(targetDirectory);
                    reader = new StreamReader(fileStream);
                    content = await reader.ReadToEndAsync();
                    string appDataFilePath = Path.Combine(FileSystem.AppDataDirectory, Path.GetFileName(targetDirectory));
                    await File.WriteAllTextAsync(appDataFilePath, content);

                }
                catch
                {

                }
                content = File.ReadAllText(targetDirectory);
                if (!Directory.Exists(targetDirectory))
                {
                    Directory.CreateDirectory(targetDirectory);
                }
                
                foreach (string file in Directory.GetFiles(filePath))
                {
                    fileStream = await FileSystem.Current.OpenAppPackageFileAsync(file);
                    reader = new StreamReader(fileStream);

                    content = await reader.ReadToEndAsync();
                    string appDataFilePath = Path.Combine(targetDirectory, Path.GetFileName(file));
                    await File.WriteAllTextAsync(appDataFilePath, content);
                }

                return targetDirectory;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }
        public static async Task<string> ReadBundledTextFile(string filePath)
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
            }
            return jsonData;
        }
    }
}
