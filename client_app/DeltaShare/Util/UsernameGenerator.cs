using System.Diagnostics;

namespace DeltaShare.Util
{
    public static class UsernameGenerator
    {
        private static string AdjectivesFileName = "adjectives.txt";
        private static string NounsFileName = "nouns.txt";
        private static int AdjectivesLineCount = 4939;
        private static int NounsLineCount = 4939;

        private static async Task<string?> GetRandomLine(string fileName, int totalLineCount)
        {
            Random random = new();
            int lineNumber = random.Next(1, totalLineCount + 1);
            try
            {
                using var stream = await FileSystem.OpenAppPackageFileAsync(fileName);
                using var reader = new StreamReader(stream);
                string? line = null;
                for (int currentLine = 1; currentLine <= lineNumber; currentLine++)
                {
                    line = await reader.ReadLineAsync();
                }
                Debug.WriteLine($"random name from {fileName} lineNumber: {lineNumber} line: {line}");
                return line;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"The file could not be read: {e.Message}");
                return "";
            }
        }

        public static async Task<string> GetRandomUsername()
        {
            try
            {
                string adjective = await GetRandomLine(AdjectivesFileName, AdjectivesLineCount) ?? "";
                string noun = await GetRandomLine(NounsFileName, NounsLineCount) ?? "";
                return $"{adjective}-{noun}";
            }
            catch (Exception e)
            {
                Debug.WriteLine($"The file could not be read: {e.Message}");
                return "unnamed";
            }
        }
    }
}
