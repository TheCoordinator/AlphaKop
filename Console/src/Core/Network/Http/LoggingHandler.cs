using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

public class LoggingHandler : DelegatingHandler {
    private readonly ILogger? logger;
    public LoggingHandler(HttpMessageHandler innerHandler, ILogger<LoggingHandler>? logger)
        : base(innerHandler) {
        this.logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) {
        logger?.LogDebug(
            $@"
Request:
{request.ToString()}
            "
        );

        if (request.Content != null) {
            logger?.LogDebug(
                $@"
Request Content:                
{await request.Content.ReadAsStringAsync()}                
                "
            );
        }

        HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

        logger?.LogDebug(
            $@"
Response:
{response.ToString()}
            "
        );

        if (response.Content != null) {
            logger?.LogTrace(
                $@"
Response Content:                
{await response.Content.ReadAsStringAsync()}                
                "
            );
        }

        return response;
    }
}