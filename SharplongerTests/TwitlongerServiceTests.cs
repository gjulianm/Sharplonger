using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sharplonger;
using NUnit.Framework;
using AncoraMVVM.Rest;

namespace Sharplonger.Tests
{
    [TestFixture]
    public class TwitlongerServiceTests
    {
        public TwitlongerService Service
        {
            get
            {
                return new TwitlongerService(SensitiveData.AppName, SensitiveData.ApiKey, "TestUser");
            }
        }

        private async Task<T> TestEndpoint<T>(Func<Task<HttpResponse<T>>> task)
        {
            var response = await task();

            Assert.IsTrue(response.Succeeded, "Request not suceeded. Error code {0}, inner exception {1}, response {2}", response.StatusCode, response.InnerException, response.StringContents);

            return response.Content;
        }

        private async Task TestEndpoint(Func<Task<HttpResponse>> task)
        {
            var response = await task();

            Assert.IsTrue(response.Succeeded, "Request not suceeded. Error code {0}, inner exception {1}, response {2}", response.StatusCode, response.InnerException, response.StringContents);;
        }

        [Test]
        [Ignore("Live update")]
        public async Task PostUpdate()
        {
            await TestEndpoint(() => Service.PostUpdate(DateTime.Now.ToString("o")));
        }
        
        [Test]
        [Ignore("Live update")]
        public async Task SetId()
        {
            var response = await Service.PostUpdate(DateTime.Now.ToString("o"));

            await TestEndpoint(() => Service.SetId(response.Content.Post.Id, 1000));
        }
    }
}
