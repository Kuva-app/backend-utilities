using System;
using System.Net;
using System.Threading.Tasks;
using Bitx.General.Extensions;
using Bitx.General.Http;
using Xunit;

namespace Bitx.UtilitiesTest
{
    public class SandboxClientTest : HttpClientUtil
    {
        private SandboxClientTest() : base(new HttpConfiguration { BaseUri = "https://sandbox.kuva.com.br/api/" })
        {
        }

        private static SandboxClientTest _instance;
        public static SandboxClientTest Shared => _instance ?? (_instance = new SandboxClientTest());

        protected override Action<Exception> TrackerErrorAction {
            get { return (e) => System.Diagnostics.Debug.WriteLine(e); }
        }
    }

    public class WebClientTest
    {
        private readonly Func<HttpResponseResult, HttpError, Task<dynamic>> onCompletedAction = async (responseResult, error) =>
        {
            if (error != null)
            {
                await error.LogErrorAsync().ConfigureAwait(false);
                return null;
            }

            if (responseResult == null || responseResult.StatusCode != HttpStatusCode.OK)
                return null;
            var jsonResult = await responseResult
                                    .Content
                                    .ReadAsStringAsync()
                                    .ConfigureAwait(false);
            return jsonResult.ConvertJsonObject<dynamic>();
        };

        [Fact]
        public async Task GetTest()
        {
            dynamic result = null;

            var task1 = Task.Run(() =>  SandboxClientTest.Shared.GetAsync("values", 
                HttpClientUtil.HttpApplication.Json, async (responseMessage, httpError) =>
                {
                    result = await onCompletedAction(responseMessage, httpError);
                }));
            var task2 = Task.Run(() => SandboxClientTest.Shared.GetAsync("values",
                HttpClientUtil.HttpApplication.Json, async (responseMessage, httpError) =>
                {
                    result = await onCompletedAction(responseMessage, httpError);
                }));
            var task3 = Task.Run(() => SandboxClientTest.Shared.GetAsync("values",
                HttpClientUtil.HttpApplication.Json, async (responseMessage, httpError) =>
                {
                    result = await onCompletedAction(responseMessage, httpError);
                }));

            await task1.ConfigureAwait(false);
            Assert.NotNull(result);
            await task2.ConfigureAwait(false);
            Assert.NotNull(result);
            await task3.ConfigureAwait(false);
            Assert.NotNull(result);
        }
    }
}
