using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

using SkiaSharp;

namespace RecipeReader.Helpers
{
    public static  class PDFToImageHelper
    {
        public static async Task<byte[]> PDFFileToByte(string pdfName)
        {
            Console.WriteLine($"Processing PDF file: {pdfName}");
            var pdf = await File.ReadAllBytesAsync(pdfName);
            var pageImages = PDFtoImage.Conversion.ToImages(pdf);

            // Calculate the total height of all pages and the maximum width
            int totalHeight = pageImages.Sum(image => image.Height);
            int width = pageImages.Max(image => image.Width);

            // Create a new bitmap to hold the stitched image
            var stitchedImage = new SKBitmap(width, totalHeight);
            var canvas = new SKCanvas(stitchedImage);

            int currentHeight = 0;
            foreach (var pageImage in pageImages)
            {
                canvas.DrawBitmap(pageImage, 0, currentHeight);
                currentHeight += pageImage.Height;
            }

            // Convert the stitched image to a byte array
            var imageBytes = stitchedImage.Encode(SKEncodedImageFormat.Jpeg, 100).ToArray();

            // Save the imageBytes to a file
            //await File.WriteAllBytesAsync("c:\\temp\\output.jpg", imageBytes);

            return imageBytes;
        }

        public static async Task<string> PDFFileToBase64String(string pdfName)
        {
            Console.WriteLine($"Processing PDF file: {pdfName}");
            var pdf = await File.ReadAllBytesAsync(pdfName);
            var pageImages = PDFtoImage.Conversion.ToImages(pdf);
            List<JsonNode> recipeImages = new List<JsonNode>();

            var totalPageCount = pageImages.Count();

            double maxImageCount = 25;
            int maxSize = (int)Math.Ceiling(totalPageCount / maxImageCount);
            var pageImageGroups = new List<List<SKBitmap>>();
            for (int i = 0; i < totalPageCount; i += maxSize)
            {
                var pageImageGroup = pageImages.Skip(i).Take(maxSize).ToList();
                pageImageGroups.Add(pageImageGroup);
            }

            var count = 0;
            string returnImage = "";
            foreach (var pageImageGroup in pageImageGroups)
            {
                int totalHeight = pageImageGroup.Sum(image => image.Height);
                int width = pageImageGroup.Max(image => image.Width);
                var stitchedImage = new SKBitmap(width, totalHeight);
                var canvas = new SKCanvas(stitchedImage);
                int currentHeight = 0;
                foreach (var pageImage in pageImageGroup)
                {
                    canvas.DrawBitmap(pageImage, 0, currentHeight);
                    currentHeight += pageImage.Height;
                }

                // Convert stitched image to byte array
                var imageBytes = stitchedImage.Encode(SKEncodedImageFormat.Jpeg, 100).ToArray();

                // Convert byte array to base64 string
                var base64Image = Convert.ToBase64String(imageBytes);
                returnImage = returnImage + base64Image;
                // Create JSON object with base64 image
                var jsonObject = new JsonObject
                            {
                                { "type", "image_url" },
                                { "image_url", new JsonObject { { "url", $"data:image/jpeg;base64,{base64Image}" } } }
                            };

                // Add JSON object to list
                recipeImages.Add(jsonObject);

                //Console.WriteLine($"Added image to list ({count + 1}/{pageImageGroups.Count})");
                count++;
            }

            return returnImage;
        }
        public static async Task<List<JsonNode>> PDFFileToImageListJson (string pdfName)
            
        {
            Console.WriteLine($"Processing PDF file: {pdfName}");
            var pdf = await File.ReadAllBytesAsync(pdfName);
            var pageImages = PDFtoImage.Conversion.ToImages(pdf);
            //PDFtoImage.Conversion.SaveJpeg( "c:\\temp\\pdfname.jpg",pdf);
            List<JsonNode> recipeImages = new List<JsonNode>();

            var totalPageCount = pageImages.Count();

            double maxImageCount = 25;
            int maxSize = (int)Math.Ceiling(totalPageCount / maxImageCount);
            var pageImageGroups = new List<List<SKBitmap>>();
            for (int i = 0; i < totalPageCount; i += maxSize)
            {
                var pageImageGroup = pageImages.Skip(i).Take(maxSize).ToList();
                pageImageGroups.Add(pageImageGroup);
            }

            var count = 0;
            foreach (var pageImageGroup in pageImageGroups)
            {
                int totalHeight = pageImageGroup.Sum(image => image.Height);
                int width = pageImageGroup.Max(image => image.Width);
                var stitchedImage = new SKBitmap(width, totalHeight);
                var canvas = new SKCanvas(stitchedImage);
                int currentHeight = 0;
                foreach (var pageImage in pageImageGroup)
                {
                    canvas.DrawBitmap(pageImage, 0, currentHeight);
                    currentHeight += pageImage.Height;
                }

                // Convert stitched image to byte array
                var imageBytes = stitchedImage.Encode(SKEncodedImageFormat.Jpeg, 100).ToArray();

                // Convert byte array to base64 string
                var base64Image = Convert.ToBase64String(imageBytes);

                // Create JSON object with base64 image
                var jsonObject = new JsonObject
                            {
                                { "type", "image_url" },
                                { "image_url", new JsonObject { { "url", $"data:image/jpeg;base64,{base64Image}" } } }
                            };

                // Add JSON object to list
                recipeImages.Add(jsonObject);

                //Console.WriteLine($"Added image to list ({count + 1}/{pageImageGroups.Count})");
                count++;
            }

            return recipeImages;
        }
    }
}
