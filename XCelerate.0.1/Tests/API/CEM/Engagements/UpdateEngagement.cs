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
    public class UpdateEngagement : CEMBaseTest
    {
        String maxLengthName = GenerateRandomString(200);
        String tooLongName = GenerateRandomString(210);
        String tooLongDescription = GenerateRandomString(251);
        String maxLengthDescription = GenerateRandomString(250);
        Guid _wrongGuid = new Guid("8bc1fc6a-3fae-4126-b8fb-4eec108b3112");
        String _statusForUpdate;
        String _engagementLOBForUpdate;
        String _engagementType;
        String _status = "Active";
        String _inactiveStatus = "Inactive";
        String _lineOfBisiness = "Audit";
        String _type = "Audit";
        String _taxLOB = "Tax";
       
        DateTimeOffset today = DateTimeOffset.Now;


        [TestMethod]
        [Description("304875 | Verify all Updatable fields can be updated")]
        public async Task UpdateAllUpdatableFieldsAsync()
        {
            var _result = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Engagements_GetAllAsync());
            _result.Should().NotBeNull();

            Guid _randomEngagementId = this.GetRandomEngagement(_result.Results).Id;
            GetEngagementByIdQueryResponse _randomEngagementForUpdate = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Engagements_GetByIdAsync(_randomEngagementId));
            String _engagementStatus = _randomEngagementForUpdate.Status;
            String _engagementLOB = _randomEngagementForUpdate.LineOfBusiness;
            int _mdmClientId = _randomEngagementForUpdate.MdmClientId;

            var timestamp = DateTime.Now.ToFileTime();

            if (_engagementStatus.Equals(_status))
            {
                _statusForUpdate = _inactiveStatus;
            }
            else
            {
                _statusForUpdate = _status;
            }

            if (_engagementLOB.Equals(_taxLOB))
            {
                _engagementLOBForUpdate = _lineOfBisiness;
                _engagementType = _type;
            }
            else
            {
                _engagementLOBForUpdate = _taxLOB;
                _engagementType = "Tax Return";
            }

            var request = new UpdateEngagementCommandRequest()
            {
                Id = _randomEngagementId,
                Status = _statusForUpdate,
                Name = "AutoName_" + timestamp,
                Description = "",
                LineOfBusiness = _engagementLOBForUpdate,
                Type = _engagementType,
                ScheduledEndDate = today.AddMonths(1),
                ScheduledStartDate = today,
                Sow = "",
                SowDate = today

            };

            var result = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Engagements_PutAsync(_randomEngagementId, request));
            result.Should().NotBeNull();

            this.VerifyCEMEngagementFieldsCollectionForUpdate(result, request, _mdmClientId);

        }


        //@TODO Update Validatio error message verufucation after ANdrei's updates
        [TestMethod]
        [Description("295469 | Update engagement - validation for duplicate Engagement Name")]
        public async Task DuplicateNameValidationAsync()
        {
            var _result = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Engagements_GetAllAsync());
            _result.Should().NotBeNull();

            Guid _randomEngagementId = this.GetRandomEngagement(_result.Results).Id;
            GetEngagementByIdQueryResponse _randomEngagementForUpdate = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Engagements_GetByIdAsync(_randomEngagementId));
            int _mdmClientId = _randomEngagementForUpdate.MdmClientId;

            string filterQuery = "mdmClientId eq '" + _mdmClientId + "'";
            var _result2 = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Engagements_GetAllAsync(filter: filterQuery));

            Random random = new Random();

            Guid _engagementId = _result2.Results.ElementAt(random.Next(_result2.Results.Count)).Id;


            var request = new UpdateEngagementCommandRequest()
            {
                Id = _engagementId,
                Status = _status,
                Name = _randomEngagementForUpdate.Name,
                Description = "",
                LineOfBusiness = _lineOfBisiness,
                Type = _type,
                ScheduledEndDate = today.AddMonths(1),
                ScheduledStartDate = today,
                Sow = "",
                SowDate = today

            };

            await CEMHttpResponseHelper.VerifyValidationErrorAsync(() => CemUserClient.Engagements_PutAsync(_engagementId, request));

        }

        //@TODO investigate and update
        [TestMethod]
        [Ignore]
        [Description("305736 | Update engagement - validation for wrong ID Update")]
        public async Task WrongIdValidationAsync()
        {
            var _result = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Engagements_GetAllAsync());
            _result.Should().NotBeNull();

            Guid _randomEngagementId = this.GetRandomEngagement(_result.Results).Id;
            var timestamp = DateTime.Now.ToFileTime();

            var request = new UpdateEngagementCommandRequest()
            {
                Id = _randomEngagementId,
                Status = _status,
                Name = "AutoName_" + timestamp,
                Description = "",
                LineOfBusiness = _lineOfBisiness,
                Type = _type,
                ScheduledEndDate = today.AddMonths(1),
                ScheduledStartDate = today,
                Sow = "",
                SowDate = today

            };

            await CEMHttpResponseHelper.VerifyNotFoundAsync(() => CemUserClient.Engagements_PutAsync(_wrongGuid, request));

        }

        [TestMethod]
        [Description("305738 | Engagement update field values without token")]
        public async Task NoTokenValidationAsync()
        {
            var _result = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Engagements_GetAllAsync());
            _result.Should().NotBeNull();

            Guid _randomEngagementId = this.GetRandomEngagement(_result.Results).Id;
            var timestamp = DateTime.Now.ToFileTime();

            var request = new UpdateEngagementCommandRequest()
            {
                Id = _randomEngagementId,
                Status = _status,
                Name = "AutoName_" + timestamp,
                Description = "",
                LineOfBusiness = _lineOfBisiness,
                Type = _type,
                ScheduledEndDate = today.AddMonths(1),
                ScheduledStartDate = today,
                Sow = "",
                SowDate = today

            };

            await CEMHttpResponseHelper.VerifyUnauthorizedAsync(() => CemNoTokenClient.Engagements_PutAsync(_randomEngagementId, request));

        }

        [TestMethod]
        [Description("305650 | Validation for Max Length Update")]
        public async Task MaxLengthFieldsUpdateAsync()
        {

            var _result = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Engagements_GetAllAsync());
            _result.Should().NotBeNull();

            Guid _randomEngagementId = this.GetRandomEngagement(_result.Results).Id;
            GetEngagementByIdQueryResponse _randomEngagementForUpdate = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Engagements_GetByIdAsync(_randomEngagementId));
            int _mdmClientId = _randomEngagementForUpdate.MdmClientId;

            var timestamp = DateTime.Now.ToFileTime();

            var request = new UpdateEngagementCommandRequest()
            {
                Id = _randomEngagementId,
                Status = _status,
                Name = maxLengthName,
                Description = maxLengthDescription,
                LineOfBusiness = _lineOfBisiness,
                Type = _type,
                ScheduledEndDate = today.AddMonths(1),
                ScheduledStartDate = today,
                Sow = maxLengthName,
                SowDate = today

            };

            var result = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Engagements_PutAsync(_randomEngagementId, request));
            result.Should().NotBeNull();

            this.VerifyCEMEngagementFieldsCollectionForUpdate(result, request, _mdmClientId);

        }

        [TestMethod]
        [Description("305657 | Validation for invalid Description Update")]
        public async Task TooLongDescriptionUpdateAsync()
        {
            var _result = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Engagements_GetAllAsync());
            _result.Should().NotBeNull();

            Guid _randomEngagementId = this.GetRandomEngagement(_result.Results).Id;

            var timestamp = DateTime.Now.ToFileTime();

            var request = new UpdateEngagementCommandRequest()
            {
                Id = _randomEngagementId,
                Status = _status,
                Name = maxLengthName,
                Description = tooLongDescription,
                LineOfBusiness = _lineOfBisiness,
                Type = _type,
                ScheduledEndDate = today.AddMonths(1),
                ScheduledStartDate = today,
                Sow = maxLengthName,
                SowDate = today

            };

            await CEMHttpResponseHelper.VerifyValidationErrorAsync(() => CemUserClient.Engagements_PutAsync(_randomEngagementId, request));

        }

        [TestMethod]
        [Description("305664 | Validation for invalid Name Update")]
        public async Task TooLongNameFieldAsync()
        {
            var _result = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Engagements_GetAllAsync());
            _result.Should().NotBeNull();

            Guid _randomEngagementId = this.GetRandomEngagement(_result.Results).Id;

            var request = new UpdateEngagementCommandRequest()
            {
                Id = _randomEngagementId,
                Status = _status,
                Name = tooLongName,
                Description = "",
                LineOfBusiness = _lineOfBisiness,
                Type = _type,
                ScheduledEndDate = today.AddMonths(1),
                ScheduledStartDate = today,
                Sow = maxLengthName,
                SowDate = today

            };

            await CEMHttpResponseHelper.VerifyValidationErrorAsync(() => CemUserClient.Engagements_PutAsync(_randomEngagementId, request));
        }

        [TestMethod]
        [Description("305675 | Validation for invalid SOW Update")]
        public async Task TooLongSOWUpdateAsync()
        {
            var _result = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Engagements_GetAllAsync());
            _result.Should().NotBeNull();

            Guid _randomEngagementId = this.GetRandomEngagement(_result.Results).Id;
            var timestamp = DateTime.Now.ToFileTime();

            var request = new UpdateEngagementCommandRequest()
            {
                Id = _randomEngagementId,
                Status = _status,
                Name = "Name" + timestamp,
                Description = "",
                LineOfBusiness = _lineOfBisiness,
                Type = _type,
                ScheduledEndDate = today.AddMonths(1),
                ScheduledStartDate = today,
                Sow = tooLongName,
                SowDate = today

            };

            await CEMHttpResponseHelper.VerifyValidationErrorAsync(() => CemUserClient.Engagements_PutAsync(_randomEngagementId, request));
        }

        [TestMethod]
        [Description("305686 | Validation for invalid Date Update")]
        public async Task StartDateMoreThenEndDateValidationAsync()
        {
            var _result = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Engagements_GetAllAsync());
            _result.Should().NotBeNull();

            Guid _randomEngagementId = this.GetRandomEngagement(_result.Results).Id;
            var timestamp = DateTime.Now.ToFileTime();

            var request = new UpdateEngagementCommandRequest()
            {
                Id = _randomEngagementId,
                Status = _status,
                Name = "Name" + timestamp,
                Description = "",
                LineOfBusiness = _lineOfBisiness,
                Type = _type,
                ScheduledEndDate = today,
                ScheduledStartDate = today.AddMonths(1),
                Sow = "",
                SowDate = today

            };

            await CEMHttpResponseHelper.VerifyValidationErrorAsync(() => CemUserClient.Engagements_PutAsync(_randomEngagementId, request));

        }

        [TestMethod]
        [Description("305723 | Validation for invalid Date startDate = End Date Update")]
        public async Task StartDateAsEndDateValidationAsync()
        {
            var _result = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Engagements_GetAllAsync());
            _result.Should().NotBeNull();

            Guid _randomEngagementId = this.GetRandomEngagement(_result.Results).Id;
            var timestamp = DateTime.Now.ToFileTime();

            var request = new UpdateEngagementCommandRequest()
            {
                Id = _randomEngagementId,
                Status = _status,
                Name = "Name" + timestamp,
                Description = "",
                LineOfBusiness = _lineOfBisiness,
                Type = _type,
                ScheduledEndDate = today.AddMonths(1),
                ScheduledStartDate = today.AddMonths(1),
                Sow = "",
                SowDate = today

            };

            await CEMHttpResponseHelper.VerifyValidationErrorAsync(() => CemUserClient.Engagements_PutAsync(_randomEngagementId, request));

        }

        [TestMethod]
        [Description("305734 | Verify its 200 and Engagement Id can't be updated")]
        public async Task VerifyEngagementIdCantBeUpdatedAsync()
        {
            var _result = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Engagements_GetAllAsync());
            _result.Should().NotBeNull();

            Guid _randomEngagementId = this.GetRandomEngagement(_result.Results).Id;
            var timestamp = DateTime.Now.ToFileTime();

            var request = new UpdateEngagementCommandRequest()
            {
                Id = _wrongGuid,
                Status = _status,
                Name = "Name" + timestamp,
                Description = "",
                LineOfBusiness = _lineOfBisiness,
                Type = _type,
                ScheduledEndDate = today.AddMonths(1),
                ScheduledStartDate = today.AddMonths(1),
                Sow = "",
                SowDate = today

            };

            var _resultUpdate = CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Engagements_PutAsync(_randomEngagementId, request));
            _resultUpdate.Should().NotBeNull();

        }

        [TestMethod]
        [Description("305747 | Verify its 200 Update body is empty")]
        public async Task VerifyUpdateWithEmtyBodyAsync()
        {
            var _result = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Engagements_GetAllAsync());
            _result.Should().NotBeNull();

            Guid _randomEngagementId = this.GetRandomEngagement(_result.Results).Id;

            var request = new UpdateEngagementCommandRequest()
            {
                Id = _randomEngagementId

            };

            var _resultUpdate = CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Engagements_PutAsync(_randomEngagementId, request));
            _resultUpdate.Should().NotBeNull();

        }

        private void VerifyCEMEngagementFieldsCollectionForUpdate(UpdateEngagementCommandResponse _results, UpdateEngagementCommandRequest _request, int _mdmClientId)
        {
            Assert.IsNotNull(_results.CreatorId, "CreatorID should not be empty");
            Assert.AreEqual(_results.Id, _request.Id);
            Assert.AreEqual(_results.Name, _request.Name);
            Assert.AreEqual(_results.LineOfBusiness, _request.LineOfBusiness);
            Assert.AreEqual(_results.Type, _request.Type);
            Assert.AreEqual(_results.Description, _request.Description);
            Assert.AreEqual(_results.Sow, _request.Sow);
            Assert.AreEqual(_results.Status, _request.Status);
            Assert.AreEqual(_results.MdmClientId, _mdmClientId);

        }

        // Generate Random String
        public static string GenerateRandomString(int length)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[length];
            Random random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }
            var finalString = new String(stringChars);

            return finalString;

        }

        private EngagementDto GetRandomEngagement(ICollection<EngagementDto> results)
        {
            Random random = new Random();
            int _randomEngagementIndex = random.Next(results.Count);
            EngagementDto _randomEngagement = results.ElementAt(_randomEngagementIndex);
            return _randomEngagement;

        }

    }

}

