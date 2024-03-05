using Utilities.General.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Utilities.General.Http
{
    /// <summary>
    /// HttpClientUtil
    /// </summary>
    /// <seealso cref="IHttpClientUtil" />
    /// <seealso cref="IDisposable" />
    public abstract class HttpClientUtil : IHttpClientUtil, IDisposable
    {
        /// <summary>
        /// Http application.
        /// </summary>
        public enum HttpApplication
        {
            Formurlencoded,
            Json
        }

        /// <summary>
        /// Gets the tracker error action.
        /// </summary>
        /// <value>
        /// The tracker error action.
        /// </value>
        protected virtual Action<Exception> TrackerErrorAction { get; }

        /// <summary>
        /// The http client.
        /// </summary>
        private readonly HttpClient _httpClient;

        private readonly HttpConfiguration _httpConfiguration;

        private const uint DefaultAttemps = 3;
        private const int DefaultMilliseconds = 1_000;
        private const long DefaultMaxResponseContentBufferSize = 2_147_483_646;

        /// <summary>
        /// Gets or sets the authorization.
        /// </summary>
        /// <value>
        /// The authorization.
        /// </value>
        public AuthenticationHeaderValue Authorization { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpClientUtil"/> class.
        /// </summary>
        /// <param name="httpConfiguration">The configuration.</param>
        /// <param name="trackerErrorAction">The tracker error action.</param>
        /// <param name="timeout">The timeout.</param>
        /// <param name="decompressionMethods">The decompression methods.</param>
        /// <param name="authorization">The authorization.</param>
        /// <exception cref="ArgumentNullException">baseUri</exception>
        protected HttpClientUtil(HttpConfiguration httpConfiguration,
                                 Action<Exception> trackerErrorAction = null,
                                 TimeSpan timeout = default,
                                 DecompressionMethods decompressionMethods = DecompressionMethods.GZip |
                                                                             DecompressionMethods.Deflate,
                                 AuthenticationHeaderValue authorization = null)
        {
            _httpConfiguration = httpConfiguration ?? throw new ArgumentNullException(nameof(httpConfiguration));
            if (string.IsNullOrEmpty(httpConfiguration.BaseUri))
                throw new ArgumentNullException(nameof(httpConfiguration.BaseUri));
            var customHandler = new HttpClientCustomHandler { AutomaticDecompression = decompressionMethods };
            if (timeout == default)
                timeout = TimeSpan.FromMinutes(5);
            _httpClient = new HttpClient(customHandler)
            {
                BaseAddress = new Uri(httpConfiguration.BaseUri),
                Timeout = timeout,
                MaxResponseContentBufferSize = httpConfiguration.MaxResponseContentBufferSize ??
                                               DefaultMaxResponseContentBufferSize
            };
            _httpClient.DefaultRequestHeaders.Clear();
            TrackerErrorAction = trackerErrorAction;
            Authorization = authorization;
        }

        #region IHttpClientHelper

        /// <summary>
        /// Gets the async.
        /// </summary>
        /// <param name="requestUri">Request URI.</param>
        /// <param name="httpApplication">Http application.</param>
        /// <param name="onCompleted">On completed.</param>
        /// <exception cref="ArgumentException">requestUri</exception>
        /// <exception cref="ArgumentNullException"></exception>
        public virtual async Task GetAsync(string requestUri,
                                           HttpApplication httpApplication = HttpApplication.Json,
                                           Action<HttpResponseResult, HttpError> onCompleted = null)
        {
            SetApplicationType(httpApplication);
            if (Authorization != null)
                _httpClient.DefaultRequestHeaders.Authorization = Authorization;
            if (string.IsNullOrEmpty(requestUri) || string.IsNullOrWhiteSpace(requestUri))
                throw new ArgumentException(nameof(requestUri));
            HttpResponseResult responseResult = null;
            HttpError error = null;
            var n = 0;
            bool trying;
            do
            {
                try
                {
                    var response = await _httpClient.GetAsync(requestUri).ConfigureAwait(false);
                    responseResult = new HttpResponseResult(response.StatusCode, response.Content);
                    break;
                }
                catch (Exception e)
                {
                    error = new HttpError(e, TrackerErrorAction);
                    n++;
                    Thread.Sleep(DefaultMilliseconds);
                }
                trying = n < (_httpConfiguration.AttempsRequest ?? DefaultAttemps);
            } while (trying);
            if (error != null)
                await error.LogErrorAsync().ConfigureAwait(false);
            if (onCompleted == null)
                return;
            await Task.Run(() => onCompleted(responseResult, error)).ConfigureAwait(false);
        }


        /// <summary>
        /// Posts the async.
        /// </summary>
        /// <param name="requestUri">Request URI.</param>
        /// <param name="request">Request.</param>
        /// <param name="httpApplication">Http application.</param>
        /// <param name="onCompleted">On completed.</param>
        /// <exception cref="ArgumentNullException">requestUri</exception>
        public virtual async Task PostAsync(string requestUri,
                                            object request,
                                            HttpApplication httpApplication = HttpApplication.Json,
                                            Action<HttpResponseResult, HttpError> onCompleted = null)
        {
            SetApplicationType(httpApplication);
            if (Authorization != null)
                _httpClient.DefaultRequestHeaders.Authorization = Authorization;
            if (string.IsNullOrEmpty(requestUri) || string.IsNullOrWhiteSpace(requestUri))
                throw new ArgumentNullException(nameof(requestUri));
            HttpResponseResult completedArgs = null;
            HttpError error = null;
            using (var content = Prepare(request, httpApplication))
            {
                var n = 0;
                bool trying;
                do
                {
                    try
                    {
                        var response = await _httpClient.PostAsync(requestUri, content)
                                                        .ConfigureAwait(false);
                        completedArgs = new HttpResponseResult(response.StatusCode, response.Content);
                        break;
                    }
                    catch (Exception e)
                    {
                        error = new HttpError(e, TrackerErrorAction);
                        n++;
                        Thread.Sleep(DefaultMilliseconds);
                    }
                    trying = n < (_httpConfiguration.AttempsRequest ?? DefaultAttemps);
                } while (trying);
            }
            if (error != null)
                await error.LogErrorAsync().ConfigureAwait(false);
            if (onCompleted == null)
                return;
            await Task.Run(() => onCompleted(completedArgs, error)).ConfigureAwait(false);
        }

        /// <summary>
        /// Puts the async.
        /// </summary>
        /// <param name="requestUri">Request URI.</param>
        /// <param name="request">Request.</param>
        /// <param name="httpApplication">Http application.</param>
        /// <param name="onComplete">On complete.</param>
        /// <exception cref="ArgumentNullException">requestUri</exception>
        public virtual async Task PutAsync(string requestUri,
                                           object request,
                                           HttpApplication httpApplication = HttpApplication.Json,
                                           Action<HttpResponseResult, HttpError> onComplete = null)
        {
            SetApplicationType(httpApplication);
            if (Authorization != null)
                _httpClient.DefaultRequestHeaders.Authorization = Authorization;
            if (string.IsNullOrEmpty(requestUri) || string.IsNullOrWhiteSpace(requestUri))
                throw new ArgumentNullException(nameof(requestUri));
            HttpResponseResult completedArgs = null;
            HttpError error = null;
            using (var content = Prepare(request, httpApplication))
            {
                var n = 0;
                bool trying;
                do
                {
                    try
                    {
                        var response = await _httpClient.PutAsync(requestUri, content).ConfigureAwait(false);
                        completedArgs = new HttpResponseResult(response.StatusCode, response.Content);
                        break;
                    }
                    catch (Exception e)
                    {
                        error = new HttpError(e, TrackerErrorAction);
                        n++;
                        Thread.Sleep(DefaultMilliseconds);
                    }
                    trying = n < (_httpConfiguration.AttempsRequest ?? DefaultAttemps);
                } while (trying);
            }

            if (error != null)
                await error.LogErrorAsync().ConfigureAwait(false);
            if (onComplete == null)
                return;
            await Task.Run(() => onComplete(completedArgs, error)).ConfigureAwait(false);
        }

        /// <summary>
        /// Deletes the async.
        /// </summary>
        /// <param name="requestUri">Request URI.</param>
        /// <param name="httpApplication">Http application.</param>
        /// <param name="onCompleted">On completed.</param>
        /// <exception cref="ArgumentNullException">requestUri</exception>
        public virtual async Task DeleteAsync(string requestUri,
                                              HttpApplication httpApplication = HttpApplication.Json,
                                              Action<HttpResponseResult, HttpError> onCompleted = null)
        {
            SetApplicationType(httpApplication);
            if (Authorization != null)
                _httpClient.DefaultRequestHeaders.Authorization = Authorization;
            if (string.IsNullOrEmpty(requestUri) || string.IsNullOrWhiteSpace(requestUri))
                throw new ArgumentNullException(nameof(requestUri));
            HttpResponseResult completedArgs = null;
            HttpError error = null;
            var n = 0;
            bool trying;
            do
            {
                try
                {
                    var response = await _httpClient.DeleteAsync(requestUri).ConfigureAwait(false);
                    completedArgs = new HttpResponseResult(response.StatusCode, response.Content);
                    break;
                }
                catch (Exception e)
                {
                    error = new HttpError(e, TrackerErrorAction);
                    n++;
                    Thread.Sleep(DefaultMilliseconds);
                }
                trying = n < (_httpConfiguration.AttempsRequest ?? DefaultAttemps);
            } while (trying);
            if (error != null)
                await error.LogErrorAsync().ConfigureAwait(false);
            if (onCompleted == null)
                return;
            await Task.Run(() => onCompleted(completedArgs, error)).ConfigureAwait(false);
        }

        /// <summary>
        /// Adds the header.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="value">Value.</param>
        public void AddHeader(string name, string value)
        {
            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation(name, value);
        }

        /// <summary>
        /// Cancels the pending requests.
        /// </summary>
        public void CancelPendingRequests()
        {
            _httpClient.CancelPendingRequests();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            CancelPendingRequests();
            _httpClient.Dispose();
            GC.Collect();
        }
        #endregion

        /// <summary>
        /// Sets the type of the application.
        /// </summary>
        /// <param name="application">Application.</param>
        private void SetApplicationType(HttpApplication application)
        {
            if (_httpClient == null)
                return;
            var contentType = application == HttpApplication.Formurlencoded
                ? "application/x-www-form-urlencoded"
                : "application/json";
            if (_httpClient.DefaultRequestHeaders.Accept.Any())
                _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", contentType);
        }

        /// <summary>
        /// Prepare the specified requestBody and httpApplication.
        /// </summary>
        /// <param name="requestBody">Request body.</param>
        /// <param name="httpApplication">Http application.</param>
        /// <returns>
        /// The prepare.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">httpApplication - null</exception>
        private ByteArrayContent Prepare(object requestBody, HttpApplication httpApplication)
        {
            ByteArrayContent content;
            if (requestBody == null) return null;
            switch (httpApplication)
            {
                case HttpApplication.Json:
                    {
                        var jsonOptions = new JsonSerializerOptions
                        {
                            WriteIndented = true,
                        };
                        content = new StringContent(JsonSerializer.Serialize(requestBody, jsonOptions), Encoding.UTF8, "application/json");
                        break;
                    }
                case HttpApplication.Formurlencoded:
                    {
                        var input = RouteValueDictionary(requestBody);
                        var keys = input.ToDictionary(k => k.Key);
                        var data = keys.Select(item => new KeyValuePair<string, string>(item.Value.Key, item.Value.Value.ToString())).ToList();
                        content = new FormUrlEncodedContent(data);
                        break;
                    }
                default:
                    throw new ArgumentOutOfRangeException(nameof(httpApplication), httpApplication, null);
            }
            return content;
        }

        /// <summary>
        /// Routes the value dictionary.
        /// </summary>
        /// <param name="requestBody">Request body.</param>
        /// <returns>
        /// The value dictionary.
        /// </returns>
        private static Dictionary<string, object> RouteValueDictionary(object requestBody)
        {
            var keyValuePairs = new Dictionary<string, object>();
            if (requestBody == null)
                return keyValuePairs;
            var type = requestBody.GetType();
            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                var value = property.GetValue(requestBody);
                if (value == null)
                    continue;
                keyValuePairs.Add(property.Name, value);
            }

            return keyValuePairs;
        }
    }
}
