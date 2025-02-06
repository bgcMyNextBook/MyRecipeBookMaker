using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Schema;

using OpenAI.Chat;

namespace MyRecipeBookMaker.Common
{

    internal static class StructuredOutputsExtensions
    {
        public static Func<JsonSchemaExporterContext, JsonNode, JsonNode> StructuredOutputsTransform = new((context, node) =>
        {
            static void ProcessJsonObject(JsonObject jsonObject)
            {
                if (jsonObject["type"]?.ToString().Contains("object") == true)
                {
                    // Ensures that object types include the "additionalProperties" field, set to false.
                    if (!jsonObject.ContainsKey("additionalProperties"))
                    {
                        jsonObject.Add("additionalProperties", false);
                    }

                    var required = new JsonArray();
                    var properties = jsonObject["properties"] as JsonObject;
                    var keysToRemove = new List<string>();

                    foreach (var property in properties!)
                    {
                        // Collect keys to be removed
                        if (property.Key == "Uid")
                        {
                            keysToRemove.Add(property.Key);
                            continue;
                        }

                        required.Add(property.Key);

                        if (property.Value is JsonObject nestedObject)
                        {
                            // Process nested objects to ensure schema validity.
                            ProcessJsonObject(nestedObject);
                        }
                    }

                    // Remove keys after iteration
                    foreach (var key in keysToRemove)
                    {
                        properties.Remove(key);
                    }

                    // Ensures that object types include the "required" field containing all of the property keys.
                    if (!jsonObject.ContainsKey("required"))
                    {
                        jsonObject.Add("required", required);
                    }
                }
            }

            if (node is JsonObject rootObject)
            {
                ProcessJsonObject(rootObject);
            }

            return node;
        });

        public static T? CompleteChat<T>(this ChatClient chatClient, List<ChatMessage> messages, ChatCompletionOptions options)
        {
            ChatCompletion completion = chatClient.CompleteChat(messages, options);
            return JsonSerializer.Deserialize<T>(completion.Content[0].Text);
        }

        public static ChatResponseFormat CreateJsonSchemaFormat<T>(string jsonSchemaFormatName, string? jsonSchemaFormatDescription = null, bool? jsonSchemaIsStrict = null)
        {
            string s = JsonSchemaExporter.GetJsonSchemaAsNode(JsonSerializerOptions.Default, typeof(T), new JsonSchemaExporterOptions() { TreatNullObliviousAsNonNullable = true, TransformSchemaNode = StructuredOutputsTransform }).ToString();
            return ChatResponseFormat.CreateJsonSchemaFormat(
                jsonSchemaFormatName,
                jsonSchema: BinaryData.FromString(s),
                jsonSchemaFormatDescription: jsonSchemaFormatDescription,
                jsonSchemaIsStrict: jsonSchemaIsStrict
            );
        }
    }
}
