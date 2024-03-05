using System;
using System.Threading.Tasks;

namespace Utilities.General.Http
{
    /// <summary>
    /// HttpClientUtil Interface
    /// </summary>
    public interface IHttpClientUtil
    {
        /// <summary>
        /// Gets the async.
        /// </summary>
        /// <param name="requestUri">Request URI.</param>
        /// <param name="httpApplication">Http application.</param>
        /// <param name="onCompleted">On completed.</param>
        /// <returns>
        /// The async.
        /// </returns>
        /// <exception cref="ArgumentNullException"></exception>
        Task GetAsync(string requestUri,
                      HttpClientUtil.HttpApplication httpApplication = HttpClientUtil.HttpApplication.Json,
                      Action<HttpResponseResult, HttpError> onCompleted = null);


        /// <summary>
        /// Posts the async.
        /// </summary>
        /// <param name="requestUri">Request URI.</param>
        /// <param name="request">Request.</param>
        /// <param name="httpApplication">Http application.</param>
        /// <param name="onCompleted">On completed.</param>
        /// <returns>
        /// The async.
        /// </returns>
        /// <exception cref="ArgumentNullException"></exception>
        Task PostAsync(string requestUri,
                       object request,
                       HttpClientUtil.HttpApplication httpApplication = HttpClientUtil.HttpApplication.Json,
                       Action<HttpResponseResult, HttpError> onCompleted = null);

        /// <summary>
        /// Puts the async.
        /// </summary>
        /// <param name="requestUri">Request URI.</param>
        /// <param name="request">Request.</param>
        /// <param name="httpApplication">Http application.</param>
        /// <param name="onCompleted">On completed.</param>
        /// <returns>
        /// The async.
        /// </returns>
        /// <exception cref="ArgumentNullException"></exception>
        Task PutAsync(string requestUri,
                      object request,
                      HttpClientUtil.HttpApplication httpApplication = HttpClientUtil.HttpApplication.Json,
                      Action<HttpResponseResult, HttpError> onCompleted = null);

        /// <summary>
        /// Deletes the async.
        /// </summary>
        /// <param name="requestUri">Request URI.</param>
        /// <param name="httpApplication">Http application.</param>
        /// <param name="onComplete">On complete.</param>
        /// <returns>
        /// The async.
        /// </returns>
        /// <exception cref="ArgumentNullException"></exception>
        Task DeleteAsync(string requestUri,
                         HttpClientUtil.HttpApplication httpApplication = HttpClientUtil.HttpApplication.Json,
                         Action<HttpResponseResult, HttpError> onComplete = null);

        /// <summary>
        /// Adds the header.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="value">Value.</param>
        void AddHeader(string name, string value);

        /// <summary>
        /// Cancels the pending requests.
        /// </summary>
        void CancelPendingRequests();
    }
}
