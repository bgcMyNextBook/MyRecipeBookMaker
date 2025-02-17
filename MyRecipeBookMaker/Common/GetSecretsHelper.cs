using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Azure.Security.KeyVault.Secrets;

using Microsoft.Identity.Client;

using Newtonsoft.Json;

namespace MyRecipeBookMaker.Common
{
    static public class GetSecretsHelper
    {
        public static Dictionary<string, KeyVaultSecret> appSecrets = new();

        public static async Task AddOrUpdateAppSecrets()
        {
            foreach (var secret in appSecrets)
            {
                await SecureStorage.SetAsync(secret.Key, secret.Value.Value);
            }
        }
        public static async Task<string> GetAppSecret(string secretName)
        {

            if (appSecrets.ContainsKey(secretName))
            {
                return appSecrets[secretName].Value;
            }
            else
            {
                //todo: check SecureStorage first...
                await GetSecrets();
                if (appSecrets.ContainsKey(secretName))
                {
                    return appSecrets[secretName].Value;
                }
            }
            throw new Exception("Secret not found in appSecrets.");
        }
        public static async Task GetSecrets()
        {
            //todo: check if appSecrets is null or empty
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    client.Timeout = TimeSpan.FromSeconds(130); // Set custom timeout value
                                                               //string[] scopes = { "https://MyNextbook.onmicrosoft.com/MyNextBookAPIs/GetSecrets" };
                                                               //AuthenticationResult authResult = await PCAWrapperB2C.Instance.AcquireTokenSilentAsync(scopes);
                                                               //todo: check if authResult is null and expiration
                                                               //string token = authResult.AccessToken;
                                                               //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


                    string functionUrl = "https://myrecipebookmakerbackend.azurewebsites.net/api/GetSecrets?code=vPKcgeePAtEbcYtbw2pPqbmjOSW9Gowo4Ffje2hVBFOOAzFu3dojxw%3D%3D";
                    HttpResponseMessage response = await client.GetAsync(functionUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        if (jsonResponse != null)
                        {
                            appSecrets = JsonConvert.DeserializeObject<Dictionary<string, KeyVaultSecret>>(jsonResponse);
                            await AddOrUpdateAppSecrets();
                            return;
                        }
                    }
                    throw new Exception("Failed to retrieve secrets from Azure Function App.");

                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    throw new Exception("Failed to retrieve secrets from Azure Function App.",ex);
                }
            }
        }
    }
}
