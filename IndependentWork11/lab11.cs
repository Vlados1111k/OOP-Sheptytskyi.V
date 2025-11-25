using Polly;
using Polly.CircuitBreaker;
using System;
using System.Threading;

namespace IndependentWork11
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== IndependentWork11: Polly Scenarios ===\n");

            DemonstrateRetryPolicy();
            Console.WriteLine("\n--------------------------------------------------\n");

            DemonstrateFallbackPolicy();
            Console.WriteLine("\n--------------------------------------------------\n");

            DemonstrateCircuitBreakerPolicy();

            Console.WriteLine("\n=== Done ===");
        }

        static void DemonstrateRetryPolicy()
        {
            Console.WriteLine("[Scenario 1] Retry Policy");
            int attemptCounter = 0;

            // Retry 3 рази з паузою 1 сек
            var retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetry(
                    retryCount: 3,
                    sleepDurationProvider: attempt => TimeSpan.FromSeconds(1),
                    onRetry: (exception, timeSpan, retryCount, context) =>
                    {
                        Console.WriteLine($"   [LOG] Retry #{retryCount} after {timeSpan.TotalSeconds}s. Error: {exception.Message}");
                    });

            try
            {
                retryPolicy.Execute(() =>
                {
                    attemptCounter++;
                    Console.WriteLine($"   -> Request attempt {attemptCounter}...");

                    if (attemptCounter < 3)
                        throw new Exception("503 Service Unavailable");

                    Console.WriteLine("   [SUCCESS] Data received!");
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"   [FAILURE] Failed after retries: {ex.Message}");
            }
        }

        static void DemonstrateFallbackPolicy()
        {
            Console.WriteLine("[Scenario 2] Fallback Policy");

            // Якщо помилка - повертаємо дефолтне значення
            var fallbackPolicy = Policy<string>
                .Handle<Exception>()
                .Fallback(
                    fallbackValue: "Default User",
                    onFallback: (outcome, context) =>
                    {
                        Console.WriteLine($"   [LOG] Error caught: \"{outcome.Exception?.Message}\". Using Fallback.");
                    });

            string result = fallbackPolicy.Execute(() =>
            {
                Console.WriteLine("   -> Fetching user data...");
                throw new Exception("Database Timeout");
            });

            Console.WriteLine($"   [RESULT] User: {result}");
        }

        static void DemonstrateCircuitBreakerPolicy()
        {
            Console.WriteLine("[Scenario 3] Circuit Breaker");

            // Розрив ланцюга після 2 помилок на 3 секунди
            var circuitBreakerPolicy = Policy
                .Handle<Exception>()
                .CircuitBreaker(
                    exceptionsAllowedBeforeBreaking: 2,
                    durationOfBreak: TimeSpan.FromSeconds(3),
                    onBreak: (ex, breakDelay) =>
                    {
                        Console.WriteLine($"   [LOG] Circuit OPEN for {breakDelay.TotalSeconds}s. Reason: {ex.Message}");
                    },
                    onReset: () =>
                    {
                        Console.WriteLine("   [LOG] Circuit CLOSED.");
                    });

            for (int i = 0; i < 7; i++)
            {
                try
                {
                    Console.WriteLine($"   -> Request #{i + 1}:");
                    
                    circuitBreakerPolicy.Execute(() =>
                    {
                        // Імітуємо збій на перших 4 спробах
                        if (i < 4) 
                            throw new Exception("Timeout");
                        
                        Console.WriteLine("      Success!");
                    });
                }
                catch (BrokenCircuitException)
                {
                    Console.WriteLine("      [FAST FAIL] Circuit is open.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"      Error: {ex.Message}");
                }

                Thread.Sleep(800); 
            }
        }
    }
}