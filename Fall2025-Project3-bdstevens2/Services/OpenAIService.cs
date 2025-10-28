using Azure;
using Azure.AI.OpenAI; 
using OpenAI;
using OpenAI.Chat;     
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using System.ClientModel; 


using OpenAIChat = OpenAI.Chat;

namespace Fall2025_Project3_bdstevens2.Services
{
    public class OpenAIService
    {
        private readonly ChatClient _chatClient;
        private readonly string _deploymentName;

        public OpenAIService(IConfiguration configuration)
        {
            string? endpoint = configuration["OpenAI:Endpoint"];
            string? key = configuration["OpenAI:Key"];

            _deploymentName = "gpt-4.1-mini"; // Your deployment name

            if (string.IsNullOrEmpty(endpoint) || string.IsNullOrEmpty(key))
            {
                throw new InvalidOperationException("OpenAI endpoint or key is not configured in user secrets.");
            }

            AzureOpenAIClient azureClient = new(
                new Uri(endpoint),
                new AzureKeyCredential(key)
            );

            _chatClient = azureClient.GetChatClient(_deploymentName);
        }

        private async Task<string> GetChatCompletionAsync(string systemPrompt, string userPrompt)
        {
            var messages = new OpenAIChat.ChatMessage[]
            {
                new OpenAIChat.SystemChatMessage(systemPrompt),
                new OpenAIChat.UserChatMessage(userPrompt)
            };


            ClientResult<OpenAIChat.ChatCompletion> response = await _chatClient.CompleteChatAsync(messages, null);


            if (response.Value?.Content?.Count > 0)
            {
                return response.Value.Content[0].Text;
            }

            return "Error: No response from AI.";
        }

        public async Task<string> GetMovieReviewsAsync(string movieTitle)
        {
            string sysPrompt = "You are a movie critic. Generate 10 short, unique reviews for a movie. Each review must be separated by the exact delimiter '|||'.";
            string userPrompt = $"Generate 10 reviews for the movie: {movieTitle}";
            return await GetChatCompletionAsync(sysPrompt, userPrompt);
        }

        public async Task<string> GetActorTweetsAsync(string actorName)
        {
            string sysPrompt = "You are a social media simulator. Generate 20 short, unique tweets about an actor as if from different fans or critics. Each tweet must be separated by the exact delimiter '|||'.";
            string userPrompt = $"Generate 20 tweets about the actor: {actorName}";
            return await GetChatCompletionAsync(sysPrompt, userPrompt);
        }
    }
}