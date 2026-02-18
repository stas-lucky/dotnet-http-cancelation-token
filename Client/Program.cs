namespace Client
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            using var httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:5000") };
            using var cts = new CancellationTokenSource();

            Console.WriteLine("Press any key to cancel the request...");

            var task = Task.Run(async () =>
            {
                try
                {
                    Console.WriteLine("Starting long-running request...");
                    var response = await httpClient.GetAsync("/LongRunning/process", cts.Token);
                    var content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Server response: {response.StatusCode} - {content}");
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("Request was cancelled on the client side.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            });

            // Wait for a key press to cancel
            _ = Task.Run(() =>
            {
                Console.ReadKey(true);
                cts.Cancel();
                Console.WriteLine("Cancellation signal sent.");
            });

            await task;
            Console.WriteLine("Application finished.");
        }
    }
}
