namespace Presentation.Helper
{
    public static class Logger
    {
        private static readonly string logFilePath = "Logs.txt"; 

        public static async Task LogAsync(string message)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(logFilePath, true))
                {
                    string logEntry = $"{DateTime.UtcNow}: {message}";
                    await writer.WriteLineAsync(logEntry);
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during logging
                Console.WriteLine($"Logging error: {ex.Message}");
            }
        }
    }
}
