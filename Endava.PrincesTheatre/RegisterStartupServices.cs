using Endava.PrincesTheatre.Services;
using Microsoft.Net.Http.Headers;
using Polly;
using Polly.Extensions.Http;

namespace Endava.PrincesTheatre
{
    public static class RegisterStartupServices
    {
        public static WebApplicationBuilder RegisterServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllersWithViews();

            builder.Services.AddHttpClient("HttpClient", httpClient =>
            {
                httpClient.BaseAddress = new Uri(Environment.GetEnvironmentVariable("API_BASE_URL")!);
                httpClient.DefaultRequestHeaders.Add("x-api-key", Environment.GetEnvironmentVariable("API_KEY"));
                httpClient.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
            }).AddPolicyHandler(GetRetryPolicy());

            builder.Services.AddScoped<IMoviesService, MoviesService>();

            return builder;
        }

        public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2,
                                                                            retryAttempt)));
        }
    }
}

