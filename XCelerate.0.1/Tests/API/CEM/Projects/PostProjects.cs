using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RSM.Xcelerate.CEM.Service.Client;
using Tests.API.Utilities;
using Tests.API.Utilities.Helpers;

namespace Tests.API.CEM.Projects
{
    [TestClass]
    public class PostProjects : CEMBaseTest
    {
        String maxLengthName = GenerateRandomString(50);
        String tooLongName = GenerateRandomString(51);
        String tooLongDescription = GenerateRandomString(151);
        String maxLengthDescription = GenerateRandomString(150);

        Int32 validYearMin = 2010;
        Int32 invalidYearMin = 2009;
        Int32 validYearMax = 9999;
        Int32 invalidYearMax = 10000;

        // Generate Random String
        public static string GenerateRandomString(int length)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[length];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }
            var finalString = new String(stringChars);

            return finalString;

        }

        [TestMethod, TestCategory("Smoke")]
        [Description("207393 |POST /projects with all blank schema values: with attributes without values")]
        public async Task PostBlankProjectAuthorizedAsync()
        {

            var request = new CreateProjectCommandRequest()
            {
                Name = "",
                Description = "",
                LineOfBusiness = "",
                Type = "",
                EngagementId = Guid.Empty,
                ProjectYear = null

            };

            var result = await CEMHttpResponseHelper.VerifyValidationErrorAsync(() => CemUserClient.Projects_CreateAsync(request));

        }

        [TestMethod, TestCategory("Smoke")]
        [Description("207395 |POST /projects with all blank schema values: with attributes without values without token")]
        public async Task PostBlankProjectUnauthorizedAsync()
        {

            var request = new CreateProjectCommandRequest()
            {
                Name = "",
                Description = "",
                LineOfBusiness = "",
                Type = "",
                EngagementId = Guid.Empty,
                ProjectYear = null

            };

            var result = await CEMHttpResponseHelper.VerifyUnauthorizedAsync(() => CemNoTokenClient.Projects_CreateAsync(request));

        }

        [TestMethod, TestCategory("Smoke")]
        [Description("207889|233922 |POST /projects with all valid schema values - max Year boarder")]
        public async Task PostProjectWithMaxYearAsync()
        {

            var timestamp = DateTime.Now.ToFileTime();
            DateTimeOffset today = DateTimeOffset.Now;

            string filterQuery = "mdmClientId eq '" + Constants.CemClientId + "'";
            var resultEngagementsByMDMClientID = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Engagements_GetAllAsync(filter: filterQuery));
            resultEngagementsByMDMClientID.Should().NotBeNull();
            var resultActiveEngagementsByMDMClientID = GetActiveEngagements(resultEngagementsByMDMClientID.Results);
            Guid randomEngagementId = this.GetRandomEngagement(resultActiveEngagementsByMDMClientID).Id;
            Console.WriteLine("Random  Engagement ID : " + randomEngagementId);

            var request = new CreateProjectCommandRequest()
            {
                Name = maxLengthName,
                Description = maxLengthDescription,
                LineOfBusiness = "Audit",
                Type = "Audit",
                ScheduledStartDate = today,
                ScheduledEndDate = today.AddDays(1),
                EngagementId = randomEngagementId,
                ProjectYear = validYearMax,
                MdmClientId = Constants.CemClientId
            };

            var result = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Projects_CreateAsync(request));
            this.VerifyCEMProjectFieldsCollection(result, request);
        }

        private static IEnumerable<object[]> GetCasesTaxTypeLOB()
        {

            var typesTax = new List<string> { "Compliance",
                                                "Consulting",
                                                "Estimated Payment",
                                                "Estimates",
                                                "Extension",
                                                "Projections",
                                                "State and Local",
                                                "Tax Return",
                                                "Other" };


            return IterateParams("Tax", typesTax);  

        }

        private static IEnumerable<object[]> GetCasesConsultingTypeLOB()
        {

            var typesConsulting = new List<string> { "Business Applications",
                                                        "Compilation",
                                                        "Data and Digital Services",
                                                        "Financial",
                                                        "Managed Technology Services",
                                                        "Management",
                                                        "Risk",
                                                        "Staff Augmentation",
                                                        "TAS",
                                                        "Other" };

            return IterateParams("Consulting", typesConsulting);

        }

        private static IEnumerable<object[]> GetCasesAuditTypeLOB()
        {

            var typesAudit = new List<string> { "Attestation",
                                                "Audit",
                                                "Compilation",
                                                "Compliance",
                                                "Preparation",
                                                "Review" };

            return IterateParams("Audit", typesAudit);

        }

        private static IEnumerable<object[]> GetCasesGlobalTypeLOB()
        {

            var typesGlobal = new List<string> { "Foreign Direct Investment",
                                                "GCRS",
                                                "International Audit",
                                                "International Consulting",
                                                "International Tax" };

            return IterateParams("Global", typesGlobal);

        }

        private static IEnumerable<object[]> GetCasesWealthManagementTypeLOB()
        {

            var typesWealthManagement = new List<string> { "Asset Protection Planning",
                                                            "Business Advisory Services",
                                                            "Cash Flow, Debt Management",
                                                            "Employer Benefits Planning",
                                                            "Financial Design",
                                                            "Investment Management",
                                                            "Philanthropic Giving",
                                                            "Retirement Advisory Services",
                                                            "Risk Management",
                                                            "Tax Planning",
                                                            "Wealth Transfer Planning" };

            return IterateParams("Wealth Management", typesWealthManagement);

        }

        private static IEnumerable<object[]> IterateParams(string line, List<string> types)
        {
            for (var i = 0; i < types.Count; i++)
            {
                yield return new GetTestCase
                {
                    lineOfBusiness = line,
                    type = types[i]
                }.ToObjectArray();
            }
        }

        private struct GetTestCase
        {
            public string lineOfBusiness { get; set; }
            public string type { get; set; }

            public object[] ToObjectArray()
            {
                return new object[] { lineOfBusiness, type };
            }
        }

        [TestMethod, TestCategory("Smoke")]
        [DynamicData(nameof(GetCasesTaxTypeLOB), DynamicDataSourceType.Method)]
        [DynamicData(nameof(GetCasesConsultingTypeLOB), DynamicDataSourceType.Method)]
        [DynamicData(nameof(GetCasesAuditTypeLOB), DynamicDataSourceType.Method)]
        [DynamicData(nameof(GetCasesGlobalTypeLOB), DynamicDataSourceType.Method)]
        [DynamicData(nameof(GetCasesWealthManagementTypeLOB), DynamicDataSourceType.Method)]
        [Description("207889 |POST /projects with all valid schema values - with all LOB and Types")]
        public async Task PostProjectWithAllTDOAsync(string lineOfBusiness, string type)
        {

            var timestamp = DateTime.Now.ToFileTime();
            DateTimeOffset today = DateTimeOffset.Now;

            string filterQuery = "mdmClientId eq '" + Constants.CemClientId + "'";
            var resultEngagementsByMDMClientID = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Engagements_GetAllAsync(filter: filterQuery));
            resultEngagementsByMDMClientID.Should().NotBeNull();
            var resultActiveEngagementsByMDMClientID = GetActiveEngagements(resultEngagementsByMDMClientID.Results);
            Guid randomEngagementId = this.GetRandomEngagement(resultActiveEngagementsByMDMClientID).Id;
            Console.WriteLine("Random  Engagement ID : " + randomEngagementId);

            var request = new CreateProjectCommandRequest()
            {
                Name = maxLengthName,
                Description = maxLengthDescription,
                LineOfBusiness = lineOfBusiness,
                Type = type,
                ScheduledStartDate = today,
                ScheduledEndDate = today.AddDays(1),
                EngagementId = randomEngagementId,
                ProjectYear = validYearMax,
                MdmClientId = Constants.CemClientId
            };

            var result = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Projects_CreateAsync(request));
            VerifyCEMProjectFieldsCollection(result, request);
        }

        [TestMethod, TestCategory("Smoke")]
        [Description("207889 |POST /projects with all valid schema values - min Year boarder")]
        public async Task PostProjectWithMinYearAsync()
        {

            var timestamp = DateTime.Now.ToFileTime();
            DateTimeOffset today = DateTimeOffset.Now;

            string filterQuery = "mdmClientId eq '" + Constants.CemClientId + "'";
            var resultEngagementsByMDMClientID = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Engagements_GetAllAsync(filter: filterQuery));
            resultEngagementsByMDMClientID.Should().NotBeNull();
            var resultActiveEngagementsByMDMClientID = GetActiveEngagements(resultEngagementsByMDMClientID.Results);
            Guid randomEngagementId = this.GetRandomEngagement(resultActiveEngagementsByMDMClientID).Id;
            Console.WriteLine("Random  Engagement ID : " + randomEngagementId);

            var request = new CreateProjectCommandRequest()
            {
                Name = "AutoName_" + timestamp,
                Description = "AutoDescription_" + timestamp,
                LineOfBusiness = "Audit",
                Type = "Audit",
                ScheduledStartDate = today,
                ScheduledEndDate = today.AddDays(1),
                EngagementId = randomEngagementId,
                ProjectYear = validYearMin,
                MdmClientId = Constants.CemClientId
            };

            var result = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Projects_CreateAsync(request));
            VerifyCEMProjectFieldsCollection(result, request);
        }

        [TestMethod, TestCategory("Smoke")]
        [Description("207889 |POST /projects with  Not valid schema Body Post values - min Year edge case")]
        public async Task PostProjectWithMinYearNotValidAsync()
        {

            var timestamp = DateTime.Now.ToFileTime();
            DateTimeOffset today = DateTimeOffset.Now;

            string filterQuery = "mdmClientId eq '" + Constants.CemClientId + "'";
            var resultEngagementsByMDMClientID = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Engagements_GetAllAsync(filter: filterQuery));
            resultEngagementsByMDMClientID.Should().NotBeNull();
            var resultActiveEngagementsByMDMClientID = GetActiveEngagements(resultEngagementsByMDMClientID.Results);
            Guid randomEngagementId = this.GetRandomEngagement(resultActiveEngagementsByMDMClientID).Id;
            Console.WriteLine("Random  Engagement ID : " + randomEngagementId);

            var request = new CreateProjectCommandRequest()
            {
                Name = "AutoName_" + timestamp,
                Description = "AutoDescription_" + timestamp,
                LineOfBusiness = "Audit",
                Type = "Audit",
                ScheduledStartDate = today,
                ScheduledEndDate = today.AddDays(1),
                EngagementId = randomEngagementId,
                ProjectYear = invalidYearMin,
                MdmClientId = Constants.CemClientId
            };

            var result = await CEMHttpResponseHelper.VerifyValidationErrorAsync(() => CemUserClient.Projects_CreateAsync(request));
        }

        [TestMethod, TestCategory("Smoke")]
        [Description("207889 |POST /projects with  Not valid schema values - max Year boarder")]
        public async Task PostProjectWithMaxYearNotValidAsync()
        {

            var timestamp = DateTime.Now.ToFileTime();
            DateTimeOffset today = DateTimeOffset.Now;

            string filterQuery = "mdmClientId eq '" + Constants.CemClientId + "'";
            var resultEngagementsByMDMClientID = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Engagements_GetAllAsync(filter: filterQuery));
            resultEngagementsByMDMClientID.Should().NotBeNull();
            var resultActiveEngagementsByMDMClientID = GetActiveEngagements(resultEngagementsByMDMClientID.Results);
            Guid randomEngagementId = this.GetRandomEngagement(resultActiveEngagementsByMDMClientID).Id;
            Console.WriteLine("Random  Engagement ID : " + randomEngagementId);

            var request = new CreateProjectCommandRequest()
            {
                Name = "AutoName_" + timestamp,
                Description = "AutoDescription_" + timestamp,
                LineOfBusiness = "Audit",
                Type = "Audit",
                ScheduledStartDate = today,
                ScheduledEndDate = today.AddDays(1),
                EngagementId = randomEngagementId,
                ProjectYear = invalidYearMax,
                MdmClientId = Constants.CemClientId
            };

            var result = await CEMHttpResponseHelper.VerifyValidationErrorAsync(() => CemUserClient.Projects_CreateAsync(request));
        }

        [TestMethod, TestCategory("Smoke")]
        [Description("207392-5 |POST /projects with  Not valid schema values - too Long Name edge case")]
        public async Task PostProjectTooLongNameAsync()
        {

            var timestamp = DateTime.Now.ToFileTime();
            DateTimeOffset today = DateTimeOffset.Now;

            string filterQuery = "mdmClientId eq '" + Constants.CemClientId + "'";
            var resultEngagementsByMDMClientID = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Engagements_GetAllAsync(filter: filterQuery));
            resultEngagementsByMDMClientID.Should().NotBeNull();
            var resultActiveEngagementsByMDMClientID = GetActiveEngagements(resultEngagementsByMDMClientID.Results);
            Guid randomEngagementId = this.GetRandomEngagement(resultActiveEngagementsByMDMClientID).Id;
            Console.WriteLine("Random  Engagement ID : " + randomEngagementId);

            var request = new CreateProjectCommandRequest()
            {
                Name = tooLongName,
                Description = "AutoDescription_" + timestamp,
                LineOfBusiness = "Audit",
                Type = "Audit",
                ScheduledStartDate = today,
                ScheduledEndDate = today.AddDays(1),
                EngagementId = randomEngagementId,
                ProjectYear = validYearMin,
                MdmClientId = Constants.CemClientId
            };

            var result = await CEMHttpResponseHelper.VerifyValidationErrorAsync(() => CemUserClient.Projects_CreateAsync(request));
        }

        [TestMethod, TestCategory("Smoke")]
        [Description("207392-5 |POST /projects with  Not valid schema values - too Long Descripton edge case")]
        public async Task PostProjectTooLongDescriptionAsync()
        {

            var timestamp = DateTime.Now.ToFileTime();
            DateTimeOffset today = DateTimeOffset.Now;

            string filterQuery = "mdmClientId eq '" + Constants.CemClientId + "'";
            var resultEngagementsByMDMClientID = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Engagements_GetAllAsync(filter: filterQuery));
            resultEngagementsByMDMClientID.Should().NotBeNull();
            var resultActiveEngagementsByMDMClientID = GetActiveEngagements(resultEngagementsByMDMClientID.Results);
            Guid randomEngagementId = this.GetRandomEngagement(resultActiveEngagementsByMDMClientID).Id;
            Console.WriteLine("Random  Engagement ID : " + randomEngagementId);

            var request = new CreateProjectCommandRequest()
            {
                Name = "AutoName_" + timestamp,
                Description = tooLongDescription,
                LineOfBusiness = "Audit",
                Type = "Audit",
                ScheduledStartDate = today,
                ScheduledEndDate = today.AddDays(1),
                EngagementId = randomEngagementId,
                ProjectYear = validYearMin,
                MdmClientId = Constants.CemClientId
            };

            var result = await CEMHttpResponseHelper.VerifyValidationErrorAsync(() => CemUserClient.Projects_CreateAsync(request));
        }

        [TestMethod, TestCategory("Smoke")]
        [Description("207430 |POST /projects with  Not valid schema values - scheduledStartDate > scheduledEndDate")]
        public async Task PostProjectStartLaterThanEndAsync()
        {

            var timestamp = DateTime.Now.ToFileTime();
            DateTimeOffset today = DateTimeOffset.Now;

            string filterQuery = "mdmClientId eq '" + Constants.CemClientId + "'";
            var resultEngagementsByMDMClientID = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Engagements_GetAllAsync(filter: filterQuery));
            resultEngagementsByMDMClientID.Should().NotBeNull();
            var resultActiveEngagementsByMDMClientID = GetActiveEngagements(resultEngagementsByMDMClientID.Results);
            Guid randomEngagementId = this.GetRandomEngagement(resultActiveEngagementsByMDMClientID).Id;
            Console.WriteLine("Random  Engagement ID : " + randomEngagementId);

            var request = new CreateProjectCommandRequest()
            {
                Name = "AutoName_" + timestamp,
                Description = "AutoDescription_" + timestamp,
                LineOfBusiness = "Audit",
                Type = "Audit",
                ScheduledStartDate = today.AddDays(1),
                ScheduledEndDate = today,
                EngagementId = randomEngagementId,
                ProjectYear = validYearMin,
                MdmClientId = Constants.CemClientId
            };

            var result = await CEMHttpResponseHelper.VerifyValidationErrorAsync(() => CemUserClient.Projects_CreateAsync(request));
        }

        [TestMethod, TestCategory("Smoke")]
        [Description("207430 |POST /projects with  Not valid schema values - scheduledStartDate = scheduledEndDate")]
        public async Task PostProjectStartEqualToEndAsync()
        {

            var timestamp = DateTime.Now.ToFileTime();
            DateTimeOffset today = DateTimeOffset.Now;

            string filterQuery = "mdmClientId eq '" + Constants.CemClientId + "'";
            var resultEngagementsByMDMClientID = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Engagements_GetAllAsync(filter: filterQuery));
            resultEngagementsByMDMClientID.Should().NotBeNull();
            var resultActiveEngagementsByMDMClientID = GetActiveEngagements(resultEngagementsByMDMClientID.Results);
            Guid randomEngagementId = this.GetRandomEngagement(resultActiveEngagementsByMDMClientID).Id;
            Console.WriteLine("Random  Engagement ID : " + randomEngagementId);

            var request = new CreateProjectCommandRequest()
            {
                Name = "AutoName_" + timestamp,
                Description = "AutoDescription_" + timestamp,
                LineOfBusiness = "Tax",
                Type = "Tax Return",
                ScheduledStartDate = today.AddDays(1),
                ScheduledEndDate = today.AddDays(1),
                EngagementId = randomEngagementId,
                ProjectYear = validYearMin,
                MdmClientId = Constants.CemClientId
            };

            var result = await CEMHttpResponseHelper.VerifyValidationErrorAsync(() => CemUserClient.Projects_CreateAsync(request));
        }

        [TestMethod, TestCategory("Smoke")]
        [Description("234874 |POST Its forbidden to create project for engagement with No Active status")]
        public async Task PostProjectCreateForNotActiveEngagementIsForbiddenAsync()
        {

            var timestamp = DateTime.Now.ToFileTime();
            DateTimeOffset today = DateTimeOffset.Now;

            var requestToCreateEngagement = new CreateEngagementCommandRequest()
            {
                Name = "AutoName_" + timestamp,
                Description = "AutoDescription_" + timestamp,
                LineOfBusiness = "Tax",
                Type = "Tax Return",
                ScheduledStartDate = today,
                ScheduledEndDate = today.AddDays(2),
                Sow = "",
                SowDate = today,
                MdmClientId = Constants.CemClientId
            };

            var _responseCreateEngagement = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Engagements_CreateAsync(requestToCreateEngagement));
            Assert.IsNotNull(_responseCreateEngagement, "Error of Engagement creation. Response is empty");
            Guid _engagementId = _responseCreateEngagement.Id;

            var requestToUpdateEngagement = new UpdateEngagementCommandRequest()
            {
                Name = "AutoName_" + timestamp,
                Description = "AutoDescription_" + timestamp,
                LineOfBusiness = "Tax",
                Type = "Tax Return",
                ScheduledStartDate = today,
                ScheduledEndDate = today.AddDays(2),
                Sow = "",
                SowDate = today,
                Id = _engagementId,
                Status = "Inactive"
                //MdmClientId = Constants.CemClientId
            };

            var _responseToUpdateEngagement = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Engagements_PutAsync(_engagementId, requestToUpdateEngagement));
            Assert.IsNotNull(_responseToUpdateEngagement, "Error of Engagement updating. Response is empty");

            var request = new CreateProjectCommandRequest()
            {
                Name = "AutoName_" + timestamp,
                Description = "AutoDescription_" + timestamp,
                LineOfBusiness = "Tax",
                Type = "Tax Return",
                ScheduledStartDate = today,
                ScheduledEndDate = today.AddDays(1),
                EngagementId = _engagementId,
                ProjectYear = validYearMin,
                MdmClientId = Constants.CemClientId
            };

            var result = await CEMHttpResponseHelper.VerifyValidationErrorAsync(() => CemUserClient.Projects_CreateAsync(request));

        }

        [TestMethod, TestCategory("Smoke")]
        [Description("207402 |POST /projects project creation with the same name is allowed within one client (different Engagements)")]
        public async Task PostProjectCreationWithTheSameNameWithinDifferenEngagementAllowedAsync()
        {

            var timestamp = DateTime.Now.ToFileTime();
            DateTimeOffset today = DateTimeOffset.Now;

            string filterQuery = "mdmClientId eq '" + Constants.CemClientId + "'";
            var resultEngagementsByMDMClientID = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Engagements_GetAllAsync(filter: filterQuery));
            resultEngagementsByMDMClientID.Should().NotBeNull();
            var resultActiveEngagementsByMDMClientID = GetActiveEngagements(resultEngagementsByMDMClientID.Results);
            Guid randomEngagementId = this.GetRandomEngagement(resultActiveEngagementsByMDMClientID).Id;
            Console.WriteLine("Random Engagement ID : " + randomEngagementId);

            var request = new CreateProjectCommandRequest()
            {
                Name = "AutoName_" + timestamp,
                Description = "AutoDescription_" + timestamp,
                LineOfBusiness = "Audit",
                Type = "Audit",
                ScheduledStartDate = today,
                ScheduledEndDate = today.AddDays(1),
                EngagementId = randomEngagementId,
                ProjectYear = validYearMin,
                MdmClientId = Constants.CemClientId
            };

            var result = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Projects_CreateAsync(request));

            Guid newEngagementId = this.GetNewEngagementInTheContextOfClient(resultEngagementsByMDMClientID.Results, randomEngagementId);
            Console.WriteLine("New Engagement ID : " + newEngagementId);

            request.EngagementId = newEngagementId;
            var resultWithTheSameName = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Projects_CreateAsync(request));
        }

        [TestMethod, TestCategory("Smoke")]
        [Description("207401 |POST /projects Project creation with the same name is not allowed with the same engagement")]
        public async Task PostProjectCreationWithTheSameNameWithinTheSameEngagementForbiddenAsync()
        {

            var timestamp = DateTime.Now.ToFileTime();
            DateTimeOffset today = DateTimeOffset.Now;

            string filterQuery = "mdmClientId eq '" + Constants.CemClientId + "'";
            var resultEngagementsByMDMClientID = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Engagements_GetAllAsync(filter: filterQuery));
            resultEngagementsByMDMClientID.Should().NotBeNull();
            var resultActiveEngagementsByMDMClientID = GetActiveEngagements(resultEngagementsByMDMClientID.Results);
            Guid randomEngagementId = this.GetRandomEngagement(resultActiveEngagementsByMDMClientID).Id;
            Console.WriteLine("Random Engagement ID : " + randomEngagementId);

            var request = new CreateProjectCommandRequest()
            {
                Name = "AutoName_" + timestamp,
                Description = "AutoDescription_" + timestamp,
                LineOfBusiness = "Audit",
                Type = "Audit",
                ScheduledStartDate = today,
                ScheduledEndDate = today.AddDays(1),
                EngagementId = randomEngagementId,
                ProjectYear = validYearMin,
                MdmClientId = Constants.CemClientId
            };

            var result = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Projects_CreateAsync(request));

            var resultWithTheSameName = await CEMHttpResponseHelper.VerifyValidationErrorAsync(() => CemUserClient.Projects_CreateAsync(request));
        }

        private Guid GetNewEngagementInTheContextOfClient(ICollection<EngagementDto> results, Guid engagementID)
        {
            Guid newEngagementID = new Guid();
            foreach (var item in results)
            {
                if (item.Id != engagementID)
                {
                    Guid id = item.Id;
                    return id;
                }
                else
                {
                    throw new Exception("No new Engagements were found");
                }
            }
            return newEngagementID;
        }

        private EngagementDto GetRandomEngagement(ICollection<EngagementDto> results)
        {
            Random random = new Random();
            int randomEngagementIndex = random.Next(results.Count);
            EngagementDto randomEngagement = results.ElementAt(randomEngagementIndex);
            return randomEngagement;
        }

        private ICollection<EngagementDto> GetActiveEngagements(ICollection<EngagementDto> results)
        {
            ICollection<EngagementDto> collection = new List<EngagementDto>();
            foreach (var item in results)
            {
                try 
                {
                    if (item.Status == "Active")
                    {
                        collection.Add(item);
                    }
                }
                catch
                {
                    throw new Exception("No active engagements were found");
                }
            }
            return collection;
        }

        private void VerifyCEMProjectFieldsCollection(CreateProjectCommandResponse _results, CreateProjectCommandRequest _request)
        {
            Assert.AreEqual(_request.Name, _results.Name);
            Assert.AreEqual(_request.Description, _results.Description);
            Assert.AreEqual(_request.LineOfBusiness, _results.LineOfBusiness);
            Assert.AreEqual(_request.Type, _results.Type);
            Assert.AreEqual(_request.EngagementId, _results.EngagementId);
            Assert.AreEqual(_request.ProjectYear, _results.ProjectYear);

            Assert.IsNotNull(_results.CreatorId, "CreatorID should not be empty");
            Assert.IsNotNull(_results.Id, "Id should not be empty");
            Assert.AreEqual(_results.Status, "Active");

        }


    }

}
