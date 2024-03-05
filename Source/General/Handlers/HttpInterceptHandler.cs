using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Utilities.General.Handlers
{
    /// <summary>
    /// HttpInterceptHandler
    /// </summary>
    /// <seealso cref="DelegatingHandler" />
    public class HttpInterceptHandler : DelegatingHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpInterceptHandler"/> class.
        /// </summary>
        /// <param name="innerHandler">The inner handler which is responsible for processing the HTTP response messages.</param>
        public HttpInterceptHandler(HttpMessageHandler innerHandler) : base(innerHandler)
        {

        }

        /// <summary>
        /// Sends an HTTP request to the inner handler to send to the server as an asynchronous operation.
        /// </summary>
        /// <param name="request">The HTTP request message to send to the server.</param>
        /// <param name="cancellationToken">A cancellation token to cancel operation.</param>
        /// <returns>
        /// The task object representing the asynchronous operation.
        /// </returns>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var defaultForegroundColor = Console.ForegroundColor;
            var defaultBackgroundColor = Console.BackgroundColor;

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("[Request]");
            Console.ForegroundColor = defaultForegroundColor;

            Console.WriteLine(request.ToString());
            if (request.Content != null)
            {
                Console.WriteLine(await request.Content.ReadAsStringAsync().ConfigureAwait(false));
            }
            Console.WriteLine();
            var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.Accepted:
                case System.Net.HttpStatusCode.Created:
                case System.Net.HttpStatusCode.OK:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case System.Net.HttpStatusCode.NoContent:
                case System.Net.HttpStatusCode.ResetContent:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case System.Net.HttpStatusCode.Ambiguous:
                case System.Net.HttpStatusCode.BadGateway:
                case System.Net.HttpStatusCode.BadRequest:
                case System.Net.HttpStatusCode.Conflict:
                case System.Net.HttpStatusCode.Continue:
                case System.Net.HttpStatusCode.ExpectationFailed:
                case System.Net.HttpStatusCode.Forbidden:
                case System.Net.HttpStatusCode.Found:
                case System.Net.HttpStatusCode.GatewayTimeout:
                case System.Net.HttpStatusCode.Gone:
                case System.Net.HttpStatusCode.HttpVersionNotSupported:
                case System.Net.HttpStatusCode.InternalServerError:
                case System.Net.HttpStatusCode.LengthRequired:
                case System.Net.HttpStatusCode.MethodNotAllowed:
                case System.Net.HttpStatusCode.Moved:
                case System.Net.HttpStatusCode.NonAuthoritativeInformation:
                case System.Net.HttpStatusCode.NotAcceptable:
                case System.Net.HttpStatusCode.NotFound:
                case System.Net.HttpStatusCode.NotImplemented:
                case System.Net.HttpStatusCode.NotModified:
                case System.Net.HttpStatusCode.PartialContent:
                case System.Net.HttpStatusCode.PaymentRequired:
                case System.Net.HttpStatusCode.PreconditionFailed:
                case System.Net.HttpStatusCode.ProxyAuthenticationRequired:
                case System.Net.HttpStatusCode.RedirectKeepVerb:
                case System.Net.HttpStatusCode.RedirectMethod:
                case System.Net.HttpStatusCode.RequestedRangeNotSatisfiable:
                case System.Net.HttpStatusCode.RequestEntityTooLarge:
                case System.Net.HttpStatusCode.RequestTimeout:
                case System.Net.HttpStatusCode.RequestUriTooLong:
                case System.Net.HttpStatusCode.ServiceUnavailable:
                case System.Net.HttpStatusCode.SwitchingProtocols:
                case System.Net.HttpStatusCode.Unauthorized:
                case System.Net.HttpStatusCode.UnsupportedMediaType:
                case System.Net.HttpStatusCode.Unused:
                case System.Net.HttpStatusCode.UpgradeRequired:
                case System.Net.HttpStatusCode.UseProxy:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    break;
            }
            Console.WriteLine("[Response]");
            Console.ForegroundColor = defaultForegroundColor;

            Console.WriteLine(response.ToString());
            if (response.Content != null)
            {
                Console.WriteLine(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            }
            Console.WriteLine();

            Console.BackgroundColor = defaultBackgroundColor;
            Console.ForegroundColor = defaultForegroundColor;

            return response;
        }
    }
}
