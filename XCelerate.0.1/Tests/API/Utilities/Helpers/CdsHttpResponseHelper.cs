using System;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RSM.Xcelerate.CDS.Service.Client;

namespace Tests.API.Utilities.Helpers
{
    internal static class CdsHttpResponseHelper
    {
        public static async Task<T> VerifySuccessfulStatusAsync<T>(Func<Task<T>> method)
        {
            try
            {
                return await method();
            }
            catch (Exception ex)
            {
                Assert.Fail($"API call should return 200 status, but returned exception={ex.Message}");
                return default;
            }
        }

        public static async Task<T> VerifyBadRequestAsync<T>(Func<Task<T>> method)
        {
            try
            {
                var result = await method();
                Assert.Fail("API call should return 400 http status");
                return result;
            }
            catch (ApiException ex)
            {
                Assert.AreEqual(ex.StatusCode, (int)HttpStatusCode.BadRequest);
                return default;
            }
            catch (Exception)
            {
                Assert.Fail("API call should return 400 http status");
                return default;
            }
        }

        public static async Task<T> VerifyUnauthorizedAsync<T>(Func<Task<T>> method)
        {
            try
            {
                var result = await method();
                Assert.Fail("API call should return 401 http status");
                return result;
            }
            catch (ApiException ex)
            {
                Assert.AreEqual(ex.StatusCode, (int)HttpStatusCode.Unauthorized);
                return default;
            }
            catch (Exception)
            {
                Assert.Fail("API call should return 401 http status");
                return default;
            }
        }

        public static async Task<T> VerifyAccessForbiddenAsync<T>(Func<Task<T>> method)
        {
            try
            {
                var result = await method();
                Assert.Fail("API call should return 403 http status");
                return result;
            }
            catch (ApiException ex)
            {
                Assert.AreEqual(ex.StatusCode, (int)HttpStatusCode.Forbidden);
                return default;
            }
            catch (Exception)
            {
                Assert.Fail("API call should return 403 http status");
                return default;
            }
        }

        public static async Task<T> VerifyNotFoundAsync<T>(Func<Task<T>> method)
        {
            try
            {
                var result = await method();
                Assert.Fail("API call should return 404 http status");
                return result;
            }
            catch (ApiException ex)
            {
                Assert.AreEqual(ex.StatusCode, (int)HttpStatusCode.NotFound);
                return default;
            }
            catch (Exception)
            {
                Assert.Fail("API call should return 404 http status");
                return default;
            }
        }

        public static async Task<ValidationResponse> VerifyValidationErrorAsync<T>(Func<Task<T>> method)
        {
            try
            {
                var result = await method();
                Assert.Fail("API call should return 422 http status");
                return default;
            }
            catch (ApiException ex)
            {
                Assert.AreEqual(ex.StatusCode, (int)HttpStatusCode.UnprocessableEntity);
                var response = JsonConvert.DeserializeObject<ValidationResponse>(ex.Response);
                response.Should().NotBeNull();
                response.Error.Should().Be("Input validation failed");
                return response;
            }
            catch (Exception)
            {
                Assert.Fail("API call should return 422 http status");
                return default;
            }
        }
    }
}
