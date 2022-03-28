using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.API.Utilities.Helpers;
using System.Collections.Generic;
using RSM.Xcelerate.ETL.Service.Client;
using Data.API;

namespace Tests.API.ETL.AlteryxQuestions
{
    [TestClass]
    public class ETLGetAlteryxQuestions : ETLBaseTest
    {
        [TestMethod, TestCategory("Smoke")]
        [Description("240047 | GET /AlteryxQuestions - get all questions")]
        public async Task GetAllAlteryxQuestionsTest()
        {
            var result = await EtlHttpResponseHelper.VerifySuccessfulStatusAsync(() => EtlUserClient.AlteryxQuestions_GetQuestionListAsync());
            result.Should().NotBeNull();
            VerifyAlteryxQuestionsFields(result.Results);
        }

        [TestMethod, TestCategory("Smoke")]
        [Description("309439 | GET /AlteryxQuestions - filter by valid AlteryxWorkflowId")]
        public async Task GetAllAlteryxQuestionsFilterByAlteryxWorkflowIdTest()
        {
            Guid randomAlteryxWf = await GetRandomAlteryxWorkflowIdAsync();
            string filterQuery = "alteryxWorkflowId eq '" + randomAlteryxWf.ToString() + "'";
            var result = await EtlHttpResponseHelper.VerifySuccessfulStatusAsync(() => EtlUserClient.AlteryxQuestions_GetQuestionListAsync(filter: filterQuery));
            result.Should().NotBeNull();
            VerifyAlteryxQuestionsFields(result.Results);
        }

        [TestMethod]
        [Description("309440 | GET /AlteryxQuestions - filter by not existing AlteryxWorkflowId")]
        public async Task GetAllAlteryxQuestionsFilterByNotExistingAlteryxWorkflowIdTest()
        {
            string filterQuery = "alteryxWorkflowId eq '" + Config._notExistingGuid + "'";
            var result = await EtlHttpResponseHelper.VerifySuccessfulStatusAsync(() => EtlUserClient.AlteryxQuestions_GetQuestionListAsync(filter: filterQuery));
            result.Results.Should().BeEmpty();
        }

        [TestMethod]
        [Description("309441 | GET /AlteryxQuestions - filter by empty AlteryxWorkflowId")]
        public async Task GetAllAlteryxQuestionsFilterByEmptyAlteryxWorkflowIdTest()
        {
            string filterQuery = "alteryxWorkflowId eq ''";
            var result = await EtlHttpResponseHelper.VerifyNotInternalServerErrorAsync(() => EtlUserClient.AlteryxQuestions_GetQuestionListAsync(filter: filterQuery));
            result.Should().BeNull();
        }

        [TestMethod]
        [Description("309442 | GET /AlteryxQuestions - filter by null AlteryxWorkflowId")]
        public async Task GetAllAlteryxQuestionsFilterByNullAlteryxWorkflowIdTest()
        {
            string filterQuery = "alteryxWorkflowId eq null";
            var result = await EtlHttpResponseHelper.VerifySuccessfulStatusAsync(() => EtlUserClient.AlteryxQuestions_GetQuestionListAsync(filter: filterQuery));
            result.Results.Should().BeEmpty();
        }

        [TestMethod]
        [Description("247897 | GET /AlteryxQuestions - unauthorized request")]
        public async Task GetAllAlteryxQuestionsWithoutTokenTest()
        {
            await EtlHttpResponseHelper.VerifyUnauthorizedAsync(() => EtlNoTokenClient.AlteryxQuestions_GetQuestionListAsync());
        }

        [TestMethod]
        [Description("309443 | GET /AlteryxQuestions - filter by AlteryxWorkflowId without token")]
        public async Task GetAllAlteryxQuestionsFilterByAlteryxWorkflowIdWithoutTokenTest()
        {
            string filterQuery = "alteryxWorkflowId eq '" + Config._notExistingGuid + "'";
            await EtlHttpResponseHelper.VerifyUnauthorizedAsync(() => EtlNoTokenClient.AlteryxQuestions_GetQuestionListAsync(filter: filterQuery));
            await EtlHttpResponseHelper.VerifySuccessfulStatusAsync(() => EtlUserClient.Files_GetListAsync(top: 3));
        }

        private static void VerifyAlteryxQuestionsFields(ICollection<AlteryxQuestionListDto> alteryxQuestionResult)
        {
            foreach (var item in alteryxQuestionResult)
            {
                VerifyAlteryxQuestionsFields(item);
            }
        }

        private static void VerifyAlteryxQuestionsFields(AlteryxQuestionListDto alteryxQuestionActualResult)
        {
            Assert.IsNotNull(alteryxQuestionActualResult.Id, "Id should not be empty");
            Assert.IsNotNull(alteryxQuestionActualResult.Name, "Name should not be empty");
            Assert.IsNotNull(alteryxQuestionActualResult.Description, "Description should not be empty");
        }
    }
}