using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using RSM.Xcelerate.CEM.Service.Client;
using Tests.API.Utilities.Helpers;
using Tests.API.Utilities;
using Tests.UI;




namespace Tests.API.CEM.Engagements
{
    [TestClass]
    public class PostEngagement : CEMBaseTest
    {
        String maxLengthName = GenerateRandomString(200);
        String tooLongName = GenerateRandomString(210);
        String tooLongDescription = GenerateRandomString(251);
        String maxLengthDescription = GenerateRandomString(250);

        [TestMethod]
        [Description("303117 | Add new engagement")]
        public async Task PostEngagementAsync()
        {
            var timestamp = DateTime.Now.ToFileTime();
            DateTimeOffset today = DateTimeOffset.Now;

            var request = new CreateEngagementCommandRequest()
            {
                Name = "AutoName_" + timestamp,
                Description = "",
                LineOfBusiness = "Audit",
                Type = "Audit",
                MdmClientId = Constants.CemClientId,
                ScheduledEndDate = today.AddMonths(1),
                ScheduledStartDate = today,
                Sow = "",
                SowDate = today

            };

            var result = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Engagements_CreateAsync(request));
            result.Should().NotBeNull();

            this.VerifyCEMEngagementFieldsCollection(result, request);

        }

        [TestMethod]
        [Description("303129 | Add new engagement with Max Name Length")]
        public async Task PostEngagementWithMaxNameLengthAsync()
        {
            DateTimeOffset today = DateTimeOffset.Now;

            var request = new CreateEngagementCommandRequest()
            {
                Name = maxLengthName,
                Description = "",
                LineOfBusiness = "Audit",
                Type = "Audit",
                MdmClientId = Constants.CemClientId,
                ScheduledEndDate = today.AddMonths(1),
                ScheduledStartDate = today,
                Sow = "",
                SowDate = today

            };

            var result = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Engagements_CreateAsync(request));
            result.Should().NotBeNull();

            this.VerifyCEMEngagementFieldsCollection(result, request);

        }

        [TestMethod]
        [Description("205379 | 303128 | Add New eng with max description")]
        public async Task PostEngagementWithMaxDescriptionLengthAsync()
        {
            DateTimeOffset today = DateTimeOffset.Now;
            var timestamp = DateTime.Now.ToFileTime();

            var request = new CreateEngagementCommandRequest()
            {
                Name = "AutoName_" + timestamp,
                Description = maxLengthDescription,
                LineOfBusiness = "Audit",
                Type = "Audit",
                MdmClientId = Constants.CemClientId,
                ScheduledEndDate = today.AddMonths(1),
                ScheduledStartDate = today,
                Sow = "",
                SowDate = today

            };

            var result = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Engagements_CreateAsync(request));
            result.Should().NotBeNull();

            this.VerifyCEMEngagementFieldsCollection(result, request);

        }

        [TestMethod]
        [Description("303131 | Add New eng with max sow")]
        public async Task PostEngagementWithMaxSowLengthAsync()
        {
            DateTimeOffset today = DateTimeOffset.Now;
            var timestamp = DateTime.Now.ToFileTime();

            var request = new CreateEngagementCommandRequest()
            {
                Name = "AutoName_" + timestamp,
                Description = "",
                LineOfBusiness = "Audit",
                Type = "Audit",
                MdmClientId = Constants.CemClientId,
                ScheduledEndDate = today.AddMonths(1),
                ScheduledStartDate = today,
                Sow = maxLengthName,
                SowDate = today

            };

            var result = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Engagements_CreateAsync(request));
            result.Should().NotBeNull();

            this.VerifyCEMEngagementFieldsCollection(result, request);

        }

        [TestMethod]
        [Description("303171 | Add new engagement without token")]
        public async Task PostEngagementWithoutToken()
        {
            DateTimeOffset today = DateTimeOffset.Now;
            var timestamp = DateTime.Now.ToFileTime();

            var request = new CreateEngagementCommandRequest()
            {
                Name = "AutoName_" + timestamp,
                Description = "",
                LineOfBusiness = "Audit",
                Type = "Audit",
                MdmClientId = Constants.CemClientId,
                ScheduledEndDate = today.AddMonths(1),
                ScheduledStartDate = today,
                Sow = "",
                SowDate = today

            };

            await CEMHttpResponseHelper.VerifyUnauthorizedAsync(() => CemNoTokenClient.Engagements_CreateAsync(request));
            
        }

        //@TODO : Andrei - Need add ability to verify object class property
        //@TODO : Olya - Update tests after Andrei's updates
        [TestMethod]
        [Description("303122 | Add new engagement with all LOB and Types ")]
        [Ignore]
        public async Task PostEngagementWithAllLOBsTypesAsync()
        {
            var _lob = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.UIConfigurations_GetAllAsync());
            var _engagTypes = _lob.EngagementTypesWithGroups;

            foreach (var _type in _engagTypes)
            {
                DateTimeOffset today = DateTimeOffset.Now;
                var timestamp = DateTime.Now.ToFileTime();

                var request = new CreateEngagementCommandRequest()
                {
                    Name = "AutoName_" + timestamp,
                    Description = "",
                    /* LineOfBusiness = _type.group,
                     Type = _engagTypes.name,*/
                    MdmClientId = Constants.CemClientId,
                    ScheduledEndDate = today.AddMonths(1),
                    ScheduledStartDate = today,
                    Sow = "",
                    SowDate = today

                };

                var result = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Engagements_CreateAsync(request));
                result.Should().NotBeNull();

                this.VerifyCEMEngagementFieldsCollection(result, request);
            }

        }

        [TestMethod]
        [Description("303187 | Validation For Name Field > max length")]
        public async Task PostEngagementWithTooLongName()
        {
            DateTimeOffset today = DateTimeOffset.Now;

            var request = new CreateEngagementCommandRequest()
            {
                Name = tooLongName,
                Description = "",
                LineOfBusiness = "Audit",
                Type = "Audit",
                MdmClientId = Constants.CemClientId,
                ScheduledEndDate = today.AddMonths(1),
                ScheduledStartDate = today,
                Sow = "",
                SowDate = today

            };

            await CEMHttpResponseHelper.VerifyValidationErrorAsync(() => CemUserClient.Engagements_CreateAsync(request));
          
        }

        [TestMethod]
        [Description("303133 | Validation For Empty Name Field ")]
        public async Task PostEngagementWithNoName()
        {
            DateTimeOffset today = DateTimeOffset.Now;
            var timestamp = DateTime.Now.ToFileTime();

            var request = new CreateEngagementCommandRequest()
            {
                Name = "",
                Description = "",
                LineOfBusiness = "Audit",
                Type = "Audit",
                MdmClientId = Constants.CemClientId,
                ScheduledEndDate = today.AddMonths(1),
                ScheduledStartDate = today,
                Sow = "",
                SowDate = today

            };

            await CEMHttpResponseHelper.VerifyValidationErrorAsync(() => CemUserClient.Engagements_CreateAsync(request));

        }

        [TestMethod]
        [Description("303178 | Validation For Description field > Max length  ")]
        public async Task PostEngagementWithTooLongDescription()
        {
            DateTimeOffset today = DateTimeOffset.Now;
            var timestamp = DateTime.Now.ToFileTime();

            var request = new CreateEngagementCommandRequest()
            {
                Name = "AutoName_" + timestamp,
                Description = tooLongDescription,
                LineOfBusiness = "Audit",
                Type = "Audit",
                MdmClientId = Constants.CemClientId,
                ScheduledEndDate = today.AddMonths(1),
                ScheduledStartDate = today,
                Sow = "",
                SowDate = today

            };

            await CEMHttpResponseHelper.VerifyValidationErrorAsync(() => CemUserClient.Engagements_CreateAsync(request));

        }

        [TestMethod]
        [Description("303188 | Validation For SOW Reference field > max length ")]
        public async Task PostEngagementWithTooLongSow()
        {
            DateTimeOffset today = DateTimeOffset.Now;
            var timestamp = DateTime.Now.ToFileTime();

            var request = new CreateEngagementCommandRequest()
            {
                Name = "AutoName_" + timestamp,
                Description = "",
                LineOfBusiness = "Audit",
                Type = "Audit",
                MdmClientId = Constants.CemClientId,
                ScheduledEndDate = today.AddMonths(1),
                ScheduledStartDate = today,
                Sow = tooLongName,
                SowDate = today

            };

            await CEMHttpResponseHelper.VerifyValidationErrorAsync(() => CemUserClient.Engagements_CreateAsync(request));

        }

        [TestMethod]
        [Description("303125 | Validation if Scheduled End Date that is earlier that the Schedule Start Date")]
        public async Task PostEngagementWithEndDateLessStartDate()
        {
            DateTimeOffset today = DateTimeOffset.Now;
            var timestamp = DateTime.Now.ToFileTime();

            var request = new CreateEngagementCommandRequest()
            {
                Name = "AutoName_" + timestamp,
                Description = "",
                LineOfBusiness = "Audit",
                Type = "Audit",
                MdmClientId = Constants.CemClientId,
                ScheduledEndDate = today,
                ScheduledStartDate = today.AddMonths(1),
                Sow = tooLongName,
                SowDate = today

            };

            await CEMHttpResponseHelper.VerifyValidationErrorAsync(() => CemUserClient.Engagements_CreateAsync(request));

        }

        [TestMethod]
        [Description("303127 | Validation if Scheduled End Date that = the Schedule Start Date")]
        public async Task PostEngagementWithEndDateSameAsStartDate()
        {
            DateTimeOffset today = DateTimeOffset.Now;
            var timestamp = DateTime.Now.ToFileTime();

            var request = new CreateEngagementCommandRequest()
            {
                Name = "AutoName_" + timestamp,
                Description = "",
                LineOfBusiness = "Audit",
                Type = "Audit",
                MdmClientId = Constants.CemClientId,
                ScheduledEndDate = today,
                ScheduledStartDate = today,
                Sow = tooLongName,
                SowDate = today

            };

            await CEMHttpResponseHelper.VerifyValidationErrorAsync(() => CemUserClient.Engagements_CreateAsync(request));

        }

        [TestMethod]
        [Description("303124 | Validation for empty LOB field")]
        public async Task PostEngagementWithEmptyLOB()
        {
            DateTimeOffset today = DateTimeOffset.Now;
            var timestamp = DateTime.Now.ToFileTime();

            var request = new CreateEngagementCommandRequest()
            {
                Name = "AutoName_" + timestamp,
                Description = "",
                Type = "Audit",
                MdmClientId = Constants.CemClientId,
                ScheduledEndDate = today.AddMonths(1),
                ScheduledStartDate = today,
                Sow = tooLongName,
                SowDate = today

            };

            await CEMHttpResponseHelper.VerifyValidationErrorAsync(() => CemUserClient.Engagements_CreateAsync(request));

        }

        [TestMethod]
        [Description("303189 | Validation for wrong LOB field")]
        public async Task PostEngagementWithWrongLOB()
        {
            DateTimeOffset today = DateTimeOffset.Now;
            var timestamp = DateTime.Now.ToFileTime();

            var request = new CreateEngagementCommandRequest()
            {
                Name = "AutoName_" + timestamp,
                Description = "",
                LineOfBusiness = "Audit30",
                Type = "Audit",
                MdmClientId = Constants.CemClientId,
                ScheduledEndDate = today.AddMonths(1),
                ScheduledStartDate = today,
                Sow = tooLongName,
                SowDate = today

            };

            await CEMHttpResponseHelper.VerifyValidationErrorAsync(() => CemUserClient.Engagements_CreateAsync(request));

        }

        [TestMethod]
        [Description("303170 | Validation for empty Engagement type field")]
        public async Task PostEngagementWithNoType()
        {
            DateTimeOffset today = DateTimeOffset.Now;
            var timestamp = DateTime.Now.ToFileTime();

            var request = new CreateEngagementCommandRequest()
            {
                Name = "AutoName_" + timestamp,
                Description = "",
                LineOfBusiness = "Audit",
                MdmClientId = Constants.CemClientId,
                ScheduledEndDate = today.AddMonths(1),
                ScheduledStartDate = today,
                Sow = tooLongName,
                SowDate = today

            };

            await CEMHttpResponseHelper.VerifyValidationErrorAsync(() => CemUserClient.Engagements_CreateAsync(request));

        }

        [TestMethod]
        [Description("303190 | Validation for wrong Engagement type field")]
        public async Task PostEngagementWithWrongType()
        {
            DateTimeOffset today = DateTimeOffset.Now;
            var timestamp = DateTime.Now.ToFileTime();

            var request = new CreateEngagementCommandRequest()
            {
                Name = "AutoName_" + timestamp,
                Description = "",
                LineOfBusiness = "Audit",
                Type = "Audit30",
                MdmClientId = Constants.CemClientId,
                ScheduledEndDate = today.AddMonths(1),
                ScheduledStartDate = today,
                Sow = tooLongName,
                SowDate = today

            };

            await CEMHttpResponseHelper.VerifyValidationErrorAsync(() => CemUserClient.Engagements_CreateAsync(request));

        }


        //@TODO : Update with validation message verification after Andrei's updates
        [TestMethod]
        [Description("295468 |Validation for duplicate Engagement Name")]
        public async Task PostEngagementWithDublicateName()
        {
            DateTimeOffset today = DateTimeOffset.Now;
            var timestamp = DateTime.Now.ToFileTime();

            var request = new CreateEngagementCommandRequest()
            {
                Name = "AutoName_" + timestamp,
                Description = "",
                LineOfBusiness = "Audit",
                Type = "Audit",
                MdmClientId = Constants.CemClientId,
                ScheduledEndDate = today.AddMonths(1),
                ScheduledStartDate = today,
                Sow = "",
                SowDate = today

            };

            await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Engagements_CreateAsync(request));
            await CEMHttpResponseHelper.VerifyValidationErrorAsync(() => CemUserClient.Engagements_CreateAsync(request));

        }



        private void VerifyCEMEngagementFieldsCollection(CreateEngagementCommandResponse _results, CreateEngagementCommandRequest _request)
        {

            Assert.AreEqual(_results.MdmClientId, _request.MdmClientId);
            Assert.IsNotNull(_results.CreatorId, "CreatorID should not be empty");
            Assert.IsNotNull(_results.Id, "Id should not be empty");
            Assert.AreEqual(_results.Name, _request.Name);
            Assert.AreEqual(_results.LineOfBusiness, _request.LineOfBusiness);
            Assert.AreEqual(_results.Type, _request.Type);
            Assert.AreEqual(_results.Description, _request.Description);
            Assert.AreEqual(_results.Sow, _request.Sow);
            Assert.AreEqual(_results.Status, "Active");

        }

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

    }

}

