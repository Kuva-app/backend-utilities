using System.Net;
using System.Net.Http;

namespace Utilities.General.Http
{
    /// <summary>
    /// Http Response Result
    /// </summary>
    public class HttpResponseResult
    {
        /// <summary>
        /// The status code
        /// </summary>
        private readonly HttpStatusCode _statusCode;
        /// <summary>
        /// The content
        /// </summary>
        private readonly HttpContent _content;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpResponseResult" /> class.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        /// <param name="content">The content.</param>
        public HttpResponseResult(HttpStatusCode statusCode, HttpContent content)
        {
            _statusCode = statusCode;
            _content = content;
        }

        /// <summary>
        /// Gets the status code.
        /// </summary>
        /// <value>
        /// The status code.
        /// </value>
        public HttpStatusCode StatusCode => _statusCode;

        /// <summary>
        /// Gets the content.
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        public HttpContent Content => _content;
    }
}
