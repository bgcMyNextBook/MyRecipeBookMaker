using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Schema;
using System.Threading.Tasks;

using Microsoft.ML.OnnxRuntimeGenAI;

using MyRecipeBookMaker.Common;
using MyRecipeBookMaker.Models;

using static System.Net.Mime.MediaTypeNames;

namespace MyRecipeBookMaker.AI
{
    class LocalAIReadRecipe
    {
        string developerPrompt = "";
        string userPrompt = "";
        Tokenizer tokenizer;
        MultiModalProcessor processor;
        TokenizerStream? tokenizerStream;
        Model model;
        public LocalAIReadRecipe()
        {
            // Get the application data directory
            //string appDataPath = FileSystem.AppDataDirectory;

            //Console.WriteLine($"App Data Directory: {appDataPath}");

            // Load the model from the app data directory
            
            // Create a session
            //var session = new InferenceSession(model);

            // Create an input tensor
            //var input = new Tensor(new float[] { 1.0f, 2.0f, 3.0f, 4.0f }, new int[] { 1, 4 });

            // Run the model
            //var output = session.Run(input);

            // Print the output
            //Console.WriteLine(output);
        }
        public async Task LoadPrompts()
        {
            developerPrompt = await ListHelper.ReadBundledTextFile("DeveloperRole.txt");
            userPrompt = await ListHelper.ReadBundledTextFile("UserRole.txt");
        }
   
        public async Task<Recipe> ReadRecipeFromImage(byte[] imageData)
        {
            try
            {
                string modelFileName = "phi-3.5-mini-instruct-cpu-int4-awq-block-128-acc-level-4.onnx";
                var modelPath = Path.Combine(FileSystem.AppDataDirectory, modelFileName);
                await ListHelper.CopyBundledModelFilesToAppDataDirectory(modelFileName);
                //using Stream fileStream = FileSystem.Current.(modelFileName);
                model = new Model(modelPath);
                tokenizer = new Tokenizer(model);


                processor = new MultiModalProcessor(model);
                tokenizerStream = processor.CreateStream();
                await LoadPrompts();
                string s = JsonSchemaExporter.GetJsonSchemaAsNode(JsonSerializerOptions.Default, typeof(Recipe), new JsonSchemaExporterOptions() { TreatNullObliviousAsNonNullable = true }).ToString();

                string prompt = "";
                string[] tempFilePath = new string[1];
                tempFilePath[0] = Path.Combine(FileSystem.CacheDirectory, $"{Guid.NewGuid()}.jpg");
                await File.WriteAllBytesAsync(tempFilePath[0], imageData);
                Images images = Images.Load(tempFilePath);
                prompt = $"<|user|>\n{userPrompt}\n<|end|>\n<|image|>\n<|assistant|>\n{developerPrompt}\nJson format to return recipe in: {s}";
                using var inputTensors = processor.ProcessImages(prompt, images);
                using GeneratorParams generatorParams = new GeneratorParams(model);
                generatorParams.SetSearchOption("max_length", 7680);
                generatorParams.SetInputs(inputTensors);

                using var generator = new Generator(model, generatorParams);
                var watch = System.Diagnostics.Stopwatch.StartNew();
                while (!generator.IsDone())
                {
                    generator.GenerateNextToken();
                    Console.Write(tokenizerStream.Decode(generator.GetSequence(0)[^1]));
                }
                watch.Stop();
                var runTimeInSeconds = watch.Elapsed.TotalSeconds;
                Console.WriteLine();
                Console.WriteLine($"Total Time: {runTimeInSeconds:0.00}");
                return new Recipe();
            } catch (Exception ex)
            {
                Debug.WriteLine($"{ex.Message}");
                throw ex;
            }
            
        }
    }
}
