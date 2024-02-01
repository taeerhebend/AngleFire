using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace YourNamespace.Jobs
{
    public static class BingApiScoutJob
    {
        private static readonly HttpClient httpClient = new HttpClient();

        public static void ProcessQueueMessage([QueueTrigger("scoutjobs")] Stream myQueueItem, ILogger log)
        {
            try
            {
                byte[] messageBytes = new byte[myQueueItem.Length];
                myQueueItem.Read(messageBytes, 0, (int)myQueueItem.Length);
                string jsonString = Encoding.UTF8.GetString(messageBytes);
                ScoutJobDataModel inputModel = JsonConvert.DeserializeObject<ScoutJobDataModel>(jsonString);

                RunBingSearch(inputModel.Query, log);
                inputModel.Completed = true;

                string outputJson = JsonConvert.SerializeObject(inputModel);
                LogInformationAndWriteOutput(log, $"Scouted query '{inputModel.Query}' successfully.", outputJson);
            }
            catch (Exception ex)
            {
                LogError(log, $"Processing error occurred: {ex.Message}", null);
            }
        }

        private static async Task RunBingSearch(string searchTerm, ILogger logger)
        {
            var apiKey = "<YOUR_BING_SEARCH_API_KEY>";
            var baseUrl = "https://api.bing.microsoft.com/v7.0/search";
            var paramsList = new List<string>() { $"q={Uri.EscapeDataString(searchTerm)}" };

            var urlWithParams = String.Join("&", paramsList);
            var finalUrl = $"{baseUrl}?{urlWithParams}&count=10&offset=0&mkt=en-us&safeSearch=Moderate";

            HttpResponseMessage response = await httpClient.GetAsync($"{finalUrl}&key={apiKey}").ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                logger.LogInformation($"Received response: \n\n{content}\n");
            }
            else
            {
                logger.LogWarning($"Failed to receive successful response.\nStatus Code:{response.StatusCode},\nReason Phrase:{response.ReasonPhrase}\nHeaders:\n{response.Headers}\n");
            }
        }

        private static void LogError(ILogger logger, string errorMessage, Exception exception)
        {
            if (exception != null)
            {
                logger.LogError(errorMessage + "\r\n" + exception.StackTrace);
            }
            else
            {
                logger.LogError(errorMessage);
            }
        }

        private static void LogInformationAndWriteOutput(ILogger logger, string informationMessage, string outputJson)
        {
            logger.LogInformation(informationMessage);
            File.AppendAllText(@".\output.txt", $"{DateTime.UtcNow}: {informationMessage}{Environment.NewLine}");
            File.AppendAllText(@".\output.txt", $"{outputJson}{Environment.NewLine}");
        }
    }
}