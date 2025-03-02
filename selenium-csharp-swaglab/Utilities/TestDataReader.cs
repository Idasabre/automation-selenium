using System;
using System.IO;
using System.Text.Json.Nodes;
using Newtonsoft.Json.Linq;

namespace selenium_csharp_swaglab.Utilities
{
    public class TestDataReader
    {
        public string ExtractData(string TokenName)
        {
            string jsonData = File.ReadAllText("Utilities/TestData.json");
            var jsonObject = JObject.Parse(jsonData);
            var tokenValue = jsonObject.SelectToken(TokenName)?.ToString();
            return tokenValue ?? "Token not found";
        }

        public List<string> ExtractDataList(string TokenName)
        {
            string jsonData = File.ReadAllText("Utilities/TestData.json");
            var jsonObject = JObject.Parse(jsonData);
            // Convert JTokens to strings and handle null values properly
            List<string> tokenValues = jsonObject.SelectTokens(TokenName)
                                                 .Select(t => t.ToString())
                                                 .ToList();

            // Return an empty list if no values are found
            return tokenValues.Any() ? tokenValues : new List<string> { "Token not found" };
        }

        public string[] ExtractDataArray(string TokenName)
        {
            string jsonData = File.ReadAllText("Utilities/TestData.json");
            var jsonObject = JObject.Parse(jsonData);

            // Convert JTokens to strings and return as an array
            string[] tokenValues = jsonObject.SelectTokens(TokenName)
                                             .Select(t => t.ToString())
                                             .ToArray();

            // Return a default message if no values are found
            return tokenValues.Length > 0 ? tokenValues : new string[] { "Token not found" };
        }
    }
}
