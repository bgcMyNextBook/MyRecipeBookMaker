using System;
using System.Collections.Generic;
using System.Text;

using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace MyRecipeBookMaker.Services
{
    public class OnnxModelServiceHelper
    {
        public static string GetOnnxModelPath()
        {
            return "MyRecipeBook";
        }

        public static async Task<List<string>> GetFolderNamesFromBlobStorageAsync()
        {
            var folderNames = new List<string>();
            string blobStorageUrl = "https://stmynextbookbackend.blob.core.windows.net/models";
            BlobContainerClient containerClient = new BlobContainerClient(new Uri(blobStorageUrl));

            await foreach (BlobHierarchyItem blobItem in containerClient.GetBlobsByHierarchyAsync(delimiter: "/"))
            {
                if (blobItem.IsPrefix)
                {
                    folderNames.Add(blobItem.Prefix.TrimEnd('/'));
                }
            }

            return folderNames;
        }
    }
}
