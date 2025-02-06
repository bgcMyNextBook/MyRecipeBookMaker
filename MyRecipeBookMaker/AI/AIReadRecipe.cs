using System;

using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Azure.AI.OpenAI;
using Azure.AI.OpenAI.Chat;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using System.ClientModel;

using MyRecipeBookMaker.Common;
using MyRecipeBookMaker.Models;

using Newtonsoft.Json;

using OpenAI.Chat;
using System.Diagnostics;
using Azure;


namespace MyRecipeBookMaker.AI
{
    class AIReadRecipe
    {
      

        private const int Zero = 0;
        private DefaultAzureCredential? credential;
        private AzureOpenAIClient? openAIClient;
        private ChatClient? chatClient;
        private ChatCompletionOptions? chatOptions;
        private string? developerPrompt;
        private string? userPrompt;

        private AIReadRecipe()
        {
            
        }
        public static async Task<AIReadRecipe> CreateAsync()
        {
            var result = new AIReadRecipe();
            await result.InitializeAsync();
            return result;
        }
        private async Task InitializeAsync()
        {
            try
            {
                string AIEndPoint = await GetSecretsHelper.GetAppSecret("AIEndPoint");
                string ModelDeployment = await GetSecretsHelper.GetAppSecret("dep-ai-recipereader-model");
                string AIEndPointAPIKey = await GetSecretsHelper.GetAppSecret("AIEndPointAPIKey");

                openAIClient = new AzureOpenAIClient(new Uri(AIEndPoint), new AzureKeyCredential(AIEndPointAPIKey));
                chatClient = openAIClient.GetChatClient(ModelDeployment);
                await LoadPrompts();
                chatOptions = new ChatCompletionOptions()
                {
                    ResponseFormat = StructuredOutputsExtensions.CreateJsonSchemaFormat<Recipe>("Recipe", jsonSchemaIsStrict: true),
                    MaxOutputTokenCount = 14000,
                    Temperature = 0.1f,
                    TopP = 1.0f,
                };
              
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
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
                await InitializeAsync();
                List<ChatMessage> messages = new List<ChatMessage>();

                // User Prompt includes the prompt and the immage to read
                UserChatMessage uPrompt = new UserChatMessage(userPrompt);
                ChatMessageContentPart? imagePart = ChatMessageContentPart.CreateImagePart(new BinaryData(imageData), "image/jpeg");
                uPrompt.Content.Add(imagePart);


                // Prompt - System, User
                messages.Add(new SystemChatMessage(developerPrompt));
                messages.Add(uPrompt);

                ClientResult<ChatCompletion>? completionResults = await chatClient.CompleteChatAsync(messages, chatOptions);
                if (completionResults == null)
                {
                    throw new Exception("results null");
                }
#pragma warning disable AOAI001
                ResponseContentFilterResult? responseFilterResults = completionResults.Value.GetResponseContentFilterResult();
                RequestContentFilterResult? requestFilterResults = completionResults.Value.GetRequestContentFilterResult();
#pragma warning restore AOAI001
                var (filtered, message) = AreResultsFiltered(responseFilterResults, requestFilterResults);
                if (filtered)
                {
                    throw new Exception($"Filtered content detected: {message}");
                }


                Recipe r = new Recipe();
                if (!string.IsNullOrEmpty(completionResults.Value.Content[0].Text))
                {
                    r = JsonConvert.DeserializeObject<Recipe>(completionResults.Value.Content[0].Text);
                }
                else
                {
                    throw new Exception("No recipe returned .");
                }

                return r;
            }
            catch (Exception ex)
            {
                throw new Exception("Error calling chat completion:", ex);
            }

        }
#pragma warning disable AOAI001
        (bool, string) AreResultsFiltered(ResponseContentFilterResult respResults, RequestContentFilterResult reqResults)
#pragma warning restore AOAI001
        {
            bool filterDetected = false;
            StringBuilder sb = new StringBuilder();
            if (respResults != null)
            {

                sb.AppendLine("Response Content Filter Results:");

                if (respResults.Hate?.Filtered == true)
                {
                    filterDetected = true;
                    sb.AppendLine($"- Hate speech detected: {respResults.Hate.Severity} {respResults.Hate.ToString()}");

                }
                if (respResults.Profanity?.Filtered == true) { filterDetected = true; sb.AppendLine($"- Profanity content detected: {respResults.Profanity.ToString()}"); }
                if (respResults.SelfHarm?.Filtered == true) { filterDetected = true; sb.AppendLine($"- Self harm content detected: {respResults.SelfHarm.Severity} {respResults.SelfHarm.ToString()}"); }
                if (respResults.Sexual?.Filtered == true) { filterDetected = true; sb.AppendLine($"- Sexual content detected: {respResults.Sexual.Severity} {respResults.Sexual.ToString()}"); }
                if (respResults.Violence?.Filtered == true) { filterDetected = true; sb.AppendLine($"- Violence content detected: {respResults.Violence.Severity} {respResults.Violence.ToString()}"); }

                if (filterDetected)
                {
                    Console.WriteLine(sb.ToString());
                }

            }
            return (filterDetected, sb.ToString());
        }
    }
}
